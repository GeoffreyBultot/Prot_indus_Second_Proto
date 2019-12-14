using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StibMinuteSelector : MonoBehaviour {

    public int StopID;
    public int line;
    private DisplaySTIB stibManager;
    private Rootobject stibDatas;
    private Text displayedText;

    private void Start()
    {
        stibManager = GameObject.Find("STIB Display UI").GetComponent<DisplaySTIB>();
        displayedText = GetComponent<Text>();
        displayedText.text = "Load";
    }
    private void DisplayMinuts()
    {
        //récupération des données mises à jour par le manager
        stibDatas = stibManager.stibData;
        displayedText.text = "";
        //boucle sur les différents arrêts de la stib
        foreach (Point p in stibDatas.points)
        {
            //si l'arrêt correspond à celui que l'on cherche
            if(p.pointId == StopID)
            {
                //on parcourt tous les passingTimes pour avoir accès à tous les trams qui vont arriver
                foreach (Passingtime pt in p.passingTimes)
                {
                    //si la ligne correspond à celle que l'on cherche
                    if(pt.lineId == line)
                    {
                        //faire quelque chose pour cet arrêt et ligne sélectionné
                        displayedText.text += (MinuteTransform(pt) + "'  ");
                    }
                }
                //soustraction des deux derniers caractères
                displayedText.text = displayedText.text.Substring(0, displayedText.text.Length - 2);
            }
        }
    }

    private void HighlightLine(int lineID)
    {
        //récupère le transform image highlight background de la ligne et l'enlève
        transform.parent.GetChild(0).gameObject.SetActive(false);
        //le nom du transform qui contient la ligne 
        if (line == lineID) transform.parent.GetChild(0).gameObject.SetActive(true);
    }


    private void OnDisable()
    {
        DisplaySTIB.StibDataLoadedDelegate -= DisplayMinuts;
        ButtonsManager.StibLineClickedDelegate -= HighlightLine;

    }
    private void OnEnable()
    {
        DisplaySTIB.StibDataLoadedDelegate += DisplayMinuts;
        ButtonsManager.StibLineClickedDelegate += HighlightLine;
    }

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
