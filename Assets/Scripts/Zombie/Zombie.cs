using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [Header("# Zombie")]
    [SerializeField] private GameObject alive;
    [SerializeField] private GameObject deadRagdoll;
    [SerializeField] private GameObject dead;
    [SerializeField] private GameObject attackZombieL;
    [SerializeField] private GameObject attackZombieR;
    [SerializeField] private float zombieHealth = 100f;

    private NavMeshAgent navAgent;

    private float currentHealth;

    private float maxWalkSpeed = 1f;
    private float minWalkSpeed = 0.5f;

    private float maxRunSpeed = 2f;
    private float minRunSpeed = 1.5f;

    private float blackHoleRadius = 7f;
    private float detectionRadius = 10f;
    private float resetTime = 10f;

    private bool isMember = true;
    private bool isInBlackHole = false;
    private bool isRun = false;

    private Vector3 blackHolePosition;

    private Transform target;
    private Transform playerPosition;
    private Animator anim;

    public delegate void ZombieFreeEventHandler(Zombie zombie);
    public ZombieFreeEventHandler OnZombieFree;



    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentHealth = zombieHealth;
        InvokeRepeating("ZombieState", 1f, 2f);

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

        //Destroy(newRagdoll, 3f);

        //GameObject newDead = Instantiate(dead, transform.position, transform.rotation);
        //DeadPosition(newRagdoll.transform, newDead.transform);


        Destroy(alive);
    }

    //private void Dead()
    //{
    //    navAgent.enabled = false;
    //    alive.SetActive(false);

    //    GameObject newRagdoll = Instantiate(deadRagdoll, transform.position, transform.rotation);
    //    RagdollPosition(alive.transform, newRagdoll.transform);

    //    //newRagdoll.SetActive(false);
    //    Invoke("waitDead(newRagdoll)", 3f);
    //    Invoke("InstanceDead", 3f);
    //    Invoke("DeadPosition(dead, newDead)", 3f);
    //    Destroy(alive);
    //    Destroy(newRagdoll);
    //}

    //private void InstanceDead()
    //{
    //    GameObject newDead = Instantiate(dead, transform.position, transform.rotation);
    //}

    //private void waitDead(GameObject _newRagdoll)
    //{
    //    _newRagdoll.SetActive(false);
    //}


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

    //private void DeadPosition(Transform _dead, Transform _newRagdoll)
    //{
    //    for (int i = 0; i < _dead.transform.childCount; ++i)
    //    {
    //        if (_dead.transform.childCount != 0)
    //            DeadPosition(_dead.transform.GetChild(i), _newRagdoll.transform.GetChild(i));

    //        _newRagdoll.transform.GetChild(i).localPosition = _dead.transform.GetChild(i).localPosition;
    //        _newRagdoll.transform.GetChild(i).localRotation = _dead.transform.GetChild(i).localRotation;
    //    }
    //    _newRagdoll.transform.position = _dead.transform.position;
    //}



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

    private void ResetAgent()
    {
        navAgent.enabled = true;
        anim.enabled = true;
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
        anim.SetTrigger("isAttack");
    }

    private void Idle()
    {
        navAgent.speed = 0.1f;
        navAgent.angularSpeed = 500f;

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
        anim.SetBool("isIdle", false);
        anim.SetBool("isRun", true);
        navAgent.speed = Random.Range(minRunSpeed, maxRunSpeed);
        anim.speed = navAgent.speed;
    }

    private void Walk()
    {
        anim.SetBool("isRun", false);
        navAgent.speed = Random.Range(minWalkSpeed, maxWalkSpeed);
        anim.speed = navAgent.speed;
    }

    public void TakeDamage(float playerAttackDamage)
    {
        currentHealth = currentHealth - playerAttackDamage;

        if (OnZombieFree != null)
        {
            OnZombieFree(this); // 이벤트 발생과 함께 좀비의 이름 전달
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
