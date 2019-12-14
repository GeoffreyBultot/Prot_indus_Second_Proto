using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carsManager : MonoBehaviour
{

    public nodeScript spawnPosition;
    public GameObject carPrefab;
    private List<nodeScript> spawnPositions;
    public List<car> activeCars;
    private Transform carParent;
    public TomTomApiTrafficFlow tomtomManager;

    private GameObject spawnedCar;
    public int spawnCount;
    int i = 0;
    int j = 0;

    //nombre de voiture à ajouter/supprimer en fonction du traffic réel, maj toutes les 5 min
    int carAmountToDestroy;
    int carAmountToCreate;
    int[] trafficBalance;
    public int CarAmountToDestroy
    {
        get
        {
            return carAmountToDestroy;
        }

        set
        {
            carAmountToDestroy = value;
        }
    }
    public int CarAmountToCreate
    {
        get
        {
            return carAmountToCreate;
        }

        set
        {
            carAmountToCreate = value;
        }
    }
    public int[] TrafficBalance
    {
        get
        {
            return trafficBalance;
        }

        set
        {
            trafficBalance = value;
        }
    }
    private GameObject entryParent;
    List<Transform> entries;

    public int carIDManager = 0;

    private void Start()
    {
        CarAmountToDestroy = 0;
        CarAmountToCreate = 0;
        entryParent = GameObject.Find("Entrees");
        entries = new List<Transform>();
        foreach (Transform child in entryParent.transform)
        {
            entries.Add(child);
        }
        trafficBalance = new int[] {20,20,20,20,20};
        spawnCount = 300;
        carParent = GameObject.Find("Cars").transform;
        GetSpawnedPoints();
        activeCars = new List<car>();
        StartCoroutine(StartSpawning());
    }

    #region Spawn des voitures au début
    void GetSpawnedPoints()
    {
        spawnPositions = new List<nodeScript>();
        foreach (Transform t in transform)
        {
            spawnPositions.Add(t.gameObject.GetComponent<nodeScript>());
        }
        Debug.Log("Successfuly found " + spawnPositions.Count + " spawn points");
    }

    void SpawnCar(int id)
    {
        if (j >= spawnPositions.Count) j = 0;
        spawnPosition = spawnPositions[j];
        GameObject myCar = Instantiate(carPrefab, spawnPosition.transform.position, Quaternion.identity, carParent);
        car carScript = myCar.GetComponent<car>();
        carScript.lastNode = spawnPosition;
        carScript.nextNode = spawnPosition.voisins[0];
        carScript.CarID = GetCarID();
        activeCars.Add(carScript);
        j++;
    }

    IEnumerator StartSpawning()
    {
        while (i < spawnCount)
        {
            yield return new WaitForSeconds(0.01f);
            SpawnCar(i);
            i++;
        }
        j = 0;
    }
    #endregion

    #region Gestion de la modification de la densité de traffic
    /*private int[] TrafficLoad(bool upOrDown)
    {
        int[] load;
        if (upOrDown) load = new int[] { 3, 3, 5, 3, 4 };
        else load = new int[] { 1, 1, 2, 1, 4 };
        return load;
    }*/

    public void ModifyTrafficDensity()
    {
        List<int> trafficLoad = tomtomManager.traficLoadAtFivePoints;
        UpdateCarCount(trafficLoad);
        int totalTrafficValue = 0;
        foreach (int value in trafficLoad)
        {
            totalTrafficValue += value;
        }
        for (int i = 0; i < trafficLoad.Count; i++)
        {
            TrafficBalance[i] = (trafficLoad[i] * 100) / totalTrafficValue;
            //Debug.Log(TrafficBalance[i]);
        }
    }

    private void UpdateCarCount(List<int> trafficLoad)
    {
        //nb de voitures à afficher varie entre 100 au min = 5 de traffic total et 300 au max = 25 de traffic total
        //du coup un point de traffic = 10 voitures
        int neededCarAmount = 50;
        for (int i = 0; i < trafficLoad.Count; i++)
        {
            neededCarAmount += (trafficLoad[i] * 10);
        }
        int actualCarAmount = carParent.childCount;
        int carDifference = actualCarAmount - neededCarAmount;
        if (carDifference > 0)
        {
            CarAmountToDestroy = carDifference;
            CarAmountToCreate = 0;
        }
        else
        {
            CarAmountToCreate = -carDifference;
            CarAmountToDestroy = 0;
        }

        if (carAmountToCreate > 0) IncreaseCarTraffic(carAmountToCreate);

        Debug.Log("neededcaramount " + neededCarAmount);
        Debug.Log("actualcaramount " + actualCarAmount);
    }
    #endregion

    #region Respanw des voitures quand traffic augmente
    //rajoute du traffic quand nécessaire
    void IncreaseCarTraffic(int carAmount)
    {
        StartCoroutine(RefillCars(carAmount));
        Debug.Log("lance la création de " + carAmount + " voitures");
    }

    IEnumerator RefillCars(int amount)
    {
        //récupère le point de spawn aléatoire en fonction du traffic
        for (int i = 0; i < amount; i++)
        {
            int rand = Random.Range(1, 99);
            int[] trafficPercentage = TrafficBalance;
            int totalValue = 0;
            Transform spawnPos = entries[0];
            for (int j = 0; j < trafficPercentage.Length; j++)
            {
                totalValue += trafficPercentage[j];
                if (rand <= totalValue)
                {
                    //point d'entrée choisi est j
                    spawnPos = entries[j];
                    break;
                }
            }
            //recrée une voiture et la fait apparaitre au bon endroit
            GameObject myCar = Instantiate(carPrefab, spawnPos.position, Quaternion.identity, carParent);
            car carScript = myCar.GetComponent<car>();
            carScript.lastNode = spawnPos.GetComponent<nodeScript>();
            carScript.nextNode = spawnPos.GetComponent<nodeScript>().voisins[0];
            activeCars.Add(carScript);
            yield return new WaitForSeconds(1f);
        }
    }
    #endregion

    public int GetCarID()
    {
        carIDManager++;
        return carIDManager;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    ModifyTrafficDensity();
        //}
        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    ModifyTrafficDensity();
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    Debug.Log("caramounttodestroy " + CarAmountToDestroy);
        //    Debug.Log("caramounttocreate " + CarAmountToCreate);
        //}
    }
}
