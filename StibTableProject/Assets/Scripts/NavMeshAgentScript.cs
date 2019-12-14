using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentScript : MonoBehaviour {

    public NavMeshAgent agent;
    public Vector3 initPos;
    private bool isPathfinding;
    private Vector3 dest;

	// Use this for initialization
	void Start () {
        isPathfinding = false;
	}
	
    public void MoveAgentToDestination(Transform destination)
    {
        Debug.Log("starts moving");
        Debug.Log(destination.position);
        dest = destination.position;
        agent.SetDestination(destination.position);
        isPathfinding = true;
        agent.isStopped = true;
    }

	// Update is called once per frame
	void Update () {
        
    }
}
