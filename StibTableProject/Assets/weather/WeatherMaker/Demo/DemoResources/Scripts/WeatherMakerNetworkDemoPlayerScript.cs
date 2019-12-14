using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if MIRROR_NETWORKING_PRESENT

using Mirror;

#endif

namespace DigitalRuby.WeatherMaker
{
    public class WeatherMakerNetworkDemoPlayerScript

#if MIRROR_NETWORKING_PRESENT

        : NetworkBehaviour

#else

        : MonoBehaviour

#endif

    {

#pragma warning disable

        [SerializeField]
        [Tooltip("Objects that should only activate for local player")]
        private Behaviour[] LocalOnlyObjects;

        private void OnEnable()
        {

#if MIRROR_NETWORKING_PRESENT

            if (WeatherMakerScript.Instance == null)
            {
                return;
            }

            Camera cam = GetComponentInChildren<Camera>();
            foreach (Camera exCam in WeatherMakerScript.Instance.AllowCameras)
            {
                if (exCam == cam)
                {
                    return;
                }
            }
            WeatherMakerScript.Instance.AllowCameras.Add(cam);

#endif

        }

#pragma warning restore

        private void Update()
        {

#if MIRROR_NETWORKING_PRESENT

            // cleanup networked players of cameras, audio listener, etc.
            if (isLocalPlayer)
            {
                if (LocalOnlyObjects != null)
                {
                    foreach (Behaviour obj in LocalOnlyObjects)
                    {
                        obj.enabled = true;
                    }
                }
            }
            else if (LocalOnlyObjects != null)
            {
                foreach (Behaviour obj in LocalOnlyObjects)
                {
                    obj.enabled = false;
                }
            }

#endif

        }
    }
}
