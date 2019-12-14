using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightManager : MonoBehaviour {
    
    [SerializeField]
    private Transform GC;
    public Transform Presse;
    public Transform Madou;
    public Transform Parc;
    public Transform Congres;
    public Transform Villo;
    public ScootyManager scootyManager;
    public Light MaquetteLight;
    public Light MaquetteHighLight;
    //private Color spotColor = new Color(58f/255f,91f/522f,248f/255f,1);
    private Color spotColor = new Color(1,1,1,1);
    public NavMeshAgentManager agent;

    private GameObject lightGameObject;
    private IEnumerator CloseLightCoroutine;

    [SerializeField]
    private float lowLightValue, normalLightValue;

    public void HighlightAtPosition(string obj)
    {

        Transform objToHighlight;
        switch (obj)
        {
            case "gc":
                objToHighlight = GC;
                agent.SpawnAgent(Congres, GC);
                break;
            case "villo":
                objToHighlight = Villo;
                agent.SpawnAgent(Congres, Villo);
                break;
            case "scooty":
                //récupère le scooty le plus proche de Congres qui est devant l'école
                Transform closestScooty = scootyManager.GetClosestScooty(Congres);
                //stop tout si pas de scooty trouvé
                if (closestScooty == null)
                {
                    StopHighlighting();
                    return;
                }
                objToHighlight = closestScooty;
                agent.SpawnAgent(Congres, closestScooty);
                break;
            default:
                objToHighlight = null;
                break;
        }

        MaquetteLight.gameObject.SetActive(false);
        MaquetteHighLight.gameObject.SetActive(true);
        if (lightGameObject == null) CreateLight();
        lightGameObject.transform.position = objToHighlight.position;

        //coroutine pour allumer la lumière x secondes
        lightGameObject.SetActive(true);
        if (CloseLightCoroutine != null) StopCoroutine(CloseLightCoroutine);
        CloseLightCoroutine = CloseLightAfter30s();
        StartCoroutine(CloseLightCoroutine);
    }

    public void HighlightAtPosition(string obj, string line)
    {
        MaquetteLight.gameObject.SetActive(false);
        MaquetteHighLight.gameObject.SetActive(true);
        if (lightGameObject == null) CreateLight();


        Transform objToHighlight = null;
        if(obj == "stib")
        {
            switch (line)
            {
                case "1":
                    objToHighlight = Parc;
                    agent.SpawnAgent(Congres, Parc);
                    break;
                case "2":
                    objToHighlight = Madou;
                    agent.SpawnAgent(Congres, Madou);
                    break;
                case "5":
                    objToHighlight = Parc;
                    agent.SpawnAgent(Congres, Parc);
                    break;
                case "6":
                    objToHighlight = Madou;
                    agent.SpawnAgent(Congres, Madou);
                    break;
                case "29":
                    objToHighlight = Presse;
                    agent.SpawnAgent(Congres, Presse);
                    break;
                case "63":
                    objToHighlight = Presse;
                    agent.SpawnAgent(Congres, Presse);
                    break;
                case "65":
                    objToHighlight = Presse;
                    agent.SpawnAgent(Congres, Presse);
                    break;
                case "66":
                    objToHighlight = Presse;
                    agent.SpawnAgent(Congres, Presse);
                    break;
                case "92":
                    objToHighlight = Congres;
                    agent.SpawnAgent(Congres, Congres);
                    break;
                case "93":
                    objToHighlight = Congres;
                    agent.SpawnAgent(Congres, Congres);
                    break;
                default:
                    objToHighlight = null;
                    break;
            }
        }
        lightGameObject.transform.position = objToHighlight.position;

        //coroutine pour allumer la lumière x secondes
        lightGameObject.SetActive(true);
        if (CloseLightCoroutine != null) StopCoroutine(CloseLightCoroutine);
        CloseLightCoroutine = CloseLightAfter30s();
        StartCoroutine(CloseLightCoroutine);
    }

    public void StopHighlighting()
    {
        if (CloseLightCoroutine != null) StopCoroutine(CloseLightCoroutine);
        ClosingHighlightMode();
    }

    private void CreateLight()
    {
        //instantiate light at position determined
        lightGameObject = new GameObject("spotLight");

        // Add the light component
        Light lightComp = lightGameObject.AddComponent<Light>();

        // Set color and position
        lightComp.color = spotColor;
        lightComp.type = LightType.Point;
        lightComp.range = 2.2f;
        lightComp.intensity = 50;
        // Set the position (or any transform property)
        lightGameObject.transform.rotation = new Quaternion(-90, 0, 0, 1);
    }

    IEnumerator CloseLightAfter30s()
    {
        yield return new WaitForSeconds(30);
        ClosingHighlightMode();
    }

    private void ClosingHighlightMode()
    {
        if(lightGameObject != null) lightGameObject.SetActive(false);
        MaquetteHighLight.gameObject.SetActive(false);
        MaquetteLight.gameObject.SetActive(true);
        agent.DestroyAgent();
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
