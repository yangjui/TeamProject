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
            int dummyType;
            BakeZombie zombie = _other.GetComponent<BakeZombie>();
            zombie.TakeDamage(damage);

            Vector3 Dir = transform.position - _other.transform.position; // _other를 바라보는 방향
            float angle = Vector3.SignedAngle(Dir, transform.forward, Vector3.up);

            if (Mathf.Abs(angle) > 160) dummyType = 3;
            else dummyType = angle < 0 ? 1 : 2;

            zombie.DeadType(dummyType);
        }
    }
}