using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZombie : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            _other.GetComponent<PlayerController>().TakeDamage(5);
            this.gameObject.SetActive(false);
        }

        else if (_other.CompareTag("Barricade"))
        {
            _other.GetComponent<Barricade>().BarricadeHP(5);
            this.gameObject.SetActive(false);
        }
    }
}
