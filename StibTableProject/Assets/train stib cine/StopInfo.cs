using System;

[System.Serializable]
public class StopInfo
{
	public Points[] points;
}
[System.Serializable]
public class Points
{
	public string pointId;
	public PassingTime[] passingTimes;
}
[System.Serializable]
public class PassingTime
{
	public string expectedArrivalTime;
	public string lineId;
}
