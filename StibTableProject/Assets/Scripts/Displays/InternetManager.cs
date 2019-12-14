using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetManager : MonoBehaviour {

    public GameObject InternetConnectionDisplay;

	// Use this for initialization
	void Start () {
	}
	
    public bool CheckInternetConnection()
    {
        bool hasInternet = true;
        //si réseau non accessible
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            hasInternet = false;
            InternetConnectionDisplay.SetActive(true);
        }
        else
        {
            InternetConnectionDisplay.SetActive(false);
        }
        return hasInternet;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
