using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using airquality_namespace;

namespace airquality_namespace
{

    [System.Serializable]
    public class AirQualityData
    {
        public string status;
        public Data data;
    }

    [System.Serializable]
    public class Data
    {
        public string city;
        public string state;
        public string country;
        public Location location;
        public Current current;
    }

    [System.Serializable]
    public class Location
    {
        public string type;
        public List<double> coordinates;
    }

    [System.Serializable]
    public class Current
    {
        public Weather weather;
        public Pollution pollution;
    }

    [System.Serializable]
    public class Weather
    {
        public DateTime ts;
        public int hu;
        public string ic;
        public int pr;
        public int tp;
        public int wd;
        public double ws;
    }

    [System.Serializable]
    public class Pollution
    {
        public DateTime ts;
        public int aqius;
        public string mainus;
        public int aqicn;
        public string maincn;
    }
}


