using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testtest : MonoBehaviour
{
    [SerializeField]
    private Animator animator = null;

    private void OnBecameInvisible()
    {
        animator.enabled = false;
        Debug.Log("�Ⱥ���");
    }

    private void OnBecameVisible()
    {
        animator.enabled = true;
        Debug.Log("�ߺ���");
    }

}
