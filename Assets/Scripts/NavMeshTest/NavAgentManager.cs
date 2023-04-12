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
    private List<Transform> target = null;

    private Transform playerTransform;

    private List<NavMeshAgent> navMeshAgents = new List<NavMeshAgent>();

    private float maxSpeed = 2f;
    private float minSpeed = 5f;

    private float detectionRadius = 13f;
    private float blackHoleRadius = 7f;

    public void Init(Transform _position)
    {
        playerTransform = _position;
        for (int i = 0; i < agentNum; ++i)
        {
            int randomPrefab = Random.Range(0, agentPrefab.Count);
            NavMeshAgent newAgent = Instantiate(
            agentPrefab[randomPrefab],
            transform.position + new Vector3(Random.Range(-20f, 20f), 0, Random.Range(-20f, 20f) * agentNum * 0.002f),
            Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)),
            transform
            );

            newAgent.name = "Agent" + i;
            newAgent.GetComponent<BakeZombie>().SetNewTarget(target[0]);
            newAgent.GetComponent<BakeZombie>().PlayerPosition(playerTransform);
            newAgent.GetComponent<BakeZombie>().OnZombieFree2 += RemoveZombieFromList;
            navMeshAgents.Add(newAgent);

        }
    }

    public void PlayerPosition(Transform _position)
    {
        playerTransform = _position;
    }


    public void SetNewTarget(PathTrigger _trigger, string _name)
    {
        for (int i = 0; i < navMeshAgents.Count; ++i)
        {
            if (navMeshAgents[i].name == _name)
            {
                if (Vector3.Distance(navMeshAgents[i].GetComponent<BakeZombie>().CurDestination(), _trigger.PathPosition().position) < 1f)
                {
                    navMeshAgents[i].GetComponent<BakeZombie>().SetNewTarget(_trigger.NextPos());
                }
            }
        }
    }

    public List<NavMeshAgent> GetNavMeshAgents()
    {
        return navMeshAgents;
    }

    public void ResetAgent()
    {
        for (int i = 0; i < navMeshAgents.Count; ++i)
        {
            if (navMeshAgents[i].GetComponent<BakeZombie>().AgentSpeed() >= 10f)
            {
                navMeshAgents[i].GetComponent<BakeZombie>().SetNewTarget(target[Random.Range(0, target.Count - 1)]);
                navMeshAgents[i].GetComponent<BakeZombie>().GetSpeedByManager(Random.Range(minSpeed, maxSpeed));
                navMeshAgents[i].GetComponent<BakeZombie>().GetAngularSpeedByManager(120f);
            }
        }
    }


    public void DetectNewObstacle(Vector3 position)
    {
        for (int i = 0; i < navMeshAgents.Count; ++i)
        {
            if (Vector3.Distance(position, navMeshAgents[i].transform.position) < detectionRadius && Vector3.Distance(position, navMeshAgents[i].transform.position) > blackHoleRadius)
            {
                navMeshAgents[i].GetComponent<BakeZombie>().SetNewTarget(target[Random.Range(0, target.Count - 1)]);
                navMeshAgents[i].GetComponent<BakeZombie>().GetSpeedByManager(10f);
                navMeshAgents[i].GetComponent<BakeZombie>().GetAngularSpeedByManager(500f);

                //Debug.Log(navMeshAgents[i].name + "isFleeing");
            }
        }
    }

    private void RemoveZombieFromList(BakeZombie zombie)
    {
        for (int i = 0; i < navMeshAgents.Count; ++i)
        {
            if (navMeshAgents[i].GetComponent<BakeZombie>() == zombie)
            {
                navMeshAgents[i].GetComponent<BakeZombie>().SetNewTarget(playerTransform);
                navMeshAgents[i].GetComponent<BakeZombie>().PlayerPosition(playerTransform);
                navMeshAgents[i].GetComponent<BakeZombie>().NoMoreMember();
                //Debug.Log(navMeshAgents[i].name + "is No more manber");
                navMeshAgents.RemoveAt(i);
            }
        }
    }
}