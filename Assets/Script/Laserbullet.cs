using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Laserbullet : MonoBehaviour
{
    [SerializeField]
    private GameObject groundEffectPrefab;

    [SerializeField]
    private GameObject leftZombiePrefab;
    [SerializeField]
    private GameObject rightZombiePrefab;


    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Floor"))
        {
            Debug.Log("Floor");

            Vector3 groundPosition = _other.ClosestPoint(transform.position);
          GameObject ge = Instantiate(groundEffectPrefab, groundPosition, Quaternion.identity);
            ge.transform.localScale /= 2f;

            Destroy(ge, 4f);
        }
        if (_other.CompareTag("Zombie"))
        {
            Debug.Log("Zombie!");


        }
    }


}
