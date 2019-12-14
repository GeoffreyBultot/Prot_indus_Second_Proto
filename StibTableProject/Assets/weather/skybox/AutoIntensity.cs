using UnityEngine;
using System.Collections;
using DigitalRuby.WeatherMaker;

public class AutoIntensity : MonoBehaviour
{

    //public Gradient nightDayColor;

    //public float maxIntensity = 3f;
    //public float minIntensity = 0f;
    //public float minPoint = -0.2f;

    //public float maxAmbient = 1f;
   // public float minAmbient = 0f;
   // public float minAmbientPoint = -0.2f;


    //public Gradient nightDayFogColor;
   // public AnimationCurve fogDensityCurve;
    //public float fogScale = 1f;

    //public float dayAtmosphereThickness = 0.4f;
    //public float nightAtmosphereThickness = 0.87f;

    public Vector3 dayRotateSpeed;
    public Vector3 nightRotateSpeed;

    float skySpeed = 1;


    Light mainLight;
    //Skybox sky;
    //Material skyMat;
    //public float tRange;
    public float dot;
    // public float i;
    public string sunriseHour;
    public string sunsetHour;
    float dayPercentage;
    float secondsOfDay;
    float secondsOfNight;
    float xAngleForDayCycle;
    float xAngleForNightCycle;
    float percentSunOfTheDay;
    private bool justStarted;
    private bool passedHalf;

    private bool dayState;
    public WeatherMakerLegacyCloudScript2D clouds;
    public GameObject stars;

    void Start()
    {
        dayState = true;
        mainLight = GetComponent<Light>();
        //skyMat = RenderSettings.skybox;
        //entrer l'heure du lever et couché de soleil pour calculer la position du soleil initiale et la vitesse du cycle jour nuit
        CalculateSunPosAndCycleSpeed(sunriseHour, sunsetHour);
        //dayRotateSpeed = new Vector3(0, 0, -xAngleForDayCycle);
        //nightRotateSpeed = new Vector3(0, 0, -xAngleForNightCycle);
    }

    void CalculateSunPosAndCycleSpeed(string sunrise, string sunset)
    {
        //vitesse par défaut
        dayRotateSpeed = new Vector3(0, 0, -20);
        nightRotateSpeed = new Vector3(0, 0, -20);
        justStarted = true;
        passedHalf = false;

        float sunPercentage = 0;
        float sunriseInMin = 0;
        float sunsetInMin = 0;
        string[] splitHour = sunrise.Split(char.Parse("."));
        //heure du levé de soleil en min
        sunriseInMin += 60 * int.Parse(splitHour[0]);
        sunriseInMin += int.Parse(splitHour[1]);
        splitHour = sunset.Split(char.Parse("."));
        //heure du couché de soleil en min
        sunsetInMin += 60 * int.Parse(splitHour[0]);
        sunsetInMin += int.Parse(splitHour[1]);
        //nombre de minutes de soleil
        float sunTimeInMin = sunsetInMin - sunriseInMin;
        //pourcentage de jour pas rapport à la nuit
        sunPercentage = (sunTimeInMin / 1440);

        //heure actuelle en minutes
        float actualTimeInMin = (System.DateTime.Now.Hour * 60) + System.DateTime.Now.Minute;

        //pourcentage d'avancée du soleil dans la journée
        percentSunOfTheDay = ((actualTimeInMin - sunriseInMin) / sunTimeInMin) * 100;
        Debug.Log("percentsunofday = " + percentSunOfTheDay);

        //calcul de la vitesse du cycle
        //Default value 86400f
        secondsOfDay = 86400 * sunPercentage;
        secondsOfNight = 86400 - secondsOfDay;
        xAngleForDayCycle = 144f / secondsOfDay;
        xAngleForNightCycle = 144f / secondsOfNight;
        Debug.Log("secondsofday = " + secondsOfDay);
        Debug.Log("secondsofnight = " + secondsOfNight);
    }

    private void SwitchDayState()
    {
        dayState = !dayState;
        if(dayState)
        {
            stars.SetActive(false);
            clouds.NumberOfClouds = 5;
        }
        else
        {
            stars.SetActive(true);
            clouds.NumberOfClouds = 0;

        }
    }

    void Update()
    {

        dot = Mathf.Clamp01(Vector3.Dot(mainLight.transform.forward, Vector3.down));
        if (dot > 0)
        {
            if (!dayState) SwitchDayState();
            transform.Rotate(dayRotateSpeed * Time.deltaTime * skySpeed, Space.World);
        }
        else
        {
            if (dayState) SwitchDayState();
            transform.Rotate(nightRotateSpeed * Time.deltaTime * skySpeed, Space.World);
        }
            


        if (justStarted)
        {
            //déplace le soleil où il faut en accéléré au démarrage
            //si entre 48 et 52, le met au sommet car retours bizarres en arrière
            if (percentSunOfTheDay > 48 && percentSunOfTheDay < 52)
            {
                if (dot > 0.647)
                {
                    dayRotateSpeed = new Vector3(0, 0, -xAngleForDayCycle);
                    nightRotateSpeed = new Vector3(0, 0, -xAngleForNightCycle);
                    justStarted = false;
                }

            }
            //si 47 ou moins, l'envoi proportionnellement entre dot = 0 et 0.666
            else if (percentSunOfTheDay <= 48)
            {
                if (dot > (0.651f * percentSunOfTheDay ) /50)
                {
                    dayRotateSpeed = new Vector3(0, 0, -xAngleForDayCycle);
                    nightRotateSpeed = new Vector3(0, 0, -xAngleForNightCycle);
                    justStarted = false;

                }


            }
            //si 53 ou plus, l'envoi proportionnellement entre dot = 0.666 et 0 car sommet à 0.709 puis redescend
            else
            {

                if (dot > 0.647) passedHalf = true;
                if (dot < (0.651f * (100-percentSunOfTheDay) ) /50 && passedHalf)
                {
                    dayRotateSpeed = new Vector3(0, 0, -xAngleForDayCycle);
                    nightRotateSpeed = new Vector3(0, 0, -xAngleForNightCycle);
                    justStarted = false;
                    Debug.Log("dayrotat x = " + xAngleForDayCycle);
                    Debug.Log("nightrotat x = " + xAngleForNightCycle);
                }

            }
        }




        //i = ((maxIntensity - minIntensity) * dot) + minIntensity;

        //mainLight.intensity = i;

        //tRange = 1 - minAmbientPoint;
        //dot = Mathf.Clamp01((Vector3.Dot(mainLight.transform.forward, Vector3.down) - minAmbientPoint) / tRange);
        //i = ((maxAmbient - minAmbient) * dot) + minAmbient;
        //RenderSettings.ambientIntensity = i;

        //mainLight.color = nightDayColor.Evaluate(dot);
        //RenderSettings.ambientLight = mainLight.color;

        //RenderSettings.fogColor = nightDayFogColor.Evaluate(dot);
        //RenderSettings.fogDensity = fogDensityCurve.Evaluate(dot) * fogScale;

        //i = ((dayAtmosphereThickness - nightAtmosphereThickness) * dot) + nightAtmosphereThickness;
        //skyMat.SetFloat("_AtmosphereThickness", i);


        //if (Input.GetKeyDown(KeyCode.Q)) skySpeed *= 0.5f;
        //if (Input.GetKeyDown(KeyCode.E)) skySpeed *= 2f;
    }
}