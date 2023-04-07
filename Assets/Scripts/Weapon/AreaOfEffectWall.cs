using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffectWall : MonoBehaviour
{
    [SerializeField] private GameObject AOEPrefab = null;
   
    private void Start()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, 25f);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Platform"))
            {
                Instantiate(AOEPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                break;
            }
        }
    }
}
