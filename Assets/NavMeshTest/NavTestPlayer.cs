using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] // 컴포넌트 자동 추가!

public class NavTestPlayer : MonoBehaviour
{
    [Header("Speed")]

    [SerializeField]
    private float normalSpeed = 5.0f;  // 기본 스피드
    [SerializeField]
    public float walkSpeed = 5.0f;  // 걷기
    [SerializeField]
    public float runSpeed = 10.0f; // 달리기
    [SerializeField]
    private float jump = 10.0f; // 점프
    [SerializeField]
    private float gravity = 9.81f;

    [SerializeField]
    private GameObject grenade = null;

    private bool isGrenadeRady = false;

    [Space(10f)]

    private CharacterController player = null;
    private Vector3 MoveDir = Vector3.zero;
    private float mouseX = 0.0f;
    private float mouseXSpeed = 3.0f;


    private void Awake()
    {
        player = GetComponent<CharacterController>();
    }

    private void Update()
    {
        PlayerMove();
        PlayerRotate();
    }

    private void PlayerMove()
    {

            MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical") );
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

            if(Input.GetKey(KeyCode.Alpha1))
            {
                SetGrenade();
            }

            if(Input.GetMouseButtonDown(0))
            {
                ThrowGrenade();
            }

            MoveDir.y -= gravity * Time.deltaTime;
            player.Move(MoveDir * Time.deltaTime);

    }

    private void PlayerRotate()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseXSpeed;
        transform.eulerAngles = new Vector3(0, mouseX, 0);
    }

    public Vector3 GivePlayerPosition()
    {
        return transform.position;
    }

    private void SetGrenade()
    {
        isGrenadeRady = true;
    }

    private void ThrowGrenade()
    {
        if (!isGrenadeRady) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;

        if(Physics.Raycast(ray, out rayHit, 100f))
        {
            Vector3 nextVec = rayHit.point - transform.position;
            nextVec.y = 1f;

            GameObject grenadeOBJ = Instantiate(grenade, transform.position, player.transform.rotation);
            grenadeOBJ.GetComponent<BlackHoleGrenade>().grenadeRigidbody(nextVec);
        }
    }
}