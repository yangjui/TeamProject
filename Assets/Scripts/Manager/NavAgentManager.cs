using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentManager : MonoBehaviour
{
    public delegate void Quest3Delegate();
    private Quest3Delegate quest3Callback = null;

    public delegate void ClearDelegate();
    private ClearDelegate clearCallback = null;

    public delegate void CountDelegate(int _Count);
    private CountDelegate groupACountCallback = null;
    private CountDelegate groupBCountCallback = null;
    private CountDelegate groupCCountCallback = null;
    private CountDelegate maxKillCountCallback = null;
    private CountDelegate currentMaxKillCountCallback = null;

    [Range(50, 1000)] [SerializeField] private int agentNum;
    [SerializeField] private List<NavMeshAgent> agentPrefab = null;

    [Header("# Path")]
    [SerializeField] private List<Transform> targetpathForGroupA = null;
    [SerializeField] private List<Transform> targetpathForGroupB = null;
    [SerializeField] private List<Transform> targetpathForGroupC = null;

    [Header("# Target")]
    [SerializeField] private List<Transform> inNavPos = null;
    [SerializeField] private Barricade leftBarricade;
    [SerializeField] private Barricade rightBarricade;

    private Transform playerTransform;
    private GameObject[] allPaths;
    private List<NavMeshAgent> allNavMeshAgents = new List<NavMeshAgent>();
    private List<NavMeshAgent> navMeshAgentsGroupA = new List<NavMeshAgent>();
    private List<NavMeshAgent> navMeshAgentsGroupB = new List<NavMeshAgent>();
    private List<NavMeshAgent> navMeshAgentsGroupC = new List<NavMeshAgent>();
    private int count = 0;
    private int maxKillCount = 0;
    private int currentMaxKillCount = 0;

    private void Awake()
    {
        allPaths = GameObject.FindGameObjectsWithTag("Path");
        for (int i = 0; i < allPaths.Length; ++i)
        {
            if (allPaths[i].name.Substring(0, 1) == "A")
            {
                targetpathForGroupA.Add(allPaths[i].transform);
            }
            else if (allPaths[i].name.Substring(0, 1) == "B")
            {
                targetpathForGroupB.Add(allPaths[i].transform);
            }
            else
            {
                targetpathForGroupC.Add(allPaths[i].transform);
            }
        }
    }

    public void BreakRightDoor()
    {
        for (int i = 0; i < navMeshAgentsGroupB.Count; ++i)
        {
            navMeshAgentsGroupB[i].GetComponent<BakeZombie>().SetNewTarget(playerTransform.transform);
            navMeshAgentsGroupB[i].GetComponent<BakeZombie>().TargetPosition(playerTransform.transform);
        }
    }

    public void BreakLeftDoor()
    {
        for (int i = 0; i < navMeshAgentsGroupC.Count; ++i)
        {
            navMeshAgentsGroupC[i].GetComponent<BakeZombie>().SetNewTarget(playerTransform.transform);
            navMeshAgentsGroupC[i].GetComponent<BakeZombie>().TargetPosition(playerTransform.transform);
        }
    }

    public void Init(Transform _position)
    {
        playerTransform = _position;

        for (int i = 0; i < agentNum; ++i)
        {
            int randomPrefab = Random.Range(0, agentPrefab.Count);
            NavMeshAgent newAgent = Instantiate(
            agentPrefab[randomPrefab],
            inNavPos[Random.Range(0, inNavPos.Count)].position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f) * agentNum * 0.002f),
            Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)),
            transform
            );

            if (i < agentNum / 3)
            {
                newAgent.name = "Agent" + i + "GroupA";
                navMeshAgentsGroupA.Add(newAgent);
            }
            else if (i < agentNum * 2 / 3)
            {
                newAgent.name = "Agent" + i + "GroupB";
                navMeshAgentsGroupB.Add(newAgent);
            }
            else
            {
                newAgent.name = "Agent" + i + "GroupC";
                navMeshAgentsGroupC.Add(newAgent);
            }

            BakeZombie bakeZombie = newAgent.GetComponent<BakeZombie>();
            bakeZombie.OnZombieFree2 += RemoveZombieFromList;
            bakeZombie.OnZombieFree2 += RemoveZombieFromGroupList;

            allNavMeshAgents.Add(newAgent);
        }
        groupACountCallback?.Invoke(navMeshAgentsGroupA.Count);
        groupBCountCallback?.Invoke(navMeshAgentsGroupB.Count);
        groupCCountCallback?.Invoke(navMeshAgentsGroupC.Count);
        StartCoroutine(MaxKillCountCoroutine());
    }

    public void SetNewTargetForGroupB(PathTriggerManager _trigger, string _name)
    {
        for (int i = 0; i < navMeshAgentsGroupB.Count; ++i)
        {
            if (navMeshAgentsGroupB[i].name == _name)
            {
                navMeshAgentsGroupB[i].GetComponent<BakeZombie>().SetNewTarget(_trigger.NextPosForB());
            }
        }
    }

    public void SetNewTargetForGroupC(PathTriggerManager _trigger, string _name)
    {
        for (int i = 0; i < navMeshAgentsGroupC.Count; ++i)
        {
            if (navMeshAgentsGroupC[i].name == _name)
            {
                navMeshAgentsGroupC[i].GetComponent<BakeZombie>().SetNewTarget(_trigger.NextPosForC());
            }
        }
    }

    public void SetTargetForA(PathTriggerManager _trigger, string _name)
    {
        for (int i = 0; i < navMeshAgentsGroupA.Count; ++i)
        {
            if (navMeshAgentsGroupA[i].name == _name)
            {
                navMeshAgentsGroupA[i].GetComponent<BakeZombie>().TargetPosition(playerTransform);
            }
        }
    }

    public void NavAgentOn()
    {
          StartCoroutine("SetPathForEachGroupCoroutine");
    }

    public void ChangeWave(string _name)
    {
        if (_name == "Quest1")
        {
            NavAgentOn();
        }
        else if (_name == "Quest3")
        {
            bool useB = true;
            int bIndex = 0;
            int cIndex = 0;

            while (bIndex < navMeshAgentsGroupB.Count || cIndex < navMeshAgentsGroupC.Count)
            {
                if (useB && bIndex < navMeshAgentsGroupB.Count)
                {
                    navMeshAgentsGroupB[bIndex].GetComponent<BakeZombie>().SetNewTarget(rightBarricade.transform);
                    navMeshAgentsGroupB[bIndex].GetComponent<BakeZombie>().TargetPosition(rightBarricade.transform);
                    bIndex++;
                }
                else if (!useB && cIndex < navMeshAgentsGroupC.Count)
                {
                    navMeshAgentsGroupC[cIndex].GetComponent<BakeZombie>().SetNewTarget(leftBarricade.transform);
                    navMeshAgentsGroupC[cIndex].GetComponent<BakeZombie>().TargetPosition(leftBarricade.transform);
                    cIndex++;
                }

                useB = !useB;
            }
        }
    }
    private IEnumerator SetPathForEachGroupCoroutine()
    {
        int aIndex = 0;
        int bIndex = 0;
        int cIndex = 0;

        while (aIndex < navMeshAgentsGroupA.Count || bIndex < navMeshAgentsGroupB.Count || cIndex < navMeshAgentsGroupC.Count)
        {
            if (aIndex < navMeshAgentsGroupA.Count)
            {
                navMeshAgentsGroupA[aIndex].GetComponent<BakeZombie>().SetNewTarget(targetpathForGroupA[0]);
                aIndex++;
            }

            if (bIndex < navMeshAgentsGroupB.Count)
            {
                navMeshAgentsGroupB[bIndex].GetComponent<BakeZombie>().SetNewTarget(targetpathForGroupB[0]);
                bIndex++;
            }

            if (cIndex < navMeshAgentsGroupC.Count)
            {
                navMeshAgentsGroupC[cIndex].GetComponent<BakeZombie>().SetNewTarget(targetpathForGroupC[0]);
                cIndex++;
            }
            yield return null;
        }

        for (int i = 0; i < allNavMeshAgents.Count; ++i)
        {
            allNavMeshAgents[i].GetComponent<BakeZombie>().navAgent.enabled = true;
        }

        StopCoroutine("SetPathForEachGroupCoroutine");
    }

    private IEnumerator MaxKillCountCoroutine()
    {
        while (true)
        {
            count = allNavMeshAgents.Count;
            yield return new WaitForSeconds(0.3f);
            if (count - allNavMeshAgents.Count >= 30)
            {
                currentMaxKillCount = count - allNavMeshAgents.Count;
                currentMaxKillCountCallback?.Invoke(currentMaxKillCount);
                if (maxKillCount < count - allNavMeshAgents.Count)
                {
                    maxKillCount = count - allNavMeshAgents.Count;
                    maxKillCountCallback?.Invoke(maxKillCount);
                }
            }
            if (allNavMeshAgents.Count == 0)
            {
                break;
            }
        }
    }

    private void RemoveZombieFromList(BakeZombie zombie)
    {
        for (int i = 0; i < allNavMeshAgents.Count; ++i)
        {
            if (allNavMeshAgents[i].GetComponent<BakeZombie>() == zombie)
            {
                allNavMeshAgents.RemoveAt(i);
            }
        }
    }    

    private void RemoveZombieFromGroupList(BakeZombie zombie)
    {
        if (zombie.name.Substring(zombie.name.Length - 1) == "A")
        {
            for (int i = 0; i < navMeshAgentsGroupA.Count; ++i)
            {
                if (navMeshAgentsGroupA[i].GetComponent<BakeZombie>() == zombie)
                {
                    navMeshAgentsGroupA.RemoveAt(i);
                    groupACountCallback?.Invoke(navMeshAgentsGroupA.Count);
                }
                if (navMeshAgentsGroupA.Count <= 0)
                {
                    quest3Callback?.Invoke();
                }
            }
        }
        else if (zombie.name.Substring(zombie.name.Length - 1) == "B")
        {
            for (int i = 0; i < navMeshAgentsGroupB.Count; ++i)
            {

                if (navMeshAgentsGroupB[i].GetComponent<BakeZombie>() == zombie)
                {
                    navMeshAgentsGroupB.RemoveAt(i);
                    groupBCountCallback?.Invoke(navMeshAgentsGroupB.Count);
                }
            }
        }
        else if (zombie.name.Substring(zombie.name.Length - 1) == "C")
        {
            for (int i = 0; i < navMeshAgentsGroupC.Count; ++i)
            {
                if (navMeshAgentsGroupC[i].GetComponent<BakeZombie>() == zombie)
                {
                    navMeshAgentsGroupC.RemoveAt(i);
                    groupCCountCallback?.Invoke(navMeshAgentsGroupC.Count);
                }
            }
        }

        if (navMeshAgentsGroupA.Count == 0 && navMeshAgentsGroupB.Count == 0 && navMeshAgentsGroupC.Count == 0)
        {
            clearCallback?.Invoke();
        }
    }

    public void SetQuestDelegate(Quest3Delegate _quest3Callback)
    {
        quest3Callback = _quest3Callback;
    }

    public void SetClearDelegate(ClearDelegate _clearCallback)
    {
        clearCallback = _clearCallback;
    }

    public void SetCountDelegate(CountDelegate _groupACountCallback, CountDelegate _groupBCountCallback, CountDelegate _groupCCountCallback, CountDelegate _maxKillCountCallback, CountDelegate _currentMaxKillCountCallback)
    {
        groupACountCallback = _groupACountCallback;
        groupBCountCallback = _groupBCountCallback;
        groupCCountCallback = _groupCCountCallback;
        maxKillCountCallback = _maxKillCountCallback;
        currentMaxKillCountCallback = _currentMaxKillCountCallback;
    }
}