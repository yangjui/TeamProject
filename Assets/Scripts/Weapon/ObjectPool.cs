using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private class PoolItem
    {
        public bool isActive;                   // ���ӿ�����Ʈ�� Ȱ��ȭ / ��Ȱ��ȭ ����
        public GameObject gameObject;           // ȭ�鿡 ���̴� ���� ���ӿ�����Ʈ
    }

    private int increaseCount = 5;              // ������Ʈ�� ������ �� �߰��� �����Ǵ� ������Ʈ ����
    private int maxCount;                       // ���� ����Ʈ�� ��ϵǾ� �ִ� ������Ʈ ����
    private int activeCount;                    // ���� ���ӿ� ���ǰ� �ִ�(Ȱ��ȭ) ������Ʈ ����

    private GameObject poolObject;              // ������Ʈ Ǯ������ �����ϴ� ���� ������Ʈ ������
    private List<PoolItem> poolItemList;        // �����Ǵ� ��� ������Ʈ�� �����ϴ� ����Ʈ

    public int MaxCount => maxCount;            // �ܺο��� ���� ����Ʈ�� ��ϵǾ� �ִ� ������Ʈ ���� Ȯ���� ���� ������Ƽ
    public int ActiveCount => activeCount;      // �ܺο��� ���� Ȱ��ȭ �Ǿ� �ִ� ������Ʈ ���� Ȯ���� ���� ������Ƽ

    public ObjectPool(GameObject _poolObject)   // ������
    {
        maxCount = 0;
        activeCount = 0;
        this.poolObject = _poolObject;

        poolItemList = new List<PoolItem>();
        InstantiateObjects();
    }

    public void InstantiateObjects()            // increaseCount ������ ������Ʈ�� �������ش�.
    {
        maxCount += increaseCount;

        for (int i = 0; i < increaseCount; ++i)
        {
            PoolItem poolItem = new PoolItem();

            poolItem.isActive = false;         
            poolItem.gameObject = GameObject.Instantiate(poolObject);
            poolItem.gameObject.SetActive(false);   // ���� �� ������� ���� �� �ֱ⿡ false�� �����Ͽ� ������ �ʰ� ���ش�.

            poolItemList.Add(poolItem);
        }
    }

    public void DestroyObject()                 // ���� �������� ��� ������Ʈ ����, ���� �ٲٰų� ������ �����Ҷ� �ѹ��� ���۽�Ų��.
    {
        if (poolItemList == null) return;
        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }
        poolItemList.Clear();
    }

    public GameObject ActivatePoolItem()        // ��� ������Ʈ�� Ȱ��ȭ ���¶�� InstantiateObjects �Լ� ȣ���� �߰� ����
    {
        if (poolItemList == null) return null;

        if (maxCount == activeCount) InstantiateObjects();

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.isActive == false)
            {
                activeCount++;

                poolItem.isActive = true;
                poolItem.gameObject.SetActive(true);

                return poolItem.gameObject;     // �ܺο��� ����ϵ��� ������Ʈ ��ȯ
            }
        }

        return null;
    }

    public void DeactivatePoolItem(GameObject _removeObject)    // ���� ����� �Ϸ�� ������Ʈ�� ��Ȱ��ȭ ������
    {
        if (poolItemList == null || _removeObject == null) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject == _removeObject)
            {
                activeCount--;

                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);

                return;
            }
        }
    }

    public void DeactivateAllPoolItems()        // ���ӿ��� ������� ��� ������Ʈ�� ��Ȱ��ȭ
    {
        if (poolItemList == null) return;
        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject != null && poolItem.isActive == true)
            {
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
            }
        }
        activeCount = 0;
    }
}
