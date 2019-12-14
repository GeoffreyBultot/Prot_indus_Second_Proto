using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaysActivation : MonoBehaviour {

    public GameObject menuScreen;
    public GameObject meteoScreen;
    public Text debugtxt;
    // Use this for initialization
    void Start () {
        //if(!Screen.fullScreen) Screen.fullScreen = !Screen.fullScreen;
#if !UNITY_EDITOR

        Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON.
        // Check if additional displays are available and activate each.
        if (Display.displays.Length == 1)
        {
            menuScreen.SetActive(false);
            meteoScreen.SetActive(false);
        }
        if (Display.displays.Length == 2)
        {
            Display.displays[1].Activate();
            menuScreen.SetActive(false);
        }
        if (Display.displays.Length == 3)
        {
            Display.displays[1].Activate();
            Display.displays[2].Activate();

        }
#endif
    }
}
