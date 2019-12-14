using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace ScootyAPI
{
    public class DataSetScooty : MonoBehaviour
    {
        public static DataSetScooty Instance;
        public ScootyManager manager;
        public RootObjectScooty scootyData;
        public InternetManager internetManager;

        // Use this for initialization
        void Start()
        {
            Instance = this;
            scootyData = new RootObjectScooty();
            InvokeRepeating("ScootyDataImport", 0, 60f);
        }

        private void ScootyDataImport()
        {
            //début de la coroutine, URL de la stib contenant les 3 x 2 arrêts qui nous intéressent
            if (internetManager.CheckInternetConnection()) StartCoroutine(GetScootyInfos("http://albert-prod.lab-box.be/v1/vehiculesPositions?query=closest&api_token=3528482b-314f-4c95-bfab-397c602b36e7&lat=50.846829&lon=4.362495&radius=0&provider=scooty&limit=1000"));
            else Debug.Log("no internet connection for Scooty");
        }

        IEnumerator GetScootyInfos(string uri)
        {
            //store l'url de requête dans une unitywebrequest
            UnityWebRequest request = UnityWebRequest.Get(uri);

            //ajoute les headers d'application + token à la requête
            request.SetRequestHeader("Accept", "application/json");

            //envoi la requête et attend la réponse
            yield return request.SendWebRequest();

            // Log de la réponse en UTF8
            Debug.Log("Data received from Scooty.");
            //Debug.Log(request.downloadHandler.text);


            //traitement du JSON récupéré et envoi dans la classé créée au préalable
            //string jsonModified = "{\"scootys\": " + request.downloadHandler.text + "}";
            //scootyData = JsonUtility.FromJson<RootObjectScooty>(jsonModified);
            //manager.UpdateData(scootyData);
            request.Dispose();
        }
    }
}