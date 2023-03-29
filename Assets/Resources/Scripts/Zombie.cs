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
    [SerializeField] private float zombieHealth = 10f;


    private float currentHealth;

    private Animator anim;
    private GameObject player;

    public Transform target;
    NavMeshAgent agent;

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
            agent.isStopped = true; // �׺꿡����Ʈ ���ֱ�
            Dead();

            return; // �ؿ� �� ���� �ʵ���
        }

        agent.SetDestination(target.position);
    }

    // ������ �ٲ�ġ��
    private void Dead()
    {
        GameObject newRagdoll = Instantiate(dead, transform.position, transform.rotation);

        RagDollPosition(alive.transform, newRagdoll.transform);

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
        anim.SetBool("isAttack", true);

        // �÷��̾� ü�� ���� �Լ����ٰ� ������ �ֱ� @@(ȿ��)
        // ex) PlayerHealth(zombieAttackDamage);
    }

    // �÷��̾� �ν� ��!!!! �ȱ� -> �޷����鼭 �� �ֵθ�. ���� �ִϸ��̼��� �� ������ ���� �������� �����!
    // �ణ �������� @@(�غ�) ��Ʈ���� Ư�� Ʈ���� �Ѿ���� �׶����� �� ������� �׺�Ž��� õõ�� walk �ϴ� ����
    // enter �ؾ��ϴµ� ������ time.dletatime���� �װ� �س��� �ϴ� stay �ص�
    private void OnTriggerStay(Collider _other)
    {
        if (_other.CompareTag("PlayerAround"))
        {
            Debug.Log("!!!!!!!!!!");

            // �̰Ŵ� ���� ���ó�� (�Ѿ� ���) ó�� ���ؼ� �״°� ���� ������. ���ٰ���
            currentHealth -= Time.deltaTime;

            anim.SetBool("isRun", true);
            anim.SetBool("isAttack", true);
        }
    }

    // �Ÿ� �־����� �ٽ� �ɾ��. �÷��̾�� �־����� ��찡 �������� �ͱ� �ѵ� �ϴ� Ȥ�� ���� �־�� -> ���� �ٽ� ��� �� ����
    private void OnTriggerExit(Collider _other)
    {
        if (_other.CompareTag("PlayerAround"))
        {
            anim.SetBool("isAttack", false);
            anim.SetBool("isRun", false);
        }
    }

    // ���� ��ũ��Ʈ���� ��ȣ�ۿ� �� �� �Լ� @@(����)
    // ex) if (������ ray�� �����϶�) CurrentHealthZombie(originalBulletDamage);
    public void CurrentHealthZombie(float playerAttackDamage)
    {
        currentHealth = currentHealth - playerAttackDamage;
    }

    // @@ (����)
    // �̿��̸� enum������ ���ּ����� ���ھ��!
    // ex) if (���� ray�� ������ Head �����϶�) BodyPartsHitName(Head);
    // �̷������� ������ ���� �� ���� ��ġ�� ���� �ִϸ��̼� �����ų�Կ�!
    // �Լ��̸��̶� ������ �����Ұ̴ϴ� 
    public void BodyPartsHitName(string HitRange)
    {
        // HitRange.CompareTo("Head")
    }
}  