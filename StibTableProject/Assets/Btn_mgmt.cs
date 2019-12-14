using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Btn_mgmt : MonoBehaviour
{
    Physical_Buttons_Management phys_buttons;
    GaresManager gareManager;
    iRailDisplay railManager;
    public GameObject meteoCamera;
    private IEnumerator unloadCoroutine;
    public HighLightManager hlManager;
    
    public delegate void OnStibLineClicked(int line);
    public static event OnStibLineClicked StibLineClickedDelegate;

    // Use this for initialization
    void Start()
    {
        gareManager = GameObject.Find("GaresManager").GetComponent<GaresManager>();
        hlManager = GameObject.Find("HighLightManager").GetComponent<HighLightManager>();
        phys_buttons = GameObject.Find("CameraFilmingScene/Canvas").GetComponent<Physical_Buttons_Management>();
        meteoCamera = GameObject.Find("2DWeather");
    }

    IEnumerator UnloadAfter30s()
    {
        yield return new WaitForSeconds(30);
        Debug.Log("Auto Unload");
        meteoCamera.SetActive(true);
        UnloadGare();
        UnloadStib();
        UnloadVillo();
        phys_buttons.GoToMainMode();
    }

    public void TraficButton()
    {
        hlManager.StopHighlighting();
        Debug.Log("trafic clicked");
        if (!isScene_CurrentlyLoaded("Trafic"))
        {
            UnloadStib();
            UnloadGare();
            UnloadVillo();
            SceneManager.LoadScene("Trafic", LoadSceneMode.Additive);
        }
        meteoCamera.SetActive(false);
        if (unloadCoroutine != null) StopCoroutine(unloadCoroutine);
        unloadCoroutine = UnloadAfter30s();
        StartCoroutine(unloadCoroutine);
    }

    public void VilloButton()
    {
        hlManager.StopHighlighting();
        Debug.Log("villo clicked");
        if (!isScene_CurrentlyLoaded("Villo"))
        {
            UnloadStib();
            UnloadGare();
            UnloadTrafic();
            SceneManager.LoadScene("Villo", LoadSceneMode.Additive);
        }
        meteoCamera.SetActive(false);
        if (unloadCoroutine != null) StopCoroutine(unloadCoroutine);
        unloadCoroutine = UnloadAfter30s();
        StartCoroutine(unloadCoroutine);
        hlManager.HighlightAtPosition("villo");
    }

    public void MeteoButton()
    {
        hlManager.StopHighlighting();
        Debug.Log("meteo clicked");
        meteoCamera.SetActive(true);
        UnloadGare();
        UnloadStib();
        UnloadTrafic();
        UnloadVillo();
        if (unloadCoroutine != null) StopCoroutine(unloadCoroutine);
        hlManager.StopHighlighting();

    }
    public void OnGDNButtonClick()
    {
        hlManager.StopHighlighting();
        Debug.Log("gare du nord clicked");
        gareManager.LoadedGare = "GDN";
        if (!isScene_CurrentlyLoaded("SNCB"))
        {
            UnloadStib();
            UnloadTrafic();
            UnloadVillo();
            SceneManager.LoadScene("SNCB", LoadSceneMode.Additive);
        }
        else
        {
            railManager = GameObject.Find("CameraFilmingGare").GetComponent<iRailDisplay>();
            railManager.LoadGare();
        }
        meteoCamera.SetActive(false);
        if (unloadCoroutine != null) StopCoroutine(unloadCoroutine);
        unloadCoroutine = UnloadAfter30s();
        StartCoroutine(unloadCoroutine);
    }
    public void OnGDMButtonClick()
    {
        hlManager.StopHighlighting();
        Debug.Log("gare du midi clicked");
        gareManager.LoadedGare = "GDM";
        if (!isScene_CurrentlyLoaded("SNCB"))
        {
            UnloadStib();
            UnloadTrafic();
            UnloadVillo();
            SceneManager.LoadScene("SNCB", LoadSceneMode.Additive);
        }
        else
        {
            railManager = GameObject.Find("CameraFilmingGare").GetComponent<iRailDisplay>();
            railManager.LoadGare();
        }
        meteoCamera.SetActive(false);
        if (unloadCoroutine != null) StopCoroutine(unloadCoroutine);
        unloadCoroutine = UnloadAfter30s();
        StartCoroutine(unloadCoroutine);

    }
    public void OnGDLButtonClick()
    {
        hlManager.StopHighlighting();
        Debug.Log("gare du luxembourg clicked");
        gareManager.LoadedGare = "GDL";
        if (!isScene_CurrentlyLoaded("SNCB"))
        {
            UnloadStib();
            UnloadTrafic();
            UnloadVillo();
            SceneManager.LoadScene("SNCB", LoadSceneMode.Additive);
        }
        else
        {
            railManager = GameObject.Find("CameraFilmingGare").GetComponent<iRailDisplay>();
            railManager.LoadGare();
        }
        meteoCamera.SetActive(false);
        if (unloadCoroutine != null) StopCoroutine(unloadCoroutine);
        unloadCoroutine = UnloadAfter30s();
        StartCoroutine(unloadCoroutine);

    }

    public void Show_GC()
    {
        Debug.Log("gare centrale clicked");
        gareManager.LoadedGare = "GC";
        if (!isScene_CurrentlyLoaded("SNCB"))
        {
            UnloadTrafic();
            UnloadStib();
            UnloadVillo();
            SceneManager.LoadScene("SNCB", LoadSceneMode.Additive);
        }
        else
        {
            railManager = GameObject.Find("CameraFilmingGare").GetComponent<iRailDisplay>();
            railManager.LoadGare();
        }
        meteoCamera.SetActive(false);
        if (unloadCoroutine != null) StopCoroutine(unloadCoroutine);
        unloadCoroutine = UnloadAfter30s();
        StartCoroutine(unloadCoroutine);
        hlManager.HighlightAtPosition("gc");
    }

    public void Stib_Button()
    {
        hlManager.StopHighlighting();
        Debug.Log("stib clicked");
        if (!isScene_CurrentlyLoaded("STIB"))
        {
            UnloadGare();
            UnloadTrafic();
            UnloadVillo();
            SceneManager.LoadScene("STIB", LoadSceneMode.Additive);
        }
        meteoCamera.SetActive(false);
        if (unloadCoroutine != null) StopCoroutine(unloadCoroutine);
        unloadCoroutine = UnloadAfter30s();
        StartCoroutine(unloadCoroutine);
    }
    public void ScootyButton()
    {
        Debug.Log("scooty clicked");
        //if (!isScene_CurrentlyLoaded("STIB"))
        //{
        //    UnloadGare();
        //    SceneManager.LoadScene("STIB", LoadSceneMode.Additive);
        //}
        //meteoCamera.SetActive(false);
        meteoCamera.SetActive(true);
        UnloadGare();
        UnloadStib();
        UnloadTrafic();
        UnloadVillo();
        if (unloadCoroutine != null) StopCoroutine(unloadCoroutine);
        unloadCoroutine = UnloadAfter30s();
        StartCoroutine(unloadCoroutine);
        hlManager.HighlightAtPosition("scooty");

    }

    public void StibLineButton(string line)
    {
        if (unloadCoroutine != null) StopCoroutine(unloadCoroutine);
        unloadCoroutine = UnloadAfter30s();
        StartCoroutine(unloadCoroutine);
        hlManager.HighlightAtPosition("stib", line);
        //event qui highlight les bonnes lignes sur la télé
        //StibLineClickedDelegate(int.Parse(line));
    }
    public void OnReloadButtonClick()
    {
        System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe")); //new program
        Application.Quit(); //kill current process
    }

    bool isScene_CurrentlyLoaded(string sceneName_no_extention)
    {
        for (int i = 0; i < SceneManager.sceneCount; ++i)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == sceneName_no_extention)
            {
                //the scene is already loaded
                return true;
            }
        }

        return false;//scene not currently loaded in the hierarchy
    }

    private void UnloadGare()
    {
        gareManager.LoadedGare = "none";
        if (isScene_CurrentlyLoaded("SNCB")) SceneManager.UnloadSceneAsync("SNCB");
    }
    private void UnloadStib()
    {
        if (isScene_CurrentlyLoaded("STIB")) SceneManager.UnloadSceneAsync("STIB");
    }
    private void UnloadTrafic()
    {
        if (isScene_CurrentlyLoaded("Trafic")) SceneManager.UnloadSceneAsync("Trafic");
    }
    private void UnloadVillo()
    {
        if (isScene_CurrentlyLoaded("Villo")) SceneManager.UnloadSceneAsync("Villo");
    }
}