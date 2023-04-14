
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
    [SerializeField] private float runSpeed = 6f;
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
        if (navAgent.enabled && (!isInBlackHole || !isIdle))
            navAgent.SetDestination(target.position);

        if (currentHealth <= 0) Dead();

        distance = Vector3.Distance(transform.position, playerPosition.position);

        ZombieState();
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
            case < 15.0f and >= 2.1f:
                if (!isRun || !isIdle || !isAttack) Run();
                break;
            case < 2.1f:
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
            case 3:
                Debug.Log("Disappear");
                Destroy(this.gameObject);
                break;
            default:
                newRagdoll = Instantiate(deadRagdoll, transform.position, transform.rotation);
                break;
        }

        Destroy(this.gameObject);
    }

    public void DeadType(int _type)
    {
        deadType = _type;
    }

    public void NoMoreMember()
    {
        isMember = false;
    }

    public void BlackHole()
    {
        if (!isInBlackHole)
            isInBlackHole = true;
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
        if (navAgent.enabled) navAgent.SetDestination(target.position);
    }

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

            attack.SetActive(true);

            AnimTextureType(3);

            BoolFlags(false, false, false, true);

            navAgent.speed = 0f;

            yield return new WaitForSeconds(1.3f);

            attack.SetActive(false);
            isAttack = false;

            StartCoroutine("Idle");
        }
    }

    private IEnumerator Idle()
    {
        attack.SetActive(false);
        stateRenderer.material.color = Color.red;

        navAgent.speed = 0f;
        // if (!navAgent.isStopped) navAgent.isStopped = true;
        navAgent.velocity = Vector3.zero;

        AnimTextureType(2);

        BoolFlags(false, false, true, false);

        StartCoroutine("Attack");

        isIdle = false;

        yield return null;
    }

    private void Run()
    {
        //StopCoroutine("Attack");
        StopCoroutine("Idle");

        stateRenderer.material.color = Color.green;

        navAgent.speed = runSpeed;

        AnimTextureType(1);

        if (OnZombieFree2 != null) OnZombieFree2(this);

        BoolFlags(false, true, false, false);
    }

    private void Walk()
    {
        stateRenderer.material.color = Color.yellow;

        navAgent.speed = walkSpeed;
        AnimTextureType(0);

        BoolFlags(true, false, false, false);
    }

    private void BoolFlags(bool _isWalk, bool _isRun, bool _isIdle, bool _isAttack)
    {
        isWalk = _isWalk;
        isRun = _isRun;
        isIdle = _isIdle;
        isAttack = _isAttack;
    }

    private void AnimTextureType(int _type)
    {
        bodyMr.material = newBodyAnimMate[_type];
        clothesMr.material = newClothesAnimMate[_type];
    }

    public void TakeDamage(float _playerAttackDamage)
    {
        currentHealth -= _playerAttackDamage;

        if (OnZombieFree2 != null) OnZombieFree2(this);
    }

    public void Onfire()
    {
        if (!fireEffect.activeSelf)
        {
            fireEffect.SetActive(true);
            StartCoroutine(ApplyDamageOverTime(20));
        }
    }

    IEnumerator ApplyDamageOverTime(float _damage)
    {
        while (currentHealth > 0)
        {
            yield return new WaitForSeconds(1f);

            TakeDamage(_damage);

            Debug.Log(this.name + currentHealth);
            if (currentHealth <= 0) break;
        }
    }
}