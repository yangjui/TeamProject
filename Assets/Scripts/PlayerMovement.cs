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
        set => moveSpeed = Mathf.Max(0, value); // 이동속도가 음수로 내려가지 않게 제한걸어줌.
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
        // 이동 방향 = 캐릭터의 회전값 + 방향값
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);
        // 이동 힘 = 이동방향 * 속도, y축은 velocity.y 같은것.
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
