using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponName { AssaultRifle = 0, Laser, HandGrenade, GravityGrenade }

[System.Serializable]
public struct WeaponSetting
{
    public WeaponName weaponName;   // 무기 이름
    public int currentMagazine;     // 현재 탄창 수
    public int maxMagazine;         // 최대 탄창 수
    public int currentAmmo;         // 현재 탄약 수
    public int maxAmmo;             // 최대 탄약 수
    public float attackRate;        // 공격 속도
    public float attackDistance;    // 공격 사거리
    public bool isAutomaticAttack;  // 연속공격 여부
    public int damage;
}
