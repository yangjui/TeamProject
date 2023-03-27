using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("# Components")]
    [SerializeField] private WeaponAssaultRifle weapon;         // 현재 정보가 출력되는 무기

    [Header("# Weapon Base")]
    [SerializeField] private TextMeshProUGUI textWeaponName;    // 무기 이름
    [SerializeField] private Image weaponIcon;                  // 무기 아이콘
    [SerializeField] private Sprite[] spriteWeaponIcons;        // 무기 아이콘에 사용되는 sprite 배열

    [Header("Ammo")]
    [SerializeField] private TextMeshProUGUI textAmmo;          // 현재/최대 탄 수 출력 Text

    private void Awake()
    {
        SetupWeapon();

        // 메소드가 등록되어 있는 이벤트 클래스 (weapon.xx)의
        // Invoke() 메소드가 호출될 때 등록된 메소드(매개변수)가 실행된다.
        weapon.onAmmoEvent.AddListener(UpdateAmmoHUD);
    }

    private void SetupWeapon()
    {
        textWeaponName.text = weapon.WeaponName.ToString();
        weaponIcon.sprite = spriteWeaponIcons[(int)weapon.WeaponName];
    }

    private void UpdateAmmoHUD(int currentAmmo, int maxAmmo)
    {
        textAmmo.text = $"<size=40>{currentAmmo}/</size>{maxAmmo}";
    }
}
