using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Flock/Behavior/Avoidance")]
public class AvoidanceBehavior : FlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {

       // Debug.Log(context.Count);
        //if no neighbors, return no adjustment
        if (context.Count == 0)
        {
            return flock.transform.position;
        }

        Vector3 avoidanceMove = flock.transform.position;
        int nAvoid = 0;

        foreach (Transform item in context)
        {
            if(Vector3.SqrMagnitude((item.position - agent.transform.position)) < flock.SquareAvoidanceRadius)
            {
                ++nAvoid;
                avoidanceMove += (agent.transform.position - item.position);
            }
        }

        if(nAvoid > 0)
        {
            avoidanceMove /= nAvoid;
        }

        return avoidanceMove;

    }
}
