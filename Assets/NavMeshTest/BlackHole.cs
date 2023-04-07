using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlackHole : MonoBehaviour
{
    private NavAgentManager navAgentManager = null;
    private List<GameObject> agents = new List<GameObject>();

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
            if (agents[i].gameObject != null)
            {
                if (agents[i].CompareTag("Zombie"))
                {
                    if (agents[i].GetComponent<Zombie>().CurrentHealth() == 0)
                    {
                        agents.RemoveAt(i);
                    }
                    agents[i].GetComponent<Zombie>().HitByBlackHole(this.transform.position);
                    agents[i].GetComponent<Zombie>().TakeDamage(0);
                }
                else if (agents[i].CompareTag("Dead"))
                {
                    agents[i].GetComponent<Dead>().HitByBlackHole(this.transform.position);
                }
                else if (agents[i].CompareTag("Ragdoll"))
                {
                    agents[i].GetComponent<Ragdoll>().HitByBlackHole(this.transform.position);
                }
            }
        }
    }

    private void OnDestroy()
    {
        navAgentManager.ResetAgent();

        for (int i = 0; i < agents.Count; ++i)
        {
            if (agents[i].gameObject != null)
            {
                if (agents[i].CompareTag("Zombie"))
                {
                    if (agents[i].GetComponent<Zombie>().CurrentHealth() == 0)
                        agents.RemoveAt(i);
                    agents[i].GetComponent<Zombie>().ResetAgent();
                }
            }
        }
    }

}