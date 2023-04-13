using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitchingSystem : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private WeaponBase[] weapons;

    [HideInInspector]
    public PlayerHUD playerHUD;
    private WeaponBase currentWeapon;
    private WeaponBase previousWeapon;
    private WeaponAssaultRifle weaponAssaultRifle;
    private int index = 0;
    private float timer = 0f;
    private float currentTime = 0f;

    private void Awake()
    {
        weaponAssaultRifle = GetComponentInChildren<WeaponAssaultRifle>();
    }

    public void Init()
    {
        playerHUD.SetupAllWeapons(weapons);

        for (int i = 0; i < weapons.Length; ++i)
        {
            if (weapons[i].gameObject != null)
            {
                weapons[i].gameObject.SetActive(false);
            }
        }
        SwitchingWeapon(WeaponType.���ݼ���);
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;
        UpdateSwitch();
    }

    private void UpdateSwitch()
    {
        if (/*!Input.anyKeyDown || */weaponAssaultRifle.isAimMode) return;

        int inputIndex = 0;
        // �Էµ� ���� 1~4 ������ ���ڶ�� inputIndex�� �Է°��� ����ǰ�
        // �Է°��� �´� ���Ⱑ ȣ��ȴ�.
        if (int.TryParse(Input.inputString, out inputIndex) && (inputIndex > 0 && inputIndex < 5))
        {
            SwitchingWeapon((WeaponType)(inputIndex - 1));
            index = inputIndex - 1;
        }

        timer = Time.time;
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && timer - currentTime > 0.2f)
        {
            index++;
            if (index > 3) index = 0;
            SwitchingWeapon((WeaponType)(index));
            currentTime = timer;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && timer - currentTime > 0.2f)
        {
            index--;
            if (index < 0)
                index = 3;
            SwitchingWeapon((WeaponType)(index));
            currentTime = timer;
        }
    }

    private void SwitchingWeapon(WeaponType _weaponType)
    {
        if (weapons[(int)_weaponType] == null) return;

        if (currentWeapon != null)
        {
            previousWeapon = currentWeapon;
        }
        // ���ⱳü
        currentWeapon = weapons[(int)_weaponType];

        // ���� ����� ���� ���Ⱑ ���ٸ� return
        if (currentWeapon == previousWeapon) return;

        // ���� ���� ���� ����
        playerController.SwitchingWeapon(currentWeapon);
        playerHUD.SwitchingWeapon(currentWeapon);

        if (previousWeapon != null)
        {
            // ������ ����ϴ� ���� ��Ȱ��ȭ
            previousWeapon.gameObject.SetActive(false);
        }

        // ���� ����ϴ� ���� Ȱ��ȭ
        currentWeapon.gameObject.SetActive(true);
    }
}
