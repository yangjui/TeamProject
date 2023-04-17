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

    [SerializeField]
    private List<Transform> targetpathForGroupA = null;

    [SerializeField]
    private List<Transform> targetpathForGroupB = null;

    [SerializeField]
    private List<Transform> targetpathForGroupC = null;

    private Transform playerTransform;

    private List<NavMeshAgent> allNavMeshAgents = new List<NavMeshAgent>();
    private List<NavMeshAgent> navMeshAgentsGroupA = new List<NavMeshAgent>();
    private List<NavMeshAgent> navMeshAgentsGroupB = new List<NavMeshAgent>();
    private List<NavMeshAgent> navMeshAgentsGroupC = new List<NavMeshAgent>();

    private bool isAlarmON = false;
    private Vector3 instantPositionInNavArea;
    private Vector3 instantPosition;
    [SerializeField]
    private List<Transform> inNavPos = null;

    [SerializeField] private Barricade leftBarricade;
    [SerializeField] private Barricade rightBarricade;

    public void Init(Transform _position)
    {
        playerTransform = _position;

        for (int i = 0; i < agentNum; ++i)
        {
            if (RandomPoint(inNavPos[Random.Range(0, inNavPos.Count)].position + new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f) * agentNum * 0.002f), 1f, out instantPositionInNavArea))
            {
                instantPosition = instantPositionInNavArea;
            }

            int randomPrefab = Random.Range(0, agentPrefab.Count);
            NavMeshAgent newAgent = Instantiate(
            agentPrefab[randomPrefab],
            instantPosition,
            Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)),
            transform
            );

            if(i < agentNum/3)
            {
                newAgent.name = "Agent" + i + "GroupA";
                newAgent.GetComponent<BakeZombie>().SetNewTarget(targetpathForGroupA[0]);
                navMeshAgentsGroupA.Add(newAgent);
            }
            else if(agentNum/3 < i && i < ((agentNum/3) + (agentNum/3)))
            {
                newAgent.name = "Agent" + i + "GroupB";
                newAgent.GetComponent<BakeZombie>().SetNewTarget(targetpathForGroupB[0]);
                navMeshAgentsGroupB.Add(newAgent);
            }
            else
            {
                newAgent.name = "Agent" + i + "GroupC";
                newAgent.GetComponent<BakeZombie>().SetNewTarget(targetpathForGroupC[0]);
                navMeshAgentsGroupC.Add(newAgent);
            }

            newAgent.GetComponent<BakeZombie>().TargetPosition(playerTransform);
            newAgent.GetComponent<BakeZombie>().navAgent.enabled = false;
            newAgent.GetComponent<BakeZombie>().OnZombieFree2 += RemoveZombieFromList;
            newAgent.GetComponent<BakeZombie>().OnZombieFree2 += RemoveZombieFromGroupList;
            allNavMeshAgents.Add(newAgent);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            ChangeWave();

        if (Input.GetKeyDown(KeyCode.O))
            NavAgentOn();

        if (rightBarricade.GetComponent<Barricade>().barricadeCollapse)
        {
            for (int i = 0; i < navMeshAgentsGroupB.Count; ++i)
            {
                navMeshAgentsGroupB[i].GetComponent<BakeZombie>().SetNewTarget(playerTransform.transform);
                navMeshAgentsGroupB[i].GetComponent<BakeZombie>().TargetPosition(playerTransform.transform);
            }
            for (int i = 0; i < navMeshAgentsGroupC.Count; ++i)
            {
                navMeshAgentsGroupC[i].GetComponent<BakeZombie>().SetNewTarget(playerTransform.transform);
                navMeshAgentsGroupC[i].GetComponent<BakeZombie>().TargetPosition(playerTransform.transform);
            }
        }
    }

    public void NavAgentOn()
    {
        for (int i = 0; i < allNavMeshAgents.Count; ++i)
        {
            allNavMeshAgents[i].GetComponent<BakeZombie>().navAgent.enabled = true;
        }
    }

    public void ChangeWave()
    {
        Debug.Log("Door!");
        for (int i = 0; i < navMeshAgentsGroupB.Count; ++i)
        {
            navMeshAgentsGroupB[i].GetComponent<BakeZombie>().NoMoreMember();
            navMeshAgentsGroupB[i].GetComponent<BakeZombie>().SetNewTarget(rightBarricade.transform);
            navMeshAgentsGroupB[i].GetComponent<BakeZombie>().TargetPosition(rightBarricade.transform);
        }

        for (int i = 0; i < navMeshAgentsGroupC.Count; ++i)
        {
            navMeshAgentsGroupB[i].GetComponent<BakeZombie>().NoMoreMember();
            navMeshAgentsGroupC[i].GetComponent<BakeZombie>().SetNewTarget(leftBarricade.transform);
            navMeshAgentsGroupC[i].GetComponent<BakeZombie>().TargetPosition(leftBarricade.transform);
        }
    }

    // path�� ���޽� ���� path�� �����̶�� ���� ---------------------------------------------------------------------
    public void SetNewTargetForGroupA(PathTriggerManager _trigger, string _name) 
    {
        for (int i = 0; i < navMeshAgentsGroupA.Count; ++i)
        {
            navMeshAgentsGroupA[i].GetComponent<BakeZombie>().SetNewTarget(playerTransform);
        }
    }

    public void SetNewTargetForGroupB(PathTriggerManager _trigger, string _name)
    {
        for (int i = 0; i < navMeshAgentsGroupB.Count; ++i)
        {
            if (navMeshAgentsGroupB[i].name == _name)
            {
                if (Vector3.Distance(navMeshAgentsGroupB[i].GetComponent<BakeZombie>().CurDestination(), _trigger.PathPosition().position) < 1f)
                {
                    navMeshAgentsGroupB[i].GetComponent<BakeZombie>().SetNewTarget(_trigger.NextPosForB());
                }
            }
        }
    }

    public void SetNewTargetForGroupC(PathTriggerManager _trigger, string _name)
    {
        for (int i = 0; i < navMeshAgentsGroupC.Count; ++i)
        {
            if (navMeshAgentsGroupC[i].name == _name)
            {
                if (Vector3.Distance(navMeshAgentsGroupC[i].GetComponent<BakeZombie>().CurDestination(), _trigger.PathPosition().position) < 1f)
                {
                    navMeshAgentsGroupC[i].GetComponent<BakeZombie>().SetNewTarget(_trigger.NextPosForC());
                }
            }
        }
    }
    // path�� ���޽� ���� path�� �����̶�� ���� ---------------------------------------------------------------------��

    private void SetPathForEachGroup() // �˶��� �︮�� ������ �� �� �׷츶�� ù��° path�� trasform�˷��ֱ�;
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

    private void RemoveZombieFromList(BakeZombie zombie) // ��� ���� ����Ʈ���� �� �̻� �ɹ��� �ƴ� ���� ����
    {
        for (int i = 0; i < allNavMeshAgents.Count; ++i)
        {
            if (allNavMeshAgents[i].GetComponent<BakeZombie>() == zombie)
            {
                // allNavMeshAgents[i].GetComponent<BakeZombie>().SetNewTarget(playerTransform);
                allNavMeshAgents[i].GetComponent<BakeZombie>().NoMoreMember();
                allNavMeshAgents.RemoveAt(i);
            }
        }
    }

    private void RemoveZombieFromGroupList(BakeZombie zombie) //// �׷캰 ���� ����Ʈ���� �� �̻� �ɹ��� �ƴ� ���� ����
    {
        if(zombie.name.Substring(zombie.name.Length - 1) == "A")
        {
            for (int i = 0; i < navMeshAgentsGroupA.Count; ++i)
            {
                navMeshAgentsGroupA.RemoveAt(i);
            }
        }
        else if(zombie.name.Substring(zombie.name.Length - 1) == "B")
        {
            for (int i = 0; i < navMeshAgentsGroupB.Count; ++i)
            {
                navMeshAgentsGroupB.RemoveAt(i);
            }
        }
        else if (zombie.name.Substring(zombie.name.Length - 1) == "C")
        {
            for (int i = 0; i < navMeshAgentsGroupC.Count; ++i)
            {
                navMeshAgentsGroupC.RemoveAt(i);
            }
        }
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result) //���� �׺�Ž� ���ο��� ��ȯ
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