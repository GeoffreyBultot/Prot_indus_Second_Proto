
using System;

[Serializable]
public class ViloRootobject
{
    public int nhits ;
    public Parameters parameters ;
    public Record[] records ;
}
[Serializable]
public class Parameters
{
    public string[] dataset ;
    public string timezone ;
    public int rows ;
    public string format ;
    public string lang ;
}
[Serializable]
public class Record
{
    public string datasetid ;
    public string recordid ;
    public Fields fields ;
    public Geometry geometry ;
}
[Serializable]
public class Fields
{
    public string status ;
    public string contract_name ;
    public string name ;
    public string bonus ;
    public int bike_stands ;
    public int number ;
    public DateTime last_update ;
    public int available_bike_stands ;
    public string banking ;
    public int available_bikes ;
    public string address ;
    public float[] position ;
}
[Serializable]
public class Geometry
{
    public string type ;
    public float[] coordinates ;
}
