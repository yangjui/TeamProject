using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffect : MonoBehaviour
{
    private List<GameObject> agents = new List<GameObject>();

    private void Start()
    {
        Invoke(nameof(Bomb), 0.1f);   
    }

    private void Bomb()
    {
        for (int i = 0; i < agents.Count; ++i)
        {
            agents[i].GetComponent<Rigidbody>().AddExplosionForce(5000, transform.position, 5f);
            CameraController.instance.StartShakeCamera();
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Dead"))
        {
            agents.Add(_other.gameObject);
        }
    }
}
