using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingPath : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other)
    {
        if(_other.CompareTag("Zombie"))
        {
            _other.GetComponent<TrainingZombie>().ChangePath();
        }
    }
}
