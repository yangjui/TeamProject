
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
    private GameObject groundEffectPrefab;
    [SerializeField]
    private Transform chargeEffectLocation;
    [SerializeField]
    private Transform fireEffectLocation;

    private GameObject chargeEffectInstance;

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
            if (chargeEffectPrefab != null)
            {
                chargeEffectInstance = Instantiate(chargeEffectPrefab, chargeEffectLocation.position,
                     chargeEffectLocation.rotation);

                chargeEffectInstance.transform.localScale /= 4f;


            }

        }
        if (Input.GetMouseButton(0))
        {
            currentChargingTime += Time.deltaTime;
        }
       // Debug.Log(" ÃæÀüÁß ... " + currentChargingTime);

        if (Input.GetMouseButtonUp(0))
        {
            if (currentChargingTime >= chargingTime)
            {
                //Debug.DrawRay(transform.position, transform.forward * Maxrange, Color.red, 0.5f);
                float sphereScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z) * 2;
                hits = Physics.SphereCastAll(transform.position, sphereScale, 
                    transform.forward, Maxrange);

                Destroy(chargeEffectInstance);
                Fire();
            }
            else Destroy(chargeEffectInstance);

            currentChargingTime = 0f;
        }
    }
    private void Fire()
    {
        if (fireEffectPrefab != null)
        {
            GameObject fire = Instantiate(fireEffectPrefab, fireEffectLocation.position, transform.rotation);
            fire.transform.localScale *= 2f;
            LineRenderer lineRenderer = fire.GetComponent<LineRenderer>();


            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                //MeshRenderer ChangeColor = hit.transform.GetComponent<MeshRenderer>();

                //if (ChangeColor)
                //{
                //    hit.transform.GetComponent<MeshRenderer>().material.color = Color.red;
                //}

                
                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }


                if (lineRenderer != null)
                {
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, hit.point);
                }
                //Vector3 startPos = new Vector3(0f, -1f, 0f);
                //Vector3 endPos = new Vector3(0f, -1f, 0f);


            }

            //Destroy(ground, 3f);
            Destroy(fire, 1.5f);
        }
    }


}