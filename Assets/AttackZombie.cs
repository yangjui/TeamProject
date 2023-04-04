using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZombie : MonoBehaviour
{
    private bool isColliding = false; // 충돌 여부를 저장할 변수

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // isColliding 변수가 false인 경우에만 처리
        {
            other.GetComponent<PlayerController>().TakeDamage(5);
            Debug.Log(other.GetComponent<PlayerStatus>().currentHp);
            //isColliding = true; // 충돌 상태를 true로 변경
            //StartCoroutine(ResetCollisionState()); // 충돌 상태를 0.1초 뒤에 false로 변경
        }
    }

    //IEnumerator ResetCollisionState()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    isColliding = false;
    //}
}
