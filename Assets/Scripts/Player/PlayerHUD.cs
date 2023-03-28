using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    private WeaponBase weapon;

    [Header("# Weapon Base")]
    [SerializeField] private TextMeshProUGUI textWeaponName;    // ���� �̸�
    [SerializeField] private Image weaponIcon;                  // ���� ������
    [SerializeField] private Sprite[] spriteWeaponIcons;        // ���� �����ܿ� ���Ǵ� sprite �迭
    [SerializeField] private Vector2[] sizeWeaponIcons;         // ���� �������� UI ũ�� �迭

    [Header("# Ammo")]
    [SerializeField] private TextMeshProUGUI textAmmo;          // ����/�ִ� ź �� ��� Text

    [Header("# Magazine")]
    [SerializeField] private GameObject magazineUIPrefab;       // źâ UI ������
    [SerializeField] private Transform magazineParent;          // źâ UI�� ��ġ�Ǵ� Panel
    [SerializeField] private int maxMagazineCount;              // ó�� �����ϴ� �ִ� źâ ��

    [Header("# AimMode")]
    [SerializeField] private Image[] aimMode;

    private List<GameObject> magazineList;                      // źâ UI ����Ʈ

    public void ChangeAimMode(bool _aimMode) // ���Ӹ�忡 ���� ���� ����
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
        // ��ü�� ��Ȱ��ȭ �� �� ���� źâ �� ��ŭ Ȱ��ȭ ��Ų��.
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
