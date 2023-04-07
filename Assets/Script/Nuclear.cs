using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuclear : MonoBehaviour
{
    [SerializeField]
    private GameObject markPrefab;
    [SerializeField]
    private GameObject bombPrefab;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private float explosionRadius = 10f;
    [SerializeField]
    private float explosionRange = 5f;
    [SerializeField]
    private float drophighest = 50;
    [SerializeField]
    private float bombSpeed = 100f;
    [SerializeField]
    float MaxDistance = 20f;
    [SerializeField]
    private int explosionDamage = 100;

    public LayerMask LayerMask;
    private RaycastHit hit;  

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, MaxDistance, LayerMask))
        {
                Debug.Log(hit.collider.gameObject.name);
            Debug.DrawLine(transform.position, hit.transform.forward, Color.red);
            if (hit.collider.gameObject.tag == "Floor")
            {
                GameObject mark = Instantiate(markPrefab, hit.point, Quaternion.identity);
                Destroy(mark, 4f);

                Vector3 bombPos = mark.transform.position + Vector3.up * drophighest;

                Invoke("CreatBomb", 4f);
          
            }
        }
    }

    private void CreatBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, hit.point + Vector3.up * drophighest, bombPrefab.transform.rotation);
        Vector3 dir = hit.point - bomb.transform.position;
       bomb.GetComponent<Rigidbody>().AddForce(dir * 5f, ForceMode.Impulse);
    }


}
