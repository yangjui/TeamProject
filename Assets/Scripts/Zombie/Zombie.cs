using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] private GameObject alive;
    [SerializeField] private GameObject dead;
    [SerializeField] private float attackCoolTime = 0.5f;
    [SerializeField] private float zombieAttackDamage = 10f;
    [SerializeField] private float zombieHealth = 100f;
    [SerializeField] private BoxCollider triggerCollider;
    [SerializeField] private GameObject attackZombieL;
    [SerializeField] private GameObject attackZombieR;

    [System.NonSerialized] public Transform playerTransform;
    private Vector3 playerPosition;

    private float currentHealth;
    private bool isMovingSelf = false;
    private Animator anim;
    private PathTrigger pathTrigger;
    private NavAgentManager navAgentManager;

    private PlayerController player;

    NavMeshAgent agent;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        // player = GetComponentInParent<NavAgentManager>().player;
        currentHealth = zombieHealth;

        Invoke("FetchPlayer", 1f);
    }

    private void FetchPlayer()
    {
        PlayerManager playerManager = PlayerManager.instance;
        playerTransform = playerManager.playerTransform;
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
            playerPosition = playerTransform.position;
            agent.SetDestination(playerPosition);
            
            ZombieState();
        }
    }

    private void ZombieState()
    {
        switch (Vector3.Distance(this.transform.position, playerPosition))
        {
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

    private void Attack()
    {
        attackZombieL.SetActive(true);
        attackZombieR.SetActive(true);

        anim.SetTrigger("isAttack");
    }

    private void Idle()
    {
        attackZombieL.SetActive(false);
        attackZombieR.SetActive(false);
        
        anim.SetBool("isIdle", true);
        Attack();
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Boundary"))
            isMovingSelf = true;
    }

    private void Run()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isRun", true);
    }



    private float CheckPlayerPosition()
    {
        return Vector3.SqrMagnitude(this.transform.position - playerPosition);
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.CompareTag("PlayerAround"))
        {
            anim.SetBool("isAttack", false);
            anim.SetBool("isRun", false);
        }

        if (_other.CompareTag("Player"))
        {
            anim.SetBool("isIdle", false);
            anim.SetBool("isAttack", false);
        }
    }

    public void CurrentHealthZombie(float playerAttackDamage)
    {
        currentHealth = currentHealth - playerAttackDamage;
    }
}  