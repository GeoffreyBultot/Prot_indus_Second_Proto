using UnityEngine.UI;
using UnityEngine;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using System.Net.Security;
using System.Collections;

public class MeteoYahooAPI : MonoBehaviour
{
    public WeatherData weatherData;

    const string cURL = "https://weather-ydn-yql.media.yahoo.com/forecastrss";  // url
    public string cAppID = "sP5zrc56";   // ID app obtenu sur Yahoo.
    public string cConsumerKey = "dj0yJmk9elJEZFZvRXo3SnNHJnM9Y29uc3VtZXJzZWNyZXQmc3Y9MCZ4PTcw";  // key client obtenu sur Yahoo.
    public string cConsumerSecret = "50284ae287d6827328828566bb70ca9ca17b3347"; // secret code client obtenu sur Yahoo.
    public string cWeatherID = "woeid=968019";  // Localisation fixée à bruxelles.
    const string cOAuthVersion = "1.0";
    const string cOAuthSignMethod = "HMAC-SHA1";
    const string cUnitID = "u=c"; // c pour metrique, f pour imperiale.
    const string cFormat = "json"; // xml ou json

    public Text tempText;
    public Text sunriseText;
    public Text sunsetText;
    public Text timeNowText;
    public int[] weatherCondition;

    public string temperature;
    public DateTime timeSunrise;
    public DateTime timeSunset;
    public float MAJ = 300f;
    public InternetManager internetManager;
    private WebClient lClt;
    private string lURL;

    void Start()
    {
        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });   // Pour ne pas avoir de problème de certificat.
        weatherCondition = new int[8];
        lURL = cURL + "?" + cWeatherID + "&" + cUnitID + "&format=" + cFormat;
        
        InvokeRepeating("WeatherUpdating", 2f, MAJ);   // Appel la méthode "WeatherUdating" 0 secondes après le lancement de l'application et puis toutes les 15 secondes (1 minute)
        InvokeRepeating("UpdateTemperatureDisplay", 5f, MAJ);   // Appel la méthode "WeatherUdating" 0 secondes après le lancement de l'application et puis toutes les 15 secondes (1 minute)
    }

    void UpdateTemperatureDisplay()
    {
        tempText.text = temperature;
    }

    void WeatherUpdating()
    {
        //Debug.Log("Updating weather data... " + DateTime.Now.ToString("HH:mm:ss"));
        if (internetManager.CheckInternetConnection()) StartCoroutine(getData());      // La fonction ne prend aucun paramètre et ne renvoie rien. Elle va uniquement envoyé la demande et la recevoir ensuite pour creer une classe et spécifié les variables.
        else
        {
            Debug.Log("no internet connection for METEO");
        }
    }

    IEnumerator getData()
    {
        lClt = new WebClient();
        lClt.Headers.Set("Content-Type", "application/" + cFormat);
        lClt.Headers.Add("X-Yahoo-App-Id", cAppID);
        lClt.Headers.Add("Authorization", _get_auth());
        lClt.DownloadDataCompleted += new DownloadDataCompletedEventHandler(DownloadDataCompleted);
        lClt.DownloadDataAsync(new Uri(lURL));
        yield return null;
    }

    void DownloadDataCompleted(object sender,
    DownloadDataCompletedEventArgs e)
    {
        Debug.Log("download completed");
        byte[] lDataBuffer = e.Result;
        string jsonString = Encoding.ASCII.GetString(lDataBuffer);      // On converti les bytes en caractères ASCII qu'on dépose ensuite dans un string.
        weatherData = JsonUtility.FromJson<WeatherData>(jsonString);    // On converti le string qui avait une structure JSON en classe contenant différentes variable.
        //Debug.Log(jsonString);
        AssignData();   // La fonction ne prend aucun paramètre et ne renvoie rien. Elle va assigner les variables de la classe qui a été crée précédemment dans les variables de la classe actuelle.
    }
    void AssignData()
    {
        timeSunrise = StringToDateTime(weatherData.current_observation.astronomy.sunrise);  // On appelle la fonction StringToDateTime qui converti un string en DateTime pour obtenir l'heure du levée du soleil.
        timeSunset = StringToDateTime(weatherData.current_observation.astronomy.sunset);    // On appelle la fonction StringToDateTime qui converti un string en DateTime pour obtenir l'heure du couchée du soleil.
        temperature = weatherData.current_observation.condition.temperature + " °C ";   // On assigne à tempText la température qu'il fait au format numérique.     
        //sunriseText.text = timeSunrise.ToString("HH:mm");     // On assigne à sunriseText l'heure du levée de soleil sous format DateTime (obtenu grace a la méthode StringToDateTime).
        //sunsetText.text = timeSunset.ToString("HH:mm");        // On assigne à sunriseText l'heure du couchée de soleil sous format DateTime (obtenu grace a la méthode StringToDateTime).
        //timeNowText.text = DateTime.Now.ToString("HH:mm");   // On assigne à timeNowText l'heure actuel sous format DateTime.

        InitializeTable(weatherCondition);                                  // On initialise le tableau qui sera renvoyé à chaque gameObject.
        AssignWeatherCode(weatherData.current_observation.condition.code);  // On donne a chaque case du tableau la valeur correspondante au temps qu'il fait en passant par le code météo.
    }

    // Les méthodes "_get_auth" et "_get_timestamp" et "_get_nonce" ne sont pas de moi, ils ont été donné par Yahoo.

    string _get_auth()
    {
        //string retVal;
        string lNonce = _get_nonce();
        string lTimes = _get_timestamp();
        string lCKey = string.Concat(cConsumerSecret, "&");
        string lSign = string.Format(
         "format={0}&" +
         "oauth_consumer_key={1}&" +
         "oauth_nonce={2}&" +
         "oauth_signature_method={3}&" +
         "oauth_timestamp={4}&" +
         "oauth_version={5}&" +
         "{6}&{7}",
         cFormat,
         cConsumerKey,
         lNonce,
         cOAuthSignMethod,
         lTimes,
         cOAuthVersion,
         cUnitID,
         cWeatherID
        );

        lSign = string.Concat(
         "GET&", Uri.EscapeDataString(cURL), "&", Uri.EscapeDataString(lSign)
        );

        using (var lHasher = new HMACSHA1(Encoding.ASCII.GetBytes(lCKey)))
        {
            lSign = Convert.ToBase64String(
             lHasher.ComputeHash(Encoding.ASCII.GetBytes(lSign))
            );
        }

        return "OAuth " +
               "oauth_consumer_key=\"" + cConsumerKey + "\", " +
               "oauth_nonce=\"" + lNonce + "\", " +
               "oauth_timestamp=\"" + lTimes + "\", " +
               "oauth_signature_method=\"" + cOAuthSignMethod + "\", " +
               "oauth_signature=\"" + lSign + "\", " +
               "oauth_version=\"" + cOAuthVersion + "\"";
    }

    string _get_timestamp()
    {
        TimeSpan lTS = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return Convert.ToInt64(lTS.TotalSeconds).ToString();
    }

    string _get_nonce()
    {
        return Convert.ToBase64String(
         new ASCIIEncoding().GetBytes(
          DateTime.Now.Ticks.ToString()
         )
        );
    }



    public bool IsItTheDay(DateTime timeSunset, DateTime timeSunrise)
    {
        if ((!(DateTime.Compare(DateTime.Now, timeSunrise) > 0) && (DateTime.Compare(timeSunset, DateTime.Now) > 0)))
        {
            return true;    // Return TRUE si il fait jour (calculé en fonction de l'heure du levée et du couché du soleil).
        }
        else
        {
            return false;   // Return FALSE si il fait noir (calculé en fonction de l'heure du levée et du couché du soleil). 
        }
    }

    public DateTime StringToDateTime(string dateTime)
    {
        return DateTime.ParseExact(dateTime, "h:mm tt", CultureInfo.InvariantCulture);  // Return la conversion du dateTime au format string au format DateTime.
    }

    void InitializeTable(int[] weatherCondition)
    {
        for (int i = 0; i < weatherCondition.Length; i++)
        {
            weatherCondition[i] = 0;
            // 0 is for sun (0,10) (Disable, enable)
            // 1 is for white cloud (0,3,6,10) (Disable, low, medium, high)
            // 2 is for color cloud (0,10) (Disable, enable) (for white cloud or grey cloud)
            // 3 if for fog/mist (0,10) (Disable, enable)
            // 4 is for rain (0,3,6,10) (Disable, low, medium, high)-
            // 5 is for snow (0,3,6,10) (Disable, low, medium, high)-
            // 6 is for thunderstorm (0,5,10) (Disable, low, high)-
            // 7 is for speed cloud (0,10) (Disable, enable) (for slow or fast)
        }
    }

    void AssignWeatherCode(int code)
    {
        switch (code)
        {
            case 0: // tornado
            case 1: // tropical storm
            case 2: // hurricane
                weatherCondition[0] = 0;
                weatherCondition[1] = 6;
                weatherCondition[2] = 10;
                weatherCondition[3] = 5;
                weatherCondition[4] = 3;
                weatherCondition[5] = 0;
                weatherCondition[6] = 0;
                weatherCondition[7] = 10;
                break;
            case 3: // severe thunderstorms
                weatherCondition[0] = 0;
                weatherCondition[1] = 6;
                weatherCondition[2] = 10;
                weatherCondition[3] = 0;
                weatherCondition[4] = 3;
                weatherCondition[5] = 0;
                weatherCondition[6] = 10;
                weatherCondition[7] = 0;
                break;
            case 4: // thunderstorms
            case 37: // isolated thunderstorms
            case 38: // scattered thunderstorms
            case 39: // scattered thunderstorms
            case 45: // thundershowers
            case 47: // isolated thundershowers
                weatherCondition[0] = 0;
                weatherCondition[1] = 6;
                weatherCondition[2] = 10;
                weatherCondition[3] = 0;
                weatherCondition[4] = 3;
                weatherCondition[5] = 0;
                weatherCondition[6] = 5;
                weatherCondition[7] = 0;
                break;
            case 5: // mixed rain and snow
                weatherCondition[0] = 0;
                weatherCondition[1] = 3;
                weatherCondition[2] = 0;
                weatherCondition[3] = 0;
                weatherCondition[4] = 6;
                weatherCondition[5] = 6;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;
            case 6: // mixed rain and sleet
            case 35: // mixed rain and hail
                weatherCondition[0] = 0;
                weatherCondition[1] = 3;
                weatherCondition[2] = 0;
                weatherCondition[3] = 0;
                weatherCondition[4] = 6;
                weatherCondition[5] = 3;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;
            case 7: // mixed snow and sleet
                weatherCondition[0] = 0;
                weatherCondition[1] = 3;
                weatherCondition[2] = 0;
                weatherCondition[3] = 0;
                weatherCondition[4] = 3;
                weatherCondition[5] = 3;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;
            case 8: // freezing drizzle
                weatherCondition[0] = 0;
                weatherCondition[1] = 3;
                weatherCondition[2] = 0;
                weatherCondition[3] = 5;
                weatherCondition[4] = 0;
                weatherCondition[5] = 3;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;
            case 9: // drizzle
                weatherCondition[0] = 0;
                weatherCondition[1] = 3;
                weatherCondition[2] = 0;
                weatherCondition[3] = 5;
                weatherCondition[4] = 3;
                weatherCondition[5] = 0;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;
            case 10: // freezing rain
                weatherCondition[0] = 0;
                weatherCondition[1] = 3;
                weatherCondition[2] = 0;
                weatherCondition[3] = 5;
                weatherCondition[4] = 3;
                weatherCondition[5] = 3;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;
            case 11: // showers
            case 12: // showers
            case 40: // scattered showers
                weatherCondition[0] = 0;
                weatherCondition[1] = 6;
                weatherCondition[2] = 10;
                weatherCondition[3] = 5;
                weatherCondition[4] = 10;
                weatherCondition[5] = 0;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;
            case 13: // snow flurries
                weatherCondition[0] = 0;
                weatherCondition[1] = 3;
                weatherCondition[2] = 0;
                weatherCondition[3] = 0;
                weatherCondition[4] = 0;
                weatherCondition[5] = 6;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;
            case 14: // light snow showers
            case 42: // scattered snow showers
                weatherCondition[0] = 0;
                weatherCondition[1] = 3;
                weatherCondition[2] = 0;
                weatherCondition[3] = 0;
                weatherCondition[4] = 0;
                weatherCondition[5] = 3;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;
            case 15: // blowing snow
                weatherCondition[0] = 0;
                weatherCondition[1] = 10;
                weatherCondition[2] = 0;
                weatherCondition[3] = 10;
                weatherCondition[4] = 0;
                weatherCondition[5] = 10;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;
            case 16: // snow
            case 41: // heavy snow
            case 43: // heavy snow
                weatherCondition[0] = 0;
                weatherCondition[1] = 3;
                weatherCondition[2] = 0;
                weatherCondition[3] = 0;
                weatherCondition[4] = 0;
                weatherCondition[5] = 10;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;
            case 17: // hail
            case 18: // sleet
                weatherCondition[0] = 0;
                weatherCondition[1] = 3;
                weatherCondition[2] = 10;
                weatherCondition[3] = 0;
                weatherCondition[4] = 3;
                weatherCondition[5] = 3;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;
            case 19: // dust
            case 20: // foggy
            case 21: // haze
            case 22: // smoky
                weatherCondition[0] = 0;
                weatherCondition[1] = 10;
                weatherCondition[2] = 0;
                weatherCondition[3] = 10;
                weatherCondition[4] = 0;
                weatherCondition[5] = 0;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;
            case 23: // blustery
            case 24: // windy
                weatherCondition[0] = 0;
                weatherCondition[1] = 6;
                weatherCondition[2] = 0;
                weatherCondition[3] = 0;
                weatherCondition[4] = 0;
                weatherCondition[5] = 0;
                weatherCondition[6] = 0;
                weatherCondition[7] = 10;
                break;
            case 25: // cold
                weatherCondition[0] = 0;
                weatherCondition[1] = 6;
                weatherCondition[2] = 0;
                weatherCondition[3] = 0;
                weatherCondition[4] = 0;
                weatherCondition[5] = 0;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;
            case 26: // cloudy
                weatherCondition[0] = 0;
                weatherCondition[1] = 6;
                weatherCondition[2] = 10;
                weatherCondition[3] = 5;
                weatherCondition[4] = 0;
                weatherCondition[5] = 0;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;
            case 27: // mostly cloudy(night)
            case 28: // mostly cloudy(day)
                weatherCondition[0] = 0;
                weatherCondition[1] = 10;
                weatherCondition[2] = 10;
                weatherCondition[3] = 10;
                weatherCondition[4] = 0;
                weatherCondition[5] = 0;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;
            case 29: // partly cloudy(night)
            case 30: // partly cloudy(day)
            case 44: // partly cloudy
                weatherCondition[0] = 0;
                weatherCondition[1] = 3;
                weatherCondition[2] = 10;
                weatherCondition[3] = 5;
                weatherCondition[4] = 0;
                weatherCondition[5] = 0;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;
            case 31: // clear(night)
                break;
            case 32: // sunny
            case 36: // hot
                weatherCondition[0] = 10;
                weatherCondition[1] = 0;
                weatherCondition[2] = 0;
                weatherCondition[3] = 0;
                weatherCondition[4] = 0;
                weatherCondition[5] = 0;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;

                break;
            case 33: // fair(night)
                weatherCondition[0] = 0;
                weatherCondition[1] = 3;
                weatherCondition[2] = 0;
                weatherCondition[3] = 0;
                weatherCondition[4] = 0;
                weatherCondition[5] = 0;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;
            case 34: // fair(day)
                weatherCondition[0] = 10;
                weatherCondition[1] = 3;
                weatherCondition[2] = 0;
                weatherCondition[3] = 0;
                weatherCondition[4] = 0;
                weatherCondition[5] = 0;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;

            case 46: // snow showers
                weatherCondition[0] = 0;
                weatherCondition[1] = 3;
                weatherCondition[2] = 0;
                weatherCondition[3] = 0;
                weatherCondition[4] = 0;
                weatherCondition[5] = 6;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;
            case 3200: // not available
                break;
        }

        
    }

}