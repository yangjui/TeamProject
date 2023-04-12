using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeadZombie : MonoBehaviour
{

    private float resetTime = 10f;

    private bool isInblackHole = false;

    private float blackHoleRadius = 7f;

    private Vector3 blackHolePosition;


    private void Update()
    {
        if (resetTime > 3)
        {
            InTheBlackHole();
        }

        if (isInblackHole)
        {
            //Debug.Log(transform.name + " : SetDestination" + target.name);
            resetTime -= Time.deltaTime;
            if (resetTime <= 0)
            {
                ResetAgent();
            }
        }

        if (resetTime <= 0)
        {
            resetTime = 10f;
        }
    }

    private void InTheBlackHole()
    {
        if (Vector3.Distance(blackHolePosition, transform.position) < blackHoleRadius && isInblackHole == true)
        {
            Vector3 dir = blackHolePosition - transform.position;
            transform.position += dir * 3f * Time.deltaTime;
        }
    }


    public void HitByBlackHole(Vector3 position)
    {
        blackHolePosition = position;
        isInblackHole = true;
    }


    private void ResetAgent()
    {
        isInblackHole = false;
    }
}