using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tempChoosingGare : MonoBehaviour {

    public Text txt;
    GaresManager gareManager;
	// Use this for initialization
	void Start () {
        gareManager = GameObject.Find("GaresManager").GetComponent<GaresManager>();
        LoadTrains();
	}
	
    public void LoadTrains()
    {
        switch (gareManager.LoadedGare)
        {
            case "GDN":
                txt.text = "GDN Loaded";
                break;
            case "GDM":
                txt.text = "GDM Loaded";
                break;
            case "GDL":
                txt.text = "GDL Loaded";
                break;
            case "GC":
                txt.text = "GC Loaded";
                break;
            default:
                break;
        }
    }
}
