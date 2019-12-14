using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScootyScript : MonoBehaviour {
    [SerializeField]
    private string id;
    [SerializeField]
    private int energy;
    [SerializeField]
    private float lat;
    [SerializeField]
    private float lon;

    public string Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public int Energy
    {
        get
        {
            return energy;
        }

        set
        {
            energy = value;
        }
    }

    public float Lat
    {
        get
        {
            return lat;
        }

        set
        {
            lat = value;
        }
    }

    public float Lon
    {
        get
        {
            return lon;
        }

        set
        {
            lon = value;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
