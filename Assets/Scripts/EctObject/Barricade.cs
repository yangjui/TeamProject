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
    private int currentHealth;
    private float damageThreshold = 0.1f;
    private bool round2 = false;

    public void Round2Start(bool _bool)
    {
        round2 = _bool;
    }

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
        if (!round2) return;
        currentHp -= _damage;
        BreakSound();
        Warning();

        if (currentHp == 0) BarricadeCollapse();
    }

    private void BreakSound()
    {
        float healthPercentage = (float)currentHealth / (float)maxHp;

        if (healthPercentage % damageThreshold == 0 && healthPercentage < 1f)
        {
            int random = Random.Range(0, 6);
            switch(random)
            {
                case 0: 
                    SoundManager.instance.Play3DSFX("lift_door_bangs_1", transform.position);
                    break;
                case 1:
                    SoundManager.instance.Play3DSFX("lift_door_bangs_2", transform.position);
                    break;
                case 2:
                    SoundManager.instance.Play3DSFX("lift_door_bangs_3", transform.position);
                    break;
                case 3:
                    SoundManager.instance.Play3DSFX("lift_door_bangs_4", transform.position);
                    break;
                case 4:
                    SoundManager.instance.Play3DSFX("lift_door_bangs_5", transform.position);
                    break;
                case 5:
                    SoundManager.instance.Play3DSFX("lift_door_bangs_6", transform.position);
                    break;
            }
            Debug.Log("Health percentage: " + healthPercentage);
        }
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

        SoundManager.instance.Play3DSFX("closing_metal_door_2", transform.position);
        SoundManager.instance.SFX3DVolumeControl("closing_metal_door_2", 0.5f);
    }

    public void SetWarningDelegate(WarningDelegate _warningCallback)
    {
        warningCallback = _warningCallback;
    }
}
