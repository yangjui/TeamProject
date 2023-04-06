using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField]
    private float fireRate = 0.1f;
    [SerializeField]
    private float Maxrange = 10f;
    [SerializeField]
    private int damage = 30;
    [SerializeField]
    private Vector3[] pattern;
        
   

    RaycastHit hit;

    private float nextFireTime;
    private int patternIndex = 0;

    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y, transform.position.z + 0.1f);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y, transform.position.z - 0.1f);
        }

        if(Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Debug.DrawRay(transform.position, transform.forward * Maxrange,
                Color.red, 0.5f);

            Fire();
            //if (Physics.Raycast(transform.position, transform.forward,
            //    out hit, Maxrange))
            //{
            //    hit.transform.GetComponent<MeshRenderer>().material.color = Color.red;
            //}
        }

    }

    private void Fire()
    {
        RaycastHit hit;

        Vector3 direction = pattern[patternIndex % pattern.Length];
        patternIndex++;

        if (Physics.Raycast(transform.position,transform.forward, out hit, Maxrange))
        {
            Debug.Log("hit!!!!!!!!" + hit.collider.gameObject.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null) 
            {
                target.TakeDamage(damage);
            }

        }

        nextFireTime = Time.time + fireRate;
    }

}
