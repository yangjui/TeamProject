using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] // ĳ������Ʈ�ѷ��� ������ �޾���
public class PlayerMovement : MonoBehaviour
{
    private Vector3 moveForce;
    private float moveSpeed;
    private float jumpForce = 5f;
    private float gravity = -15f;

    private CharacterController cc;

    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = Mathf.Max(0, value); // �̵��ӵ��� ������ �������� �ʰ� ���Ѱɾ���.
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
