using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("# MoveSpeed")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    [Header("# Player HP")]
    [SerializeField] private float maxHp;
    private float currentHp;

    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;

    public float MaxHp => maxHp;
    public float CurrentHp => currentHp;

    private void Awake()
    {
        currentHp = maxHp;
    }

    public bool DecreaseHP(float _damage)
    {
        float previouseHP = currentHp;

        currentHp = currentHp - _damage > 0 ? currentHp - _damage : 0;

<<<<<<< Updated upstream
        if (currentHp == 0)
        {
            return true;
        }
=======
        Debug.Log("curHP : " + currentHp);
        if (currentHp == 0) return true;

>>>>>>> Stashed changes
        return false;
    }
}
