using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dead : MonoBehaviour
{
    private bool isInBlackHole = false;

    private float blackHoleRadius = 7f;

    private Vector3 blackHolePosition;


    private void Update()
    {

        if (isInBlackHole)
        {
            InTheBlackHole();
        }
    }

    private void InTheBlackHole()
    {
        if (Vector3.Distance(blackHolePosition, transform.position) < blackHoleRadius && isInBlackHole == true)
        {
            Vector3 dir = blackHolePosition - transform.position;
            transform.position += dir * 3f * Time.deltaTime;
        }
    }

    public void HitByBlackHole(Vector3 position)
    {
        blackHolePosition = position;
        isInBlackHole = true;
    }
}