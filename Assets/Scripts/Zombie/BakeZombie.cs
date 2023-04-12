using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BakeZombie : MonoBehaviour
{
    [Header("# Zombie")]
    [SerializeField] private GameObject deadRagdoll;
    [SerializeField] private GameObject leftZombiePrefab;
    [SerializeField] private GameObject rightZombiePrefab;
    [SerializeField] private GameObject attack;
    [SerializeField] private float zombieHealth = 100f;

    [System.NonSerialized] public NavMeshAgent navAgent;

    private int deadType = 0;

    private float currentHealth;


    private float blackHoleRadius = 7f;
    private float detectionRadius = 10f;

    private bool isMember = true;
    private bool isInBlackHole = false;

    private bool isAttack = false;
    private bool isIdle = false;
    private bool isRun = false;
    private bool isWalk = false;
    private bool isCoroutineRunning = false;


    private Vector3 blackHolePosition;

    private float distance = 0f;

    private Transform target;
    private Transform playerPosition;
    private Animator anim;

    public delegate void ZombieFreeEventHandler(BakeZombie zombie);
    public ZombieFreeEventHandler OnZombieFree2;

    private Rigidbody rb;

    [SerializeField] private float curSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float walkSpeed = 5f;


    [SerializeField] private MeshRenderer bodyMr = null;
    [SerializeField] private MeshRenderer clothesMr = null;
    [SerializeField] private List<Material> bodyAnimMate = null;
    [SerializeField] private List<Material> clothesAnimMate = null;
    private List<Material> newBodyAnimMate = new List<Material>();
    private List<Material> newClothesAnimMate = new List<Material>();

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentHealth = zombieHealth;
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        for (int i = 0; i < bodyAnimMate.Count; ++i)
        {
            newBodyAnimMate.Add(bodyAnimMate[i]);
            newClothesAnimMate.Add(clothesAnimMate[i]);
        }

        for (int i = 0; i < newBodyAnimMate.Count; ++i)
        {
            float randomAnimSpeed = Random.Range(1.2f, 1.5f);
            newBodyAnimMate[i].SetFloat("_Length", randomAnimSpeed);
            newClothesAnimMate[i].SetFloat("_Length", randomAnimSpeed);
        }
    }

    private void Update()
    {
        if (!isMember)
        {
            ChasePlayer(target.position);
        }

        else
        {
            navAgent.SetDestination(target.position);
        }

        if (currentHealth <= 0)
        {
            Dead();
        }

        if (isInBlackHole)
        {
            //newBodyAnimMate[i].SetFloat("_Length", 2f);
            //newClothesAnimMate[i].SetFloat("_Length", 2f);
        }
        
        distance = Vector3.Distance(transform.position, playerPosition.position);

        ZombieState();
    }

    public void ChasePlayer(Vector3 _target) // vellocity
    {
        Vector3 direction = (_target - transform.position).normalized;
        Vector3 playerLookAt = new Vector3(_target.x, transform.position.y, _target.z);
        rb.MovePosition(transform.position + direction * curSpeed * Time.deltaTime);
        transform.LookAt(playerLookAt);
    }


    public void PlayerPosition(Transform _playerPosition)
    {
        playerPosition = _playerPosition;
    }


    private void ZombieState()
    {
        switch (Mathf.Round(distance * 10f) / 10f)
        {
            case >= 15.0f:
                if (!isWalk) Walk();
                break;
            case < 15.0f and >= 2.0f:
                if (!isRun) Run();
                break;
            case < 2.0f:
                if (!isAttack && !isIdle && !isCoroutineRunning) StartCoroutine(Idle());
                break;
            default:
                return;
        }
    }

    private void Dead()
    {
        this.gameObject.SetActive(false);
        GameObject newRagdoll;

        switch (deadType)
        {
            case 1:
                newRagdoll = Instantiate(leftZombiePrefab, transform.position, transform.rotation);
                break;
            case 2:
                newRagdoll = Instantiate(rightZombiePrefab, transform.position, transform.rotation);
                break;
            default:
                newRagdoll = Instantiate(deadRagdoll, transform.position, transform.rotation);
                break;
        }
        RagdollPosition(this.transform, newRagdoll.transform);

        Destroy(this.gameObject);
    }

    public void DeadType(int _type)
    {
        deadType = _type;
    }

    // 렉돌처리 안 할거면 굳이 포지션을 맞출 필요는 없을듯!
    private void RagdollPosition(Transform _alive, Transform _dead)
    {
        for (int i = 0; i < _alive.transform.childCount; ++i)
        {
            if (_alive.transform.childCount != 0)
                RagdollPosition(_alive.transform.GetChild(i), _dead.transform.GetChild(i));

            _dead.transform.GetChild(i).localPosition = _alive.transform.GetChild(i).localPosition;
            _dead.transform.GetChild(i).localRotation = _alive.transform.GetChild(i).localRotation;
        }
        _dead.transform.position = _alive.transform.position; // 렉돌 자체의 위치 맞추기
    }

    public void NoMoreMember()
    {
        isMember = false;
        navAgent.enabled = false;
    }

    public void BlackHole()
    {
        if (!isInBlackHole)
        {
            isInBlackHole = true;
        }
    }

    //public void StopAnimation() // 원한다면 멈추기
    //{
    //    anim.enabled = false; // 땅에 떨어지는 순간으로 다시 애니메이션 작동하도록 만들어야함
    //}

    public void DetectNewObstacle(Vector3 _position)
    {
        if (Vector3.Distance(_position, transform.position) < detectionRadius && Vector3.Distance(_position, transform.position) > blackHoleRadius)
        {
            navAgent.speed = 15f;
            navAgent.angularSpeed = 500f;
        }
    }

    public void ResetAgent()
    {
        isInBlackHole = false;
    }

    public Vector3 CurDestination()
    {
        return navAgent.destination;
    }

    public void SetNewTarget(Transform _newTarget)
    {
        target = _newTarget;
    }

    public float AgentSpeed()
    {
        return navAgent.speed;
    }

    public void GetSpeedByManager(float _speed)
    {
        navAgent.speed = _speed;
    }

    public void GetAngularSpeedByManager(float _speed)
    {
        navAgent.angularSpeed = _speed;
    }

    private IEnumerator Attack()
    {
        if (!isAttack)
        {
            isCoroutineRunning = true;

            Debug.Log(this.name + "Attack");

            attack.SetActive(true);
            bodyMr.material = newBodyAnimMate[3];
            clothesMr.material = newClothesAnimMate[3];

            BoolFlags(false, false, false, true);

            curSpeed = 0.2f;

            yield return new WaitForSeconds(1.3f);

            StartCoroutine(Idle());
            isAttack = false;
            isCoroutineRunning = false;
        }
    }


    private IEnumerator Idle()
    {
        if (isAttack || isCoroutineRunning)
        {
            yield break;
        }

        Debug.Log(this.name + "IDle");

        attack.SetActive(false);

        bodyMr.material = newBodyAnimMate[2];
        clothesMr.material = newClothesAnimMate[2];

        BoolFlags(false, false, true, false);
        curSpeed = 0f;

        yield return new WaitForSeconds(1f);

        StartCoroutine(Attack());
    }


    private void Run()
    {
        Debug.Log(this.name + "run");

        curSpeed = runSpeed;

        bodyMr.material = newBodyAnimMate[1];
        clothesMr.material = newClothesAnimMate[1];

        if (OnZombieFree2 != null) OnZombieFree2(this);

        BoolFlags(false, true, false, false);
    }

    private void Walk()
    {
        Debug.Log(this.name + "walk");
        curSpeed = walkSpeed;
        bodyMr.material = newBodyAnimMate[0];
        clothesMr.material = newClothesAnimMate[0];

        BoolFlags(true, false, false, false);
    }

    private void BoolFlags(bool _isWalk, bool _isRun, bool _isIdle, bool _isAttack)
    {
        isWalk = _isWalk;
        isRun = _isRun;
        isIdle = _isIdle;
        isAttack = _isAttack;
    }

    // 텍스처 보간 좀 하고싶은데 못하겠음 일단 보류
    //private IEnumerator ChangeAnimMaterial(Material _curMaterial, Material _newMaterial)
    //{
    //    float t = 0f;

    //    while (t < 1f)
    //    {
    //        t += Time.deltaTime * 0.5f;
    //        Material newMaterial = new Material(_curMaterial); // 현재 머티리얼의 복사본 생성
    //        newMaterial.Lerp(_curMaterial, _newMaterial, t); // 새로운 머티리얼과 보간
    //        bodyMr.material = newMaterial; // 새로운 머티리얼 할당
    //        yield return null;
    //    }

    //    _curMaterial = _newMaterial;
    //}


    public void TakeDamage(float playerAttackDamage)
    {
        currentHealth = currentHealth - playerAttackDamage;

        if (OnZombieFree2 != null)
        {
            OnZombieFree2(this);
            target = playerPosition;
        }
    }
}