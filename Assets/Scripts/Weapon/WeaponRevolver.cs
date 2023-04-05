using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRevolver : WeaponBase
{
    [Header("# Fire Effects")]
    [SerializeField] private GameObject muzzleFlashEffect;

    [Header("# SpawnPoints")]
    [SerializeField] private Transform bulletSpawnPoint;

    private ImpactObjectPool impactObjectPool;
    private Camera mainCamera;

    private void OnEnable()
    {
        muzzleFlashEffect.SetActive(false);
        onMagazineEvent.Invoke(weaponSetting.currentMagazine);
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

        ResetVariables();
    }

    private void Awake()
    {
        base.SetUp();

        impactObjectPool = GetComponent<ImpactObjectPool>();
        mainCamera = Camera.main;

        weaponSetting.currentMagazine = weaponSetting.maxMagazine;
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }

    public override void StartWeaponAction(int type = 0)
    {
        if (type == 0 && !isAttack && !isReload)
        {
            OnAttack();
        }
    }

    public override void StopWeaponAction(int type = 0)
    {
        isAttack = false;
    }

    public override void StartReload()
    {
        if (isReload || weaponSetting.currentMagazine <= 0) return;

        StopWeaponAction();

        StartCoroutine("OnReload");
    }

    public void OnAttack()
    {
        if (Time.time - lastAttackTime > weaponSetting.attackRate) // 공격주기가 되어야 공격할 수 있다.
        {
            if (anim.MoveSpeed > 0.5f) return; // 달리기중일땐 공격불가

            //공격주기를 알 수 있도록 현재 시간 저장
            lastAttackTime = Time.time;

            // 총알이 없다면 return
            if (weaponSetting.currentAmmo <= 0) return;

            // 공격 시 currentAmmo 1 감소
            weaponSetting.currentAmmo--;

            // 탄 수 UI 업데이트
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            // 무기 애니메이션 재생
            anim.Play("Fire", -1, 0);
            
            // 공격 사운드 재생
            SoundManager.instance.Play2DSFX("Shoot", transform.position);

            // 총구 이펙트 재생
            StartCoroutine("OnMuzzleFlashEffect");

            // 광선을 발사해 원하는 위치 공격
            TwoStepRaycast();
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
        SoundManager.instance.Play2DSFX("handgun_reload_out", transform.position);

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

    private void TwoStepRaycast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        // 화면 중앙좌표(Aim기준으로 Raycast 연산)
        //Vector2 sdf = new Vector2(x, y);

        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f);
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
        }
        Debug.DrawRay(bulletSpawnPoint.position, attackDirection * weaponSetting.attackDistance, Color.blue);
    }

    private void ResetVariables()
    {
        isReload = false;
        isAttack = false;
    }
}
