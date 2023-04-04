using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrenadeProjectile : MonoBehaviour
{
    [Header("# Explosion Barrel")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;
    [SerializeField] private float throwForce;

    private int explosionDamage;
    private Rigidbody rb;

    public void Setup(int _damage, Vector3 _rotation)
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(_rotation * throwForce);

        explosionDamage = _damage;
    }

    private void OnCollisionEnter(Collision _collision)
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            //// 플레이어가 맞았다면
            //PlayerController player = hit.GetComponent<PlayerController>();
            //if (player != null)
            //{
            //    player.TakeDamage((int)(explosionDamage * 0.2f));
            //    continue;
            //}
            //적이 맞았다면
            Zombie zombie = hit.GetComponent<Zombie>();
            if (zombie != null)
            {
                zombie.Takedamage(explosionDamage);
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
