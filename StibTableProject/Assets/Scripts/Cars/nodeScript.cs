using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeScript : MonoBehaviour
{

    #region VARIABLES

    private int surchargeTrafic;
    public List<nodeScript> voisins;

    //définir si les points sont des entrées ou des sorties de manière à téléporter les voitures quand nécessaire
    public bool isExitPoint = false;
    public bool isEntryPoint = false;

    //le parent pour faire le path finding
    public nodeScript parent;

    public int SurchargeTrafic
    {
        get
        {
            return surchargeTrafic;
        }

        set
        {
            surchargeTrafic = value;
        }
    }

    int amount;
    Collider[] collidersInRange;

    #endregion


    // Use this for initialization
    void Start()
    {
        surchargeTrafic = 0;
        InvokeRepeating("CountSurchargeCars", 0, 1);
    }

    //récupère le nombre de voitures autour du noeud à un certain radius
    private void CountSurchargeCars()
    {
        amount = 0;
        collidersInRange = Physics.OverlapSphere(transform.position, 1.5f);

        foreach (Collider c in collidersInRange)
        {
            if (c.gameObject.tag == "car") amount++;
        }
        surchargeTrafic = amount;
        //Debug.Log("Point numéro :" + gameObject.name + " compte :" + surchargeTrafic);
    }
}
