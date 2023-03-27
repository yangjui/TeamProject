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
        GameObject item = objectPool.ActivatePoolItem();            // �ش� ������Ʈ�� ��Ȱ��ȭ �� ������Ʈ �ϳ��� ã�Ƽ� Ȱ��ȭ
        item.transform.position = _position;                        // ��ġ ����
        item.transform.rotation = Random.rotation;                  // ȸ���� ����
        item.GetComponent<Casing>().Setup(objectPool, _direction);  // Setup ����
    }
}
