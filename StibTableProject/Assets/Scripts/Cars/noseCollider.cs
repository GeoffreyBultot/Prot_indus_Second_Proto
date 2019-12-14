using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class noseCollider : MonoBehaviour {

    car parentCarScript;
    private void Start()
    {
        parentCarScript = transform.parent.GetComponent<car>();
    }
    //fait attendre la voiture x secondes avant de pouvoir redémarrer
    IEnumerator RestartMoving()
    {
        yield return new WaitForSeconds(0.5f);
        parentCarScript.canMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("nom de l'objet en collisison : " + other.name);
        if (other.name == "dimension") parentCarScript.canMove = false;
        if (other.name == "Tram92(Clone)" || other.name=="Tram93(Clone)") parentCarScript.canMove = false;
    }
    private void OnTriggerStay(Collider other)
    {
        //si deux voitures à l'arret, permet à la plus proche du noeud de se déplacer
        if (other.name == "dimension" && Vector3.Distance(other.transform.position, parentCarScript.nextNode.transform.position) > Vector3.Distance(parentCarScript.transform.position, parentCarScript.nextNode.transform.position)
                && !parentCarScript.canMove
                && !other.transform.parent.GetComponent<car>().canMove)
        {
            parentCarScript.canMove = true;
        }

        //si par hasard la distance est égale
        else if (Vector3.Distance(other.transform.position, parentCarScript.nextNode.transform.position) == Vector3.Distance(parentCarScript.transform.position, parentCarScript.nextNode.transform.position))
        {
            Debug.Log("hard destroying car");
            Destroy(parentCarScript.gameObject);
        }
        else if (other.name == "dimension")
        {
            parentCarScript.canMove = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(RestartMoving());
    }
    //private void OnCollisionExit(Collision collision)
    //{
    //    StartCoroutine(RestartMoving());
    //}
}
