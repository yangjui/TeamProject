
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
    private bool isCoroutineRunning = false;


    [SerializeField] private GameObject StateColor;
    private Renderer stateRenderer;


    Vector3 playerLookAt;

    private Transform target;
    private Transform barricadePosition;
    private Transform targetPosition;

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
        if (navAgent.enabled && (!isInBlackHole || !isIdle) && target != null)
            navAgent.SetDestination(target.position);


        if (target.CompareTag("Barricade") || target.CompareTag("Player"))
        {
            distance = Vector3.Distance(transform.position, target.position);
            ZombieState();
            Debug.Log(this.name + "distance" + distance);
        }
    }

    public void TargetPosition(Transform _targetPosition)
    {
        targetPosition = _targetPosition;
    }

    public void ZombieState()
    {
        if (distance >= 15.0f)
        {
            if (!isWalk) Walk();
        }
        else if (distance >= 2.1f)
        {
            if (!isRun || !isIdle || !isAttack) Run();
        }
        else if (distance < 2.1f)
        {
            if (isRun) StartCoroutine(Idle());
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
                Destroy(this.gameObject);
                break;
            default:
                newRagdoll = Instantiate(deadRagdoll, transform.position, transform.rotation);
                break;
        }

        Destroy(this.gameObject);

        if (OnZombieFree2 != null) OnZombieFree2(this);

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
        if (!isInBlackHole) isInBlackHole = true;
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
            Debug.Log(this.name + "Attack");
            isAttack = true;
            isIdle = false;
            yield return new WaitForSeconds(1f);

            stateRenderer.material.color = Color.blue;

            attack.SetActive(true);

            AnimTextureType(3);

            navAgent.speed = 0f;

            yield return new WaitForSeconds(1.3f);

            attack.SetActive(false);

            StartCoroutine("Idle");
        }
    }

    private IEnumerator Idle()
    {
        Debug.Log(this.name + "Idle");
        isIdle = true;
        isAttack = false;
        isRun = false;

        attack.SetActive(false);
        stateRenderer.material.color = Color.red;

        navAgent.speed = 0f;
        navAgent.velocity = Vector3.zero;

        AnimTextureType(2);

        StartCoroutine("Attack");

        isIdle = false;

        yield return null;
    }

    private void Run()
    {
        isRun = true;
        isWalk = false;

        Debug.Log(this.name + "Run");         

        stateRenderer.material.color = Color.green;

        navAgent.speed = runSpeed;

        AnimTextureType(1);
    }

    private void Walk()
    {
        isWalk = true;
        isRun = false;
        Debug.Log(this.name + "Walk");

        stateRenderer.material.color = Color.yellow;

        navAgent.speed = walkSpeed;
        AnimTextureType(0);
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

        if (currentHealth <= 0) Dead();
        //if (OnZombieFree2 != null) OnZombieFree2(this);
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