using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BakeZombie : MonoBehaviour
{
    [Header("# Zombie")]
    [SerializeField] private GameObject deadRagdoll;
    [SerializeField] private GameObject dead;

    [SerializeField] private float zombieHealth = 100f;

    [SerializeField] private MeshRenderer bodyMr = null;
    [SerializeField] private MeshRenderer clothesMr = null;
    [SerializeField] private List<Material> bodyAnimMate = null;
    [SerializeField] private List<Material> clothesAnimMate = null;    
    private List<Material> newBodyAnimMate = new List<Material>();
    private List<Material> newClothesAnimMate = new List<Material>();

    private NavMeshAgent navAgent;

    private float currentHealth;

    private float maxWalkSpeed = 5f;
    private float minWalkSpeed = 3f;

    private float maxRunSpeed = 11f;
    private float minRunSpeed = 9f;

    private float walkSpeed;
    private float runSpeed;

    private float blackHoleRadius = 7f;
    private float detectionRadius = 10f;


    private float resetTime = 10f;

    private bool isMember = true;
    private bool isInBlackHole = false;
    private bool isRun = false;

    private Vector3 blackHolePosition;

    private Transform target;
    private Transform playerPosition;

    public delegate void ZombieFreeEventHandler(BakeZombie zombie);
    public ZombieFreeEventHandler OnZombieFree2;

    private Rigidbody rb;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        currentHealth = zombieHealth;
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

        walkSpeed = Random.Range(minWalkSpeed, maxWalkSpeed);
        runSpeed = Random.Range(minRunSpeed, maxRunSpeed);
    }

    private void Update()
    {
        if (!isMember)
        {
            if (resetTime > 3)
            {
                InTheBlackHole();
            }

            if (isInBlackHole)
            {
                resetTime -= Time.deltaTime;
                if (resetTime <= 0)
                {
                    ResetAgent();
                }
            }

            if (!isInBlackHole)
            {
                navAgent.SetDestination(target.position);
            }
        }
        else
        {
            navAgent.SetDestination(target.position);
        }

        if (resetTime <= 0)
        {
            resetTime = 10f;
        }

        if (currentHealth <= 0 && !isInBlackHole)
        {
            Dead();
        }

        ZombieState();
    }

    public void PlayerPosition(Transform _playerPosition)
    {
        playerPosition = _playerPosition;
    }

    private void ZombieState()
    {
        switch (Vector3.Distance(transform.position, playerPosition.position))
        {
            case >= 10:
                Walk();
                break;
            case < 10 and >= 2:
                Run();
                break;
            case < 2:
                Idle();
                break;
            default:
                return;
        }
    }



    private void Dead()
    {
        this.gameObject.SetActive(false);
        navAgent.enabled = false;
        GameObject newRagdoll = Instantiate(deadRagdoll, transform.position, transform.rotation);
        RagdollPosition(this.transform, newRagdoll.transform);

        Destroy(this);
    }


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

    private void InTheBlackHole()
    {
        if (Vector3.Distance(blackHolePosition, transform.position) < blackHoleRadius && isInBlackHole)
        {
            navAgent.enabled = false;
            Vector3 dir = blackHolePosition - transform.position;
            transform.position += dir * 3f * Time.deltaTime;
        }
    }

    public void NoMoreMember()
    {
        isMember = false;
    }

    public void HitByBlackHole(Vector3 _position)
    {
        blackHolePosition = _position;
        isInBlackHole = true;
    }

    public void DetectNewObstacle(Vector3 _position)
    {
        if (Vector3.Distance(_position, transform.position) < detectionRadius && Vector3.Distance(_position, transform.position) > blackHoleRadius)
        {
            navAgent.speed = 15f;
            navAgent.angularSpeed = 500f;
        }
    }

    private void ResetAgent()
    {
        navAgent.enabled = true;
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

    private void Attack()
    {
        bodyMr.material = newBodyAnimMate[3];
        clothesMr.material = newClothesAnimMate[3];
    }

    private void Idle()
    {
        navAgent.speed = 0.1f;
        navAgent.angularSpeed = 500f;
        bodyMr.material = newBodyAnimMate[2];
        clothesMr.material = newClothesAnimMate[2];
        Attack();
    }

    private void Run()
    {
        if (!isRun && OnZombieFree2 != null) 
        {
            OnZombieFree2(this); // 이벤트 발생과 함께 좀비의 이름 전달
            isRun = true;
        }

        bodyMr.material = newBodyAnimMate[1];
        clothesMr.material = newClothesAnimMate[1];

        navAgent.speed = runSpeed;
    }

    private void Walk()
    {
        bodyMr.material = newBodyAnimMate[0];
        clothesMr.material = newClothesAnimMate[0];
        navAgent.speed = walkSpeed;
    }

    public void TakeDamage(float playerAttackDamage)
    {
        currentHealth = currentHealth - playerAttackDamage;

        if (OnZombieFree2 != null)
        {
            OnZombieFree2(this); // 이벤트 발생과 함께 좀비의 이름 전달
        }
    }

    public float CurrentHealth()
    {
        return currentHealth;
    }

}
