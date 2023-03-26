using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("# Input keyCodes")]
    [SerializeField] private KeyCode keyCodeRun = KeyCode.LeftShift;
    [SerializeField] private KeyCode keyCodeJump = KeyCode.Space;
    

    private PlayerRotate playerRotate;
    private PlayerMovement playerMovement;
    private PlayerStatus playerStatus;
    private PlayerAnimationController playerAnim;
    private WeaponAssaultRifle weapon;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerRotate = GetComponent<PlayerRotate>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStatus = GetComponent<PlayerStatus>();
        playerAnim = GetComponent<PlayerAnimationController>();
        weapon = GetComponentInChildren<WeaponAssaultRifle>();
    }

    private void Update()
    {
        Rotate();
        Move();
        Jump();
        WeaponAction();
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        playerRotate.Rotate(mouseX, mouseY);
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x != 0 || z != 0)
        {
            bool isRun = false;
            if (z > 0) isRun = Input.GetKey(keyCodeRun); // 앞으로 갈때만 대쉬키가 활성화됨.

            playerMovement.MoveSpeed = isRun ? playerStatus.RunSpeed : playerStatus.WalkSpeed; // 달리고있다면 RunSpeed, 아니라면 WalkSpeed
            playerAnim.MoveSpeed = isRun ? 1 : 0.5f; // 0이면 Idle 0.5면 Walk, 1이면 Run 애니메이션 재생됨.
        }
        else // 입력값이 없다 = 멈췄다.
        {
            playerMovement.MoveSpeed = 0f;
            playerAnim.MoveSpeed = 0f;
        }

        playerMovement.MoveToDir(new Vector3(x, 0, z));
    }

    private void Jump()
    {
        if (Input.GetKeyDown(keyCodeJump))
        {
            playerMovement.Jump();
        }
    }

    private void WeaponAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            weapon.StartWeaponAction();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            weapon.StopWeaponAction();
        }
    }
}
