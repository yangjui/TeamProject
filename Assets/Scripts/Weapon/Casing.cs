using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casing : MonoBehaviour
{
    [SerializeField] private float deactivateTime;
    [SerializeField] private float casingSpin;

    private Rigidbody rb;
    private ObjectPool objectPool;

    public void Setup(ObjectPool _pool, Vector3 _direction)
    {
        rb = GetComponent<Rigidbody>();
        objectPool = _pool;

        rb.velocity = new Vector3(_direction.x, 1f, _direction.z);
        rb.angularVelocity = new Vector3(Random.Range(-casingSpin, casingSpin), 
                                         Random.Range(-casingSpin, casingSpin), 
                                         Random.Range(-casingSpin, casingSpin));

        StartCoroutine("DeactivateAfterTime");
    }

    private void OnCollisionEnter(Collision _collision)
    {
        int random = Random.Range(0, 5);
        switch (random)
        {
            case 0:
                SoundManager.instance.Play3DSFX("casing-sound-1", transform.position);
                break;
            case 1:
                SoundManager.instance.Play3DSFX("casing-sound-2", transform.position);
                break;
            case 2:
                SoundManager.instance.Play3DSFX("casing-sound-3", transform.position);
                break;
            case 3:
                SoundManager.instance.Play3DSFX("casing-sound-4", transform.position);
                break;
            case 4:
                SoundManager.instance.Play3DSFX("casing-sound-5", transform.position);
                break;
        }
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(deactivateTime);
        objectPool.DeactivatePoolItem(this.gameObject);
    }
}
