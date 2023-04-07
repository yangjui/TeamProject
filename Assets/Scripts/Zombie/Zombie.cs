using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [Header("# Zombie")]
    [SerializeField] private GameObject alive;
    [SerializeField] private GameObject deadRagdoll;
    [SerializeField] private GameObject attackZombieL;
    [SerializeField] private GameObject attackZombieR;
    [SerializeField] private float zombieHealth = 100f;

    [System.NonSerialized] public NavMeshAgent navAgent;

    private float currentHealth;

    private float maxWalkSpeed = 1f;
    private float minWalkSpeed = 0.5f;

    private float maxRunSpeed = 2f;
    private float minRunSpeed = 1.5f;

    private float blackHoleRadius = 7f;
    private float detectionRadius = 10f;

    private bool isMember = true;
    private bool isInBlackHole = false;
    private bool isRun = false;

    private Vector3 blackHolePosition;

    private Transform target;
    private Transform playerPosition;
    private Animator anim;

    public delegate void ZombieFreeEventHandler(Zombie zombie);
    public ZombieFreeEventHandler OnZombieFree;

    private Rigidbody rb;
    [SerializeField] private float curSpeed = 2f;
    [SerializeField] private float runSpeed = 4f;
    [SerializeField] private float walkSpeed = 2f;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentHealth = zombieHealth;
        InvokeRepeating("ZombieState", 1f, 2f);

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isMember)
        {
            if (isInBlackHole)
            {
                InTheBlackHole();
            }

            if (!isInBlackHole)
            {
                ChasePlayer(target.position);
            }
        }

        else
        {
            navAgent.SetDestination(target.position);
        }

        if (currentHealth <= 0)
        {
            Dead();
        }
    }

    public void ChasePlayer(Vector3 _target) // vellocity
    {
        Vector3 direction = (_target - transform.position).normalized;
        Vector3 playerLookAt = new Vector3(_target.x, transform.position.y, _target.z);
        rb.MovePosition(transform.position + direction * curSpeed * Time.deltaTime);
        transform.LookAt(playerLookAt);
    }

    public void AniController(RuntimeAnimatorController _controller)
    {
        anim.runtimeAnimatorController = _controller;
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
        navAgent.enabled = false;
        alive.SetActive(false);

        GameObject newRagdoll = Instantiate(deadRagdoll, transform.position, transform.rotation);
        RagdollPosition(alive.transform, newRagdoll.transform);

        Destroy(alive);
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
        anim.SetTrigger("isBlackHole");
    }


    public void StopAnimation()
    {
        anim.enabled = false; // 땅에 떨어지는 순간으로 다시 애니메이션 작동하도록 만들어야함
    }


    public void DetectNewObstacle(Vector3 _position)
    {
        if (Vector3.Distance(_position, transform.position) < detectionRadius && Vector3.Distance(_position, transform.position) > blackHoleRadius)
        {
            navAgent.speed = 15f;
            navAgent.angularSpeed = 500f;
        }
    }

    // 없어질지도~!
    public void ResetAgent()
    {
        // navAgent.enabled = true;
        // anim.enabled = true;
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
        curSpeed = 0f;
        anim.SetTrigger("isAttack");
    }

    private void Idle()
    {
        curSpeed = 0f;
        anim.SetBool("isIdle", true);
        Attack();
    }

    private void Run()
    {
        if (!isRun && OnZombieFree != null)
        {
            OnZombieFree(this); // 이벤트 발생과 함께 좀비의 이름 전달
            isRun = true;
        }
        curSpeed = runSpeed;
        target = playerPosition;
        anim.SetBool("isIdle", false);
        anim.SetBool("isRun", true);
    }

    private void Walk()
    {
        curSpeed = walkSpeed;
        anim.SetBool("isRun", false);
    }

    public void TakeDamage(float playerAttackDamage)
    {
        currentHealth = currentHealth - playerAttackDamage;

        if (OnZombieFree != null)
        {
            OnZombieFree(this);
            target = playerPosition;
        }
    }

    public float CurrentHealth()
    {
        return currentHealth;
    }

    public void RAttackOn()
    {
        attackZombieR.SetActive(true);
    }

    public void RAttackOff()
    {
        attackZombieR.SetActive(false);
    }

    public void LAttackOn()
    {
        attackZombieL.SetActive(true);
    }

    public void LAttackOff()
    {
        attackZombieL.SetActive(false);
    }
}
