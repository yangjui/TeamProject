using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Laserbullet : MonoBehaviour
{
    [SerializeField]
    private float cutoutValue = 0.5f;
    [SerializeField]
    private float cutoutRange = 3f;


    private Renderer monsterRenderer;
    private List<Monster> monstersInRange = new List<Monster>();

    private void Start()
    {
        monsterRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Laser"))
        {



            Monster monster = _other.GetComponentInParent<Monster>();
            if (monster != null)
            {
                monstersInRange.Add(monster);
            }
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.CompareTag("Laser"))
        {
            Monster monster = _other.GetComponentInParent<Monster>();
            if (monster != null)
            {
                monstersInRange.Remove(monster);
            }
        }
    }

    private void Update()
    {
        foreach (Monster monster in monstersInRange)
        {
            float currentCutoutValue = monster.GetAlphaCutoutValue();
            monster.SetAlphaCutoutValue(Mathf.Clamp(currentCutoutValue - cutoutValue, 0f, 1f));
        }
    }
}
