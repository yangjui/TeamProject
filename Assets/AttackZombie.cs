using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZombie : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other) // 다른 오브젝트의 이름!
    {
        if (_other.CompareTag("Player"))
            Debug.Log("PLAYER HIT!!!!!!!!!!!!!!!!");

        // 플레이어 체력 관련 함수에다가 데미지 주기 @@(효석)
        // ex) PlayerHealth(zombieAttackDamage);
    }
}
