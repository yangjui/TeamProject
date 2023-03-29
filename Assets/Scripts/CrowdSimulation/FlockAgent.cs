using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    //Collider agentCollider;
    //public Collider AgentCollider { get { return agentCollider; } }

    //private void Start()
    //{
    //    agentCollider = GetComponent<Collider>();

    //}

    //public void Move(Vector3 velocity)
    //{
    //    transform.forward = velocity;
    //    transform.position += velocity * Time.deltaTime;
    //}

    private CharacterController controller;
    public CharacterController AgentController { get { return controller; } }
    private Collider agentCollider;
    public Collider AgentCollider { get { return agentCollider; } }

    private void Start()
    {
        agentCollider = GetComponent<Collider>();
        controller = GetComponent<CharacterController>();
    }

    public void Move(Vector3 velocity)
    {
        controller.SimpleMove(velocity.normalized);

    }

}

