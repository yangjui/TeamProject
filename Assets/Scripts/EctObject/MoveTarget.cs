using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float moveSpeed;

    private bool movingToA = false;

    private void Start()
    {
        transform.position = pointA.position;
    }

    private void Update()
    {
        if (movingToA)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointA.position, moveSpeed * Time.deltaTime);
            if (transform.position == pointA.position)
            {
                movingToA = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pointB.position, moveSpeed * Time.deltaTime);
            if (transform.position == pointB.position)
            {
                movingToA = true;
            }
        }
    }
}
