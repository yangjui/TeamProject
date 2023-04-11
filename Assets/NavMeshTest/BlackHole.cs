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
        if (_other.CompareTag("Zombie") || _other.CompareTag("Dead"))
        {
            agents.Add(_other.gameObject);
        }
    }

    private void Update()
    {
        if(agents.Count > 0)
        {
            for (int i = 0; i < agents.Count; ++i)
            {
                if (agents[i].CompareTag("Zombie"))
                {
                    agents[i].GetComponent<BakeZombie>().HitByBlackHole(this.transform.position);
                    agents[i].GetComponent<BakeZombie>().TakeDamage(0);
                }
                else if (agents[i].CompareTag("Dead"))
                {
                    agents[i].GetComponent<DeadZombie>().HitByBlackHole(this.transform.position);
                }
            }
        }
    }


}