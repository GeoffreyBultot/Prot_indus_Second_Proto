using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualBasic;
using System.Globalization;


public class CityWeatherSystem : MonoBehaviour
{
    public WeatherData weatherData;

    const string cURL = "https://weather-ydn-yql.media.yahoo.com/forecastrss";  // url
    const string cAppID = "sP5zrc56";   // ID app obtenu sur Yahoo.
    const string cConsumerKey = "dj0yJmk9elJEZFZvRXo3SnNHJnM9Y29uc3VtZXJzZWNyZXQmc3Y9MCZ4PTcw";  // key client obtenu sur Yahoo.
    const string cConsumerSecret = "50284ae287d6827328828566bb70ca9ca17b3347"; // secret code client obtenu sur Yahoo.
    const string cOAuthVersion = "1.0";
    const string cOAuthSignMethod = "HMAC-SHA1";
    const string cWeatherID = "woeid=968019";  // Localisation fixée à bruxelles.
    const string cUnitID = "u=c"; // c pour metrique, f pour imperiale.
    const string cFormat = "json"; // xml ou json

    public Text tempText;
    public Text tempTText;
    public Text humText;
    public Text ventText;
    public Text sunriseText;
    public Text sunsetText;
    public Text timeNowText;
    public int[] weatherCondition = new int[8];

    DateTime timeSunrise;
    DateTime timeSunset;

    void Start()
    {
        InvokeRepeating("WeatherUpdating", 0f, 900f);   // Appel la méthode "WeatherUdating" 0 secondes après le lancement de l'application et puis toutes les 900 secondes (15 minutes)
    }

    void WeatherUpdating()
    {
        getData();      // La fonction ne prend aucun paramètre et ne renvoie rien. Elle va uniquement envoyé la demande et la recevoir ensuite pour creer une classe et spécifié les variables.
        AssignData();   // La fonction ne prend aucun paramètre et ne renvoie rien. Elle va assigner les variables de la classe qui a été crée précédemment dans les variables de la classe actuelle.
    }

    void getData()
    {
        const string lURL = cURL + "?" + cWeatherID + "&" + cUnitID + "&format=" + cFormat;
        var lClt = new WebClient();

        lClt.Headers.Set("Content-Type", "application/" + cFormat);     
        lClt.Headers.Add("Yahoo-App-Id", cAppID);                     
        lClt.Headers.Add("Authorization", _get_auth());                 

        byte[] lDataBuffer = lClt.DownloadData(lURL);                   // On met le contenu de la réponse dans un tableau de byte (car ce sont des bytes qui sont envoyé).
        string jsonString = Encoding.ASCII.GetString(lDataBuffer);      // On converti les bytes en caractères ASCII qu'on dépose ensuite dans un string.
        weatherData = JsonUtility.FromJson<WeatherData>(jsonString);    // On converti le string qui avait une structure JSON en classe contenant différentes variable.

        timeSunrise = StringToDateTime(weatherData.current_observation.astronomy.sunrise);  // On appelle la fonction StringToDateTime qui converti un string en DateTime pour obtenir l'heure du levée du soleil.
        timeSunset = StringToDateTime(weatherData.current_observation.astronomy.sunset);    // On appelle la fonction StringToDateTime qui converti un string en DateTime pour obtenir l'heure du couchée du soleil.
    }

    void AssignData()
    {
        tempTText.text = weatherData.current_observation.condition.text;    // On assigne à tempTText le temps qu'il fait au format string.
        tempText.text = weatherData.current_observation.condition.temperature + " °C (T. ressentie : " + weatherData.current_observation.wind.chill + " °C)";   // On assigne à tempText la température qu'il fait au format numérique.
        humText.text = weatherData.current_observation.atmosphere.humidity + " %";  // On assigne à humText l'humidité actuel sous forme de pourcentage.
        ventText.text = weatherData.current_observation.wind.speed + " km/h (Direction : " + weatherData.current_observation.wind.direction + "° )";    // On assigne à ventText la vitesse du vent ainsi que la direction.
        sunriseText.text = timeSunrise.Hour + "h" + timeSunrise.Minute;     // On assigne à sunriseText l'heure du levée de soleil sous format DateTime (obtenu grace a la méthode StringToDateTime).
        sunsetText.text = timeSunset.Hour + "h" + timeSunset.Minute;        // On assigne à sunriseText l'heure du couchée de soleil sous format DateTime (obtenu grace a la méthode StringToDateTime).
        timeNowText.text = DateTime.Now.Hour + "h" + DateTime.Now.Minute;   // On assigne à timeNowText l'heure actuel sous format DateTime.

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



    bool IsItTheDay(DateTime timeSunset, DateTime timeSunrise)
    {
        if (((DateTime.Compare(DateTime.Now, timeSunrise) == 1) && (DateTime.Compare(timeSunset, DateTime.Now) == 1)))  // Compare(a,b) renvoie 1 si "a" est plus avancé que "b", renvoie -1 si l'inverse, renvoie 0 si égale.
        {   // Je dois essayer sans la deuxieme condition si ça marche. Ca marcherait si la comparaison se fait en prenant en compte la date, si c'est juste l'heure ça ne marche pas. A VERIFIER !!
            return true;    // Return TRUE si il fait jour (calculé en fonction de l'heure du levée et du couché du soleil).
        }
        else
        {
            return false;   // Return FALSE si il fait noir (calculé en fonction de l'heure du levée et du couché du soleil). 
        }
    }

    DateTime StringToDateTime(string dateTime)
    {
        return DateTime.ParseExact(dateTime, "h:mm tt", CultureInfo.InvariantCulture);  // Return la conversion du dateTime au format string au format DateTime.
    }

    void InitializeTable(int[] weatherCondition)
    {
        for (int i = 0; i < 8; i++)
        {
            weatherCondition[i] = 0;
            // 0 is for sun (0,3,6,10) (Disable, low, medium, high)
            // 1 is for white cloud (0,3,6,10) (Disable, low, medium, high)
            // 2 is for color cloud (0,10) (Disable, enable) (for grey cloud or whith cloud)
            // 3 if for fog/mist (0,5,10) (Disable, low, high)
            // 4 is for rain (0,3,6,10) (Disable, low, medium, high)
            // 5 is for snow (0,3,6,10) (Disable, low, medium, high)
            // 6 is for thunderstorm (0,5,10) (Disable, low, high)
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
                weatherCondition[0] = 6;
                weatherCondition[1] = 0;
                weatherCondition[2] = 0;
                weatherCondition[3] = 0;
                weatherCondition[4] = 0;
                weatherCondition[5] = 0;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;

                break;
            case 33: // fair(night)
            case 34: // fair(day)
                weatherCondition[0] = 3;
                weatherCondition[1] = 3;
                weatherCondition[2] = 0;
                weatherCondition[3] = 0;
                weatherCondition[4] = 0;
                weatherCondition[5] = 0;
                weatherCondition[6] = 0;
                weatherCondition[7] = 0;
                break;
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