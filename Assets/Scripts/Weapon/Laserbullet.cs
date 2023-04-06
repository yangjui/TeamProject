using System.Collections;
using System.Collections.Generic;
using System.Threading;
//using Unity.Burst.CompilerServices;
using UnityEngine;

public class Laserbullet : MonoBehaviour
{
    [SerializeField]
    private GameObject groundEffectPrefab;
    [SerializeField] private float damage;

    [SerializeField]
    private GameObject leftZombiePrefab;
    [SerializeField]
    private GameObject rightZombiePrefab;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Platform"))
        {
            //Debug.Log("Platform");
            //Vector3 groundPosition = _other.ClosestPoint(transform.position);
            //Quaternion rotation = transform.rotation;
            //rotation.y += 90f;
            //transform.rotation = rotation;
            //GameObject ge = Instantiate(groundEffectPrefab, groundPosition, transform.rotation);
            //ge.transform.localScale /= 2f;

            groundEffectPrefab.SetActive(true);
        }

        if (_other.CompareTag("Zombie"))
        {
            Zombie zombie = _other.GetComponent<Zombie>();
            zombie.TakeDamage(damage);
            
            Vector3 Dir = transform.position - _other.transform.position;
            float angle = Vector3.SignedAngle(Dir, transform.forward, Vector3.up);

            GameObject dummyPrefab = angle < 0 ? leftZombiePrefab : rightZombiePrefab;
            Instantiate(dummyPrefab, _other.transform.position, Quaternion.identity);

            //bool dummyPrefab = angle < 0 ? true : false;
            //zombie.레이저좀비소환(dummyPrefab);
        }
    }
}