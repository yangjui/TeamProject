using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private class PoolItem
    {
        public bool isActive;                   // 게임오브젝트의 활성화 / 비활성화 정보
        public GameObject gameObject;           // 화면에 보이는 실제 게임오브젝트
    }

    private int increaseCount = 5;              // 오브젝트가 부족할 때 추가로 생성되는 오브젝트 개수
    private int maxCount;                       // 현재 리스트에 등록되어 있는 오브젝트 개수
    private int activeCount;                    // 현재 게임에 사용되고 있는(활성화) 오브젝트 개수

    private GameObject poolObject;              // 오브젝트 풀링에서 관리하는 게임 오브젝트 프리팹
    private List<PoolItem> poolItemList;        // 관리되는 모든 오브젝트를 저장하는 리스트

    public int MaxCount => maxCount;            // 외부에서 현재 리스트에 등록되어 있는 오브젝트 개수 확인을 위한 프로퍼티
    public int ActiveCount => activeCount;      // 외부에서 현재 활성화 되어 있는 오브젝트 개수 확인을 위한 프로퍼티

    public ObjectPool(GameObject _poolObject)   // 생성자
    {
        maxCount = 0;
        activeCount = 0;
        this.poolObject = _poolObject;

        poolItemList = new List<PoolItem>();
        InstantiateObjects();
    }

    public void InstantiateObjects()            // increaseCount 단위로 오브젝트를 생성해준다.
    {
        maxCount += increaseCount;

        for (int i = 0; i < increaseCount; ++i)
        {
            PoolItem poolItem = new PoolItem();

            poolItem.isActive = false;         
            poolItem.gameObject = GameObject.Instantiate(poolObject);
            poolItem.gameObject.SetActive(false);   // 생성 후 사용하지 않을 수 있기에 false로 설정하여 보이지 않게 해준다.

            poolItemList.Add(poolItem);
        }
    }

    public void DestroyObject()                 // 현재 관리중인 모든 오브젝트 삭제, 씬을 바꾸거나 게임을 종료할때 한번만 동작시킨다.
    {
        if (poolItemList == null) return;
        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }
        poolItemList.Clear();
    }

    public GameObject ActivatePoolItem()        // 모든 오브젝트가 활성화 상태라면 InstantiateObjects 함수 호출해 추가 생성
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

                return poolItem.gameObject;     // 외부에서 사용하도록 오브젝트 반환
            }
        }

        return null;
    }

    public void DeactivatePoolItem(GameObject _removeObject)    // 현재 사용이 완료된 오브젝트를 비활성화 시켜줌
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

    public void DeactivateAllPoolItems()        // 게임에서 사용중인 모든 오브젝트를 비활성화
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
