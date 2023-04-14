using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class WeaponLaserRifle : WeaponBase
{
    public delegate void ChangeChargeModeDelegate(bool _chargeMode);
    private ChangeChargeModeDelegate changeChargeModeCallback = null;

    [Header("# SpawnPoints")]
    [SerializeField] private Transform chargeEffectPoint;
    [SerializeField] private Transform laserEffectPoint;

    [Header("# Prefab")]
    [SerializeField] private GameObject chargeEffectPrefab = null;
    [SerializeField] private GameObject laserEffectPrefab = null;

    [Header("# LaserSetting")]
    [SerializeField] private float chargingTime = 2f;
    [SerializeField] private float chargingSize = 0.1f;
    [SerializeField] private float maxchargingTime = 5f;

    private Vector3 attackDirection;
    private Vector3 targetPoint;
    private GameObject chargeEffect;
    private Camera mainCamera;

    private bool isTakeOut = false;
    private bool isCharging = false;
    private float currentChargingTime = 0f;

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

    private void Update()
    {
        if (this.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CancelLaser();
        }
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
        targetPoint = Vector3.zero;

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

        attackDirection = targetPoint - chargeEffectPoint.position;
    }

    private IEnumerator ChargingLaserCoroutine()
    {
        Debug.Log("StartCharging!");
        isCharging = true;
        changeChargeModeCallback?.Invoke(isCharging);
        currentChargingTime = 0f;

        if (chargeEffect != null) Destroy(chargeEffect);

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
            if (currentChargingTime > maxchargingTime)
            {
                ShotLaser();
            }
        }
    }

    private void ShotLaser()
    {
        TwoStepRaycast();
        isCharging = false;
        changeChargeModeCallback?.Invoke(isCharging);
        StopCoroutine("ChargingLaserCoroutine");
        if (currentChargingTime >= chargingTime)
        {
            CameraController.instance.StartShakeCamera();
            Destroy(chargeEffect);
            GameObject go = Instantiate(laserEffectPrefab, laserEffectPoint.position, Quaternion.identity);
            go.transform.LookAt(targetPoint);
            Debug.Log(targetPoint);
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
        changeChargeModeCallback?.Invoke(isCharging);
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
        }
    }


    /// �ִϸ��̼��Լ�, �ݹ�

    public void IsStopAttack()
    {
        isAttack = false;
    }

    public void IsTakeOutStart()
    {
        SoundManager.instance.Play2DSFX("take_out_weapon");
        isTakeOut = true;
    }

    public void IsTakeOutOver()
    {
        isTakeOut = false;
    }

    public void IsReloadStart()
    {
        SoundManager.instance.Play2DSFX("assault_rifle_reload_out");
    }

    public void IsReloadOver()
    {
        isReload = false;
    }

    public void OnChangeChargeModeDelegate(ChangeChargeModeDelegate _changeChargeModeCallback)
    {
        changeChargeModeCallback = _changeChargeModeCallback;
    } 
}