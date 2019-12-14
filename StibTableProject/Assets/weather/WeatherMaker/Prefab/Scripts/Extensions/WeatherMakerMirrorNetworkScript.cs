using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using UnityEngine;

#if MIRROR_NETWORKING_PRESENT

using Mirror;

#endif

namespace DigitalRuby.WeatherMaker
{

#if MIRROR_NETWORKING_PRESENT

    [RequireComponent(typeof(NetworkIdentity))]

#endif

    [AddComponentMenu("Weather Maker/Extensions/Mirror Networking", 1)]
    public class WeatherMakerMirrorNetworkScript :

#if MIRROR_NETWORKING_PRESENT

        NetworkBehaviour

#else

        MonoBehaviour

#endif
        
    {
        private void Cleanup()
        {

#if MIRROR_NETWORKING_PRESENT

            // new
            if (WeatherMakerScript.Instance != null)
            {
                WeatherMakerScript.Instance.WeatherProfileChanged -= WeatherProfileChanged;
            }

#endif

        }

        private void OnEnable()
        {
        }

        private void SetupState()
        {

#if MIRROR_NETWORKING_PRESENT

            Cleanup();
            if (WeatherMakerScript.Instance != null)
            {
                WeatherMakerScript.Instance.InstanceIsServer = isServer;
                WeatherMakerScript.Instance.WeatherProfileChanged += WeatherProfileChanged;
                WeatherMakerScript.Instance.InstanceClientId = (netId == 0 ? null : netId.ToString(CultureInfo.InvariantCulture));
                if (!WeatherMakerScript.IsServer && WeatherMakerDayNightCycleManagerScript.Instance != null)
                {
                    // disable day night speed, the server will be syncing the day / night
                    WeatherMakerDayNightCycleManagerScript.Instance.Speed = WeatherMakerDayNightCycleManagerScript.Instance.NightSpeed = 0.0f;
                }
            }

#endif

        }

        private void OnDisable()
        {
            Cleanup();
        }

        private void OnDestroy()
        {
            Cleanup();
        }

        private void LateUpdate()
        {

#if MIRROR_NETWORKING_PRESENT

            if (WeatherMakerScript.IsServer)
            {
                if (WeatherMakerDayNightCycleManagerScript.Instance != null)
                {
                    WeatherMakerDayNightCycleManagerScript d = WeatherMakerDayNightCycleManagerScript.Instance;
                    RpcSetTimeOfDay(d.Year, d.Month, d.Day, d.TimeOfDay);
                }
            }

#endif

        }

#if MIRROR_NETWORKING_PRESENT

        [ClientRpc]

#endif

        private void RpcSetTimeOfDay(int year, int month, int day, float timeOfDay)
        {

#if MIRROR_NETWORKING_PRESENT

            if (!WeatherMakerScript.IsServer && WeatherMakerDayNightCycleManagerScript.Instance != null)
            {
                WeatherMakerDayNightCycleManagerScript.Instance.TimeOfDay = timeOfDay;
            }

#endif

        }

#if MIRROR_NETWORKING_PRESENT

        [ClientRpc]

#endif

        private void RpcWeatherProfileChanged(string oldName, string newName, float transitionDuration, string[] clientIds)
        {

#if MIRROR_NETWORKING_PRESENT

            if (!WeatherMakerScript.IsServer && WeatherMakerScript.Instance != null && (clientIds == null || clientIds.Contains(WeatherMakerScript.ClientId)))
            {
                WeatherMakerProfileScript oldProfile = Resources.Load<WeatherMakerProfileScript>(oldName);
                WeatherMakerProfileScript newProfile = Resources.Load<WeatherMakerProfileScript>(newName);

                // notify any listeners of the change - hold duration is -1.0 meaning the server will send another profile when it is ready (hold duration unknown to client)
                WeatherMakerScript.Instance.RaiseWeatherProfileChanged(oldProfile, newProfile, transitionDuration, -1.0f, true, null);
            }

#endif

        }

        private void WeatherProfileChanged(WeatherMakerProfileScript oldProfile, WeatherMakerProfileScript newProfile, float transitionDuration, string[] clientIds)
        {

#if MIRROR_NETWORKING_PRESENT

            // send the profile change to clients
            if (WeatherMakerScript.IsServer)
            {
                RpcWeatherProfileChanged((oldProfile == null ? null : oldProfile.name), (newProfile == null ? null : newProfile.name), transitionDuration, clientIds);
            }

#endif

        }

#if MIRROR_NETWORKING_PRESENT

        public override void OnStartServer()
        {
            base.OnStartServer();

            SetupState();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            SetupState();
        }

#endif

    }
}
