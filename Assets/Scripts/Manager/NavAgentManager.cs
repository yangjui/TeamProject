using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentManager : MonoBehaviour
{
    [SerializeField]
    private List<NavMeshAgent> agentPrefab = null;

    [Range(50, 1000)]
    [SerializeField] private int agentNum;

    private GameObject[] allPaths;
    private List<Transform> targetpathForGroupA = new List<Transform>();
    private List<Transform> targetpathForGroupB = new List<Transform>();
    private List<Transform> targetpathForGroupC = new List<Transform>();

    private Transform playerTransform;

    private List<NavMeshAgent> allNavMeshAgents = new List<NavMeshAgent>();
    private List<NavMeshAgent> navMeshAgentsGroupA = new List<NavMeshAgent>();
    private List<NavMeshAgent> navMeshAgentsGroupB = new List<NavMeshAgent>();
    private List<NavMeshAgent> navMeshAgentsGroupC = new List<NavMeshAgent>();

    [SerializeField] private Barricade rightBarricade;
    [SerializeField] private Barricade leftBarricade;

    private bool isAlarmON = false;
    private Vector3 instantPositionInNavArea;
    private Vector3 instantPosition;
    [SerializeField]
    private List<Transform> inNavPos = null;

    private void Awake()
    {
        allPaths = GameObject.FindGameObjectsWithTag("Path");
        for (int i = 0; i < allPaths.Length; ++i)
        {
            if (allPaths[i].name.Substring(0, 1) == "A")
            {
                targetpathForGroupA.Add(allPaths[i].transform);
            }

            else if(allPaths[i].name.Substring(0, 1) == "B")

            {
                targetpathForGroupB.Add(allPaths[i].transform);
            }
            else
            {
                targetpathForGroupC.Add(allPaths[i].transform);
            }
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
            inNavPos[Random.Range(0, inNavPos.Count)].position,
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

        if (allNavMeshAgents.Count == agentNum)
        {
            SetPathForEachGroup();
        }
    }

    public void SetNewTargetForGroupA(PathTriggerManager _trigger, string _name) 
    {
        for (int i = 0; i < navMeshAgentsGroupA.Count; ++i)
        {
            if (navMeshAgentsGroupA[i].name == _name)
            {
                navMeshAgentsGroupA[i].GetComponent<BakeZombie>().SetNewTarget(_trigger.NextPosForA());
            }
        }
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

    public void DestroyDoor()
    {
        if (rightBarricade.GetComponent<Barricade>().barricadeCollapse) { Debug.Log("RightB"); TargetPlayerB(); }
        if (leftBarricade.GetComponent<Barricade>().barricadeCollapse) TargetPlayerC();
    }

    public void ChangeWave()
    {
        for (int i = 0; i < navMeshAgentsGroupB.Count; ++i)
        {
            navMeshAgentsGroupB[i].GetComponent<BakeZombie>().SetNewTarget(rightBarricade.transform);
        }

        for (int i = 0; i < navMeshAgentsGroupC.Count; ++i)
        {
            navMeshAgentsGroupC[i].GetComponent<BakeZombie>().SetNewTarget(leftBarricade.transform);
        }
    }

    // public void ChangeWaveGroupA()
    // {
    //     for (int i = 0; i < navMeshAgentsGroupA.Count; ++i)
    //     {
    //         if (i % 2 == 0)
    //             navMeshAgentsGroupA[i].GetComponent<BakeZombie>().SetNewTarget(rightBarricade.transform);
    //         else
    //             navMeshAgentsGroupA[i].GetComponent<BakeZombie>().SetNewTarget(leftBarricade.transform);
    //     }
    // }

    private void TargetPlayerB()
    {
        for (int i = 0; i < navMeshAgentsGroupB.Count; ++i)
        {
            navMeshAgentsGroupB[i].GetComponent<BakeZombie>().SetNewTarget(playerTransform);
        }
    }
    private void TargetPlayerC()
    {
        for (int i = 0; i < navMeshAgentsGroupC.Count; ++i)
        {
            navMeshAgentsGroupC[i].GetComponent<BakeZombie>().SetNewTarget(playerTransform);
        }
    }

    private void SetPathForEachGroup()
    {

        for (int i = 0; i < navMeshAgentsGroupA.Count; ++i)
        {
            navMeshAgentsGroupA[i].GetComponent<BakeZombie>().SetNewTarget(targetpathForGroupA[0]);
        }

        for (int i = 0; i < navMeshAgentsGroupB.Count; ++i)
        {
            navMeshAgentsGroupB[i].GetComponent<BakeZombie>().SetNewTarget(targetpathForGroupB[0]);
        }

        for (int i = 0; i < navMeshAgentsGroupC.Count; ++i)
        {
            navMeshAgentsGroupC[i].GetComponent<BakeZombie>().SetNewTarget(targetpathForGroupC[0]);
        }
    }

    private void RemoveZombieFromList(BakeZombie zombie)
    {
        for (int i = 0; i < allNavMeshAgents.Count; ++i)
        {
            if (allNavMeshAgents[i].GetComponent<BakeZombie>() == zombie)
            {
                // allNavMeshAgents[i].GetComponent<BakeZombie>().SetNewTarget(playerTransform);
                // allNavMeshAgents[i].GetComponent<BakeZombie>().NoMoreMember();

                allNavMeshAgents.RemoveAt(i);
            }
        }
    }


    private void RemoveZombieFromGroupList(BakeZombie zombie)
    {
        if(zombie.name.Substring(zombie.name.Length - 1) == "A")
        {
            for (int i = 0; i < navMeshAgentsGroupA.Count; ++i)
            {
                if(navMeshAgentsGroupA[i].GetComponent<BakeZombie>() == zombie)
                {
                    navMeshAgentsGroupA.RemoveAt(i);
                }
            }
        }

        else if(zombie.name.Substring(zombie.name.Length - 1) == "B")
        {
            for (int i = 0; i < navMeshAgentsGroupB.Count; ++i)
            {

                if (navMeshAgentsGroupB[i].GetComponent<BakeZombie>() == zombie)
                {
                    navMeshAgentsGroupB.RemoveAt(i);
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
                }
            }
        }
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result) 

    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = inNavPos[Random.Range(0, inNavPos.Count - 1)].position;
        return false;
    }
}