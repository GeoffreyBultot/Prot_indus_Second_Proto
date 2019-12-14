using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViloDisplay : MonoBehaviour
{
    public ViloApiReceive viloManager;
    private ViloRootobject viloData;
    public Text _nomStation;
    public Text _textStatus;
    public Text _textAvaible;

    // Start is called before the first frame update
    void Start()
    {
        viloManager.ViloDataLoaded += onDataReceive;
    }

    private void onDataReceive()
    {
        viloData = viloManager.viloData;

        _nomStation.text = viloData.records[0].fields.name;
        int index = _nomStation.text.IndexOf('-') + 1;
        _nomStation.text = _nomStation.text.Substring(index, _nomStation.text.Length-index);
        _textAvaible.text = viloData.records[0].fields.available_bikes.ToString()+"/"+ viloData.records[0].fields.bike_stands.ToString()+" BIKES AVAIBLE";
        _textStatus.text = viloData.records[0].fields.status;
        if (_textStatus.text == "OPEN")
        {
            _textStatus.color =new Color(0,0.5f,0,1);
            
        }


        //Debug.Log(viloData.records[0].fields.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
