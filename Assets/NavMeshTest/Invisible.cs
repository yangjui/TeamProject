using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisible : MonoBehaviour
{
    [SerializeField]
    private Animator animator = null;


    private void OnBecameInvisible()
    {
        Debug.Log("�Ⱥ���");
        animator.enabled = false;
    }

    private void OnBecameVisible()
    {
        Debug.Log("����");
        animator.enabled = true;
    }
}
