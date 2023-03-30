using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlackHole : MonoBehaviour
{
    private NavAgentManager navAgentManager = null;

    private void Awake()
    {
        navAgentManager = FindObjectOfType<NavAgentManager>();
    }




    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider.CompareTag("Agent") || collision.collider.CompareTag("Map"))
        {
            navAgentManager.DetectBlackHole(transform.position);
        }
    }


}
