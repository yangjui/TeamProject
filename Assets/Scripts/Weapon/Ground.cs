using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 4f);
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Zombie"))
        {
            _other.GetComponent<Target>().Onfire();
        }

        if (_other.CompareTag("Target"))
        {
            _other.GetComponent<Target>().Onfire();
        }
    }
}