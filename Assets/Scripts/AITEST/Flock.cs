using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    private float speed;
    private bool turning = false;

    private void Start()
    {
        speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
    }

    private void Update()
    {
        Bounds bounds = new Bounds(FlockManager.FM.transform.position, FlockManager.FM.sideLimits * 2);
        if(!bounds.Contains(transform.position))
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        if(turning)
        {
            Vector3 direction = FlockManager.FM.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
                                                    FlockManager.FM.rotationSpeed * Time.deltaTime);
        }

        if(Random.Range(0,100) < 10)
        {
            speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
        }

        if (Random.Range(0, 100) < 10)
        {
            ApplyRules();
        }

        this.transform.Translate(0, 0, speed * Time.deltaTime);
    }

    private void ApplyRules()
    {
        GameObject[] gos;
        gos = FlockManager.FM.allPrefab;

        Vector3 vCentre = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistanced;
        int groupSize = 0;

        foreach(GameObject go in gos)
        {
            if(go != this.gameObject)
            {
                nDistanced = Vector3.Distance(go.transform.position, this.transform.position);
                if(nDistanced <= FlockManager.FM.neighbourDistance)
                {
                    vCentre += go.transform.position;
                    groupSize++;

                    if(nDistanced < 1.0f)
                    {
                        vAvoid = vAvoid + (this.transform.position - go.transform.position);
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }

        }

        if(groupSize > 0 )
        {
            vCentre = vCentre / groupSize + (FlockManager.FM.goalPos - this.transform.position);
            speed = gSpeed / groupSize; 
            if(speed > FlockManager.FM.maxSpeed)
            {
                speed = FlockManager.FM.maxSpeed;
            }

            Vector3 direction = (vCentre + vAvoid) - transform.position;
            if(direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
                                                        FlockManager.FM.rotationSpeed * Time.deltaTime);
            }
        }
    }
}
