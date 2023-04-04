using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentManager : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agentPrefab = null;

    [Range(50, 1000)]
    [SerializeField] private int agentNum;

    [SerializeField]
    private List<Transform> target = null;

    [SerializeField]
    private Transform playerTrasform; // 씬매니저로 부터 받아와야함

    private List<NavMeshAgent> navMeshAgents = new List<NavMeshAgent>();

    private float maxSpeed = 2f;
    private float minSpeed = 5f;

    private float detectionRadius = 13f;
    private float blackHoleRadius = 7f;

    private void Awake()
    {
        for (int i = 0; i < agentNum; ++i)
        {
            NavMeshAgent newAgent = Instantiate(
            agentPrefab,
            new Vector3(Random.Range(-20f, 20f), 0, Random.Range(-20f, 20f) * agentNum * 0.002f),
            Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)),
            transform
            );

            newAgent.name = "Agent" + i;
            newAgent.GetComponent<NavAgentTest>().SetNewTarget(target[0]);
            navMeshAgents.Add(newAgent);
        }
    }
    
    public void SetNewTartget(PathTrigger trigger , string name)
    {
        for (int i = 0; i < navMeshAgents.Count; ++i)
        {
            if (navMeshAgents[i].name == name)
            {
                if (Vector3.Distance(navMeshAgents[i].GetComponent<NavAgentTest>().curdestination(), 
                    trigger.pathPosition().position) < 1f)
                {
                    navMeshAgents[i].GetComponent<NavAgentTest>().SetNewTarget(trigger.nextPos());
                }
            }
        }
    }




    public List<NavMeshAgent> GetNavMeshAgents()
    {
        return navMeshAgents;
    }

    public void ResetAgnet()
    {
        for (int i = 0; i < navMeshAgents.Count; ++i)
        {
            if (navMeshAgents[i].GetComponent<NavAgentTest>().AgentSpeed() >= 10f)
            {
                navMeshAgents[i].GetComponent<NavAgentTest>().SetNewTarget(target[Random.Range(0, target.Count - 1)]);
                navMeshAgents[i].GetComponent<NavAgentTest>().GetSpeepByManager(Random.Range(minSpeed, maxSpeed));
                navMeshAgents[i].GetComponent<NavAgentTest>().GetAngularSpeedByManager(120f);
            }
        }
    }


    public void DetectNewObstacle(Vector3 position)
    {
        for (int i = 0; i < navMeshAgents.Count; ++i)
        {
            if (Vector3.Distance(position, navMeshAgents[i].transform.position) < detectionRadius && Vector3.Distance(position, navMeshAgents[i].transform.position) > blackHoleRadius)
            {
                navMeshAgents[i].GetComponent<NavAgentTest>().SetNewTarget(target[Random.Range(0, target.Count - 1)]);
                navMeshAgents[i].GetComponent<NavAgentTest>().GetSpeepByManager(10f);
                navMeshAgents[i].GetComponent<NavAgentTest>().GetAngularSpeedByManager(500f);

                //Debug.Log(navMeshAgents[i].name + "isFleeing");
            }
        }
    }   


    public void DetectBlackHole(Vector3 position)
    {
        //Debug.Log("Active");
        for (int i = 0; i < navMeshAgents.Count; ++i)
        {
            if (Vector3.Distance(position, navMeshAgents[i].transform.position) < blackHoleRadius)
            {
                navMeshAgents[i].GetComponent<NavAgentTest>().SetNewTarget(playerTrasform);
                navMeshAgents[i].GetComponent<NavAgentTest>().NoMoreMember();
                //Debug.Log(navMeshAgents[i].name + "is No more manber");
                navMeshAgents.RemoveAt(i);
            }
        }
    }

    


}
