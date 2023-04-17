using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 4f);
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Zombie"))
        {
            _other.GetComponent<BakeZombie>().Onfire();
        }
        if (_other.CompareTag("Target"))
        {
            _other.GetComponent<Target>().Onfire();
        }
    }
}