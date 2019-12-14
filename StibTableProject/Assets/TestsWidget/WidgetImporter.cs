using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetImporter : MonoBehaviour {

    public Transform widget;
    

	// Use this for initialization
	void Start () {
        Instantiate(widget, new Vector3(0,0, 0), Quaternion.identity, this.transform);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
