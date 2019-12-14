using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.IO;
using UnityEngine.UI;
using airquality_namespace;
using System.Collections;

public class AirQualitySystem : MonoBehaviour
{

    public AirQualityData airQualityData;
    public Text AQText;
    public string API_KEY = "BgeWu2GWBtLjLrBoa";
    public string CityID = "Brussels";
    public string StateID = "Brussels%20Capital";
    public string CountryID = "Belgium";
    public float MAJ = 900f;
    public int AQIUS;

    void Start()
    {
        InvokeRepeating("AirQualityUpdating", .2f, MAJ);   // Appel la méthode "AirQualityUpdating" 0.2 secondes après le lancement de l'application et puis toutes les 900 secondes (15 minutes)
    }

    void AirQualityUpdating()
    {
        //Debug.Log("Updating air quality ... " + DateTime.Now.ToString("HH:mm:ss"));
        //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });   // Pour ne pas avoir de problème de certificat.
        StartCoroutine(getData());
    }

    IEnumerator getData()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://api.airvisual.com/v2/city?city={0}&state={1}&country={2}&key={3}", CityID, StateID, CountryID, API_KEY));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        airQualityData = JsonUtility.FromJson<AirQualityData>(jsonResponse);
        assignData();
        yield return null;
    }

    void assignData()
    {
        AQIUS = airQualityData.data.current.pollution.aqius;
        //AQText.text = "Qualité de l'air : " + AQIUS;
        string aqValue = "";

        if (AQIUS < 25) aqValue = "Very Good";
        else if (AQIUS < 50) aqValue = "Good";
        else if (AQIUS < 100) aqValue = "Moderate";
        else if (AQIUS < 200) aqValue = "Bad";
        else if (AQIUS < 250) aqValue = "Very Bad";
        else aqValue = "Dangerous";

        AQText.text = "Air quality : " + aqValue;
    }


}
