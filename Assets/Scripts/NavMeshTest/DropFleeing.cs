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
    private List<NavMeshAgent> agents = new List<NavMeshAgent>();
    private List<GameObject> obstacles;
    private float destroyTIme = 3f;

    private void Start()
    {

    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                GameObject newOBJ = Instantiate(obstacle, hitInfo.point, obstacle.transform.rotation);
                Destroy(newOBJ, destroyTIme);
                StartCoroutine("Return");
            }
        }
    }

    private IEnumerator Return()
    {
        yield return new WaitForSeconds(destroyTIme);
    }

}
