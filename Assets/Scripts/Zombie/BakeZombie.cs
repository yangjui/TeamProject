using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BakeZombie : MonoBehaviour
{
    [Header("# Zombie")]
    [SerializeField] private GameObject alive;
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
    private float minWalkSpeed = 7f;

    private float maxRunSpeed = 10f;
    private float minRunSpeed = 13f;

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


    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
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

        target = playerPosition;
    }

    private void Update()
    {
        ZombieState();

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
    
        navAgent.speed = Random.Range(minRunSpeed, maxRunSpeed);
    }

    private void Walk()
    {
        bodyMr.material = newBodyAnimMate[0];
        clothesMr.material = newClothesAnimMate[0];
        navAgent.speed = Random.Range(minWalkSpeed, maxWalkSpeed);
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
