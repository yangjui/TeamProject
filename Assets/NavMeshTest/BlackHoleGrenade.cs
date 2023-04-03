using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

public class BlackHoleGrenade : MonoBehaviour
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

    private IEnumerator BlackholeStart()
    {
        yield return new WaitForSeconds(3f);
        GameObject newOBJ = Instantiate(obstacle, transform.position, Quaternion.Euler(Vector3.zero));
        navAgentManager.DetectNewObstacle(transform.position);
        Destroy(newOBJ, destroyTIme);
        Destroy(this.gameObject);
    }

    public void grenadeRigidbody(Vector3 aa)
    {
        Rigidbody rigidbody =  GetComponent<Rigidbody>();
        rigidbody.AddForce(aa, ForceMode.Impulse);
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
    }

}
