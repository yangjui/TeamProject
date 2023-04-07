using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class WeaponLaserRifle : WeaponBase
{
    [Header("# SpawnPoints")]
    [SerializeField] private Transform chargeEffectPoint;
    [SerializeField] private Transform fireEffectPoint;
    private Camera mainCamera;

    [SerializeField] private float chargingTime = 2f;
    [SerializeField] private float currentChargingTime = 0f;
    [SerializeField] private GameObject chargeEffectPrefab;
    [SerializeField] private GameObject fireEffectPrefab;
    [SerializeField] private GameObject groundEffectPrefab;
    [SerializeField] private float chargingSize = 0.1f;
    private Vector3 attackDirection;
    private GameObject chargeEffect;

    private bool isTakeOut = false;
    private bool isCharging = false;

    private void Awake()
    {
        base.SetUp();
        // ù ź �� �ִ�ź���� ����
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
        // ù źâ �� �ִ�źâ���� ����
        weaponSetting.currentMagazine = weaponSetting.maxMagazine;
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        // ���Ⱑ Ȱ��ȭ�� �� �ش� ������ ź ���� ����.
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
        onMagazineEvent.Invoke(weaponSetting.currentMagazine);

        ResetVariables();
    }

    private void OnDisable()
    {
        CancelLaser();
    }

    private IEnumerator OnReload()
    {
        isReload = true;

        anim.OnReload();
        
        while (true)
        {
            if (!isReload)
            {
                if (WeaponName != 0)
                {
                    weaponSetting.currentMagazine--;
                    onMagazineEvent.Invoke(weaponSetting.currentMagazine);
                }

                weaponSetting.currentAmmo = weaponSetting.maxAmmo;
                onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

                yield break;
            }
            yield return null;
        }
    }

    private void ResetVariables() // �� �ٲܶ����� �⺻���·� ����
    {
        isReload = false;
        isAttack = false;
    } 

    private void TwoStepRaycast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f);
        if (Physics.Raycast(ray, out hit, weaponSetting.attackDistance))
        {
            // ���̰� �´´ٸ� Ÿ������Ʈ�� �������
            targetPoint = hit.point;
        }
        else
        {
            // ���� ����� ���ٸ� �ش������ �ִ��Ÿ���ġ
            targetPoint = ray.origin + ray.direction * weaponSetting.attackDistance;
        }
        Debug.DrawRay(ray.origin, ray.direction * weaponSetting.attackDistance, Color.red);

        // ������ ���� Ÿ������Ʈ���� �ѱ������� ���� �ѱ����� Ÿ������Ʈ�� ���ϴ� ������ ���� �� ����
        attackDirection = (targetPoint - chargeEffectPoint.position).normalized;
    }

    private IEnumerator ChargingLaserCoroutine()
    {
        Debug.Log("StartCharging!");
        isCharging = true;
        currentChargingTime = 0f;
        chargeEffect = Instantiate(chargeEffectPrefab, chargeEffectPoint.position, chargeEffectPoint.rotation);
        chargeEffect.transform.SetParent(chargeEffectPoint);
        chargeEffect.transform.localScale *= 0;
        while (isCharging)
        {
            //Debug.Log("ChargingTime : " + currentChargingTime);
            currentChargingTime += Time.deltaTime;
            if (currentChargingTime < chargingTime)
                chargeEffect.transform.localScale += Vector3.one * currentChargingTime * Time.deltaTime * chargingSize;
            yield return null;
        }
    }

    private void ShotLaser()
    {
        isCharging = false;
        StopCoroutine("ChargingLaserCoroutine");
        if (currentChargingTime >= chargingTime)
        {
            Destroy(chargeEffect);
            Instantiate(fireEffectPrefab, fireEffectPoint.position, fireEffectPoint.rotation);

            // ���� �� currentAmmo 1 ����
            weaponSetting.currentAmmo--;

            // ź �� UI ������Ʈ
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            currentChargingTime = 0f;
        }
        else Destroy(chargeEffect);
    }

    private void CancelLaser()
    {
        isCharging = false;
        currentChargingTime = 0f;
        StopCoroutine("ChargingLaserCoroutine");
        Destroy(chargeEffect);
    }

    public override void StartWeaponAction(int type = 0)
    {
        // �غ���°ų� �������̸� ���� �׼� �� �� ����
        if (isReload || isTakeOut) return;
        Debug.Log(type);
        if (type == 0)
        {
            if (!weaponSetting.isAutomaticAttack)
            {
                OnAttack();
            }
        }
    }

    public override void StopWeaponAction(int type = 0)
    {
        ShotLaser();
    }

    public override void StartReload()
    {
        if (isReload || isTakeOut || weaponSetting.currentMagazine <= 0) return; // ���� �� ������ �Ұ���
        CancelLaser(); // ���� ������� �� ������ ������ ������

        StartCoroutine("OnReload");
    }

    public void OnAttack()
    {
        if (Time.time - lastAttackTime > weaponSetting.attackRate) // �����ֱⰡ �Ǿ�� ������ �� �ִ�.
        {
            if (anim.MoveSpeed > 0.5f || isTakeOut) return; // �غ���°ų� �޸������϶� ���ݺҰ�

            //�����ֱ⸦ �� �� �ֵ��� ���� �ð� ����
            lastAttackTime = Time.time;

            // �Ѿ��� ���ٸ� return
            if (weaponSetting.currentAmmo <= 0) return;

            isAttack = true;

            StartCoroutine("ChargingLaserCoroutine");

            // ���� �ִϸ��̼� ���
            anim.Play("Fire", -1, 0);

            // ������ �߻��� ���ϴ� ��ġ ����
            TwoStepRaycast();
        }
    }


    /// �ִϸ��̼��Լ�, �ݹ�

    public void IsStopAttack()
    {
        isAttack = false;
    }

    public void IsTakeOutStart()
    {
        SoundManager.instance.Play2DSFX("take_out_weapon", transform.position);
        isTakeOut = true;
    }

    public void IsTakeOutOver()
    {
        isTakeOut = false;
    }

    public void IsReloadStart()
    {
        SoundManager.instance.Play2DSFX("assault_rifle_reload_out", transform.position);
    }

    public void IsReloadOver()
    {
        isReload = false;
    }
}