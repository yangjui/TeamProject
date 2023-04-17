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

    private int layerMask;
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
        layerMask = ~(LayerMask.GetMask("Player", "Path", "Zombie", "Wall"));
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
        if (Physics.Raycast(ray, out hit, weaponSetting.attackDistance, layerMask))
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
        isCharging = true;
        SoundManager.instance.Play2DSFX("LaserCharging");
        SoundManager.instance.SFX2DVolumeControl("LaserCharging", 0.5f);
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
            if (currentChargingTime > 4f)
            {
                ShotLaser();
                break;
            }
        }
    }

    private void ShotLaser()
    {
        if (!isCharging) return;
        
        TwoStepRaycast();
        isCharging = false;
        changeChargeModeCallback?.Invoke(isCharging);
        //StopCoroutine("ChargingLaserCoroutine");
        if (currentChargingTime >= chargingTime)
        {
            SoundManager.instance.Stop2DSFX("LaserCharging");
            SoundManager.instance.Play2DSFX("LaserShoot");
            SoundManager.instance.SFX2DVolumeControl("LaserShoot", 0.5f);
            CameraController.instance.StartShakeCamera();
            Destroy(chargeEffect);
            GameObject go = Instantiate(laserEffectPrefab, laserEffectPoint.position, Quaternion.identity);
            go.transform.LookAt(targetPoint);
            // ���� �� currentAmmo 1 ����
            weaponSetting.currentAmmo--;

            // ź �� UI ������Ʈ
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            currentChargingTime = 0f;
        }
        else
        {
            SoundManager.instance.Stop2DSFX("LaserCharging");
            Destroy(chargeEffect);
        }
    }

    private void CancelLaser()
    {
        isCharging = false;
        changeChargeModeCallback?.Invoke(isCharging);
        currentChargingTime = 0f;
        StopCoroutine("ChargingLaserCoroutine");
        SoundManager.instance.Stop2DSFX("LaserCharging");
        Destroy(chargeEffect);
    }

    public override void StartWeaponAction(int type = 0)
    {
        // �غ���°ų� �������̸� ���� �׼� �� �� ����
        if (isReload || isTakeOut) return;
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

    public void PlayReloadSound1()
    {
        SoundManager.instance.Play2DSFX("assault_rifle_reload_out_01");
    }

    public void PlayReloadSound2()
    {
        SoundManager.instance.Play2DSFX("assault_rifle_reload_out_02");
    }

    public void PlayReloadSound3()
    {
        SoundManager.instance.Play2DSFX("assault_rifle_reload_out_03");
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