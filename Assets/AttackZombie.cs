using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZombie : MonoBehaviour
{
    private bool isColliding = false; // �浹 ���θ� ������ ����

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // isColliding ������ false�� ��쿡�� ó��
        {
            other.GetComponent<PlayerController>().TakeDamage(5);
            Debug.Log(other.GetComponent<PlayerStatus>().currentHp);
            //isColliding = true; // �浹 ���¸� true�� ����
            //StartCoroutine(ResetCollisionState()); // �浹 ���¸� 0.1�� �ڿ� false�� ����
        }
    }

    //IEnumerator ResetCollisionState()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    isColliding = false;
    //}
}
