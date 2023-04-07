using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    private ParticleSystem particle;
    private ObjectPool objectPool;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public void Setup(ObjectPool _pool)
    {
        objectPool = _pool;
    }

    private void Update()
    {
        if (!particle.isPlaying)
        {
            objectPool.DeactivatePoolItem(gameObject);
        }
    }
}
