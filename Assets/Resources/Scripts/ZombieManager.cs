using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    [System.NonSerialized] public List<Zombie> zombies;

    private void Awake()
    {
        zombies = new List<Zombie>();

        foreach (Transform child in transform)
        {
            Zombie zombie = child.GetComponent<Zombie>();
            Debug.Log(zombie);
            if (zombie != null)
            {
                zombies.Add(zombie);
            }
        }
    }
}
