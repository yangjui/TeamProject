using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField]
    private GameObject alive;
    [SerializeField]
    private GameObject dead;
    [SerializeField]
    private float attackCoolTime = 0.5f;
    [SerializeField]
    private float attackDamage = 10f;

    private float enemyHealth = 5f;
    // �̰� ü�� �����ϴ� ��ũ��Ʈ ������ ���� �𸣰ڴ�. �츮 ������� �ڵ� ©�� �� ���ؼ�
    // ����ٸ� ���� ü�� ����ϴ� �Լ� currentHealth

    private Animator anim;
    private GameObject player;

    public Transform target;
    NavMeshAgent agent;

    private bool isPlayerAround;
    // private float timer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (enemyHealth <= 0)
        {
            agent.isStopped = true;

            // �״�ó��. �ϴ��� ������ ������ ���� �״°ŷ� �ص�
            Dead();
            return;
        }

        agent.SetDestination(target.position);

        if (isPlayerAround) // timer >= attackCoolTime &&
            Attack();
    }

    private void Dead()
    {
        RagDollPosition(alive.transform, dead.transform);

        alive.SetActive(false);
        dead.SetActive(true);
    }

    private void RagDollPosition(Transform alive, Transform dead)
    {
        for (int i = 0; i < alive.transform.childCount; ++i)
        {
            if (alive.transform.childCount != 0)
            {
                RagDollPosition(alive.transform.GetChild(i), dead.transform.GetChild(i));
            }
            dead.transform.GetChild(i).localPosition = alive.transform.GetChild(i).localPosition;
            dead.transform.GetChild(i).localRotation = alive.transform.GetChild(i).localRotation;
        }
        dead.transform.position = alive.transform.position;
    }

    private void Attack()
    {
        anim.SetBool("isAttack", true);

        // �÷��̾� ü�� ���� �Լ����ٰ� ������ �ֱ� - �÷��̾� �ڵ� �ް�
    }
    private void OnTriggerStay(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            enemyHealth -= Time.deltaTime;

            Debug.Log("Around");

            isPlayerAround = true;
            anim.SetBool("isAround", true);
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            isPlayerAround = false;
        }
    }
}
