using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZombie : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other) // �ٸ� ������Ʈ�� �̸�!
    {
        if (_other.CompareTag("Player"))
            Debug.Log("PLAYER HIT!!!!!!!!!!!!!!!!");

        // �÷��̾� ü�� ���� �Լ����ٰ� ������ �ֱ� @@(ȿ��)
        // ex) PlayerHealth(zombieAttackDamage);
    }
}
