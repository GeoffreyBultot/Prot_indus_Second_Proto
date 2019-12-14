using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HourManager : MonoBehaviour {

    Text hourTxt;
    string minut;
    string hour;
	// Use this for initialization
	void Start () {
        hourTxt = GetComponent<Text>();
        //StartCoroutine("ChangeHour");
    }
    // Update is called once per frame
    void Update () {
        // execute block of code here
        if (System.DateTime.Now.Minute == 0) minut = "00";
        else if (System.DateTime.Now.Minute < 10)
        {
            minut = "0" + System.DateTime.Now.Minute.ToString();
        }
        else minut = System.DateTime.Now.Minute.ToString();
        if (System.DateTime.Now.Hour == 0) hour = "00";
        else hour = System.DateTime.Now.Hour.ToString();
        hourTxt.text = hour + ":" + minut + "                " + System.DateTime.Now.Day.ToString() + "/" + System.DateTime.Now.Month.ToString() + "/" + System.DateTime.Now.Year.ToString();

    }
    IEnumerator ChangeHour()
    {
        for (; ; )
        {
            // execute block of code here
            if (System.DateTime.Now.Minute == 0) minut = "00";
            else if (System.DateTime.Now.Minute < 10)
            {
                minut = "0" + System.DateTime.Now.Minute.ToString();
            }
            else minut = System.DateTime.Now.Minute.ToString();
            if (System.DateTime.Now.Hour == 0) hour = "00";
            else hour = System.DateTime.Now.Hour.ToString();
            hourTxt.text = hour + ":" + minut + " " + System.DateTime.Now.Day.ToString() + "/" + System.DateTime.Now.Month.ToString() + "/" + System.DateTime.Now.Year.ToString();


            yield return new WaitForSeconds(1f);
        }
    }
}
