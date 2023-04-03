using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentTest : MonoBehaviour
{
    private NavMeshAgent navAgent = null;
    private NavTestPlayer player = null;

    private Animator animator = null;

    private float maxSpeed = 2f;
    private float minSpeed = 5f;

    private float blackHoleRadius = 7f;
    private float detectionRadius = 10f;


    private bool isMember = true;
    private bool isInblackHole = false;

    private Vector3 blackHolePosition;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<NavTestPlayer>();
    }


    private void Update()
    {
        if(!isMember)
        {
            InTheBlackHole();
            TrackPlayer();
        }
    }

    private void OnBecameInvisible()
    {
        animator.enabled = false;
    }

    private void OnBecameVisible()
    {
        animator.enabled = true;
    }

    private void TrackPlayer()
    {
        if(isInblackHole) return;
        navAgent.SetDestination(player.GivePlayerPosition());
    }

    private void InTheBlackHole()
    {
        if (Vector3.Distance(blackHolePosition, transform.position) < blackHoleRadius && isInblackHole == true)
        {
            navAgent.enabled = false;
            Vector3 dir = blackHolePosition - transform.position;
            transform.position += dir * 3f * Time.deltaTime;
            //transform.RotateAround(blackHolePosition, dir, 3f * Time.deltaTime);
        }
    }

    public void NoMoreMember()
    {
        isMember = false;
        navAgent.speed = Random.Range(minSpeed, maxSpeed);
    }

    public void HitByBlackHole(Vector3 position)
    {
        blackHolePosition = position;
        isInblackHole = true;
    }

    public void ResetAgent()
    {
        isInblackHole = false;
        navAgent.enabled = true;
    }

    public void DetectNewObstacle(Vector3 position)
    {
        if (Vector3.Distance(position, transform.position) < detectionRadius && Vector3.Distance(position, transform.position) > blackHoleRadius)
        { 
            navAgent.speed = 10f;
            navAgent.angularSpeed = 500f;
        }
    }
}
