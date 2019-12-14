using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
    public class RootobjectTT
    {
    public Flowsegmentdata flowSegmentData;
    }
[System.Serializable]
public class Flowsegmentdata
    {
    public string frc;
    public string currentSpeed;
    public int freeFlowSpeed;
    public int currentTravelTime;
    public int freeFlowTravelTime;
    public float confidence;
    public Coordinates coordinates;
    public string version;
    }
[System.Serializable]
public class Coordinates
    {
    public Coordinate[] coordinate;
    }
[System.Serializable]
public class Coordinate
    {
    public float latitude;
    public float longitude;
    }
