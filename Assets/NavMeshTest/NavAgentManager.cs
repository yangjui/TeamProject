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
    private List<Transform> paths = null;

    private List<NavMeshAgent> navMeshAgents = new List<NavMeshAgent>();

    private float maxSpeed = 2f;
    private float minSpeed = 5f;

    private float detectionRadius = 10f;
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

            newAgent.speed = Random.Range(minSpeed, maxSpeed);
            newAgent.SetDestination(paths[0].position);
            newAgent.name = "Agent" + i;
            navMeshAgents.Add(newAgent);
        }
    }
    
    public void SetNewTartget(PathTrigger trigger , string name)
    {
        for (int i = 0; i < navMeshAgents.Count; ++i)
        {
            if (navMeshAgents[i].name == name)
            {
                if (Vector3.Distance(navMeshAgents[i].destination, trigger.pathPosition()) < 1f)
                {
                    navMeshAgents[i].SetDestination(trigger.nextPos());
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
            if (navMeshAgents[i].speed >= 10f)
            {
                navMeshAgents[i].SetDestination(paths[Random.Range(0, paths.Count - 1)].position);
                navMeshAgents[i].speed = Random.Range(minSpeed, maxSpeed);
                navMeshAgents[i].angularSpeed = 120f;
            }
        }
    }

    public void DetectNewObstacle(Vector3 position)
    {
        for (int i = 0; i < navMeshAgents.Count; ++i)
        {
            if (Vector3.Distance(position, navMeshAgents[i].transform.position) < detectionRadius && Vector3.Distance(position, navMeshAgents[i].transform.position) > blackHoleRadius)
            {
                Debug.Log(navMeshAgents[i].name + "Fleeing Active");
                navMeshAgents[i].SetDestination(paths[Random.Range(0, paths.Count - 1)].position);
                navMeshAgents[i].speed = 10f;
                navMeshAgents[i].angularSpeed = 500f;
            }
        }
    }

    public void DetectBlackHole(Vector3 position)
    {
        Debug.Log("Active");
        for (int i = 0; i < navMeshAgents.Count; ++i)
        {
            if (Vector3.Distance(position, navMeshAgents[i].transform.position) < blackHoleRadius)
            {
                //navMeshAgents[i].ResetPath();
                Vector3 dir = position - navMeshAgents[i].transform.position;
                navMeshAgents[i].transform.position += dir * 10f * Time.deltaTime;
            }
        }
    }



    //private void ResetPath()
    //{
    //    for (int i = 0; i < navMeshAgents.Count; ++i)
    //    {
    //        Vector3 oldDestination = navMeshAgents[i].destination;
    //        navMeshAgents[i].SetDestination(oldDestination);
    //    }
    //}
}
