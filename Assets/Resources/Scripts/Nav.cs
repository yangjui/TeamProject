using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Navigation ���� ����� ����� �� �ʿ�.
using UnityEngine.AI;

public class Nav : MonoBehaviour
{
    // ��ǥ ����
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