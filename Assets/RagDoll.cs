using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDoll : MonoBehaviour
{
    [SerializeField]
    private GameObject alive;

    [SerializeField]
    private GameObject dead;

    [SerializeField]
    private GameObject Group;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Alive();
        }
    }

    private void Dead()
    {
        RagDollPosition(alive.transform, dead.transform);

        alive.SetActive(false);
        dead.SetActive(true);
    }

    private void Alive()
    {
        dead.SetActive(false);
        alive.SetActive(true);
    }

    private void RagDollPosition(Transform alive, Transform dead)
    {
        for (int i = 0; i < alive.transform.childCount; ++i)
        {
            if (alive.transform.childCount != 0)
            {
                RagDollPosition(alive.transform.GetChild(i), dead.transform.GetChild(i));
            }
            dead.transform.GetChild(i).localPosition = alive.transform.GetChild(i).localPosition;
            dead.transform.GetChild(i).localRotation = alive.transform.GetChild(i).localRotation;
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("bullet"))
        {
            Dead();
            Destroy(Group.GetComponent<RagDoll>());
        }
    }
}
