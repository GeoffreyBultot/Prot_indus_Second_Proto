using System;
[System.Serializable]

public class Rootobject
{
    public Point[] points;
}
[System.Serializable] 
public class Point
{
    public int pointId;
    public Passingtime[] passingTimes;
}
[System.Serializable]
public class Passingtime
{
    public string expectedArrivalTime;
    public int lineId;
}