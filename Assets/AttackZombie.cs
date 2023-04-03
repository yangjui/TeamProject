using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZombie : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other) // 다른 오브젝트의 이름!
    {
        if (_other.CompareTag("Player"))
        {
            _other.GetComponent<PlayerStatus>().DecreaseHP(5);
            Debug.Log(_other.GetComponent<PlayerStatus>().currentHp);
        }
    }
}
