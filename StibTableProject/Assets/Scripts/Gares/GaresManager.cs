using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaresManager : MonoBehaviour {

    string loadedGare;

    public string LoadedGare
    {
        get
        {
            return loadedGare;
        }

        set
        {
            loadedGare = value;
        }
    }

    // Use this for initialization
    void Start () {
        LoadedGare = "none";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
