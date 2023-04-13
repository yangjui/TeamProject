using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeadZombie : MonoBehaviour
{
    public GameObject Ragdoll;

    private Rigidbody rb;
    private float onGroundTime = 0;
    private bool kinematic = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (rb.velocity.x <= 0.1f && rb.velocity.y <= 0.1f && rb.velocity.z <= 0.1f)
        {
            if (kinematic) return;
            Debug.Log("haha");
            onGroundTime += Time.deltaTime;
            if (onGroundTime >= 1f)
                Kinematic();
        }
        else
        {
            onGroundTime -= Time.deltaTime;
            Debug.Log("HUHU");
        }
    }

    public void SetPosition()
    {
        transform.position = transform.parent.position;
    }


    private void Kinematic()
    {
        Debug.Log("Dead");
        foreach (Rigidbody child in GetComponentsInChildren<Rigidbody>())
            child.isKinematic = true;

        Ragdoll.GetComponent<Ragdoll>().Kinematic();
        kinematic = true;
    }

    //private float resetTime = 10f;

    //private bool isInblackHole = false;

    //private float blackHoleRadius = 7f;

    //private Vector3 blackHolePosition;


    //private void Update()
    //{
    //    if (resetTime > 3)
    //    {
    //        InTheBlackHole();
    //    }

    //    if (isInblackHole)
    //    {
    //        //Debug.Log(transform.name + " : SetDestination" + target.name);
    //        resetTime -= Time.deltaTime;
    //        if (resetTime <= 0)
    //        {
    //            ResetAgent();
    //        }
    //    }

    //    if (resetTime <= 0)
    //    {
    //        resetTime = 10f;
    //    }
    //}

    //private void InTheBlackHole()
    //{
    //    if (Vector3.Distance(blackHolePosition, transform.position) < blackHoleRadius && isInblackHole == true)
    //    {
    //        Vector3 dir = blackHolePosition - transform.position;
    //        transform.position += dir * 3f * Time.deltaTime;
    //    }
    //}


    //public void HitByBlackHole(Vector3 position)
    //{
    //    blackHolePosition = position;
    //    isInblackHole = true;
    //}


    //private void ResetAgent()
    //{
    //    isInblackHole = false;
    //}
}