using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGravityGrenadeProjectile : MonoBehaviour
{
    [Header("# Explosion Barrel")]
    [SerializeField] private GameObject blackholePrefab;

    private Rigidbody rb;

    public void Setup(int _damage, Vector3 _rotation, float _throwForce)
    {
        rb = GetComponent<Rigidbody>();

        // 초기 속도 벡터 계산
        Vector3 velocity = Quaternion.AngleAxis(0, transform.right) * _rotation * _throwForce;

        // Rigidbody에 힘을 가해서 수류탄 이동 시작
        rb.AddForce(velocity, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision _collision)
    {
        Instantiate(blackholePrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}