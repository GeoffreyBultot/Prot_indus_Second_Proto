using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationDebugger : MonoBehaviour {

    private NavMeshAgent agentToDebug;
    private LineRenderer line;  

	// Use this for initialization
	void Start () {
        agentToDebug = GetComponent<NavMeshAgent>();
        line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        line.positionCount = agentToDebug.path.corners.Length;
        line.SetPositions(agentToDebug.path.corners);
        line.enabled = true;
	}
}
