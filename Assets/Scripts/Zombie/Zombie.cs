using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [Header("# Zombie")]
    [SerializeField] private GameObject alive;
    [SerializeField] private GameObject dead;
    [SerializeField] private float zombieHealth = 100f;
    [SerializeField] private GameObject attackZombieL;
    [SerializeField] private GameObject attackZombieR;

    [Header("# Speed")]
    [SerializeField] private float maxRunSpeed = 3.5f;
    [SerializeField] private float minRunSpeed = 2f;
    [SerializeField] private float maxWalkSpeed = 2f;
    [SerializeField] private float minWalkSpeed = 0.5f;

    private Vector3 playerPosition;

    private float currentHealth;
    private bool isMovingSelf = false;

    private Animator anim;

    [System.NonSerialized] public NavMeshAgent agent;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        currentHealth = zombieHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            agent.isStopped = true; // 네브에이전트 꺼주기
            Dead();
        }

        if (isMovingSelf)
        {
            agent.SetDestination(playerPosition);

            ZombieState();
        }
    }

    public void PlayerPosition(Vector3 _position)
    {
        playerPosition = _position;
    }

    private void ZombieState()
    {
        switch (Vector3.Distance(transform.position, playerPosition))
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

    // 렉돌로 바꿔치기
    private void Dead()
    {
        GameObject newRagdoll = Instantiate(dead, transform.position, transform.rotation);

        RagDollPosition(alive.transform, newRagdoll.transform); 

        isMovingSelf = false;
        alive.SetActive(false);
    }

    // 렉돌 전환시 T자 아바타 되는 거 방지. 각 관절 위치 맞춰주기
    private void RagDollPosition(Transform alive, Transform dead)
    {
        for (int i = 0; i < alive.transform.childCount; ++i)
        {
            if (alive.transform.childCount != 0)
                RagDollPosition(alive.transform.GetChild(i), dead.transform.GetChild(i));

            dead.transform.GetChild(i).localPosition = alive.transform.GetChild(i).localPosition;
            dead.transform.GetChild(i).localRotation = alive.transform.GetChild(i).localRotation;
        }
        dead.transform.position = alive.transform.position; // 렉돌 자체의 위치 맞추기
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Boundary"))
            isMovingSelf = true;
    }

    private void Attack()
    {
        anim.SetTrigger("isAttack");
    }

    private void Idle()
    {
        agent.speed = 0f;

        anim.SetBool("isIdle", true);
        Attack();
    }

    private void Run()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isRun", true);

        agent.speed = Random.Range(minRunSpeed, maxRunSpeed);
    }

    private void Walk()
    {
        anim.SetBool("isRun", false);

        agent.speed = Random.Range(minWalkSpeed, maxWalkSpeed);
    }

    public void TakeDamage(float playerAttackDamage)
    {
        currentHealth = currentHealth - playerAttackDamage;
        isMovingSelf = true;
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