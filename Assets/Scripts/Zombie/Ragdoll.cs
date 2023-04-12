using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ragdoll : MonoBehaviour
{
    private Rigidbody rb;
    private float onGroundTime = 0;

    [SerializeField] private GameObject ragdoll;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        Invoke("Kinematic", 3f);
        Invoke("ColliderPosition", 0.8f);
    }

    private void ColliderPosition()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    //private void Update()
    //{
    //    if (rb.IsSleeping())
    //    {
    //        onGroundTime += Time.deltaTime;
    //        if (onGroundTime >= 3f)
    //            Kinematic();
    //    }
    //    else
    //        onGroundTime = 0f;
    //}

    private void Kinematic()
    {
        foreach (Rigidbody childRb in GetComponentsInChildren<Rigidbody>())
        {
            childRb.isKinematic = true;
        }
    }

    private void DeadPosition(Transform _newRagdoll, Transform _dead)
    {
        for (int i = 0; i < _newRagdoll.transform.childCount; ++i)
        {
            if (_newRagdoll.transform.childCount != 0)
                DeadPosition(_newRagdoll.transform.GetChild(i), _dead.transform.GetChild(i));

            _dead.transform.GetChild(i).localPosition = _newRagdoll.transform.GetChild(i).localPosition;
            _dead.transform.GetChild(i).localRotation = _newRagdoll.transform.GetChild(i).localRotation;
        }
        _dead.transform.position = _newRagdoll.transform.position;
    }
}