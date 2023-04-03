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

    private bool isModeChange = false;      // 모드전환 여부 체크
    private bool isTakeOut = false;
    private float defaultModeFOV = 60f;     // 기본모드에서의 카메라 FOV
    private float aimModeFOV = 30f;         // Aim모드에서의 카메라 FOV

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

        // 첫 탄 수 최대탄수로 설정
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
        // 첫 탄창 수 최대탄창수로 설정
        weaponSetting.currentMagazine = weaponSetting.maxMagazine;
    }

    private void OnEnable()
    {
        muzzleFlashEffect.SetActive(false);
        // 무기가 활성화될 때 해당 무기의 탄 수를 갱신.
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

        // 마지막 공격 이후 3초 이상이 지난 경우 콤보 카운트 초기화
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
    } // 총 바꿀때마다 기본상태로 리셋

    private void TwoStepRaycast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f + shotVec);
        if (Physics.Raycast(ray, out hit, weaponSetting.attackDistance))
        {
            // 레이가 맞는다면 타겟포인트는 맞은대상
            targetPoint = hit.point;
        }
        else
        {
            // 맞은 대상이 없다면 해당방향의 최대사거리위치
            targetPoint = ray.origin + ray.direction * weaponSetting.attackDistance;
        }
        Debug.DrawRay(ray.origin, ray.direction * weaponSetting.attackDistance, Color.red);

        // 위에서 나온 타겟포인트에서 총구방향을 빼면 총구에서 타겟포인트로 향하는 방향을 구할 수 있음
        Vector3 attackDirection = (targetPoint - bulletSpawnPoint.position).normalized;
        // 총구에서 위 방향으로 레이를 쏨
        if (Physics.Raycast(bulletSpawnPoint.position, attackDirection, out hit, weaponSetting.attackDistance))
        {
            impactObjectPool.SpawnImpact(hit);
            bulletHoleObjectPool.SpawnImpact(hit);

            if (hit.transform.CompareTag("Zombie")) // 대미지 주는 함수
            {
                hit.transform.GetComponent<Zombie>().Takedamage(weaponSetting.damage);
            }
        }
        Debug.DrawRay(bulletSpawnPoint.position, attackDirection * weaponSetting.attackDistance, Color.blue);
    }

    private void ShotTimer()
    {
        // 현재 시간을 기록
        currentShotTime = Time.time;

        // 이전 공격 시간과의 차이를 계산하여 ShotIsVaild의 참거짓을 판단
        shotIsVaild = currentShotTime - lastShotTime <= shotVaildTime;
    }

    private void ShotCount()
    {
        if (shotIsVaild)
        {
            // Shot 카운트 증가
            shotCount++;
        }
        else // 유효 시간 내에 공격을 안 한 경우
        {
            // Shot 카운트 초기화 후 1부터 시작
            shotCount = 1;
        }
        // 현재 공격 시간을 마지막 공격 시간으로 저장
        lastShotTime = currentShotTime;

        // 공격 후 shotVaildTime동안 공격이 없으면 Shot카운트 초기화
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
        // 준비상태거나 장전중이면 무기 액션 할 수 없게
        if (isReload || isTakeOut) return;

        // 모드 전환중이면 무기 액션 할수없게
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
            // 공격중이면 모드전환 할 수 없게
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
        if (isReload || isTakeOut || weaponSetting.currentMagazine <= 0) return; // 장전 중 재장전 불가능
        StopWeaponAction(); // 무기 사용중일 수 있으니 무기사용 멈춰줌

        StartCoroutine("OnReload");
    }

    public void OnAttack()
    {
        if (Time.time - lastAttackTime > weaponSetting.attackRate) // 공격주기가 되어야 공격할 수 있다.
        {
            if (anim.MoveSpeed > 0.5f || isTakeOut) return; // 준비상태거나 달리기중일땐 공격불가

            //공격주기를 알 수 있도록 현재 시간 저장
            lastAttackTime = Time.time;

            // 총알이 없다면 return
            if (weaponSetting.currentAmmo <= 0) return;

            // 공격 시 currentAmmo 1 감소
            weaponSetting.currentAmmo--;

            // 반동제어용 발수 카운트
            ShotCount();

            // 반동제어용 Vector 변환
            ShotVec();

            // 탄 수 UI 업데이트
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            // 무기 애니메이션 재생
            string animation = anim.AimModeIs ? "AimFire" : "Fire";
            anim.Play(animation, -1, 0);
            if (!anim.AimModeIs) StartCoroutine("OnMuzzleFlashEffect");

            // 공격 사운드 재생
            SoundManager.instance.Play2DSFX("Shoot", transform.position);

            // 총구 이펙트 재생
            StartCoroutine("OnMuzzleFlashEffect");

            // 탄피 생성
            casingObjectPool.SpawnCasing(casingSpawnPoint.position, transform.right);

            // 광선을 발사해 원하는 위치 공격
            TwoStepRaycast();
        }
    }


    /// 애니메이션함수, 콜백

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
    } // 에임모드 바뀔때마다 조준선 바꿔주려고 콜백
}