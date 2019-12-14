using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StibAPI
{
    public class TramNoseCollider : MonoBehaviour
    {

        private Tram parentTramScript;

        // Use this for initialization
        void Start()
        {
            parentTramScript = transform.parent.GetComponent<Tram>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Tram") parentTramScript.canMove = false;
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.name == "front" && (other.transform.parent.gameObject.GetComponent<Tram>().lineDir == "92dirGillon" || other.transform.parent.gameObject.GetComponent<Tram>().lineDir == "92dirSablon")) parentTramScript.canMove = false;
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Tram") parentTramScript.canMove = true;
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}