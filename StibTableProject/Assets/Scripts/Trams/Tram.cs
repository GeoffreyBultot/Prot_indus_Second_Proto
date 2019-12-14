using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StibAPI
{
    public class Tram : MonoBehaviour
    {
        private TramManager manager;

        //pour déplacer le tram entre deux arrêts
        public TramStop nextStop;
        public TramStop lastStop;
        public float moveSpeed;
        private float maxMoveSpeed;
        private float minMoveSpeed;
        public float rotSpeed;

        //pour faire clignoter le tram à l'arrêt
        public new Renderer renderer;

        //propre au tram
        public string lineDir;
        public int id;
        public bool canMove;
        public bool justSpawned;
        public bool isWaiting;
        public bool outOfArray;
        public Material coloredMat;
        public Material whiteMat;

        private GameObject stopLight;

        void Start()
        {
            stopLight = transform.GetChild(2).gameObject;
            //récupère les composants
            manager = GameObject.Find("TramsManager").GetComponent<TramManager>();
            renderer = transform.GetChild(0).GetComponent<Renderer>();

            //initialise ce qu'il faut
            justSpawned = true;
            canMove = true;
            outOfArray = false;
            moveSpeed = 0.175f;
            maxMoveSpeed = 0.25f;
            minMoveSpeed = 0.1f;
            rotSpeed = 0.6f;

            //fait attendre le tram au début
            WaitAtStop();
        }

        void WaitAtStop()
        {
            isWaiting = true;
            //détruit les trams quand ils arrivent au bout
            if (transform.position == GameObject.Find("CongrèsE").transform.position && !outOfArray)
            {
                manager.DestroyTram(lineDir);
                outOfArray = true;
            }

            if (transform.position == GameObject.Find("CongrèsW").transform.position && !outOfArray)
            {
                manager.DestroyTram(lineDir);
                outOfArray = true;
            }
            StartCoroutine(MoveAfter20());


        }

        //méthode qui fait accélérer ou freiner le tram
        public void ChangeSpeed(string how)
        {
            if (how == "more")
            {
                if (moveSpeed < maxMoveSpeed)
                {
                    moveSpeed += 0.03f;
                }
            }
            if (how == "less")
            {
                if (moveSpeed > minMoveSpeed)
                {
                    moveSpeed -= 0.03f;
                }
            }
        }

        IEnumerator MoveAfter20()
        {
            canMove = false;
            if (!nextStop.nextStop)
            {
                gameObject.transform.position = GameObject.Find("destroyer").transform.position;
                yield return new WaitForSeconds(1f);
                Destroy(gameObject);
            }
            //time 20 secondes
            double whenAreWeDone = Time.time + 20;
            while (Time.time < whenAreWeDone)
            {
                //fait clignoter le tram toutes les 0.6s
                //renderer.enabled = !renderer.enabled;
                stopLight.SetActive(!stopLight.activeSelf);
                yield return new WaitForSeconds(0.6f);
            }
            //renderer.enabled = true;
            stopLight.SetActive(false);
            //modifie l'arrêt suivant et précédent
            if (!justSpawned && !canMove)
            {
                lastStop = nextStop;
                nextStop = nextStop.nextStop;
            }
            canMove = true;
            justSpawned = false;
            isWaiting = false;
        }


        // Update is called once per frame
        void Update()
        {
            //déplace le tram si il peut bouger
            if (canMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, nextStop.transform.position, Time.deltaTime * moveSpeed);
            }

            //rotation sur le tram
            Vector3 targetDir = nextStop.transform.position - transform.position;
            float step = rotSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
            //vérifie si le tram est arrivé à l'arrêt et lance l'attente + changement de direction
            if (transform.position == nextStop.transform.position && !isWaiting) WaitAtStop();
        }
    }
}