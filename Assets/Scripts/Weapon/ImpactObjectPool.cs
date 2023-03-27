using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ImpactType { Platform = 0, Enemy, }
// 타격할 수 있는 대상을 지정하고 대상에 맞는 이펙트를 터트림

public class ImpactObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject[] impactPrefab;
    private ObjectPool[] objectPool;

    private void Awake()
    {
        objectPool = new ObjectPool[impactPrefab.Length];
        for (int i = 0; i < impactPrefab.Length; ++i)
        {
            objectPool[i] = new ObjectPool(impactPrefab[i]);
        }
    }

    public void SpawnImpact(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Platform"))
        {
            OnSpawnImpact(ImpactType.Platform, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if (hit.transform.CompareTag("Enemy"))
        {
            OnSpawnImpact(ImpactType.Enemy, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    public void OnSpawnImpact(ImpactType _type, Vector3 _position, Quaternion _rotation)
    {
        GameObject item = objectPool[(int)_type].ActivatePoolItem();
        item.transform.position = _position;
        item.transform.rotation = _rotation;
        item.GetComponent<Impact>().Setup(objectPool[(int)_type]);
    }
}
