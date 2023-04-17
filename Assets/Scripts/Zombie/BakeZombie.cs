
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
    private float distance = 0f;

    private bool isMember = true;
    private bool isInBlackHole = false;
    private bool isAttack = false;
    private bool isIdle = false;
    private bool isRun = false;
    private bool isWalk = false;


    [SerializeField] private GameObject StateColor;
    private Renderer stateRenderer;
    private Transform target;
    private Transform playerPosition;
    // private Rigidbody rb;

    private List<Material> newBodyAnimMate = new List<Material>();
    private List<Material> newClothesAnimMate = new List<Material>();

    private int attackNum;
    private int runNum;
    private int idleNum;

    private float attackTime = 0;
    private float returnIdleTime = 0;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        currentHealth = zombieHealth;

        stateRenderer = StateColor.GetComponent<Renderer>();
        target = null;
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
            if(i>5 && i<8)
            {
                newBodyAnimMate[i].SetFloat("_Length", randomAnimSpeed * 0.5f);
                newClothesAnimMate[i].SetFloat("_Length", randomAnimSpeed * 0.5f);
            }
        }

        attackNum = Random.Range(0, 2);
        runNum = Random.Range(5, 8);
        idleNum = Random.Range(3, 5);
        Debug.Log(this.name + "  att : " + attackNum + "   Run : " + runNum + "    idle : " + idleNum);
    }

    private void Update()
    {
        if (navAgent.enabled && (!isInBlackHole || !isIdle) && target != null)
            navAgent.SetDestination(target.position);

        DistanceCheck();

        if (!isMember)
        {
            if(!isAttack)
            {
                attackTime = 0f;
                returnIdleTime = 0f;
                ZombieState();
            }
            else if(isAttack)
            { 
                Attack();
            }
        }
    }

    public void PlayerPosition(Transform _playerPosition)
    {
        playerPosition = _playerPosition;
    }

    private void ZombieState()
    {
        switch (Mathf.Round(DistanceCheck() * 10f) / 10f)
        {
            case >= 15.0f:
                    Walk();
                break;
            case < 15.0f and >= 2.1f:
                     Run();
                break;
            case < 3.1f:
                     Idle();
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
    }

    public void GetAngularSpeedByManager(float _speed)
    {
        navAgent.angularSpeed = _speed;
    }

    private void Attack()       
    {
        attackTime += Time.deltaTime;
        stateRenderer.material.color = Color.red;
        if(Mathf.Round(DistanceCheck() * 10f) / 10f > 3.1f)
        {
            isAttack = false;
        }

        if (attackTime >= 2f)
        {
            isAttack = true;
            returnIdleTime += Time.deltaTime;
            stateRenderer.material.color = Color.blue;
            AnimTextureType(attackNum);
            attack.SetActive(true);
            if(returnIdleTime >= 1.3f)
            {
                attack.SetActive(false);
                isAttack = false;
            }
        }
    }

    private void Idle()
    {
        navAgent.speed = 0f;
        AnimTextureType(idleNum);
        isAttack = true;
    }

    private void Run()
    {
        stateRenderer.material.color = Color.green;
        navAgent.speed = runSpeed;
        AnimTextureType(runNum);
        if (OnZombieFree2 != null) OnZombieFree2(this);
    }

    private void Walk()
    {
        stateRenderer.material.color = Color.yellow;
        navAgent.speed = walkSpeed;
        AnimTextureType(8);
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
        if (currentHealth <= 0) Dead();
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

    private float DistanceCheck()
    {
           return distance = Vector3.Distance(transform.position, playerPosition.position);
    }
}