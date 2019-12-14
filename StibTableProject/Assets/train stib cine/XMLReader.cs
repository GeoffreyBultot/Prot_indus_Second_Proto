using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;

public class XMLReader : MonoBehaviour {

	private string url = "http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20weather.forecast%20where%20woeid%20in%20(44418)&format=xml&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";

	void Start()
	{
		StartCoroutine(Weather());
	}
	public IEnumerator Weather()
	{
		WWW www = new WWW(url);
		yield return www;


		if (www.isDone)
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(www.text);
			Debug.Log(www.text);
			XmlNamespaceManager nameSpaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
			nameSpaceManager.AddNamespace ("yweather","http://xml.weather.yahoo.com/ns/rss/1.0");
			XmlNodeList locations = xmlDoc.SelectNodes("/query/results/channel/yweather:location", nameSpaceManager);
			foreach (XmlNode location in locations) {
				GameObject cityUI = GameObject.Find("City");
				cityUI.GetComponent<Text>().text = location.Attributes["city"].Value;
			}
			XmlNodeList weatherCondition = xmlDoc.SelectNodes("/query/results/channel/item/yweather:condition", nameSpaceManager);
			foreach (XmlNode condition in weatherCondition) {
				Debug.Log(condition.Attributes["temp"].Value);
				GameObject cityUI = GameObject.Find("Temperature");
				cityUI.GetComponent<Text>().text = condition.Attributes["temp"].Value + "°F";
				//break;
			}
			Debug.Log("Complete");
		}
	}
}
