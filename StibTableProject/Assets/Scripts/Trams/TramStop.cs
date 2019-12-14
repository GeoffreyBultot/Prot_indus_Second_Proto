using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace StibAPI
{
    public class TramStop : MonoBehaviour
    {

        private static int totalNbr = 0;

        //id de l'arrêt
        [SerializeField]
        public int stationID;

        public TramStop prevStop;
        public TramStop nextStop;
        public string direction;
        [SerializeField]
        private int distToNext;

        //nom de l'arret
        public string nomArret;

        public int Nbr { get; private set; }

        public static int TotalNbr
        {
            get
            {
                return totalNbr;
            }
        }

        public int DistToStart
        {
            get
            {
                if (prevStop != null)
                {
                    return prevStop.distToNext + prevStop.DistToStart;
                }

                return 0;
            }
        }
        // ---------------------------------------------

        private void Awake()
        {
            Nbr = totalNbr;
            totalNbr++;
        }
    }
}