using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using UnityEngine.Networking;
using System;

public class RSSInfoRoute : MonoBehaviour
{
    public Text _textBand;

    XmlDocument fichierRSS;

    private string[] evenements;
    bool bruxelles = false; //variable indiquant s'il y a des evenements disponibles sur Bruxelles


    private string url;
    private Movement mouv;

    public int EspaceInterEvent = 10;

    private void Start()
    {
        mouv = transform.GetComponent<Movement>();
        mouv.finDeDefilement += ReloadInfoRoute;
        ReloadInfoRoute();
    }

    public void ReloadInfoRoute()
    {

        string text = "http://www.wegeninfo.be/rssfr.php";

        fichierRSS = new XmlDocument();
        fichierRSS.Load(text);
        XmlNode mainNode = fichierRSS.ChildNodes[1].ChildNodes[0];

        int arrayLength = 0;
        int j = 0; //variable d'index pour se deplacer dans le tableau

        bruxelles = false;
        foreach (XmlNode node in mainNode)
        {


            if (node.Name == "item")
            {
                foreach (XmlNode child in node)
                {
                    if (child.Name == "title")
                    {


                        if (child.ChildNodes[0].InnerText.Contains("Bxl"))
                        {
                            arrayLength++;
                            bruxelles = true;
                        }
                    }
                }
            }
        }
        if (bruxelles == true)
        {


            // S'il y a des evenement disponibles pour Bruxelles,on instantie le tableau grace au comptage fait precedament
            evenements = new string[arrayLength];
            foreach (XmlNode node in mainNode)
            {


                if (node.Name == "item")
                {



                    foreach (XmlNode child in node)
                    {
                        if (child.Name == "title")
                        {


                            if (child.ChildNodes[0].InnerText.Contains("Bxl"))
                            {
                                if (child.ChildNodes[0].InnerText.Contains("INCIDENT"))
                                {
                                    evenements[j] = PimpMyString(child.ChildNodes[0].InnerText, true);
                                }
                                else if (child.ChildNodes[0].InnerText.Contains("TRAVAUX"))
                                {
                                    evenements[j] = PimpMyString(child.ChildNodes[0].InnerText, false);
                                }
                                else
                                {
                                    Debug.Log("≠");
                                }

                                j++;

                            }
                        }

                    }

                }

            }
        }
        // Si'il n'y a pas d'evenements disponibles sur Bruxelles, on recommence à compter, mais cette fois ci en prenant tous les evenements
        else
        {
            foreach (XmlNode node in mainNode)
            {


                if (node.Name == "item")
                {
                    foreach (XmlNode child in node)
                    {
                        if (child.Name == "title")
                        {


                            arrayLength++;


                        }
                    }
                }
            }
            //on instantie le tableau grace au comptage fait precedament
            evenements = new string[arrayLength];
            foreach (XmlNode node in mainNode)
            {


                if (node.Name == "item")
                {



                    foreach (XmlNode child in node)
                    {
                        if (child.Name == "title")
                        {


                            if (child.ChildNodes[0].InnerText.Contains("Bxl"))
                            {
                                if (child.ChildNodes[0].InnerText.Contains("INCIDENT"))
                                {
                                    evenements[j] = PimpMyString(child.ChildNodes[0].InnerText, true);
                                }
                                else if (child.ChildNodes[0].InnerText.Contains("TRAVAUX"))
                                {
                                    evenements[j] = PimpMyString(child.ChildNodes[0].InnerText, false);
                                }
                                else
                                {
                                    Debug.Log("≠");
                                }

                                j++;

                            }
                        }

                    }

                }

            }
        }


        _textBand.text = createString(evenements);


    }

    private string createString(string[] array)
    {
        string exitString = "";
        foreach (string item in array)
        {
            exitString += item;
            exitString += " // ";
            //for (int i = 0; i < EspaceInterEvent; i++)
            //{
            //    exitString += " ";
            //}

        }
        //Debug.Log("exitString =" + exitString);
        exitString = exitString.Remove(exitString.Length - 4);
        return exitString;
    }

    private string PimpMyString(string inputString, bool accident)
    {
        string pimpedString;

        //Nettoyage de la String, en supprimant toutes les infos preliminaires telles que l'heure et la date
        int index = inputString.IndexOf("|", 0) + 1;
        pimpedString = inputString.Substring(index, inputString.Length - index);
        index = pimpedString.IndexOf("-", 0) + 2;
        pimpedString = pimpedString.Substring(index, pimpedString.Length - index);
        index = pimpedString.IndexOf(":", 0) + 1;
        pimpedString = pimpedString.Substring(index, pimpedString.Length - index);

        // On remplace l'intitulé de l'evenement, car accident n'etait pas ecris en Français
        if (accident)
        {
            pimpedString = "ACCIDENT: " + pimpedString;
        }
        else
        {
            pimpedString = "TRAVAUX: " + pimpedString;
        }

        //On remplace les élément ecris de façon bizzare/pas en français
        pimpedString = pimpedString.Replace("Bxl/Bsl", "Bruxelles");
        pimpedString = pimpedString.Replace("Bsl/Bxl", "Bruxelles");
        pimpedString = pimpedString.Replace("Aachen", "Aix-la-Chapelle");


        pimpedString = pimpedString.Replace('Ã', 'à');

        //Debug.Log("PimpMyString =" + pimpedString);
        return pimpedString;
    }
}