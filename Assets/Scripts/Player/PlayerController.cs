using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("# Input keyCodes")]
    [SerializeField] private KeyCode keyCodeRun = KeyCode.LeftShift;
    [SerializeField] private KeyCode keyCodeJump = KeyCode.Space;
    [SerializeField] private KeyCode KeyCodeReload = KeyCode.R;

    private PlayerRotate playerRotate;
    private PlayerMovement playerMovement;
    private PlayerStatus playerStatus;
    private WeaponBase weapon;

    private bool isAimMode = false;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerRotate = GetComponent<PlayerRotate>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStatus = GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;
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

            if (!isAimMode)
            {
                playerMovement.MoveSpeed = isRun ? playerStatus.RunSpeed : playerStatus.WalkSpeed; // 달리고있다면 RunSpeed, 아니라면 WalkSpeed
                weapon.Animator.MoveSpeed = isRun ? 1 : 0.5f; // 0이면 Idle 0.5면 Walk, 1이면 Run 애니메이션 재생됨.
            }
            else if (isAimMode)
            {
                playerMovement.MoveSpeed = (isRun ? playerStatus.RunSpeed : playerStatus.WalkSpeed) * 0.5f; // 달리고있다면 RunSpeed, 아니라면 WalkSpeed
            }
        }
        else // 입력값이 없다 = 멈췄다.
        {
            playerMovement.MoveSpeed = 0f;
            weapon.Animator.MoveSpeed = 0f;
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

        if (Input.GetMouseButtonDown(1))
        {
            weapon.StartWeaponAction(1);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            weapon.StopWeaponAction(1);
        }
        if (Input.GetKeyDown(KeyCodeReload))
        {
            weapon.StartReload();
        }
    }

    public void SwitchingWeapon(WeaponBase _newWeapon)
    {
        weapon = _newWeapon;
    }

    public void ChangeAimMode(bool _bool)
    {
        isAimMode = _bool;
    }

    public void TakeDamage(float _damage)
    {
        bool isDead = playerStatus.DecreaseHP(_damage);
        if (isDead)
        {
            // 사망 애니메이션 => 이건 한번만 나와야함. 여러번나오지않게.
            // 사망 사운드
            // Restart? UI
            // 죽었다고 콜백 => 게임매니저한테 죽었다고 알려주고 게임매니저가 EnemyManager한테 플레이어 죽었으니 다른 타겟으로 바꾸라던지 명령해줌. 이건 해도되고 안해도됨
        }
    }
}
