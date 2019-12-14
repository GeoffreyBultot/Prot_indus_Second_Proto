using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public AirQualitySystem AirQualitySystem;
    public int MAJ;
    public int angle;
    public int Index;
    public float IndexNR;
    public int test;

    void Start()
    {
        InvokeRepeating("AirQualityIndexUpdating", .3f, MAJ);   // Appel la méthode "WeatherUdating" 0 secondes après le lancement de l'application et puis toutes les 900 secondes (15 minutes)
    }

    void AirQualityIndexUpdating()
    {
        test = AirQualitySystem.AQIUS;
        IndexNR = AirQualitySystem.AQIUS / 20;
        Index = (int)Mathf.Floor(IndexNR);

        switch (Index)
        {
            case 0:
                angle = 116;
                break;
            case 1:
                angle = 97;
                break;
            case 2:
                angle = 78;
                break;
            case 3:
                angle = 59;
                break;
            case 4:
                angle = 40;
                break;
            case 5:
                angle = 21;
                break;
            case 6:
                angle = 0;
                break;
            case 7:
                angle = -21;
                break;
            case 8:
                angle = -40;
                break;
            case 9:
                angle = -59;
                break;
            case 10:
                angle = -78;
                break;
            case 11:
                angle = -97;
                break;
            default:
                angle = -116;
                break;
        }
        transform.eulerAngles = new Vector3(0, 0, angle);
        Debug.Log(AirQualitySystem.AQIUS);
    }




}

