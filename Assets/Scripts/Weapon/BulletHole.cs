using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletHole : MonoBehaviour
{
    [SerializeField] private Sprite[] bulletHoleImage;
    [SerializeField] private float deactivateTime;

    private SpriteRenderer image;
    private ObjectPool objectPool;

    private void Awake()
    {
        image = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        int random = Random.Range(0, bulletHoleImage.Length);
        image.sprite = bulletHoleImage[random];
    }

    public void Setup(ObjectPool _pool)
    {
        objectPool = _pool;
        StartCoroutine(DeactivateAfterTime());
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(deactivateTime);
        objectPool.DeactivatePoolItem(this.gameObject);
    }
}
