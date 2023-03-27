using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehaviour behaviour;

    [Range(10, 500)]
    public int startingCount = 250;
    const float AgentDesity = 0.1f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 100f)]
    public float minSpeed = 1f;
    [Range(1f, 10f)]
    public float neighbotRadius = 1.5f;
    [Range(0f, 10f)]
    public float avoidanceRadiusMultiplier = 0.5f;



    private float squareSpeed;
    private float squareNeighborRadius;
    private float squareAvoidanceRadius;

    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    // Start is called before the first frame update
    private void Start()
    {
        squareSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighbotRadius * neighbotRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for(int i = 0; i <startingCount; ++i)
        {
            FlockAgent newAgent = Instantiate(agentPrefab,
                                             new Vector3(Random.insideUnitSphere.x, 0f, Random.insideUnitSphere.z) * startingCount * AgentDesity,
                                             Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)),
                                             transform
                                             );
            newAgent.name = "Agent " + i;
            agents.Add(newAgent);
        }

        Debug.Log(squareAvoidanceRadius);
    }

    // Update is called once per frame
    private void Update()
    {
        foreach(FlockAgent agent in agents)
        {
            List<Transform> context = GetNearByObjects(agent);

            Vector3 move = behaviour.CalculateMove(agent, context, this);
            move *= driveFactor;

            if (move.sqrMagnitude > squareSpeed)
            {
                move = move.normalized * maxSpeed;
            }

            if(move != Vector3.zero)
            {
                agent.Move(move);
            }

        }
    }
    
    private List<Transform> GetNearByObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighbotRadius);
        foreach(Collider c in contextColliders)
        {
            if(c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
}
