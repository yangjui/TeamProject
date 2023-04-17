using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    [Header("# Barricade HP")]
    [SerializeField] private float maxHp = 50;
    private float currentHp;
    [System.NonSerialized] public bool barricadeCollapse = false;
    private NavAgentManager navAgentManager;

    private void Awake()
    {
        currentHp = maxHp;
    }

    private void Start()
    {
        navAgentManager = FindObjectOfType<NavAgentManager>();
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
        // this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (this.name == "RightDoor")
            navAgentManager.BreakRightDoor();

        if (this.name == "LeftDoor")
            navAgentManager.BreakLeftDoor();
    }
}
