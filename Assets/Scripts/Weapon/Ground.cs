using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField]
    private int damage = 10;
    [SerializeField]
    private float damageInterval = 1f;

    private float nextdamageTime;

    private void Start()
    {
        Destroy(gameObject, 4f);
    }

    private void OnTriggerStay(Collider _other)
    {
        if (_other.CompareTag("Zombie") && Time.time >= nextdamageTime)
        {
            _other.GetComponent<Zombie>().TakeDamage(damage);
            nextdamageTime = Time.time + damageInterval;
        }
    }
}