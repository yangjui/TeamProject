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

    private float resetTime = 11f;

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
           
            if(isInblackHole)
            {
                //Debug.Log(this.name + ": resetTime : " + resetTime);
                resetTime -= Time.deltaTime;
                if(resetTime <= 0)
                {
                    RayCheckGround();
                }
            }
            else
            {
                TrackPlayer();
            }
        }
    }


    private void TrackPlayer()
    {
        navAgent.SetDestination(player.GivePlayerPosition());
        Debug.Log("track Player");
    }



    private void InTheBlackHole()
    {
        if (Vector3.Distance(blackHolePosition, transform.position) < blackHoleRadius && isInblackHole == true)
        {
            Debug.Log(transform.name + ": is inblackHole");
            navAgent.enabled = false;
            Vector3 dir = blackHolePosition - transform.position;
            transform.position += dir * 3f * Time.deltaTime;  
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

    public void StopAnimation()
    {
        animator.enabled = false; // 땅에 떨어지는 순간으로 다시 애니메이션 작동하도록 만들어야함
    }

    public void ResetAgent() 
    {
        isInblackHole = false;
        navAgent.enabled = true;
        animator.enabled = true;
        resetTime = 10f;
    }

    public void DetectNewObstacle(Vector3 position)
    {
        if (Vector3.Distance(position, transform.position) < detectionRadius && Vector3.Distance(position, transform.position) > blackHoleRadius)
        { 
            navAgent.speed = 10f;
            navAgent.angularSpeed = 500f;
        }
    }

    private void RayCheckGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 0.1f))
        {
            ResetAgent();
        }
    }
}
