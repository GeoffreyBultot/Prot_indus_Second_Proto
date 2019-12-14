using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TramWidgetManager : MonoBehaviour {

    Text displayedResult;

    [SerializeField]
    private List<int> Arrets;
    [SerializeField]
    [Range(1, 2)]
    private int tramAmount = 2;

    Rootobject stibData;

    // Use this for initialization
    void Start() {
        displayedResult = transform.parent.GetChild(0).GetChild(0).GetComponent<Text>();
        InvokeRepeating("DataImport", 0, 60f);
    }

    public void UpdateAPIData(Rootobject data)
    {
        displayedResult.text = "";
        foreach (Point p in stibData.points)
        {
            foreach (Passingtime pt in p.passingTimes)
            {
                displayedResult.text += pt.lineId.ToString() + " direction ";
                displayedResult.text += StopIDtoDirectionConverter(p.pointId, pt.lineId) + " : ";               
                displayedResult.text += MinuteTransform(pt) + " \n";
            }
        }
    }

    //méthode invoquée toutes les 21 secondes pour lancer la requête à l'api de la stib
    private void DataImport()
    {
        //début de la coroutine, URL de la stib contenant les 3 x 2 arrêts qui nous intéressent
        string URL = "https://opendata-api.stib-mivb.be/OperationMonitoring/1.0/PassingTimeByPoint/";
        for (int i = 0; i < Arrets.Count; i++)
        {
            URL += Arrets[i].ToString();
            if (i != Arrets.Count - 1) URL += "%2C";
        }
        StartCoroutine(GetVehiculesInfos(URL));
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
        Debug.Log("Data received.");
        Debug.Log(request.downloadHandler.text);


        //traitement du JSON récupéré et envoi dans la classé créée au préalable
        stibData = JsonUtility.FromJson<Rootobject>(request.downloadHandler.text);
        UpdateAPIData(stibData);
        request.Dispose();
    }

    string StopIDtoDirectionConverter(int stopID, int lineNumber)
    {
        string direction = "not found";

        switch (stopID)
        {
            case 6357:
                if(lineNumber == 92)
                {
                    direction = "SCHAERBEEK GARE";
                }
                if (lineNumber == 93)
                {
                    direction = "STADE";
                }
                break;
            case 6308:
                if (lineNumber == 92)
                {
                    direction = "FORT-JACO";
                }
                if (lineNumber == 93)
                {
                    direction = "LEGRAND";
                }
                break;
            default:
                break;
        }

        return direction;
    }

    //retourne un int équivalent au nombre de minutes restant avant l'arrivée du tram
    private int MinuteTransform(Passingtime receivedDate)
    {

        int result = 0;
        //récupération de l'heure actuelle
        System.DateTime now = DateTime.Now;
        int hour = now.Hour;
        int min = now.Minute;
        //addition des minutes et des heures converties en minutes
        int actualtotalMinut = hour * 60 + min;
        //traitement du string reçu avec l'heure d'arrivée pour récupérer l'heure et les minutes
        string substringedDate = receivedDate.expectedArrivalTime.Substring(receivedDate.expectedArrivalTime.Length - 8);
        int receivedHour = int.Parse(substringedDate.Substring(0, 2));
        string receivedMinut = substringedDate.Substring(0, 5);
        receivedMinut = receivedMinut.Substring(receivedMinut.Length - 2);
        int receivedTotalMinut = receivedHour * 60 + int.Parse(receivedMinut);
        //soustrait l'heure d'arriver en minutes avec l'heure actuelle en minutes
        result = receivedTotalMinut - actualtotalMinut;

        return result;
    }
}
