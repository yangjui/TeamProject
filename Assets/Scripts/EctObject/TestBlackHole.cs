using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBlackHole : MonoBehaviour
{
    public float attractionRange = 10f;
    public float attractionForce = 10f;

    void FixedUpdate()
    {
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, attractionRange);

        foreach (Collider obj in objectsInRange)
        {
            Rigidbody objRigidbody = obj.GetComponent<Rigidbody>();

            if (objRigidbody != null)
            {
                Vector3 direction = transform.position - obj.transform.position;
                float distance = direction.magnitude;
                float forceMagnitude = attractionForce / distance;
                Vector3 force = direction.normalized * forceMagnitude;
                objRigidbody.AddForce(force);
            }
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Zombie"))
        {
            _other.GetComponent<Zombie>().TakeDamage(0);
        }
    }
}
