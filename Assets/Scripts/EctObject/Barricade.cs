using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    public delegate void WarningDelegate(string _name);
    private WarningDelegate warningCallback = null;

    [Header("# Barricade HP")]
    [SerializeField] private float maxHp = 50;
    
    private float currentHp;
    [System.NonSerialized] public bool barricadeCollapse = false;
    private NavAgentManager navAgentManager;
    private bool warningA = false;
    private bool warningB = false;

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
        Warning();

        if (currentHp == 0) BarricadeCollapse();
    }

    public void BarricadeCollapse()
    {
        Debug.Log("Collapse");
        barricadeCollapse = true;
        // this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    private void Warning()
    {
        if (maxHp >= currentHp * 2)
        {
            if (this.name == "RightDoor" && !warningA)
            {
                warningA = true;
                warningCallback?.Invoke(this.name);
            }
            else if (this.name == "LeftDoor" && !warningB)
            {
                warningB = true;
                warningCallback?.Invoke(this.name);
            }
        }
    }

    private void OnDestroy()
    {
        if (this.name == "RightDoor")
            navAgentManager.BreakRightDoor();

        if (this.name == "LeftDoor")
            navAgentManager.BreakLeftDoor();
    }

    public void SetWarningDelegate(WarningDelegate _warningCallback)
    {
        warningCallback = _warningCallback;
    }
}
