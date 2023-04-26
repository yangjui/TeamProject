using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { 돌격소총 = 0, 레이저라이플, 수류탄, 중력자탄 }

// 이벤트 클래스
// 특정 상황에서 호출될 때 마다 매개변수값을 받아서 원하는 함수들을 실행시킨다.
[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { } 
[System.Serializable]
public class MagazineEvent : UnityEngine.Events.UnityEvent<int> { }

public abstract class WeaponBase : MonoBehaviour
{
    [Header("# WeaponBase")]
    [SerializeField] protected WeaponType weaponType;
    [SerializeField] protected WeaponSetting weaponSetting;

    protected float lastAttackTime = 0f;      // 마지막 발사시간 체크
    protected bool isReload = false;          // 재정전 중인지 체크
    protected bool isAttack = false;          // 공격 여부 체크
    protected PlayerAnimationController anim;

    [HideInInspector]
    public AmmoEvent onAmmoEvent = new AmmoEvent();
    [HideInInspector]
    public MagazineEvent onMagazineEvent = new MagazineEvent();

    public PlayerAnimationController Animator => anim;
    public WeaponName WeaponName => weaponSetting.weaponName;
    public int CurrentMagazine => weaponSetting.currentMagazine;
    public int MaxMagazine => weaponSetting.maxMagazine;

    public abstract void StartWeaponAction(int type = 0);
    public abstract void StopWeaponAction(int type = 0);
    public abstract void StartReload();

    protected void SetUp()
    {
        anim = GetComponent<PlayerAnimationController>();
    }

    public virtual void IncreaseMagazine(int _magazine) // 탄약상자 먹었을 때 총알충전
    {
        weaponSetting.currentMagazine = CurrentMagazine + _magazine > MaxMagazine ? MaxMagazine : CurrentMagazine + _magazine;
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
        onMagazineEvent.Invoke(weaponSetting.currentMagazine);
    }
}