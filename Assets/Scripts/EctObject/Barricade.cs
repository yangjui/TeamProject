using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    [Header("# Barricade HP")]
    [SerializeField] private float maxHp = 50;
    private float currentHp;
    [System.NonSerialized] public bool barricadeCollapse = false;

    private void Awake()
    {
        currentHp = maxHp;
    }

    public void BarricadeHP(float _damage)
    {
        currentHp -= _damage;

        if (currentHp == 0) BarricadeCollapse();
    }

    public void BarricadeCollapse()
    {
        Debug.Log("Collapse");
        barricadeCollapse = true;
        this.gameObject.SetActive(false);
        // Destroy(this.gameObject);
    }
}
