using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    [System.NonSerialized] public List<Zombie> zombies;

    public Vector3 playerPosition;

    private void Awake()
    {
        Invoke("ZombieAwake", 1f);
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

    private void Update()
    {
        foreach (Transform child in transform)
        {
            Zombie zombie = child.GetComponent<Zombie>();

            zombie.PlayerPosition(playerPosition);
        }
    }

    public void PlayerPosition(Vector3 _position)
    {
        playerPosition = _position;
    }
}
