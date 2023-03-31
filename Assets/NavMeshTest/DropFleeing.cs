using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DropFleeing : MonoBehaviour
{
    [SerializeField]
    private NavAgentManager navAgentManager = null;
    [SerializeField] 
    private GameObject obstacle;
    private float destroyTIme = 3f;
    private Vector3 point;



    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            navAgentManager.GetNavMeshAgents();
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                point = hitInfo.point;
                GameObject newOBJ = Instantiate(obstacle, hitInfo.point, obstacle.transform.rotation);
                navAgentManager.DetectNewObstacle(point);
                Destroy(newOBJ, destroyTIme);
                Invoke("Return", destroyTIme);
            } 
        }
    }

    private void Return()
    {
        navAgentManager.ResetAgnet();
    }



}
