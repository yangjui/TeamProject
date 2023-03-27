using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { } // 이벤트 클래스
// 특정 상황에서 호출될 때 마다 매개변수값을 받아서 원하는 함수들을 실행시킨다.

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

        // 첫 탄수 최대탄수로 설정
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;

        // 무기가 활성화될 때 해당 무기의 탄 수를 갱신.
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
        if (isReload) return; // 장전 중 재장전 불가능
        StopWeaponAction(); // 무기 사용중일 수 있으니 무기사용 멈춰줌

        StartCoroutine("OnReload");
    }

    public void OnAttack()
    {
        if (Time.time - lastAttackTime > weaponSetting.attackRate) // 공격주기가 되어야 공격할 수 있다.
        {
            if (playerAnim.MoveSpeed > 0.5f) return; // 달리기중일땐 공격불가

            //공격주기를 알 수 있도록 현재 시간 저장
            lastAttackTime = Time.time;

            // 총알이 없다면 return
            if (weaponSetting.currentAmmo <= 0) return; 

            // 공격 시 currentAmmo 1 감소
            weaponSetting.currentAmmo--;

            // 탄 수 UI 업데이트
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            // 무기 애니메이션 재생
            playerAnim.Play("Fire", -1, 0); 

            // 공격 사운드 재생
            SoundManager.instance.Play2DSFX("Shoot", transform.position);

            // 총구 이펙트 재생
            StartCoroutine("OnMuzzleFlashEffect");

            // 탄피 생성
            casingObjectPool.SpawnCasing(casingSpawnPoint.position, transform.right);
        }
    }

    public void TakeOut()
    {
        SoundManager.instance.Play2DSFX("take_out_weapon", transform.position);
    }
}
