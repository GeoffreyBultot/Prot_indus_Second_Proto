using UnityEngine;
using System.Collections;

public class SetSunLight : MonoBehaviour
{



    public Transform stars;
    // Use this for initialization
    void Start()
    {

    }

    bool lighton = false;

    // Update is called once per frame
    void Update()
    {

        stars.transform.rotation = transform.rotation;

        if (Input.GetKeyDown(KeyCode.T))
        {

            lighton = !lighton;

        }


        if (lighton)
        {
            Color final = Color.white * Mathf.LinearToGammaSpace(5);
        }
        else
        {
            Color final = Color.white * Mathf.LinearToGammaSpace(0);
        }

        Vector3 tvec = Camera.main.transform.position;

    }
}