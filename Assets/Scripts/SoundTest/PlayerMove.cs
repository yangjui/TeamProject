using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private Rigidbody rb;
    private float speed = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Moving();
    }

    private void Moving()
    {
        rb.velocity = new Vector3(horizontal * speed, 0f, vertical * speed );
    }
}
