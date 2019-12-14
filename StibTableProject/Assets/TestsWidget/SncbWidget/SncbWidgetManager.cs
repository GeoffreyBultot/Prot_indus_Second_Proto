using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SncbWidgetManager : MonoBehaviour {

    Text displayedResult;

    // Use this for initialization
    void Start () {
        displayedResult = transform.parent.GetChild(0).GetChild(0).GetComponent<Text>();
        displayedResult.text = "SNCB";
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
