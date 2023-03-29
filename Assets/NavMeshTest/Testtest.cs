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
        Debug.Log("안보여");
    }

    private void OnBecameVisible()
    {
        animator.enabled = true;
        Debug.Log("잘보여");
    }

}
