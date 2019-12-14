using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshAgentManager : MonoBehaviour {

    public GameObject agentPrefab;
    public Transform agentParent;

	// Use this for initialization
	void Start () {
		
	}

    public void SpawnAgent(Transform startPos, Transform endPos)
    {
        //détruit l'agent si existe
        DestroyAgent();
        //instancie l'agent et lance son trajet
        GameObject agent = Instantiate(agentPrefab, startPos.GetChild(0).position, Quaternion.identity, agentParent);
        if(endPos != null) agent.GetComponent<NavMeshAgentScript>().MoveAgentToDestination(endPos.GetChild(0));
    }
	
    public void DestroyAgent()
    {
        if (transform.childCount != 0) Destroy(transform.GetChild(0).gameObject);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
