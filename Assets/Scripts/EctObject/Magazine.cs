using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            _other.GetComponent<WeaponSwitchingSystem>().IncreseMagazine(1);
            Destroy(gameObject);
        }
    }
}
