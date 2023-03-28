using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    private WeaponBase weapon;

    [Header("# Weapon Base")]
    [SerializeField] private TextMeshProUGUI textWeaponName;    // 무기 이름
    [SerializeField] private Image weaponIcon;                  // 무기 아이콘
    [SerializeField] private Sprite[] spriteWeaponIcons;        // 무기 아이콘에 사용되는 sprite 배열
    [SerializeField] private Vector2[] sizeWeaponIcons;         // 무기 아이콘의 UI 크기 배열

    [Header("# Ammo")]
    [SerializeField] private TextMeshProUGUI textAmmo;          // 현재/최대 탄 수 출력 Text

    [Header("# Magazine")]
    [SerializeField] private GameObject magazineUIPrefab;       // 탄창 UI 프리팹
    [SerializeField] private Transform magazineParent;          // 탄창 UI가 배치되는 Panel
    [SerializeField] private int maxMagazineCount;              // 처음 생성하는 최대 탄창 수

    [Header("# AimMode")]
    [SerializeField] private Image[] aimMode;

    private List<GameObject> magazineList;                      // 탄창 UI 리스트

    public void ChangeAimMode(bool _aimMode) // 에임모드에 따라 에임 변경
    {
        if (_aimMode)
        {
            aimMode[0].enabled = false;
            aimMode[1].enabled = true;
        }
        else if (!_aimMode)
        {
            aimMode[0].enabled = true;
            aimMode[1].enabled = false;
        }
    }

    public void SetupAllWeapons(WeaponBase[] _weapons)
    {
        SetupMagazine();

        for (int i = 0; i < _weapons.Length; ++i)
        {
            _weapons[i].onAmmoEvent.AddListener(UpdateAmmoHUD);
            _weapons[i].onMagazineEvent.AddListener(UpdateMagazineHUD);
        }
    }

    public void SwitchingWeapon(WeaponBase _newWeapon)
    {
        weapon = _newWeapon;
        SetupWeapon();
    }

    private void SetupWeapon()
    {
        textWeaponName.text = weapon.WeaponName.ToString();
        weaponIcon.sprite = spriteWeaponIcons[(int)weapon.WeaponName];
        weaponIcon.rectTransform.sizeDelta = sizeWeaponIcons[(int)weapon.WeaponName];
    }

    private void SetupMagazine()
    {
        magazineList = new List<GameObject>();
        for (int i = 0; i < maxMagazineCount; ++i)
        {
            GameObject clone = Instantiate(magazineUIPrefab);
            clone.transform.SetParent(magazineParent);
            clone.SetActive(false);

            magazineList.Add(clone);
        }
    }

    private void UpdateAmmoHUD(int _currentAmmo, int _maxAmmo)
    {
        textAmmo.text = $"<size=40>{_currentAmmo}/</size>{_maxAmmo}";
    }

    private void UpdateMagazineHUD(int _currentMagazine)
    {
        // 전체를 비활성화 한 후 현재 탄창 수 만큼 활성화 시킨다.
        for (int i = 0; i < magazineList.Count; ++i)
        {
            magazineList[i].SetActive(false);
        }
        for (int i = 0; i < _currentMagazine; ++i)
        {
            magazineList[i].SetActive(true);
        }
    }

}
