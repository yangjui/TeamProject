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
            agent.isStopped = true; // 네브에이전트 꺼주기
            Dead();

            return; // 밑에 줄 가지 않도록
        }

        agent.SetDestination(target.position);
    }

    // 렉돌로 바꿔치기
    private void Dead()
    {
        GameObject newRagdoll = Instantiate(dead, transform.position, transform.rotation);

        RagDollPosition(alive.transform, newRagdoll.transform);

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
        anim.SetBool("isAttack", true);

        // 플레이어 체력 관련 함수에다가 데미지 주기 @@(효석)
        // ex) PlayerHealth(zombieAttackDamage);
    }

    // 플레이어 인식 시!!!! 걷기 -> 달려오면서 팔 휘두름. 공격 애니메이션은 더 가까이 오면 시작할지 고민중!
    // 약간 군집제어 @@(해빈) 파트에서 특정 트리거 넘어오면 그때부터 각 좀비들이 네비매쉬로 천천히 walk 하는 느낌
    // enter 해야하는데 지금은 time.dletatime으로 죽게 해놔서 일단 stay 해둠
    private void OnTriggerStay(Collider _other)
    {
        if (_other.CompareTag("PlayerAround"))
        {
            Debug.Log("!!!!!!!!!!");

            // 이거는 지금 사망처리 (총알 등등) 처리 못해서 죽는거 대충 만들어둠. 없앨거임
            currentHealth -= Time.deltaTime;

            anim.SetBool("isRun", true);
            anim.SetBool("isAttack", true);
        }
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

    // 무기 스크립트에서 상호작용 해 줄 함수 @@(명하)
    // ex) if (무기의 ray가 좀비일때) CurrentHealthZombie(originalBulletDamage);
    public void CurrentHealthZombie(float playerAttackDamage)
    {
        currentHealth = currentHealth - playerAttackDamage;
    }

    // @@ (명하)
    // 이왕이면 enum형으로 해주셨으면 좋겠어요!
    // ex) if (무기 ray가 좀비의 Head 판정일때) BodyPartsHitName(Head);
    // 이런식으로 받으면 제가 각 판정 위치에 따른 애니메이션 실행시킬게요!
    // 함수이름이랑 변수명 수정할겁니당 
    public void BodyPartsHitName(string HitRange)
    {
        // HitRange.CompareTo("Head")
    }
}  