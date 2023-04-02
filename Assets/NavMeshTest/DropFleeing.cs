using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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

    private void Awake()
    {
        navAgentManager = FindObjectOfType<NavAgentManager>();
    }

    private void Start()
    {
        StartCoroutine(BlackholeStart());
    }

    private void Return()
    {
        navAgentManager.ResetAgnet();
    }


    private IEnumerator BlackholeStart()
    {
        yield return new WaitForSeconds(2f);
        GameObject newOBJ = Instantiate(obstacle, transform.position, transform.rotation);
        navAgentManager.DetectNewObstacle(transform.position);
        Destroy(newOBJ, destroyTIme);
        Invoke("Return", destroyTIme);
    }

    public void grenadeRigidbody(Vector3 aa)
    {
        Rigidbody rigidbody =  GetComponent<Rigidbody>();
        rigidbody.AddForce(aa, ForceMode.Impulse);
    }

}
