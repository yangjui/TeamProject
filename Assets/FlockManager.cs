using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlockManager : MonoBehaviour
{
    public static FlockManager FM;
    [SerializeField]
    private GameObject testPrefab;
    [SerializeField]
    private int numOfPrefab;
    public GameObject[] allPrefab;
    public Vector3 sideLimits = new Vector3(5, 5, 5);
    public Vector3 goalPos = Vector3.zero;

    [Header("Human Settings")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(1f, 10.0f)]
    public float neighbourDistance;
    [Range(1.0f, 5.0f)]
    public float rotationSpeed;

    [SerializeField]
    private GameObject goal = null;

    private void Start()
    {
        allPrefab = new GameObject[numOfPrefab]; 
        for(int i =0; i< numOfPrefab; ++i)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-sideLimits.x, sideLimits.x),
                                                               Random.Range(-sideLimits.y, sideLimits.y),
                                                                Random.Range(-sideLimits.z, sideLimits.z));

            allPrefab[i] = Instantiate(testPrefab, pos, Quaternion.identity);
        }

        FM = this;
        goalPos = this.transform.position;

        //transform.GetComponent<NavMeshAgent>().SetDestination(goal.transform.position);
    }

    private void Update()
    {
        if(Random.Range(0,100) < 10)
        {
            goalPos = this.transform.position + new Vector3(Random.Range(-sideLimits.x, sideLimits.x),
                                                               Random.Range(-sideLimits.y, sideLimits.y),
                                                                Random.Range(-sideLimits.z, sideLimits.z));

        }
    }

}
