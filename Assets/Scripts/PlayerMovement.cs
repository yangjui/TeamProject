using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed;
    private Vector3 moveForce;

    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;



    private CharacterController cc;

    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value); // �̵��ӵ��� ������ �������� �ʰ� ���Ѱɾ���.
        get => moveSpeed;
    }

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Gravity();
    }

    private void Gravity()
    {
        if (!cc.isGrounded)
        {
            moveForce.y += gravity * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        cc.Move(moveForce * Time.deltaTime);
    }

    public void MoveToDir(Vector3 direction)
    {
        // �̵� ���� = ĳ������ ȸ���� + ���Ⱚ
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);
        // �̵� �� = �̵����� * �ӵ�, y���� velocity.y ������.
        moveForce = new Vector3(direction.x * moveSpeed, moveForce.y, direction.z * moveSpeed);
    }

    public void Jump()
    {
        if (cc.isGrounded)
        {
            moveForce.y = jumpForce;
        }
    }
}
