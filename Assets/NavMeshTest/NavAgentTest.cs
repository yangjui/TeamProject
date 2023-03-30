using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentTest : MonoBehaviour
{
    private NavMeshAgent navAgent = null;
    [SerializeField] NavTestPlayer player = null;

    private float maxSpeed = 2f;
    private float minSpeed = 5f;

    private float detectionRadius = 10f;
    private float blackHoleRadius = 7f;

    private bool isMember = true;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        if(!isMember)
        {
            navAgent.SetDestination(player.GivePlayerPosition());
        }
    }

    public void NoMoreMember()
    {
        isMember = false;
        navAgent.speed = Random.Range(minSpeed, maxSpeed);
    }

    public void HitByBlackHole(Vector3 position)
    {
       if (Vector3.Distance(position, transform.position) < blackHoleRadius)
       {
            Vector3 dir = position - transform.position;
            transform.position += dir * 10f * Time.deltaTime;
       }
    }

}
