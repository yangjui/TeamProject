using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CasingObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject casingPrefab;
    private ObjectPool objectPool;

    private void Awake()
    {
        objectPool = new ObjectPool(casingPrefab);
    }

    public void SpawnCasing(Vector3 _position, Vector3 _direction)
    {
        GameObject item = objectPool.ActivatePoolItem();            // 해당 오브젝트중 비활성화 된 오브젝트 하나를 찾아서 활성화
        item.transform.position = _position;                        // 위치 설정
        item.transform.rotation = Random.rotation;                  // 회전값 설정
        item.GetComponent<Casing>().Setup(objectPool, _direction);  // Setup 동작
    }
}
