using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    GameObject fireEffect ;
    [SerializeField]
    private float damage = 20f;
    [SerializeField]
    private float hp = 100f;

    
    public void Onfire()
    {
        fireEffect.SetActive(true);
        StartCoroutine(ApplyDamageOverTime(damage));
    }

    IEnumerator ApplyDamageOverTime(float damage)
    {
        while (hp > 0)
        {
            yield return new WaitForSeconds(1f);

            TakeDamage((float) damage);

            if (hp <= 0) break;
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        StopCoroutine(ApplyDamageOverTime(damage));
        Destroy(gameObject);
    }

    public float HiveHP()
    {
        return hp;
    }

}
