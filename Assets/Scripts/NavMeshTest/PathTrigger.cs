using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTrigger : MonoBehaviour
{
    [SerializeField]
    private NavAgentManager navAgentManager = null;

    [SerializeField]
    private List<Transform> pathsForA = null;

    [SerializeField]
    private List<Transform> pathsForB = null;

    [SerializeField]
    private List<Transform> pathsForC = null;

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Zombie"))
        {
            if (_other.name.Substring(_other.name.Length - 1) == "A")
            {
                navAgentManager.SetNewTargetForGroupA(this, _other.name);
            }

            else if (_other.name.Substring(_other.name.Length - 1) == "B")
            {
                navAgentManager.SetNewTargetForGroupB(this, _other.name);
            }

            else if (_other.name.Substring(_other.name.Length - 1) == "C")
            {
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
                    return pathsForA[0];
                }

                else
                {
                    return pathsForA[i + 1];
                }
            }
        }
        return this.transform;
    }

    public Transform NextPosForB()
    {
        for (int i = 0; i < pathsForB.Count; ++i)
        {
            if (pathsForB[i].name == this.name)
            {
                if (i == pathsForB.Count - 1)
                {
                    return pathsForB[0];
                }

                else
                {
                    return pathsForB[i + 1];
                }
            }
        }
        return this.transform;
    }

    public Transform NextPosForC()
    {
        for (int i = 0; i < pathsForC.Count; ++i)
        {
            if (pathsForC[i].name == this.name)
            {
                if (i == pathsForC.Count - 1)
                {
                    return pathsForC[0];
                }

                else
                {
                    return pathsForC[i + 1];
                }
            }
        }
        return this.transform;
    }

    public string PathName()
    {
        return this.name;
    }
}
