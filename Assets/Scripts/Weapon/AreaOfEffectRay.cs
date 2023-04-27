using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffectRay : MonoBehaviour
{
    [SerializeField] private GameObject AOEPrefab = null;
    private RaycastHit hit;

    private void Start()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            if (hit.collider.CompareTag("Platform"))
            {
                Instantiate(AOEPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
}
