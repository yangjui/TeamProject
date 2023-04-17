using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoleObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletHolePrefab;

    private ObjectPool objectPool;

    private void Awake()
    {
        objectPool = new ObjectPool(bulletHolePrefab);
    }

    public void SpawnImpact(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Platform"))
        {
            OnSpawnImpact(hit.point + hit.transform.up * 0.001f, Quaternion.LookRotation(hit.normal));
        }
    }

    public void OnSpawnImpact(Vector3 _position, Quaternion _rotation)
    {
        GameObject item = objectPool.ActivatePoolItem();
        item.transform.position = _position;
        item.transform.rotation = _rotation;
        item.GetComponent<BulletHole>().Setup(objectPool);
    }
}
