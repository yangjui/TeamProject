using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private float fireRate = 0.1f;
    [SerializeField]
    private float range = 100f;
    [SerializeField]
    private int damage = 50;
    [SerializeField]
    private Transform muzzle;

    private float nextFireTime;

    private void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = new Vector3(transform.position.x + 0.1f,
                transform.position.y, transform.position.z);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = new Vector3(transform.position.x - 0.1f,
                transform.position.y, transform.position.z);
        }

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime) 
        {
            Debug.DrawRay(transform.position, transform.forward * range, Color.red, 0.5f);
           // nextFireTime = Time.time + 1f / fireRate;
           Fire();
        }
    }

    private void Fire()
    {
        RaycastHit hit;
        if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, range))
        {
            Debug.Log("Clicked on" + hit.collider.gameObject.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null) 
            {
                target.TakeDamage(damage);
            }
        }
    }

}
