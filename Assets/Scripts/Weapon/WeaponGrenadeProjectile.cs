using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrenadeProjectile : MonoBehaviour
{
    [Header("# Explosion Barrel")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;

    private int explosionDamage;
    private Rigidbody rb;

    public void Setup(int _damage, Vector3 _rotation, float _throwForce)
    {
        rb = GetComponent<Rigidbody>();

        // 초기 속도 벡터 계산
        Vector3 velocity = Quaternion.AngleAxis(0, transform.right) * _rotation * _throwForce;

        // Rigidbody에 힘을 가해서 수류탄 이동 시작
        rb.AddForce(velocity, ForceMode.VelocityChange);

        explosionDamage = _damage;
    }

    private void OnCollisionEnter(Collision _collision)
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            BakeZombie zombie = hit.GetComponent<BakeZombie>();
            if (zombie != null)
            {
                zombie.TakeDamage(explosionDamage);
                continue;
            }
            
            // Rigidbody를 가지고 있는 오브젝트가 맞았다면
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        Destroy(gameObject);
    }
}
