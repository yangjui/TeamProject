using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lasertest : MonoBehaviour
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

    [SerializeField]
    private GameObject chargeEffectPrefab;
    [SerializeField]
    private GameObject fireEffectPrefab;
    [SerializeField]
    private Transform chargeEffectLocation;
    [SerializeField]
    private GameObject fireEffectLocation;

    RaycastHit[] hits;

    private float nextFireTime;


    private void Update()
    {
        //if (nextFireTime == 0f)
        //{
            if (Input.GetMouseButtonDown(0))
            {
                //if (currentChargingTime >= chargingTime)
                //{
                    currentChargingTime = 0f;
                //}

            }
            if (Input.GetMouseButton(0))
            {
                currentChargingTime += Time.deltaTime;
            }
            Debug.Log(" √Ê¿¸¡ﬂ ... " + currentChargingTime);

            if (Input.GetMouseButtonUp(0))
            {
            // Fire();

            if (currentChargingTime < chargingTime)
            {
                currentChargingTime = 0f;
            }
            else 
            {
                Debug.DrawRay(transform.position, transform.forward * Maxrange, Color.red, 0.5f);
                hits = Physics.RaycastAll(transform.position, transform.forward, Maxrange);

                Fire();
            }

           // }
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
       
        nextFireTime = Time.time + fireRate;
    }

}
