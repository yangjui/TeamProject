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
            agent.isStopped = true; // �׺꿡����Ʈ ���ֱ�
            Dead();
        }

        if (isMovingSelf)
        {
            playerPosition = playerTransform.position;
            agent.SetDestination(playerPosition);
        }
    }

    // ������ �ٲ�ġ��
    private void Dead()
    {
        GameObject newRagdoll = Instantiate(dead, transform.position, transform.rotation);

        RagDollPosition(alive.transform, newRagdoll.transform);

        isMovingSelf = false;
        alive.SetActive(false);
    }

    // ���� ��ȯ�� T�� �ƹ�Ÿ �Ǵ� �� ����. �� ���� ��ġ �����ֱ�
    private void RagDollPosition(Transform alive, Transform dead)
    {
        for (int i = 0; i < alive.transform.childCount; ++i)
        {
            if (alive.transform.childCount != 0)
                RagDollPosition(alive.transform.GetChild(i), dead.transform.GetChild(i));

            dead.transform.GetChild(i).localPosition = alive.transform.GetChild(i).localPosition;
            dead.transform.GetChild(i).localRotation = alive.transform.GetChild(i).localRotation;
        }
        dead.transform.position = alive.transform.position; // ���� ��ü�� ��ġ ���߱�
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

    private void OnTriggerEnter(Collider _other) // �ٸ� ������Ʈ�� �̸�!
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