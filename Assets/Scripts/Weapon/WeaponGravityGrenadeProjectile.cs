using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGravityGrenadeProjectile : MonoBehaviour
{
    [Header("# Explosion Barrel")]
    [SerializeField] private GameObject blackholePrefab;
    private float throwForce;

    private Rigidbody rb;

    public void Setup(int _damage, Vector3 _rotation, float _throwForce)
    {
        ThrowForce(_throwForce);
        rb = GetComponent<Rigidbody>();
        rb.AddForce(_rotation * throwForce * 50);
    }

    public void ThrowForce(float _throwForce)
    {
        throwForce = _throwForce;
    }

    private void OnCollisionEnter(Collision _collision)
    {
        Instantiate(blackholePrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}