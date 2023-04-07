using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponName { ���ݼ��� = 0, ������������, ����ź, �߷���ź }

[System.Serializable]
public struct WeaponSetting
{
    public WeaponName weaponName;   // ���� �̸�
    public int currentMagazine;     // ���� źâ ��
    public int maxMagazine;         // �ִ� źâ ��
    public int currentAmmo;         // ���� ź�� ��
    public int maxAmmo;             // �ִ� ź�� ��
    public float attackRate;        // ���� �ӵ�
    public float attackDistance;    // ���� ��Ÿ�
    public bool isAutomaticAttack;  // ���Ӱ��� ����
    public int damage;
}
