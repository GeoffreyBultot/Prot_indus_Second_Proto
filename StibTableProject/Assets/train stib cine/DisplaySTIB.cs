using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;


public class DisplaySTIB : MonoBehaviour {

    //public InternetManager internetManager;
    public Rootobject stibData;
    public delegate void OnStibDataLoaded();
    public static event OnStibDataLoaded StibDataLoadedDelegate;
    public InternetManager internetManager;

    private void Awake()
    {
        stibData = new Rootobject();
        internetManager = GameObject.Find("InternetManager").GetComponent<InternetManager>();
        //TextColors();
        DataImport();
    }

    private void TextColors()
    {
        Component[] text = GetComponentsInChildren<Text>();
        foreach (Text t in text)
        {
            t.color = Color.white;
        }
    }

    //import des données de la STIB
    private void DataImport()
    {
        //Si il y a bien une connexion internet, démarre la coroutine de récupération
        if (internetManager.CheckInternetConnection())
        {
            StartCoroutine(GetVehiculesInfos("https://opendata-api.stib-mivb.be/OperationMonitoring/1.0/PassingTimeByPoint/6357%2C6308%2C8031%2C8032%2C8411%2C8412%2C3358%2C3354"));
        }
        //sinon, log qu'il n'y avait pas de connexion pour la STIB
        else Debug.Log("no internet connection for STIB Board");
    }

    IEnumerator GetVehiculesInfos(string uri)
    {
        //store l'url de requête dans une unitywebrequest
        UnityWebRequest request = UnityWebRequest.Get(uri);

        //ajoute les headers d'application + token à la requête
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Authorization", "Bearer 01dc5ca7d2c53c40771fcce562bb0377");

        //envoi la requête et attend la réponse
        yield return request.SendWebRequest();

        // Log de la réponse en UTF8
        Debug.Log("Data received from STIB Board.");

        Debug.Log(request.downloadHandler.text);

        //traitement du JSON récupéré et envoi dans la classé créée au préalable
        stibData = JsonUtility.FromJson<Rootobject>(request.downloadHandler.text);
        request.Dispose();
        ManageDatas();
    }

    private void ManageDatas()
    {
        //lance l'event que les données ont bien été récupérées
        StibDataLoadedDelegate();
    }
}