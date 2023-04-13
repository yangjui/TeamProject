
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BakeZombie : MonoBehaviour
{
    public delegate void ZombieFreeEventHandler(BakeZombie zombie);
    public ZombieFreeEventHandler OnZombieFree2;

    [Header("# Animation")]
    [SerializeField] private MeshRenderer bodyMr = null;
    [SerializeField] private MeshRenderer clothesMr = null;
    [SerializeField] private List<Material> bodyAnimMate = null;
    [SerializeField] private List<Material> clothesAnimMate = null;

    [Header("# Zombie")]
    [SerializeField] private GameObject deadRagdoll;
    [SerializeField] private GameObject leftZombiePrefab;
    [SerializeField] private GameObject rightZombiePrefab;
    [SerializeField] private GameObject attack;

    [Header("# Status")]
    [SerializeField] private float zombieHealth = 100f;
    // [SerializeField] private float curSpeed = 5f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float walkSpeed = 2f;

    [SerializeField] private GameObject fireEffect = null;


    [System.NonSerialized] public NavMeshAgent navAgent;

    private int deadType = 0;
    private float currentHealth;
    private float blackHoleRadius = 7f;
    private float detectionRadius = 10f;
    private float distance = 0f;

    private bool isMember = true;
    private bool isInBlackHole = false;
    private bool isAttack = false;
    private bool isIdle = false;
    private bool isRun = false;
    private bool isWalk = false;
    private bool isCoroutineRunning = false;


    [SerializeField] private GameObject StateColor;
    private Renderer stateRenderer;


    Vector3 playerLookAt;

    private Transform target;
    private Transform playerPosition;
    // private Rigidbody rb;

    private List<Material> newBodyAnimMate = new List<Material>();
    private List<Material> newClothesAnimMate = new List<Material>();

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        currentHealth = zombieHealth;
        // rb = GetComponent<Rigidbody>();

        stateRenderer = StateColor.GetComponent<Renderer>();

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
        if (!isInBlackHole || !isIdle ) navAgent.SetDestination(target.position);

        if (currentHealth <= 0)
        {
            Dead();
        }
        
        distance = Vector3.Distance(transform.position, playerPosition.position);
        playerLookAt = new Vector3(playerPosition.transform.position.x, transform.position.y, playerPosition.transform.position.z);

        ZombieState();
    }

    //public void ChasePlayer(Vector3 _target) // vellocity
    //{
    //    Vector3 direction = (_target - transform.position).normalized;
    //    Vector3 playerLookAt = new Vector3(_target.x, transform.position.y, _target.z);
    //    transform.LookAt(playerLookAt);
    //    rb.MovePosition(transform.position + direction * curSpeed * Time.deltaTime);
    //}

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
            case < 15.0f and >= 2.2f:
                if (!isRun || !isIdle || !isAttack) Run();
                break;
            case < 2.2f:
                if (isRun) StartCoroutine(Idle());
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
        // RagdollPosition(this.transform, newRagdoll.transform);

        Destroy(this.gameObject);
    }

    public void DeadType(int _type)
    {
        deadType = _type;
    }

    //private void RagdollPosition(Transform _alive, Transform _dead)
    //{
    //    for (int i = 0; i < _alive.transform.childCount; ++i)
    //    {
    //        if (_alive.transform.childCount != 0)
    //            RagdollPosition(_alive.transform.GetChild(i), _dead.transform.GetChild(i));

    //        _dead.transform.GetChild(i).localPosition = _alive.transform.GetChild(i).localPosition;
    //        _dead.transform.GetChild(i).localRotation = _alive.transform.GetChild(i).localRotation;
    //    }
    //    _dead.transform.position = _alive.transform.position;
    //}


    public void NoMoreMember()
    {
        isMember = false;
        // navAgent.enabled = false;
    }

    public void BlackHole()
    {
        if (!isInBlackHole)
        {
            isInBlackHole = true;
        }
    }

    //public void StopAnimation() 
    //{
    //    anim.enabled = false; 
    //}

    //public void DetectNewObstacle(Vector3 _position)
    //{
    //    if (Vector3.Distance(_position, transform.position) < detectionRadius && Vector3.Distance(_position, transform.position) > blackHoleRadius)
    //    {
    //        navAgent.speed = 15f;
    //        navAgent.angularSpeed = 500f;
    //    }
    //}

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
        navAgent.SetDestination(target.position);
    }

    //public float AgentSpeed()
    //{
    //    return navAgent.speed;
    //}

    //public void GetSpeedByManager(float _speed)
    //{
    //    navAgent.speed = _speed;
    //}

    public void GetAngularSpeedByManager(float _speed)
    {
        navAgent.angularSpeed = _speed;
    }

    private IEnumerator Attack()
    {
        if (!isAttack)
        {
            isAttack = true;
            yield return new WaitForSeconds(1f);

            stateRenderer.material.color = Color.blue;

            isCoroutineRunning = true;

            attack.SetActive(true);
            bodyMr.material = newBodyAnimMate[3];
            clothesMr.material = newClothesAnimMate[3];

            BoolFlags(false, false, false, true);

            navAgent.speed = 0f;

            yield return new WaitForSeconds(1.3f);

            attack.SetActive(false);
            isAttack = false;
            isCoroutineRunning = false;

            StartCoroutine(Idle());
        }
    }

    private IEnumerator Idle()
    {
        //if (isAttack || isCoroutineRunning)
        //{
        //    yield break;
        //}
        attack.SetActive(false);
        stateRenderer.material.color = Color.red;

        // isIdle = true;
        navAgent.isStopped = true;
        navAgent.velocity = Vector3.zero;

        // if (navAgent.isStopped) Debug.Log("@@@@" + navAgent.destination);

        bodyMr.material = newBodyAnimMate[2];
        clothesMr.material = newClothesAnimMate[2];

        BoolFlags(false, false, true, false);


        // yield return new WaitForSeconds(1f);

        // if (distance < 2.2f)
        {
            StartCoroutine(Attack());
        }

        isIdle = false;

        yield return null;
    }

    private void Run()
    {
        StopAllCoroutines();

        navAgent.isStopped = false;

        stateRenderer.material.color = Color.green;

        navAgent.speed = runSpeed;

        bodyMr.material = newBodyAnimMate[1];
        clothesMr.material = newClothesAnimMate[1];

        if (OnZombieFree2 != null) OnZombieFree2(this);

        BoolFlags(false, true, false, false);
    }

    private void Walk()
    {
        stateRenderer.material.color = Color.yellow;

        navAgent.speed = walkSpeed;
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

    public void TakeDamage(float _playerAttackDamage)
    {
        currentHealth -= _playerAttackDamage;

        if (OnZombieFree2 != null)
        {
            OnZombieFree2(this);
        }
    }

    public void Onfire()
    {
        if (!fireEffect.activeSelf)
        {
            fireEffect.SetActive(true);
            StartCoroutine(ApplyDamageOverTime(20f));
        }
    }

    IEnumerator ApplyDamageOverTime(float damage)
    {
        while (currentHealth > 0)
        {
            yield return new WaitForSeconds(1f);

            TakeDamage(damage);

            if (currentHealth <= 0) break;
        }
    }
}
