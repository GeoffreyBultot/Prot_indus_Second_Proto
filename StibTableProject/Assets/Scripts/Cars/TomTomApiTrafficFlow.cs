using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TomTomApiTrafficFlow: MonoBehaviour
{
    // parameter to get list of currentSpeed,RootObject, string data
    // Keep track of what we got back in unity inspector 
    string recentData = "";
    string curSpeed;
    public RootobjectTT ro;
    List<string> LrecentData = new List<string>();
    List<string> Ldata = new List<string>();
    [SerializeField]
    private List<RootobjectTT> Lro = new List<RootobjectTT>();
    [SerializeField]
    List<int> LcurSpeed= new List<int>();
    List<int> traficLoad= new List<int>();
    List<string> Lurl;
    string url0, url1, url2, url3, url4;
    public carsManager carManager;
    public InternetManager internetManager;

    public List<int> traficLoadAtFivePoints
    {
        get
        {
            return traficLoad;
        }
    }

    void Start()
    {
        url0 = GenerateUrls("json", 50.848914f, 4.356895f);// first location
        url1 = GenerateUrls("json", 50.840423f, 4.365733f);//second 
        url2 = GenerateUrls("json", 50.845330f, 4.369618f);//third
        url3 = GenerateUrls("json", 50.852024f, 4.366290f);//fourth
        url4 = GenerateUrls("json", 50.849945f, 4.366325f);//fifth
        Lurl = new List<string>() { url0, url1, url2, url3, url4 };
        InvokeRepeating("DataImportTomTom", 40, 900);
    }
    
    void DataImportTomTom()
    {
        if (internetManager.CheckInternetConnection()) StartCoroutine(RequestRoutine(ResponseCallback));
        else Debug.Log("no internet connection for TOMTOM");
    }

    /**Function to Generate url */
    string GenerateUrls(string typeFile,float lat,float lon)
    {
        //key pCkAV0kht5Gf7HWytTesffBrWt0AOPAA
        const string API_KEY= "?key=4wuG6ih9vI0JdglFZiSeBaqAGxz0I0Jl";
        
        const string DEFAULT_URL = "https://api.tomtom.com/traffic/services/4/flowSegmentData/relative-delay/10/";
        typeFile = typeFile + "/";// xml or json
        string geoloc= "&point=" + lat.ToString() + "," + lon.ToString();
        string targetUrl = DEFAULT_URL + typeFile + API_KEY + geoloc;
        
        return targetUrl.ToString();
    }

    /** Web requests are typically done asynchronously, so Unity's web request system
     * returns a yield instruction while it waits for the response.
     **/
    private IEnumerator RequestRoutine(Action<string> callback = null)
    {
        LcurSpeed = new List<int>();
        foreach (string x in Lurl)
        { 
            var request = UnityWebRequest.Get(x);
            yield return request.SendWebRequest();
            var data = request.downloadHandler.text;
            Ldata.Add(data);
            callback(data);            
        }
        traficLoad = RescaleData();
        Debug.Log("Data received from Tomtom.");
        carManager.ModifyTrafficDensity();
    }
    
    //rescale les valeurs de vitesse récupérées sur tomtom (1-64) pour obtenir des valeurs entre 1 et 5
    //ensuite inverse ce chiffre pour avoir une quantité de trafic à la place d'une vitesse
    List<int> RescaleData()
    {
        List<int> values = new List<int>();
        
        int maxValue = 64;
        int rescaledValue;
        foreach (int value in LcurSpeed)
        {
            //Debug.Log("normal value = " + value);
            if (value > maxValue) rescaledValue = maxValue - 1;
            else rescaledValue = value;
            //définit la quantité de trafic inversément à la vitesse
            rescaledValue = 5 - (rescaledValue * 5 / maxValue);
            values.Add(rescaledValue);
            //Debug.Log("rescaled value = " + rescaledValue);
        }
        return values;
    }

    // Callback to act on our response data
    private void ResponseCallback(string data)
    {
        recentData = data;
        Debug.Log("tomtom = "+recentData);
        ro = JsonUtility.FromJson<RootobjectTT>(recentData);
        curSpeed = ro.flowSegmentData.currentSpeed;
        LcurSpeed.Add(int.Parse(curSpeed));
        LrecentData.Add(recentData.ToString());
        Lro.Add(ro);
    }
}


