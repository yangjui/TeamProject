using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingZombie : MonoBehaviour
{
    [SerializeField] List<GameObject> path = null;
    private int k = 0;
    private float speed;
    [SerializeField] private float zombieHealth = 100f;
    [SerializeField] private GameObject deadRagdoll;

    private void Start()
    {
        speed = Random.Range(3f, 6f);
    }

    private void Update()
    {
        this.transform.position = Vector3.MoveTowards(transform.position, path[k].transform.position, speed * Time.deltaTime);
        this.transform.LookAt(path[k].transform.position);
    }

    public void ChangePath()
    {
        if (k >= 5)
        {
            k = 0;
        }
        else
        {
            k += 1;
        }
    }
}
