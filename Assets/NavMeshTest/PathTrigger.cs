using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTrigger : MonoBehaviour
{
    [SerializeField]
    private NavAgentManager navAgentManager = null;

    [SerializeField]
    private List<Transform> paths = null;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Agent"))
        {
            navAgentManager.SetNewTartget(this, other.name);
        }
    }

    public Transform  pathPosition()
    {
        return this.transform;
    }


    public Transform nextPos()
    {
        for(int i =0; i<paths.Count; ++i)
        {
            if (paths[i].name == this.name)
            {
                if (i == paths.Count - 1)
                {
                    return paths[0];
                }
                else
                {
                    return paths[i + 1];
                }
            }   
        }
        return this.transform;
    }

    public string pathName()
    {
        return this.name;
    }
}
