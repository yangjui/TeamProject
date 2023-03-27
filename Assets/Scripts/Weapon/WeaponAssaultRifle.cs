using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponAssaultRifle : WeaponBase
{
    [Header("# Fire Effects")]
    [SerializeField] private GameObject muzzleFlashEffect;

    [Header("# SpawnPoints")]
    [SerializeField] private Transform casingSpawnPoint;
    [SerializeField] private Transform bulletSpawnPoint;

    [Header("# Aim UI")]
    [SerializeField] private Image[] imageAim;

    private CasingObjectPool casingObjectPool;
    private ImpactObjectPool impactObjectPool;
    private Camera mainCamera;

    private bool isModeChange = false;      // �����ȯ ���� üũ
    private float defaultModeFOV = 60f;     // �⺻��忡���� ī�޶� FOV
    private float aimModeFOV = 30f;         // Aim��忡���� ī�޶� FOV

    

    private void Awake()
    {
        base.SetUp();
        casingObjectPool = GetComponent<CasingObjectPool>();
        impactObjectPool = GetComponent<ImpactObjectPool>();
        mainCamera = Camera.main;

        // ù ź �� �ִ�ź���� ����
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
        // ù źâ �� �ִ�źâ���� ����
        weaponSetting.currentMagazine = weaponSetting.maxMagazine;
    }

    private void OnEnable()
    {
        muzzleFlashEffect.SetActive(false);
        // ���Ⱑ Ȱ��ȭ�� �� �ش� ������ ź ���� ����.
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
        onMagazineEvent.Invoke(weaponSetting.currentMagazine);

        ResetVariables();
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

        anim.OnReload();
        SoundManager.instance.Play2DSFX("assault_rifle_reload_out", transform.position);

        while (true)
        {
            if (anim.CurrentAnimationIs("Movement"))
            {
                isReload = false;

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

    private IEnumerator OnModeChange()
    {
        float current = 0f;
        float percent = 0f;
        float time = 0.35f;

        anim.AimModeIs = !anim.AimModeIs;
        imageAim[0].enabled = !imageAim[0].enabled;
        imageAim[1].enabled = !imageAim[1].enabled;

        float start = mainCamera.fieldOfView;
        float end = anim.AimModeIs ? aimModeFOV : defaultModeFOV;

        isModeChange = true;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            mainCamera.fieldOfView = Mathf.Lerp(start, end, percent);
            yield return null;
        }
        isModeChange = false;
    }

    private void ResetVariables()
    {
        isReload = false;
        isAttack = false;
        isModeChange = false;
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
        Vector3 attackDirection = (targetPoint - bulletSpawnPoint.position).normalized;
        // �ѱ����� �� �������� ���̸� ��
        if (Physics.Raycast(bulletSpawnPoint.position, attackDirection, out hit, weaponSetting.attackDistance))
        {
            impactObjectPool.SpawnImpact(hit);
        }
        Debug.DrawRay(bulletSpawnPoint.position, attackDirection * weaponSetting.attackDistance, Color.blue);
    }

    public override void StartWeaponAction(int type = 0)
    {
        // �������̸� ���� �׼� �� �� ����
        if (isReload) return;

        // ��� ��ȯ���̸� ���� �׼� �Ҽ�����
        if (isModeChange) return;

        if (type == 0)
        {
            if (weaponSetting.isAutomaticAttack)
            {
                isAttack = true;
                StartCoroutine("OnAttackLoop");
            }
            else
            {
                OnAttack();
            }
        }
        else
        {
            // �������̸� �����ȯ �� �� ����
            if (isAttack) return; 
            StartCoroutine("OnModeChange");
        }
    }

    public override void StopWeaponAction(int type = 0)
    {
        if (type == 0)
        {
            isAttack = false;
            StopCoroutine("OnAttackLoop");
        }
    }

    public override void StartReload()
    {
        if (isReload || weaponSetting.currentMagazine <= 0) return; // ���� �� ������ �Ұ���
        StopWeaponAction(); // ���� ������� �� ������ ������ ������

        StartCoroutine("OnReload");
    }

    public void OnAttack()
    {
        if (Time.time - lastAttackTime > weaponSetting.attackRate) // �����ֱⰡ �Ǿ�� ������ �� �ִ�.
        {
            if (anim.MoveSpeed > 0.5f) return; // �޸������϶� ���ݺҰ�

            //�����ֱ⸦ �� �� �ֵ��� ���� �ð� ����
            lastAttackTime = Time.time;

            // �Ѿ��� ���ٸ� return
            if (weaponSetting.currentAmmo <= 0) return; 

            // ���� �� currentAmmo 1 ����
            weaponSetting.currentAmmo--;

            // ź �� UI ������Ʈ
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            // ���� �ִϸ��̼� ���
            string animation = anim.AimModeIs ? "AimFire" : "Fire";
            anim.Play(animation, -1, 0);
            if (!anim.AimModeIs) StartCoroutine("OnMuzzleFlashEffect");

            // ���� ���� ���
            SoundManager.instance.Play2DSFX("Shoot", transform.position);

            // �ѱ� ����Ʈ ���
            StartCoroutine("OnMuzzleFlashEffect");

            // ź�� ����
            casingObjectPool.SpawnCasing(casingSpawnPoint.position, transform.right);

            // ������ �߻��� ���ϴ� ��ġ ����
            TwoStepRaycast();
        }
    }

    public void TakeOut()
    {
        SoundManager.instance.Play2DSFX("take_out_weapon", transform.position);
    }
}
