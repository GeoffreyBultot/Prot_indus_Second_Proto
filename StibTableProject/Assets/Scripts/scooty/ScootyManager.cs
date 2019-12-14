using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScootyManager : MonoBehaviour
{

    private RootObjectScooty dataScooty;


    // !!!! Latitude inversée de haut en bas par rapport aux coord Unity
    private float minLong = 4.35406f;
    private float maxLong = 4.37056f;
    private float longRange = 0.0165f;
    private float minLat = 50.83916f;
    private float maxLat = 50.85512f;
    private float latRange = 0.01596f;

    private float minX = -12.9f;
    private float maxX = 12.9f;
    private float XRange = 25.8f;
    private float minY = -19.4f;
    private float maxY = 19.4f;
    private float YRange = 38.8f;

    public GameObject scootyPrefab;
    public Transform scootyParent;

    //convertit les coord lat/long récupérées de la position du scooty en X/Y sur la carte pour le positionner au bon endroit
    private float[] ConvertCoordToXY(float lat, float lon)
    {

        //fait le pourcentage de latitude par rapport à la maquette
        //en X / longitude
        //float tempLong = lon - minLong;
        float longPercent = (lon - minLong) / (maxLong - minLong);
        float tempX = longPercent * XRange;
        float XValue = maxX - tempX;

        //en Y / latitude
        float tempLat = lat - minLat;
        float latPercent = tempLat / latRange;
        float tempY = latPercent * YRange;
        float YValue = maxY - tempY;

        float[] XYCoord = { XValue, YValue };
        //Debug.Log("Longitude = " + lon + " et X calculé = " + XValue);
        //Debug.Log("Latitude = " + lat + " et Y calculé = " + YValue);
        return XYCoord;
    }

    //trouver le scooty le plus proche pour le highlight et tracer le chemin
    public Transform GetClosestScooty(Transform posToBeCLoseTo)
    {
        Transform closestScooty = null;
        //récupère les scootys existant dans la scène
        List<ScootyScript> actualScootys = GetActualSpawnedScootys();
        foreach (ScootyScript scooty in actualScootys)
        {
            if (closestScooty == null) closestScooty = scooty.transform;
            else
            {
                //sauvegarde dans closestscooty le plus proche
                if (Vector3.Distance(scooty.transform.position, posToBeCLoseTo.position)
                    < Vector3.Distance(closestScooty.position, posToBeCLoseTo.position))
                {
                    closestScooty = scooty.transform;
                }
            }
        }
        return closestScooty;
    }

    //Instancie et affiche un nouveau scooty
    private void InstantiateAndDisplayScooty(float[] coord, Scooty scooty)
    {
        //instantie un gameobject avec dans l'ordre des paramètres : 1)préfab unity d'un scooty 
        //2)position en vector3 dans le plan 3)rotation 4)gameobject parent dans la hiérarchie pour les lister
        GameObject myScooty = Instantiate(scootyPrefab, new Vector3(coord[0], -5, coord[1]), Quaternion.identity, scootyParent);
        myScooty.transform.Rotate(new Vector3(90f, 270f, 0));
        //Récupère le composant Scooty pour lui donner ses attributs
        ScootyScript scootyScript = myScooty.GetComponent<ScootyScript>();
        scootyScript.Id = scooty.id;
        scootyScript.Lat = scooty.lat;
        scootyScript.Lon = scooty.lon;
        scootyScript.Energy = int.Parse(scooty.vehicleDetails.energy);
    }

    //Récupère les scootys de la scène
    private List<ScootyScript> GetActualSpawnedScootys()
    {
        List<ScootyScript> actualScootys = new List<ScootyScript>();
        foreach (Transform scootyItem in scootyParent)
        {
            ScootyScript scooty = scootyItem.gameObject.GetComponent<ScootyScript>();
            actualScootys.Add(scooty);
        }
        return actualScootys;
    }

    //exécuté quand les nouvelles données sont récupérées de l'API
    public void UpdateData(RootObjectScooty data)
    {
        dataScooty = data;

        //Récupère les scootys dans la scène et les supprime avant de les réafficher
        List<ScootyScript> actualScootys = GetActualSpawnedScootys();
        DeleteActualScooties(actualScootys);
        //pour chaque scooty du dataset, convertit ses coord en x y et l'affiche
        foreach (Scooty scooty in dataScooty.scootys)
        {
            //si le scooty est dans la zone à afficher
            if (scooty.lat > minLat && scooty.lat < maxLat && scooty.lon > minLong && scooty.lon < maxLong)
            {
                float[] XYCoord = ConvertCoordToXY(scooty.lat, scooty.lon);
                InstantiateAndDisplayScooty(XYCoord, scooty);
            }
        }
    }

    private void DeleteActualScooties(List<ScootyScript> scootiesToDelete)
    {
        //delete tout et réaffiche proprement
        foreach (ScootyScript sc in scootiesToDelete)
        {
            Destroy(sc.gameObject);
        }
        
    }


}
