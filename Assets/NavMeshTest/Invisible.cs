using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisible : MonoBehaviour
{
    [SerializeField]
    private Animator animator = null;


    private void OnBecameInvisible()
    {
        Debug.Log("안보임");
        animator.enabled = false;
    }

    private void OnBecameVisible()
    {
        Debug.Log("보임");
        animator.enabled = true;
    }
}
