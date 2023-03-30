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
    }

    private void OnBecameVisible()
    {
        animator.enabled = true;
    }

}
