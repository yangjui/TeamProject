using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class Player : MonoBehaviour
{
    [Header("Speed")]

    [SerializeField]
    private float normalSpeed = 5.0f;
    [SerializeField]
    private float walkSpeed = 5.0f;
    [SerializeField]
    private float runSpeed = 10.0f;
    [SerializeField]
    private float jump = 10.0f;
    [SerializeField]
    private float gravity = 20.0f;

    [Space(10f)]

    private CharacterController player = null;
    private Vector3 MoveDir = Vector3.zero;
    private Animator anim;
    private float mouseX = 0.0f;
    private float mouseXSpeed = 3.0f;
    private bool isWarp = false;


    void Awake()
    {
        player = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        PlayerMove();
        PlayerRotate();
    }

    void PlayerMove()
    {
        // if (Player.Position.y < 1.1)
        {
            MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            MoveDir = transform.TransformDirection(MoveDir.normalized);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                normalSpeed = runSpeed;
            }

            else
            {
                normalSpeed = walkSpeed;
            }

            MoveDir *= normalSpeed;

            if (Input.GetButton("Jump"))
            {
                MoveDir.y = jump;
            }

        }
        MoveDir.y -= gravity * Time.deltaTime;
        player.Move(MoveDir * Time.deltaTime);
    }

    void PlayerRotate()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseXSpeed;
        transform.eulerAngles = new Vector3(0, mouseX, 0);
    }
}