using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [System.Serializable]
    public class WeatherData
    {
        public Location location;
        public CurrentObservation current_observation;
        public List<Forecast> forecasts;
    }

    [System.Serializable]
    public class Location
    {
        public int woeid;
        public string city;
        public string region;
        public string country;
        public double lat;
        public double @long;
        public string timezone_id;
    }

    [System.Serializable]
    public class Wind
    {
        public int chill;
        public int direction;
        public double speed;
    }

    [System.Serializable]
    public class Atmosphere
    {
        public int humidity;
        public double visibility;
        public double pressure;
        public int rising;
    }

    [System.Serializable]
    public class Astronomy
    {
        public string sunrise;
        public string sunset;
    }

    [System.Serializable]
    public class Condition
    {
        public string text;
        public int code;
        public int temperature;
    }

    [System.Serializable]
    public class CurrentObservation
    {
        public Wind wind;
        public Atmosphere atmosphere;
        public Astronomy astronomy;
        public Condition condition;
        public int pubDate;
    }

    [System.Serializable]
    public class Forecast
    {
        public string day;
        public int date;
        public int low;
        public int high;
        public string text;
        public int code;
    }
