using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bulletPerfab;
    public Transform spawnPoint;
    [SerializeField]
    private float bulletSpeed = 10f;
    [SerializeField]
    private float fireRate = 0.1f;
    [SerializeField]
    private float bulletLifetime = 2f;
    private int maxbullets = 30;

    private void Update()
    {
        
    }
}
