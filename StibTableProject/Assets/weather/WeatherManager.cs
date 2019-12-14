using DigitalRuby.WeatherMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour {

    public WeatherMakerLegacyCloudScript2D clouds;
    public WeatherMakerFallingParticleScript2D rain;
    public WeatherMakerFallingParticleScript2D snow;
    public WeatherMakerThunderAndLightningScript2D thunder;
    public ParticleSystem particles;
    public MeteoYahooAPI weatherDatasManager;
    Color lightGrey;
    Color darkGrey;

    // Use this for initialization
    void Start()
    {
        particles.Play();
        lightGrey = new Color32(0x96, 0x96, 0x96, 0xFF);
        darkGrey = new Color32(0x70, 0x70, 0x70, 0xFF);
        //mets à jour la météo toutes les 20 secondes
        InvokeRepeating("DefineState", 3f, 20f);
    }

    void SwitchState(string weatherType)
    {
        switch (weatherType)
        {
            case "cloud":
                clouds.NumberOfClouds = 30;
                clouds.TintColor = Color.white;
                rain.Intensity = 0;
                snow.Intensity = 0;
                thunder.EnableLightning = false;

                break;

            case "sun":
                clouds.NumberOfClouds = 5;
                clouds.TintColor = Color.white;
                rain.Intensity = 0;
                snow.Intensity = 0;
                thunder.EnableLightning = false;

                break;

            case "rain":
                clouds.NumberOfClouds = 30;
                clouds.TintColor = lightGrey;
                rain.Intensity = 0.5f;
                snow.Intensity = 0;
                thunder.EnableLightning = false;

                break;

            case "snow":
                clouds.NumberOfClouds = 20;
                clouds.TintColor = darkGrey;
                rain.Intensity = 0;
                snow.Intensity = 0.5f;
                thunder.EnableLightning = false;

                break;

            case "thunder":
                clouds.NumberOfClouds = 50;
                clouds.TintColor = darkGrey;
                rain.Intensity = 0.2f;
                snow.Intensity = 0;
                thunder.EnableLightning = true;

                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void DefineState() {
        Debug.Log("changing meteo");
        if (weatherDatasManager.weatherCondition[5] >=3)
        {
            SwitchState("snow");
        }
        else if (weatherDatasManager.weatherCondition[6] >= 5)
        {
            SwitchState("thunder");
        }
        else if (weatherDatasManager.weatherCondition[4] >= 3)
        {
            SwitchState("rain");
        }
        else if (weatherDatasManager.weatherCondition[1] >= 3)
        {
            SwitchState("cloud");
        }
        else
        {
            SwitchState("sun");
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            SwitchState("rain");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchState("cloud");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SwitchState("sun");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SwitchState("snow");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            SwitchState("thunder");
        }
    }
}
