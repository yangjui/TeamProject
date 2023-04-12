using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Laserbullet : MonoBehaviour
{
    [SerializeField] private float damage;

    private void Start()
    {
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Zombie"))
        {
            BakeZombie zombie = _other.GetComponent<BakeZombie>();
            zombie.TakeDamage(damage);
            
            Vector3 Dir = transform.position - _other.transform.position;
            float angle = Vector3.SignedAngle(Dir, transform.forward, Vector3.up);

            //GameObject dummyPrefab = angle < 0 ? leftZombiePrefab : rightZombiePrefab;
            //Instantiate(dummyPrefab, _other.transform.position, Quaternion.identity);

            int dummyType = angle < 0 ? 1 : 2;

            zombie.DeadType(dummyType);
        }
    }
}