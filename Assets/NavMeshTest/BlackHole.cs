using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlackHole : MonoBehaviour
{
    private NavAgentManager navAgentManager = null;
    private List<GameObject> agents = new List<GameObject>();

    private float blackHoleRadius = 10f;

    private void Awake()
    {
        navAgentManager = FindObjectOfType<NavAgentManager>();
    }

    private void OnTriggerEnter(Collider _other)
    {
        // navAgentManager.DetectBlackHole(transform.position);
        if (_other.CompareTag("Zombie") || _other.CompareTag("Dead") || _other.CompareTag("Ragdoll"))
        {
            agents.Add(_other.gameObject);
        }
    }

    private void Update()
    {
        if (agents.Count == 0) return;

        for (int i = 0; i < agents.Count; ++i)
        {
            if (agents[i] == null) continue;
            if (agents[i].CompareTag("Zombie"))
            {
                agents[i].GetComponent<BakeZombie>().TakeDamage(0);
                agents[i].GetComponent<BakeZombie>().BlackHole();

                // if (Vector3.Distance(agents[i].transform.position, transform.position) < blackHoleRadius)
                {
                    Vector3 dir = transform.position - agents[i].transform.position;
                    agents[i].transform.position += dir * 3f * Time.deltaTime;
                }

            }
        }
    }

    private void OnDestroy()
    {
        for (int i = agents.Count - 1; i >= 0; --i)
        {
            if (agents[i] == null) continue;
            agents[i].GetComponent<BakeZombie>().TakeDamage(100);
            agents.RemoveAt(i);
        }
    }

}