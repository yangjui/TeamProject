using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { ���ݼ��� = 0, ������������, ����ź, �߷���ź }

// �̺�Ʈ Ŭ����
// Ư�� ��Ȳ���� ȣ��� �� ���� �Ű��������� �޾Ƽ� ���ϴ� �Լ����� �����Ų��.
[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { } 
[System.Serializable]
public class MagazineEvent : UnityEngine.Events.UnityEvent<int> { }

public abstract class WeaponBase : MonoBehaviour
{
    [Header("# WeaponBase")]
    [SerializeField] protected WeaponType weaponType;
    [SerializeField] protected WeaponSetting weaponSetting;

    protected float lastAttackTime = 0f;      // ������ �߻�ð� üũ
    protected bool isReload = false;          // ������ ������ üũ
    protected bool isAttack = false;          // ���� ���� üũ
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

    public virtual void IncreaseMagazine(int _magazine) // ź����� �Ծ��� �� �Ѿ�����
    {
        weaponSetting.currentMagazine = CurrentMagazine + _magazine > MaxMagazine ? MaxMagazine : CurrentMagazine + _magazine;
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
        onMagazineEvent.Invoke(weaponSetting.currentMagazine);
    }
}