using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Navigation 관련 기능을 사용할 때 필요.
using UnityEngine.AI;

public class Nav : MonoBehaviour
{
    // 목표 지점
    public Transform target;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        agent.SetDestination(target.position);

        if (Input.GetKeyDown(KeyCode.Space)){
            agent.isStopped = true;
        }
    }
}