using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTriggerManager : MonoBehaviour
{
    [SerializeField]
    private NavAgentManager navAgentManager = null;

    [SerializeField]
    private List<Transform> pathsForA = null;

    [SerializeField]
    private List<Transform> pathsForB = null;

    [SerializeField]
    private List<Transform> pathsForC = null;

    private Transform t;

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Zombie"))
        {
            if (_other.name.Substring(_other.name.Length - 1) == "A" && this.name.Substring(0, 1) == "A")
            {
                //Debug.Log("path A:  " + _other.name);
                navAgentManager.SetNewTargetForGroupA(this, _other.name);
            }

            if (_other.name.Substring(_other.name.Length - 1) == "B" && this.name.Substring(0, 1) == "B")
            {
                //Debug.Log("path B:  " + _other.name);
                navAgentManager.SetNewTargetForGroupB(this, _other.name);
            }

            if (_other.name.Substring(_other.name.Length - 1) == "C" && this.name.Substring(0,1) == "C")
            {
                //Debug.Log("path C:  " + _other.name);
                navAgentManager.SetNewTargetForGroupC(this, _other.name);
            }
        }

    } 

    public Transform PathPosition()
    {
        return this.transform;
    }


    public Transform NextPosForA()
    {

        for (int i = 0; i < pathsForA.Count; ++i)
        {
            
            if (pathsForA[i].name == this.name)
            {

                if (i == pathsForA.Count - 1)
                {
                    t = pathsForA[i];
                }
                else
                {
                    t = pathsForA[i + 1];
                }
            }
        }
        return t;
    }

    public Transform NextPosForB()
    {
        for (int i = 0; i < pathsForB.Count; ++i)
        {
           
            if (pathsForB[i].name == this.name)
            {
                if (i == pathsForB.Count - 1)
                {
                    t = pathsForB[i];
                }

                else
                {
                    t = pathsForB[i + 1];
                }
            }
        }
        return  t;
    }

    public Transform NextPosForC()
    {
        for (int i = 0; i < pathsForC.Count; ++i)
        {
            if (pathsForC[i].name == this.name)
            {
                if (i == pathsForC.Count - 1)
                {
                    t = pathsForC[i];
                }

                else
                {
                    t = pathsForC[i + 1];
                }
            }
        }
        return t;
    }

    public string PathName()
    {
        return this.name;
    }
}
