using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField]
    private int damage = 10;
    [SerializeField]
    private float damageInterval = 1f;

    private float nextdamageTime;

    private List<GameObject> Zombies = new List<GameObject>();

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Zombie"))
        {
            Zombies.Add(_other.gameObject);
        }
    }

    private void OnTriggerStay(Collider _other)
    {
        if (_other.CompareTag("Zombie") && Time.time >= nextdamageTime )
        {
            Debug.Log(Zombies.Count);
            if (Zombies.Count <= 0) return;
           
                for (int i = Zombies.Count - 1; i >= 0; i--)
                {
                Debug.Log(Zombies[i].name + " hit!");
                
                if(Zombies[i].GetComponent<Target>().HiveHP() > 0)
                {
                    Zombies[i].GetComponent<Target>().TakeDamage(damage);
                }
                if(Zombies[i].GetComponent<Target>().HiveHP() == 0)
                {
                    Zombies.Remove(Zombies[i].gameObject);
                }
                
                //_other.GetComponent<Target>().TakeDamage(damage); 
            }
            //Target target = _other.GetComponent<Target>();
            //if (target != null)
            //{
            //    target.TakeDamage(damage);
            //}
        nextdamageTime = Time.time + damageInterval;
        }
    }

    public void Remove(GameObject zombie)
    {
        Zombies.Remove(zombie);
    }
}
