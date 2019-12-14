using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace StibAPI
{
    public class DataSet : MonoBehaviour
    {
        public static DataSet Instance;
        public InternetManager internetManager;
        /*
         * Pour ajouter un arrêt : ajout de l'ID de l'arrêt dans l'URL en ajoutant à la fin %2C(virgule en url) + ID de l'arret
         * Il faut ensuite créer un GO dans le parent Arret avec le nom correspondant à l'ID de l'arrêt
         * Le programme pourra ainsi comparer automatiquement l'ID de l'ârret avec les composants Unity
         */

        public TramManager manager;
        
        //conteneur des données récupérées de la stib
        public Rootobject stibData;
        //URL récupéré à travers Oauth
        //private string requestedURL;

        private void Awake()
        {
            Instance = this;
            stibData = new Rootobject();
            StartCoroutine(ContactSTIB());
        }

        void Start()
        {

        }

        //méthode invoquée toutes les 21 secondes pour lancer la requête à l'api de la stib
        private void DataImport()
        {
            //début de la coroutine, URL de la stib contenant les 3 x 2 arrêts qui nous intéressent
            if (internetManager.CheckInternetConnection()) StartCoroutine(GetVehiculesInfos("https://opendata-api.stib-mivb.be/OperationMonitoring/1.0/PassingTimeByPoint/6357%2C6308"));
            else
            {
                Debug.Log("no internet connection for STIB");

            }
        }

        IEnumerator ContactSTIB()
        {

            //Requêtes des datas de la stib toutes les 21 secondes car leurs données sont refresh toutes les 20 secondes
            InvokeRepeating("DataImport", 0, 60f);
            yield return new WaitForSeconds(1);

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
            Debug.Log("Data received from STIB.");
           // Debug.Log(request.downloadHandler.text);

            //traitement du JSON récupéré et envoi dans la classé créée au préalable
            stibData = JsonUtility.FromJson<Rootobject>(request.downloadHandler.text);
            manager.UpdateData(stibData);
            request.Dispose();
        }
    }

}