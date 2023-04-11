using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrenade : WeaponBase
{
    public delegate void TrajectoryDelegate(bool _bool);
    private TrajectoryDelegate trajectoryCallback = null;

    [Header("# Grenade")]
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private Transform grenadeSpawnPoint;
    [SerializeField] private float throwForce;

    private void OnEnable()
    {
        onMagazineEvent.Invoke(weaponSetting.currentMagazine);
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
        GetComponent<GrenadeTrajectory>().SetThrowForce(throwForce);
        isAttack = false;
    }

    private void Awake()
    {
        base.SetUp();

        weaponSetting.currentMagazine = weaponSetting.maxMagazine;
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }

    public override void StartWeaponAction(int type = 0)
    {
        if (type == 0 && !isAttack && weaponSetting.currentAmmo > 0)
        {
            StartCoroutine("OnAttack");
        }
    }

    public override void StopWeaponAction(int type = 0)
    {

    }

    public override void StartReload()
    {

    }

    private IEnumerator OnAttack()
    {
        isAttack = true;
        trajectoryCallback?.Invoke(!isAttack);

        anim.Play("Fire", -1, 0);
        SoundManager.instance.Play3DSFX("grenade-throw-2", transform.position);

        yield return new WaitForEndOfFrame();

        while (true)
        {
            if (anim.CurrentAnimationIs("Movement"))
            {
                isAttack = false;
                trajectoryCallback?.Invoke(!isAttack);
                yield break;
            }
            yield return null;
        }
    }

    public void SpawnGrenadeProjectile()
    {
        GameObject grenadeClone = Instantiate(grenadePrefab, grenadeSpawnPoint.position, Random.rotation);
        grenadeClone.GetComponent<WeaponGrenadeProjectile>().Setup(weaponSetting.damage, transform.parent.forward, throwForce);
        weaponSetting.currentAmmo--;
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }

    public void SetTrajectotyDelegate(TrajectoryDelegate _trajectoryCallback)
    {
        trajectoryCallback = _trajectoryCallback;
    }
}
