using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private GameObject fireEffect = null;
    private float currentHealth = 500f;
    public void Onfire()
    {
        if (!fireEffect.activeSelf)
        {
            fireEffect.SetActive(true);
            StartCoroutine(ApplyDamageOverTime(20));
        }
    }

    IEnumerator ApplyDamageOverTime(float damage)
    {
        while (currentHealth > 0)
        {
            yield return new WaitForSeconds(1f);

            TakeDamage((float)damage);
        }
    }

    public void TakeDamage(float playerAttackDamage)
    {
        currentHealth -= playerAttackDamage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
