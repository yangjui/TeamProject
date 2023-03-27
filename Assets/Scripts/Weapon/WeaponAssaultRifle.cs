using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { } // �̺�Ʈ Ŭ����
// Ư�� ��Ȳ���� ȣ��� �� ���� �Ű��������� �޾Ƽ� ���ϴ� �Լ����� �����Ų��.

public class WeaponAssaultRifle : MonoBehaviour
{
    [HideInInspector]
    public AmmoEvent onAmmoEvent = new AmmoEvent();

    [Header("# Fire Effects")]
    [SerializeField] private GameObject muzzleFlashEffect;

    [Header("# Weapon Setting")]
    [SerializeField] private WeaponSetting weaponSetting;
    private float lastAttackTime = 0f;

    [Header("# SpawnPoints")]
    [SerializeField] private Transform casingSpawnPoint;

    private PlayerAnimationController playerAnim;
    private CasingObjectPool casingObjectPool;
    private bool isReload = false;

    public WeaponName WeaponName => weaponSetting.weaponName;



    private void Awake()
    {
        playerAnim = GetComponentInParent<PlayerAnimationController>(); 
        casingObjectPool = GetComponent<CasingObjectPool>();

        // ù ź�� �ִ�ź���� ����
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;

        // ���Ⱑ Ȱ��ȭ�� �� �ش� ������ ź ���� ����.
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }

    private void OnEnable()
    {
        muzzleFlashEffect.SetActive(false);
    }

    private IEnumerator OnAttackLoop()
    {
        while (true)
        {
            OnAttack();
            yield return null;
        }
    }

    private IEnumerator OnMuzzleFlashEffect()
    {
        muzzleFlashEffect.SetActive(true);
        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);
        muzzleFlashEffect.SetActive(false);
    }

    private IEnumerator OnReload()
    {
        isReload = true;

        playerAnim.OnReload();
        SoundManager.instance.Play2DSFX("assault_rifle_reload_out", transform.position);

        while (true)
        {
            if (playerAnim.CurrentAnimationIs("Movement"))
            {
                isReload = false;

                weaponSetting.currentAmmo = weaponSetting.maxAmmo;
                onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

                yield break;
            }
            yield return null;
        }
    }

    public void StartWeaponAction(int type = 0)
    {
        if (isReload) return;

        if (type == 0)
        {
            if (weaponSetting.isAutomaticAttack)
            {
                StartCoroutine("OnAttackLoop");
            }
            else
            {
                OnAttack();
            }
        }
    }

    public void StopWeaponAction(int type = 0)
    {
        if (type == 0)
        {
            StopCoroutine("OnAttackLoop");
        }
    }

    public void StartReload()
    {
        if (isReload) return; // ���� �� ������ �Ұ���
        StopWeaponAction(); // ���� ������� �� ������ ������ ������

        StartCoroutine("OnReload");
    }

    public void OnAttack()
    {
        if (Time.time - lastAttackTime > weaponSetting.attackRate) // �����ֱⰡ �Ǿ�� ������ �� �ִ�.
        {
            if (playerAnim.MoveSpeed > 0.5f) return; // �޸������϶� ���ݺҰ�

            //�����ֱ⸦ �� �� �ֵ��� ���� �ð� ����
            lastAttackTime = Time.time;

            // �Ѿ��� ���ٸ� return
            if (weaponSetting.currentAmmo <= 0) return; 

            // ���� �� currentAmmo 1 ����
            weaponSetting.currentAmmo--;

            // ź �� UI ������Ʈ
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            // ���� �ִϸ��̼� ���
            playerAnim.Play("Fire", -1, 0); 

            // ���� ���� ���
            SoundManager.instance.Play2DSFX("Shoot", transform.position);

            // �ѱ� ����Ʈ ���
            StartCoroutine("OnMuzzleFlashEffect");

            // ź�� ����
            casingObjectPool.SpawnCasing(casingSpawnPoint.position, transform.right);
        }
    }

    public void TakeOut()
    {
        SoundManager.instance.Play2DSFX("take_out_weapon", transform.position);
    }
}
