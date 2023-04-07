using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ragdoll: MonoBehaviour
{
    private bool isInblackHole = false;

    private float blackHoleRadius = 7f;

    private Vector3 blackHolePosition;

    [SerializeField] private GameObject ragdoll;
    [SerializeField] private GameObject dead;


    private void Update()
    {
        Invoke("Dead", 3f);

        if (isInblackHole)
        {
            InTheBlackHole();
        }
    }

    private void Dead()
    {
        Debug.Log(ragdoll);

        ragdoll.SetActive(false);

        GameObject newDead = Instantiate(dead, transform.position, transform.rotation);
        DeadPosition(ragdoll.transform, newDead.transform);

        Destroy(ragdoll);
    }


    private void DeadPosition(Transform _newRagdoll, Transform _dead)
    {
        for (int i = 0; i < _newRagdoll.transform.childCount; ++i)
        {
            if (_newRagdoll.transform.childCount != 0)
                DeadPosition(_newRagdoll.transform.GetChild(i), _dead.transform.GetChild(i));

            _dead.transform.GetChild(i).localPosition = _newRagdoll.transform.GetChild(i).localPosition;
            _dead.transform.GetChild(i).localRotation = _newRagdoll.transform.GetChild(i).localRotation;
        }
        _dead.transform.position = _newRagdoll.transform.position;
    }


    private void InTheBlackHole()
    {
        if (Vector3.Distance(blackHolePosition, transform.position) < blackHoleRadius && isInblackHole == true)
        {
            Vector3 dir = blackHolePosition - transform.position;
            transform.position += dir * 3f * Time.deltaTime;
        }
    }


    public void HitByBlackHole(Vector3 position)
    {
        blackHolePosition = position;
        isInblackHole = true;
    }
}