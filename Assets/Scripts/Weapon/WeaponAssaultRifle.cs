using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class WeaponAssaultRifle : WeaponBase
{
    public delegate void ChangeAimModeDelegate(bool _aimMode);
    private ChangeAimModeDelegate changeAimModeCallback = null;

    [Header("# Fire Effects")]
    [SerializeField] private GameObject muzzleFlashEffect;
    [SerializeField] private GameObject bulletHoleEffect;

    [Header("# SpawnPoints")]
    [SerializeField] private Transform casingSpawnPoint;
    [SerializeField] private Transform bulletSpawnPoint;

    private CasingObjectPool casingObjectPool;
    private ImpactObjectPool impactObjectPool;
    private BulletHoleObjectPool bulletHoleObjectPool;
    private Camera mainCamera;

    private bool isModeChange = false;      // �����ȯ ���� üũ
    private bool isTakeOut = false;
    private float defaultModeFOV = 60f;     // �⺻��忡���� ī�޶� FOV
    private float aimModeFOV = 30f;         // Aim��忡���� ī�޶� FOV

    private int shotCount = 0;
    private float lastShotTime;
    private float currentShotTime;
    private float shotVaildTime = 0.3f;
    private bool shotIsVaild;
    private Vector2 shotVec;

    public bool isAimMode = false;


    private enum AmmoType { AssaultRifle, laser };

    private void Awake()
    {
        base.SetUp();
        casingObjectPool = GetComponent<CasingObjectPool>();
        impactObjectPool = GetComponent<ImpactObjectPool>();
        bulletHoleObjectPool = GetComponent<BulletHoleObjectPool>();
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

    private void Update()
    {
        ShotTimer();
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
        if (isAimMode)
        {
            StartCoroutine(OnReloadingModeChange());
        }
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

    private IEnumerator OnModeChange()
    {
        float current = 0f;
        float percent = 0f;
        float time = 0.35f;

        isAimMode = !isAimMode;
        anim.AimModeIs = !anim.AimModeIs;

        float start = mainCamera.fieldOfView;
        float end = anim.AimModeIs ? aimModeFOV : defaultModeFOV;


        isModeChange = true;
        changeAimModeCallback?.Invoke(isAimMode);
        while (percent < 0.5)
        {
            current += Time.deltaTime;
            percent = current / time;

            mainCamera.fieldOfView = Mathf.Lerp(start, end, percent);
            yield return null;
        }
        isModeChange = false;
    }

    private IEnumerator OnReloadingModeChange()
    {
        float current = 0f;
        float percent = 0f;
        float time = 0.35f;

        float start = mainCamera.fieldOfView;

        isModeChange = true;
        isAimMode = !isAimMode;
        changeAimModeCallback?.Invoke(isAimMode);
        while (percent < 0.5)
        {
            current += Time.deltaTime;
            percent = current / time;

            mainCamera.fieldOfView = Mathf.Lerp(start, 60f, percent);
            yield return null;
        }
        isModeChange = false;
        anim.AimModeIs = !anim.AimModeIs;
    }

    private IEnumerator ResetShotAfterDelay()
    {
        yield return new WaitForSeconds(shotVaildTime);

        // ������ ���� ���� 3�� �̻��� ���� ��� �޺� ī��Ʈ �ʱ�ȭ
        if (Time.time - lastShotTime >= shotVaildTime)
        {
            shotCount = 0;
        }
    }

    private void ResetVariables()
    {
        isReload = false;
        isAttack = false;
        isModeChange = false;
    } // �� �ٲܶ����� �⺻���·� ����

    private void TwoStepRaycast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f + shotVec);
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
            bulletHoleObjectPool.SpawnImpact(hit);

            if (hit.transform.CompareTag("Zombie")) // ����� �ִ� �Լ�
            {
                hit.transform.GetComponent<Zombie>().Takedamage(weaponSetting.damage);
            }
        }
        Debug.DrawRay(bulletSpawnPoint.position, attackDirection * weaponSetting.attackDistance, Color.blue);
    }

    private void ShotTimer()
    {
        // ���� �ð��� ���
        currentShotTime = Time.time;

        // ���� ���� �ð����� ���̸� ����Ͽ� ShotIsVaild�� �������� �Ǵ�
        shotIsVaild = currentShotTime - lastShotTime <= shotVaildTime;
    }

    private void ShotCount()
    {
        if (shotIsVaild)
        {
            // Shot ī��Ʈ ����
            shotCount++;
        }
        else // ��ȿ �ð� ���� ������ �� �� ���
        {
            // Shot ī��Ʈ �ʱ�ȭ �� 1���� ����
            shotCount = 1;
        }
        // ���� ���� �ð��� ������ ���� �ð����� ����
        lastShotTime = currentShotTime;

        // ���� �� shotVaildTime���� ������ ������ Shotī��Ʈ �ʱ�ȭ
        StartCoroutine(ResetShotAfterDelay());
    }

    private void ShotVec()
    {
        switch (shotCount)
        {
            case 0:
                shotVec = Vector2.zero;
                break;
            case 1:
                shotVec = Vector2.zero;
                break;
            case 2:
                shotVec = Vector2.zero;
                break;
            case > 3:
                shotVec = new Vector2(Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f));
                break;
                //case 4:
                //    shotVec = new Vector2(1f, 0f);
                //    break;
                //case 5:
                //    shotVec = new Vector2(0.5f, 0f);
                //    break;
                //case 6:
                //    shotVec = new Vector2(0f, 0f);
                //    break;
                //case 7:
                //    shotVec = new Vector2(-0.5f, 0f);
                //    break;
                //case 8:
                //    shotVec = new Vector2(-1f, 0f);
                //    break;
                //case 9:
                //    shotVec = new Vector2(-0.5f, 0f);
                //    break;
                //case 10:
                //    shotVec = new Vector2(0f, 0f);
                //    break;
                //case > 10:
                //    shotVec = new Vector2(0f, (shotCount - 10) * 0.03f);
                //    break;
        }
    }

    public override void StartWeaponAction(int type = 0)
    {
        // �غ���°ų� �������̸� ���� �׼� �� �� ����
        if (isReload || isTakeOut) return;

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
            StopCoroutine("OnAttackLoop");
        }
    }

    public override void StartReload()
    {
        if (isReload || isTakeOut || weaponSetting.currentMagazine <= 0) return; // ���� �� ������ �Ұ���
        StopWeaponAction(); // ���� ������� �� ������ ������ ������

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

            // ���� �� currentAmmo 1 ����
            weaponSetting.currentAmmo--;

            // �ݵ������ �߼� ī��Ʈ
            ShotCount();

            // �ݵ������ Vector ��ȯ
            ShotVec();

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

    public void OnChangeAimModeDelegate(ChangeAimModeDelegate _changeAimModeCallback)
    {
        changeAimModeCallback = _changeAimModeCallback;
    } // ���Ӹ�� �ٲ𶧸��� ���ؼ� �ٲ��ַ��� �ݹ�
}