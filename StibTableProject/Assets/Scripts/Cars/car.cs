using StibAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car : MonoBehaviour
{

    public nodeScript lastNode;
    public nodeScript nextNode;

    public float moveSpeed;
    public float rotSpeed;

    public bool canMove = true;
    const float baseSpeed = 0.4f;

    public bool isPathFinding;
    public List<nodeScript> path;

    //headlights
    private SpriteRenderer headLights;
    public Material headLightsON;
    public Material headLightsOFF;
    public Light spotLeft;
    public bool lightsON;
    private GameObject manager;
    private carsManager carManager;
    private GameObject destroyer;
    private GameObject entryParent;
    private int carID;

    public int CarID
    {
        get
        {
            return carID;
        }

        set
        {
            carID = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        headLights = transform.GetChild(0).GetComponent<SpriteRenderer>();
        headLights.enabled = false;
        moveSpeed = 0.4f;
        rotSpeed = 4;
        isPathFinding = false;
        lightsON = false;
        manager = GameObject.Find("TramsManager");
        carManager = GameObject.Find("noeuds").GetComponent<carsManager>();
        destroyer = GameObject.Find("destroyer");
        entryParent = GameObject.Find("Entrees");
    }

    public void SwitchLightsIntensity()
    {
        
        if (manager.GetComponent<VisualStuff>().dayMode)
        {
            headLights.enabled = false;
            //spotLeft.intensity = 0f;
            lightsON = false;
        }
        else
        {
            headLights.enabled = true;
            //spotLeft.intensity = 8f;
            lightsON = true;
        }
    }

    //génère le chemin entre 
    public void FindPath(nodeScript end)
    {
        int memorycheck = 0;
        //Définit le début au prochain voisin vu qu'il va le lancer quand il l'atteindra
        nodeScript start = nextNode;
        //liste des nodes déjà traitées
        HashSet<nodeScript> closedSet = new HashSet<nodeScript>();
        Queue<nodeScript> q = new Queue<nodeScript>();
        q.Enqueue(start);
        while (q.Count != 0)
        {
            nodeScript firstOfList = q.Dequeue();
            if (firstOfList == end)
            {
                RetracePath(start, end);

                //si y a bien un chemin et qu'on a pas tracé jusqu'au point suivant
                if (path.Count > 0)
                {
                    //passe ispathfinding à true et peut gérer dans le choosenextnode la suite
                    isPathFinding = true;
                }

                break;
            }
            foreach (nodeScript n in firstOfList.voisins)
            {
                //évite les shortcut par les sorties vu qu'elles rerentrent dans des points aléatoires
                if (!closedSet.Contains(n))
                {
                    if (!n.isExitPoint)
                    {
                        memorycheck += 1;
                        closedSet.Add(n);
                        n.parent = firstOfList;
                        q.Enqueue(n);
                    }

                }
            }
        }

    }
    //méthode servant à dessiner le chemin
    void RetracePath(nodeScript startNode, nodeScript endNode)
    {
        path = new List<nodeScript>();
        nodeScript currentNode = endNode;

        //recrée la liste des nodes qui mènent à la solution pour dessiner le chemin
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        Debug.Log("Nombre de nodes dans le path : " + path.Count);
    }

    public void ChoseNextNode()
    {
        //si il n'est pas en mode path finding
       // if (!isPathFinding)
       // {
            lastNode = nextNode;
            List<nodeScript> voisins = lastNode.voisins;
            int lowestTrafic = 1000;
            //par défaut, envoi vers voisin 0
            nodeScript chosenNode = voisins[0];

            //choix du next node random dans la ville
            if (!nextNode.isExitPoint)
            {
                if (Random.Range(0, 10) >= 5)
                {
                    foreach (nodeScript node in voisins)
                    {
                        if (node.SurchargeTrafic < lowestTrafic)
                        {
                            lowestTrafic = node.SurchargeTrafic;
                            chosenNode = node;
                        }
                    }
                }
                else
                {
                    chosenNode = voisins[Random.Range(0, voisins.Count)];
                }
                nextNode = chosenNode;
            }
            //choix du point d'entrée de la ville en fonction du trafic
            else
            {
                int rand = Random.Range(1, 99);
                int[] trafficPercentage = carManager.TrafficBalance;
                int totalValue = 0;
                for (int i = 0; i < trafficPercentage.Length; i++)
                {
                    totalValue += trafficPercentage[i];
                    if (rand <= totalValue)
                    {
                        //le point d'entrée choisi est i
                        //Debug.Log("rand = " + rand + " i = " + i);
                        chosenNode = entryParent.transform.GetChild(i).gameObject.GetComponent<nodeScript>();
                        break;
                    }
                }

                nextNode = chosenNode;
                if (carManager.CarAmountToDestroy > 0)
                {
                    StartCoroutine(DestroyAfterOneSec());
                }
                else transform.position = nextNode.transform.position;
            }
            //redéfinit sa vitesse de manière aléatoire à chaque nouveau voisin
            moveSpeed = baseSpeed * Random.Range(0.75f, 1.25f);
        //}
        //si il est en mode path finding
        /*else
        {
            Debug.Log("changed when ispathfinding");
            //lastNode = path[0];
            nextNode = path[0];
            path.RemoveAt(0);
            //si il reste plus qu'un element dans la liste c'est la destination et quand on l'attendra il faut repasser en mode sans path finding
            if (path.Count == 0)
            {
                Debug.Log("passe ispathfindig à false");
                isPathFinding = false;
                transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.white;
            }
            moveSpeed = baseSpeed * Random.Range(0.75f, 1.25f);
        }*/
    }

    IEnumerator DestroyAfterOneSec()
    {
        gameObject.transform.position = destroyer.transform.position;
        //carManager.CarAmountToDestroy--;
        yield return new WaitForSeconds(1f);
        carManager.CarAmountToDestroy--;
        carManager.activeCars.Remove(this.GetComponent<car>());
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            //moving the car
            transform.position = Vector3.MoveTowards(transform.position, nextNode.transform.position, Time.deltaTime * moveSpeed);
        }

        //rotating the car
        Vector3 targetDir = nextNode.transform.position - transform.position;
        float step = rotSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);

        //check if the car arrived to destination
        if (transform.position == nextNode.transform.position) ChoseNextNode();
    }
}
