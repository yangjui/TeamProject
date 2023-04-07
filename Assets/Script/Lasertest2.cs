
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lasertest2 : MonoBehaviour
{
    [SerializeField]
    private float fireRate = 5f;
    [SerializeField]
    private float Maxrange = 10f;
    [SerializeField]
    private float chargingTime = 2f;
    [SerializeField]
    private float currentChargingTime = 0f;
    [SerializeField]
    private int damage = 100;

    RaycastHit[] hits;

    private float nextFireTime = 0f;


    private void Update()
    {
        if (nextFireTime == 0f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentChargingTime += Time.deltaTime;
                Debug.Log(" ÃæÀüÁß ... " + currentChargingTime);
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (currentChargingTime >= chargingTime)
                {
                    Debug.DrawRay(transform.position, transform.forward * Maxrange, Color.red, 0.5f);
                    hits = Physics.RaycastAll(transform.position, transform.forward, Maxrange);
                    Fire();
                }
            }
        }
        else if (nextFireTime > 0f)
        {
            nextFireTime -= Time.deltaTime;
            if (nextFireTime <= 0f)
            {
                nextFireTime = 0f;
            }
        }


    }

    private void Fire()
    {
        RaycastHit[] hits;

        hits = Physics.RaycastAll(transform.position, transform.forward, Maxrange);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            MeshRenderer ChangeColor = hit.transform.GetComponent<MeshRenderer>();

            if (ChangeColor)
            {
                hit.transform.GetComponent<MeshRenderer>().material.color = Color.red;
            }

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }

        currentChargingTime = 0f;
        nextFireTime = 5f;
    }

}

