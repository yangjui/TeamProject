using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class fire : MonoBehaviour
{
    [SerializeField]
    private GameObject fireEffectPrefab;
    [SerializeField]
    private int damage = 30;
    [SerializeField]
    private float damageInterval = 1f;

    private bool isFireOn = false;
    private GameObject fireEffect;
    private float nextdamageTime;

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Floor"))
        {

        }
    }
}
