using System;

[System.Serializable]
public class RootObjectScooty
{
    public Scooty[] scootys;
}

[System.Serializable]
public class Scooty
{
        public string id;
        public float lat;
        public float lon;
        public string provider;
        public Vehicledetails vehicleDetails;
}

[System.Serializable]
public class Vehicledetails
{
        public string id;
        public string model;
        public string name;
        public string pictureUrl;
        public string energy;
}
