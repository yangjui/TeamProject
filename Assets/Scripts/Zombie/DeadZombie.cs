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
            onGroundTime += Time.deltaTime;
            if (onGroundTime >= 1f)
                Kinematic();
        }
        else
        {
            onGroundTime -= Time.deltaTime;
        }
    }

    public void SetPosition()
    {
        transform.position = transform.parent.position;
    }


    private void Kinematic()
    {
        foreach (Rigidbody child in GetComponentsInChildren<Rigidbody>())
            child.isKinematic = true;

        Ragdoll.GetComponent<Ragdoll>().Kinematic();
        kinematic = true;
    }
}