using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTrigger : MonoBehaviour
{
    [SerializeField]
    private NavAgentManager navAgentManager = null;

    [SerializeField]
    private List<Transform> paths = null;

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Zombie"))
        {
            navAgentManager.SetNewTartget(this, _other.name);
        }
    }

    public Vector3  pathPosition()
    {
        return this.transform.position;
    }


    public Vector3 nextPos()
    {
        for(int i =0; i<paths.Count; ++i)
        {
            if (paths[i].name == this.name)
            {
                if (i == paths.Count - 1)
                {
                    return paths[0].position;
                }
                else
                {
                    return paths[i + 1].position;
                }
            }   
        }
        return this.transform.position;
    }

    public string pathName()
    {
        return this.name;
    }
}
