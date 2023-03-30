using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    [System.NonSerialized] public List<Zombie> zombies;

    private void Awake()
    {
        Invoke("ZombieAwake", 2f);
    }
    private void ZombieAwake()
    {
        zombies = new List<Zombie>();

        foreach (Transform child in transform)
        {
            Zombie zombie = child.GetComponent<Zombie>();

            if (zombie != null)
            {
                zombies.Add(zombie);
            }
        }
    }
}
