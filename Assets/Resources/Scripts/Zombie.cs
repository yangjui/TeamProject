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
    [SerializeField] private float zombieHealth = 1000f;
    [SerializeField] private BoxCollider triggerCollider;
    [SerializeField] private GameObject attackZombieL;
    [SerializeField] private GameObject attackZombieR;


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
        player = GetComponentInParent<NavAgentManager>().player;
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
            agent.SetDestination(player.transform.position);
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

        anim.SetBool("isAttack", true);
        anim.SetBool("isIdle", true);

        Invoke("ReAttack", 0.5f);
    }

    private void ReAttack()
    {
        attackZombieL.SetActive(false);
        attackZombieR.SetActive(false);

        anim.SetBool("isIdle", false);
    }

    private void OnTriggerEnter(Collider _other) // 다른 오브젝트의 이름!
    {
        if (_other.CompareTag("Boundary"))
        {
            triggerCollider.isTrigger = true;
            isMovingSelf = true;
        }

        if (_other.CompareTag("PlayerAround"))
            anim.SetBool("isRun", true);

        if (_other.CompareTag("Player"))
            Attack();

    }

    void OnCollisionEnter(Collision _collision)
    {

    }


    // 거리 멀어지면 다시 걸어옴. 플레이어와 멀어지는 경우가 있으려나 싶긴 한데 일단 혹시 몰라서 넣어둠 -> 차후 다시 얘기 해 볼것
    private void OnTriggerExit(Collider _other)
    {
        if (_other.CompareTag("PlayerAround"))
        {
            anim.SetBool("isAttack", false);
            anim.SetBool("isRun", false);
        }
    }

    public void CurrentHealthZombie(float playerAttackDamage)
    {
        currentHealth = currentHealth - playerAttackDamage;
    }
}  