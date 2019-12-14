using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StibAPI
{
    public class VisualStuff : MonoBehaviour
    {
        //[SerializeField]
        //private GameObject sunObj;

        [SerializeField]
        private Material waterMat;

        public Light mainLight;
        public Transform CarsParent;
        private float speedWater1 = 0.1f;
        private float speedWater2 = 0.2f;

        public bool dayMode;

        //private float angleZ;

        private void Start()
        {
            dayMode = true;
        }

        private void SwitchDayMode()
        {
            if (dayMode)
            {
                mainLight.intensity = 0f;
                dayMode = false;
                foreach(Transform car in CarsParent)
                {
                    car.GetComponent<car>().SwitchLightsIntensity();
                }
            }
            else
            {
                mainLight.intensity = 0.85f;
                dayMode = true;
                foreach (Transform car in CarsParent)
                {
                    car.GetComponent<car>().SwitchLightsIntensity();
                }
            }
        }

        private Vector2 waterSpeed1
        {
            get
            {
                return new Vector2(0, speedWater1);
            }
        }

        private Vector2 waterSpeed2
        {
            get
            {
                return new Vector2(speedWater2, 0);
            }
        }

        private void Update()
        {
            waterMat.mainTextureOffset += Time.deltaTime * waterSpeed1;
            waterMat.mainTextureOffset += Time.deltaTime * waterSpeed2;

            //angleZ = (float)(-15f * System.DateTime.Now.TimeOfDay.TotalHours + 210f);

            if (Input.GetKeyDown(KeyCode.D))
            {
                SwitchDayMode();
            }

            //sunObj.transform.rotation = Quaternion.Euler(0, 0, angleZ);

            //Resources.UnloadUnusedAssets();
        }

        private void OnDisable()
        {
            waterMat.mainTextureOffset = Vector2.zero;
        }
        private void OnApplicationQuit()
        {
            waterMat.mainTextureOffset = Vector2.zero;
        }
    }

}