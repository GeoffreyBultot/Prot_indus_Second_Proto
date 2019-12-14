using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StibAPI
{
    public class TramManager : MonoBehaviour
    {
        public static TramManager Instance;
        public GameObject prefabTram92;
        public GameObject prefabTram93;
        [SerializeField]
        private TramStop[] tramStops;
        [SerializeField]
        private GameObject tramParent;
        [SerializeField]
        private List<Tram> Trams92DirGillon;
        [SerializeField]
        private List<Tram> Trams92DirSablon;
        [SerializeField]
        private List<Tram> Trams93DirGillon;
        [SerializeField]
        private List<Tram> Trams93DirSablon;
        [SerializeField]
        private bool spawned92s;
        [SerializeField]
        private bool spawned93s;
        [SerializeField]
        private bool spawned92g;
        [SerializeField]
        private bool spawned93g;
        private Rootobject datacopy;
        private IEnumerator justSpawnedCoroutine;

        //sert à éviter que quand un tram apparait, un autre de la meme ligne apparaisse avec 1m30 plus tard. Comme ça, évite les trams identiques qui se suivent quand ils avancent pas.
        private IEnumerator WaitBeforAllowingSpawn(string line)
        {
            yield return new WaitForSeconds(90f);
            switch (line)
            {
                case "92g":
                    spawned92g = false;
                    break;
                case "93g":
                    spawned93g = false;
                    break;
                case "92s":
                    spawned92s = false;
                    break;
                case "93s":
                    spawned93s = false;
                    break;
                default:
                    break;
            }
        }

        private void Start()
        {
            Trams92DirGillon = new List<Tram>();
            Trams92DirSablon = new List<Tram>();
            Trams93DirGillon = new List<Tram>();
            Trams93DirSablon = new List<Tram>();
            spawned92g = false;
            spawned92s = false;
            spawned93g = false;
            spawned93s = false;
            if (Application.internetReachability != NetworkReachability.NotReachable) StartCoroutine(SpawnAfter3());
            else Debug.Log("no internet connection for STIB");
        }

        //spawns the trams after 3s at start
        IEnumerator SpawnAfter3()
        {
            yield return new WaitForSeconds(5);
            GenerateTramsAtStart(datacopy);
        }

        //generate trams at start
        public void GenerateTramsAtStart(Rootobject data)
        {
            foreach (Point p in data.points)
            {
                if (p.pointId == 6357) // XXXXX
                {
                    foreach (Passingtime pt in p.passingTimes)
                    {
                        if (pt.lineId == 92)
                        {
                            TramStop ts = null;

                            switch (MinuteTransform(pt))
                            {
                                case 1:
                                    ts = null;
                                    foreach (TramStop t in tramStops)
                                    {
                                        if (t.gameObject.name == "ParcE") ts = t;
                                    }

                                    SpawnTram(92, Trams92DirGillon, ts);
                                    break;
                                case 2:
                                    ts = null;
                                    foreach (TramStop t in tramStops)
                                    {
                                        if (t.gameObject.name == "PalaisE") ts = t;
                                    }

                                    SpawnTram(92, Trams92DirGillon, ts);
                                    break;
                                case 3:
                                    ts = null;
                                    foreach (TramStop t in tramStops)
                                    {
                                        if (t.gameObject.name == "RoyaleE") ts = t;
                                    }

                                    SpawnTram(92, Trams92DirGillon, ts);
                                    break;
                                //case 4:
                                //    ts = null;
                                //    foreach (TramStop t in tramStops)
                                //    {
                                //        if (t.gameObject.name == "PetitSablonE") ts = t;
                                //    }

                                //    SpawnTram(92, Trams92DirGillon, ts);
                                //    break;
                                default:
                                    break;
                            }
                        }
                        if (pt.lineId == 93)
                        {
                            TramStop ts = null;

                            switch (MinuteTransform(pt))
                            {
                                case 1:
                                    ts = null;
                                    foreach (TramStop t in tramStops)
                                    {
                                        if (t.gameObject.name == "ParcE") ts = t;
                                    }

                                    SpawnTram(93, Trams93DirGillon, ts);
                                    break;
                                case 2:
                                    ts = null;
                                    foreach (TramStop t in tramStops)
                                    {
                                        if (t.gameObject.name == "PalaisE") ts = t;
                                    }

                                    SpawnTram(93, Trams93DirGillon, ts);
                                    break;
                                case 3:
                                    ts = null;
                                    foreach (TramStop t in tramStops)
                                    {
                                        if (t.gameObject.name == "RoyaleE") ts = t;
                                    }

                                    SpawnTram(93, Trams93DirGillon, ts);
                                    break;
                                //case 4:
                                //    ts = null;
                                //    foreach (TramStop t in tramStops)
                                //    {
                                //        if (t.gameObject.name == "PetitSablonE") ts = t;
                                //    }

                                //    SpawnTram(93, Trams93DirGillon, ts);
                                //    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                else if (p.pointId == 6308) // XXXXX
                {
                    foreach (Passingtime pt in p.passingTimes)
                    {
                        if (pt.lineId == 92)
                        {
                            TramStop ts = null;

                            switch (MinuteTransform(pt))
                            {
                                case 1:
                                    ts = null;
                                    foreach (TramStop t in tramStops)
                                    {
                                        if (t.gameObject.name == "BotaW") ts = t;
                                    }

                                    SpawnTram(92, Trams92DirSablon, ts);
                                    break;
                                //case 2:
                                //    ts = null;
                                //    foreach (TramStop t in tramStops)
                                //    {
                                //        if (t.gameObject.name == "GillonW") ts = t;
                                //    }

                                //    SpawnTram(92, Trams92DirSablon, ts);
                                //    break;
                                default:
                                    break;
                            }
                        }
                        if (pt.lineId == 93)
                        {
                            TramStop ts = null;

                            switch (MinuteTransform(pt))
                            {
                                case 1:
                                    ts = null;
                                    foreach (TramStop t in tramStops)
                                    {
                                        if (t.gameObject.name == "BotaW") ts = t;
                                    }

                                    SpawnTram(93, Trams93DirSablon, ts);
                                    break;
                                //case 2:
                                //    ts = null;
                                //    foreach (TramStop t in tramStops)
                                //    {
                                //        if (t.gameObject.name == "GillonW") ts = t;
                                //    }

                                //    SpawnTram(93, Trams93DirSablon, ts);
                                //    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError("Line not registered!");
                }
            }
        }

        //destroy trams after congrès
        public void DestroyTram(string _linedir)
        {
            switch (_linedir)
            {
                case "92dirSablon":
                    if(Trams92DirSablon.Count>0) Trams92DirSablon.RemoveAt(0);
                    break;
                case "93dirSablon":
                    if (Trams93DirSablon.Count > 0) Trams93DirSablon.RemoveAt(0);
                    break;
                case "92dirGillon":
                    if (Trams92DirGillon.Count > 0) Trams92DirGillon.RemoveAt(0);
                    break;
                case "93dirGillon":
                    if (Trams93DirGillon.Count > 0) Trams93DirGillon.RemoveAt(0);
                    break;
                default:
                    break;
            }
        }

        //update data to spawn trams and change speed of close to Congrès
        public void UpdateData(Rootobject data)
        {
            datacopy = data;

            //variables utilisées pour vérifier qu'il n'y a pas eu 2 trams à moins de 5 / 2 min dans la liste mais un seul spawn
            int count92g = 0, count93g = 0, count92s = 0, count93s = 0;
            //récupère dans cb de minutes le premier tram de chaque ligne et direction va passer pour voir si il faut accélérer ou pas.
            int first92gTime = 99, first93gTime = 99, first92sTime = 99, first93sTime = 99;

            foreach (Point p in data.points)
            {
                if (p.pointId == 6357) // XXXXX
                {
                    foreach (Passingtime pt in p.passingTimes)
                    {
                        if (pt.lineId == 92)
                        {
                            int timeInMin = MinuteTransform(pt);
                            //gestion spawn si manquant
                            if (timeInMin < 5) count92g++;
                            //gestion spawn basique
                            if (timeInMin == 5 && !spawned92g)
                            {
                                SpawnTram(92, Trams92DirGillon, GameObject.Find("PetitSablonE").GetComponent<TramStop>());
                                spawned92g = true;
                                justSpawnedCoroutine = WaitBeforAllowingSpawn("92g");
                                StartCoroutine(justSpawnedCoroutine);
                            }
                            //gestion vitesse du premier tram de la liste
                            if (first92gTime == 99) first92gTime = timeInMin;
                            //Debug.Log("92 dir gillon : " + MinuteTransform(pt));
                        }
                        if (pt.lineId == 93)
                        {
                            int timeInMin = MinuteTransform(pt);
                            if (timeInMin < 5) count93g++;
                            if (timeInMin == 5 && !spawned93g)
                            {
                                SpawnTram(93, Trams93DirGillon, GameObject.Find("PetitSablonE").GetComponent<TramStop>());
                                spawned93g = true;
                                justSpawnedCoroutine = WaitBeforAllowingSpawn("93g");
                                StartCoroutine(justSpawnedCoroutine);
                            }
                            if (first93gTime == 99) first93gTime = timeInMin;
                            //Debug.Log("93 dir gillon : " + MinuteTransform(pt));
                        }
                    }
                }
                else if (p.pointId == 6308) // XXXXX
                {
                    foreach (Passingtime pt in p.passingTimes)
                    {
                        if (pt.lineId == 92)
                        {
                            int timeInMin = MinuteTransform(pt);
                            if (timeInMin < 2) count92s++;
                            if (timeInMin == 2 && !spawned92s)
                            {
                                SpawnTram(92, Trams92DirSablon, GameObject.Find("GillonW").GetComponent<TramStop>());
                                spawned92s = true;
                                justSpawnedCoroutine = WaitBeforAllowingSpawn("92s");
                                StartCoroutine(justSpawnedCoroutine);
                            }
                            if (first92sTime == 99) first92sTime = timeInMin;
                           // Debug.Log("92 dir sablon : " + MinuteTransform(pt));
                        }
                        if (pt.lineId == 93)
                        {
                            int timeInMin = MinuteTransform(pt);
                            if (timeInMin < 2) count93s++;
                            if (timeInMin == 2 && !spawned93s)
                            {
                                SpawnTram(93, Trams93DirSablon, GameObject.Find("GillonW").GetComponent<TramStop>());
                                spawned93s = true;
                                justSpawnedCoroutine = WaitBeforAllowingSpawn("93s");
                                StartCoroutine(justSpawnedCoroutine);
                            }
                            if (first93sTime == 99) first93sTime = timeInMin;
                            //Debug.Log("93 dir sablon : " + MinuteTransform(pt));
                        }
                    }
                }
                else
                {
                    Debug.LogError("Line not registered!");
                }
            }

            //gestion de spawn d'un tram skippé par saut de minute
            if (count92g == 2 && Trams92DirGillon.Count < 2)
            {
                SpawnTram(92, Trams92DirGillon, GameObject.Find("PetitSablonE").GetComponent<TramStop>());
            }
            if (count93g == 2 && Trams93DirGillon.Count < 2)
            {
                SpawnTram(93, Trams93DirGillon, GameObject.Find("PetitSablonE").GetComponent<TramStop>());
            }
            if (count92s == 2 && Trams92DirSablon.Count < 2)
            {
                SpawnTram(92, Trams92DirSablon, GameObject.Find("GillonW").GetComponent<TramStop>());
            }
            if (count93s == 2 && Trams93DirSablon.Count < 2)
            {
                SpawnTram(93, Trams93DirSablon, GameObject.Find("GillonW").GetComponent<TramStop>());
            }

            //gestion de la vitesse des trams, peut la gérer que quand nouvelles données sinon inutile...
            //rapport : distance de 6 +-= à 1 minute
            if (Trams92DirGillon.Count > 0)
            {
                switch (first92gTime)
                {
                    case 1:
                        // si il arrive dans une minute et que la distance est plus grande que 6, accélère
                        if (DistTramToCongres(Trams92DirGillon[0]) > 6)
                        {
                            Trams92DirGillon[0].ChangeSpeed("more");
                        }
                        //sinon freine
                        else Trams92DirGillon[0].ChangeSpeed("less");
                        break;
                    case 2:
                        if (DistTramToCongres(Trams92DirGillon[0]) > 12)
                        {
                            Trams92DirGillon[0].ChangeSpeed("more");
                        }
                        else Trams92DirGillon[0].ChangeSpeed("less");
                        break;
                    case 3:
                        if (DistTramToCongres(Trams92DirGillon[0]) > 18)
                        {
                            Trams92DirGillon[0].ChangeSpeed("more");
                        }
                        else Trams92DirGillon[0].ChangeSpeed("less");
                        break;
                    case 4:
                        if (DistTramToCongres(Trams92DirGillon[0]) > 24)
                        {
                            Trams92DirGillon[0].ChangeSpeed("more");
                        }
                        else Trams92DirGillon[0].ChangeSpeed("less");
                        break;
                    case 5:
                        if (DistTramToCongres(Trams92DirGillon[0]) > 30)
                        {
                            Trams92DirGillon[0].ChangeSpeed("more");
                        }
                        else Trams92DirGillon[0].ChangeSpeed("less");
                        break;
                    default:
                        break;
                }
            }
            if (Trams93DirGillon.Count > 0)
            {
                switch (first93gTime)
                {
                    case 1:
                        // si il arrive dans une minute et que la distance est plus grande que 6, accélère
                        if (DistTramToCongres(Trams93DirGillon[0]) > 6)
                        {
                            Trams93DirGillon[0].ChangeSpeed("more");
                        }
                        //sinon freine
                        else Trams93DirGillon[0].ChangeSpeed("less");
                        break;
                    case 2:
                        if (DistTramToCongres(Trams93DirGillon[0]) > 12)
                        {
                            Trams93DirGillon[0].ChangeSpeed("more");
                        }
                        else Trams93DirGillon[0].ChangeSpeed("less");
                        break;
                    case 3:
                        if (DistTramToCongres(Trams93DirGillon[0]) > 18)
                        {
                            Trams93DirGillon[0].ChangeSpeed("more");
                        }
                        else Trams93DirGillon[0].ChangeSpeed("less");
                        break;
                    case 4:
                        if (DistTramToCongres(Trams93DirGillon[0]) > 24)
                        {
                            Trams93DirGillon[0].ChangeSpeed("more");
                        }
                        else Trams93DirGillon[0].ChangeSpeed("less");
                        break;
                    case 5:
                        if (DistTramToCongres(Trams93DirGillon[0]) > 30)
                        {
                            Trams93DirGillon[0].ChangeSpeed("more");
                        }
                        else Trams93DirGillon[0].ChangeSpeed("less");
                        break;
                    default:
                        break;
                }
            }
            if (Trams92DirSablon.Count > 0)
            {
                switch (first92sTime)
                {
                    case 1:
                        // si il arrive dans une minute et que la distance est plus grande que 6, accélère
                        if (DistTramToCongres(Trams92DirSablon[0]) > 6)
                        {
                            Trams92DirSablon[0].ChangeSpeed("more");
                        }
                        //sinon freine
                        else Trams92DirSablon[0].ChangeSpeed("less");
                        break;
                    case 2:
                        if (DistTramToCongres(Trams92DirSablon[0]) > 12)
                        {
                            Trams92DirSablon[0].ChangeSpeed("more");
                        }
                        else Trams92DirSablon[0].ChangeSpeed("less");
                        break;
                    default:
                        break;
                }
            }
            if (Trams93DirSablon.Count > 0)
            {
                switch (first93sTime)
                {
                    case 1:
                        // si il arrive dans une minute et que la distance est plus grande que 6, accélère
                        if (DistTramToCongres(Trams93DirSablon[0]) > 6)
                        {
                            Trams93DirSablon[0].ChangeSpeed("more");
                        }
                        //sinon freine
                        else Trams93DirSablon[0].ChangeSpeed("less");
                        break;
                    case 2:
                        if (DistTramToCongres(Trams93DirSablon[0]) > 12)
                        {
                            Trams93DirSablon[0].ChangeSpeed("more");
                        }
                        else Trams93DirSablon[0].ChangeSpeed("less");
                        break;
                    default:
                        break;
                }
            }

        }

        float DistTramToCongres(Tram tram)
        {
            return Vector3.Distance(GameObject.Find("CongrèsW").transform.position, tram.transform.position);
        }

        //function to spawn a tram
        private void SpawnTram(int lineId, List<Tram> tramList, TramStop ts)
        {
            GameObject tram;
            // instantie un tram et le place dans le bon parent en fonction de la ligne (92/93) qui instancie un tram de couleur différente
            if (lineId == 92)
            {
                tram = Instantiate(prefabTram92, tramParent.transform) as GameObject;
                tram.transform.position = ts.transform.position;
                tram.GetComponent<Tram>().lastStop = ts;
                tram.GetComponent<Tram>().nextStop = ts.nextStop;
                if (ts.direction == "Sablon") tram.GetComponent<Tram>().lineDir = "92dirSablon";
                if (ts.direction == "Gillon") tram.GetComponent<Tram>().lineDir = "92dirGillon";
            }
            else if (lineId == 93)
            {
                tram = Instantiate(prefabTram93, tramParent.transform) as GameObject;
                tram.transform.position = ts.transform.position;
                tram.GetComponent<Tram>().lastStop = ts;
                tram.GetComponent<Tram>().nextStop = ts.nextStop;
                if (ts.direction == "Sablon") tram.GetComponent<Tram>().lineDir = "93dirSablon";
                if (ts.direction == "Gillon") tram.GetComponent<Tram>().lineDir = "93dirGillon";
            }
            else
            {
                Debug.Log("ERROR : line " + lineId + " does not exist. Defaulted to line 93.");
                tram = Instantiate(prefabTram93, tramParent.transform) as GameObject;
            }

            //ajoute le tram à la liste
            tramList.Add(tram.GetComponent<Tram>());
        }

        //retourne un int équivalent au nombre de minutes restant avant l'arrivée du tram
        private int MinuteTransform(Passingtime receivedDate)
        {

            int result = 0;
            //récupération de l'heure actuelle
            System.DateTime now = DateTime.Now;
            int hour = now.Hour;
            int min = now.Minute;
            //addition des minutes et des heures converties en minutes
            int actualtotalMinut = hour * 60 + min;
            //traitement du string reçu avec l'heure d'arrivée pour récupérer l'heure et les minutes
            string substringedDate = receivedDate.expectedArrivalTime.Substring(receivedDate.expectedArrivalTime.Length - 8);
            int receivedHour = int.Parse(substringedDate.Substring(0, 2));
            string receivedMinut = substringedDate.Substring(0, 5);
            receivedMinut = receivedMinut.Substring(receivedMinut.Length - 2);
            int receivedTotalMinut = receivedHour * 60 + int.Parse(receivedMinut);
            //soustrait l'heure d'arriver en minutes avec l'heure actuelle en minutes
            result = receivedTotalMinut - actualtotalMinut;

            return result;
        }
    }
}