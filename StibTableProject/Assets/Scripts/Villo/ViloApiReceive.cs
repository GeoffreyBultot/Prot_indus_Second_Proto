using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ViloApiReceive : MonoBehaviour
{
    public ViloRootobject viloData;

    public delegate void MyEventHandler();
    public MyEventHandler ViloDataLoaded;

    // Start is called before the first frame update
    void Awake()
    {
        InvokeRepeating("DataImport", 0, 40);
        
    }

    private void DataImport()
    {
        
        StartCoroutine(GetViloStationInfos("https://opendata.bruxelles.be/api/records/1.0/search/?dataset=stations-villo-disponibilites-en-temps-reel&lang=fr&refine.number=48"));
       
    }

    IEnumerator GetViloStationInfos(string uri)
    {
        //store l'url de requête dans une unitywebrequest
        UnityWebRequest request = UnityWebRequest.Get(uri);

        yield return request.SendWebRequest();


        //Debug.Log(request.downloadHandler.text);

        yield return viloData = JsonUtility.FromJson<ViloRootobject>(request.downloadHandler.text);
        request.Dispose();
        ManageDatas();
    }

    private void ManageDatas()
    {
        //lance l'event que les données ont bien été récupérées
        if (ViloDataLoaded != null)
        {
           ViloDataLoaded();
        }
    }

    
}
