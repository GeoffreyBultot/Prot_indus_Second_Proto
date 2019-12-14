using System.Collections;
using UnityEngine;
using iRailResponse;
using UnityEngine.UI;
using System.Globalization;
using System;
using UnityEngine.Networking;


public class iRailDisplay : MonoBehaviour {

	int tryNumber = 0;
    public int maxLine = 10;
    GaresManager gareManager;
    private string url;

    void Start () {
        
        gareManager = GameObject.Find("GaresManager").GetComponent<GaresManager>();
        LoadGare();
        
	}

    public void LoadGare()
    {
        //url de requête iRail
        url = "https://api.irail.be/liveboard/?id=BE.NMBS." + StationConverter(gareManager.LoadedGare);
        //coroutine qui lance la récupération des données
        StartCoroutine(iRail());
    }

    public string StationConverter(string station)
    {
        string stationID = "";
        switch (station)
        {
            case "GDN":
                stationID = "008812005";
                break;
            case "GDM":
                stationID = "008814001";
                break;
            case "GDL":
                stationID = "008811304";
                break;
            case "GC":
                stationID = "008813003";
                break;
            default:
                break;
        }
        return stationID;
    }

    private string RemoveAccent(string withAccent)
    {
        string[] Echars = new string[] { "è", "é", "ê", "ë"};
        string[] Ichars = new string[] { "î", "í", "ì", "ï" };
        //Iterate the number of times based on the String array length.
        for (int i = 0; i < Echars.Length; i++)
        {
            if (withAccent.Contains(Echars[i]))
            {
                withAccent = withAccent.Replace(Echars[i], "e");
            }
        }
        for (int i = 0; i < Ichars.Length; i++)
        {
            if (withAccent.Contains(Ichars[i]))
            {
                withAccent = withAccent.Replace(Ichars[i], "i");
            }
        }
        return withAccent;
    }

	public IEnumerator iRail()
	{
        UnityWebRequest request = UnityWebRequest.Get(url);
        //envoi la requête et attend la réponse
        yield return request.SendWebRequest();

        Debug.Log(request.downloadHandler.text);
        //Debug.Log(www.text);
        iRailFeed XMLFeed = new iRailFeed(request.downloadHandler.text);
		if (XMLFeed.liveBoard.Departures.Departure.Count!=0) {
            int i = 0;
            while (i < maxLine && i<XMLFeed.liveBoard.Departures.Departure.Count)
            {
                GameObject timeUI = GameObject.Find("Time"+i.ToString());
                DateTime departureTime = DateTime.ParseExact(XMLFeed.liveBoard.Departures.Departure[i].Time.Formatted, "yyyy'-'MM'-'dd'T'HH':'mm':'ss", CultureInfo.InvariantCulture);
                timeUI.GetComponent<Text>().text = departureTime.Hour.ToString().PadLeft(2, '0') + ":" + departureTime.Minute.ToString().PadLeft(2, '0');
                //Debug.Log(XMLFeed.liveBoard.Departures.Departure[i].Time.Formatted);
                GameObject destinationUI = GameObject.Find("Destination"+ i.ToString());
                destinationUI.GetComponent<Text>().text = RemoveAccent(XMLFeed.liveBoard.Departures.Departure[i].Station.Standardname);
                GameObject trackUI = GameObject.Find("Track"+ i.ToString());
                trackUI.GetComponent<Text>().text = XMLFeed.liveBoard.Departures.Departure[i].Platform.Text.PadLeft(2, '0');
                GameObject remarkUI = GameObject.Find("Remark"+ i.ToString());
                remarkUI.GetComponent<Text>().text = "";
                if (XMLFeed.liveBoard.Departures.Departure[i].Canceled == "1")
                {
                    remarkUI.GetComponent<Text>().text = "Canceled";
                }
                else if (XMLFeed.liveBoard.Departures.Departure[i].Delay != "0")
                {
                    remarkUI.GetComponent<Text>().text = "Delayed " + (int.Parse(XMLFeed.liveBoard.Departures.Departure[i].Delay) / 60).ToString() + "'";
                }
                i++;
            }
		}
	}
}
