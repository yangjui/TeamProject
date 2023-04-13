using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private GameObject pelvis = null;
    private Rigidbody rb;
    private float onGroundTime = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //if (rb.velocity == Vector3.zero)
        //{
        //    Debug.Log("haha");
        //    onGroundTime += Time.deltaTime;
        //    if (onGroundTime >= 3f)
        //        Kinematic();
        //}
        //else
        //{ 
        //    onGroundTime = 0f;
        //    Debug.Log("HUHU");
        //}
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Platform"))
        {
            ColliderPosition();
        }
    }

    private void ColliderPosition()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void Kinematic()
    {
        //Debug.Log("Dead");
        //foreach (Rigidbody child in GetComponentsInChildren<Rigidbody>())
        //    child.isKinematic = true;

        transform.position = pelvis.transform.position;
        pelvis.GetComponent<DeadZombie>().SetPosition();
    }
}