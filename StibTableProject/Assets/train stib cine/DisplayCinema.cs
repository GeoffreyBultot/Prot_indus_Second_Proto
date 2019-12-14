using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using AlloCineAPI;


public class DisplayCinema : MonoBehaviour {

    public string RssUrl;
    private int numberOfImage = 0;

	// Use this for initialization
	void Start () {
        StartCoroutine(loadRss());
	}

	public IEnumerator loadRss()
	{
		WWW www = new WWW(RssUrl);
		yield return www;
        //Debug.Log(www.text);
        RSSFeed cinemaRssFeed = new RSSFeed(www.text);
		//Debug.Log(cinemaRssFeed.rss.Channel.Item.Count);

		System.Random randomGenerator = new System.Random();
		int randomPoster = randomGenerator.Next(0, cinemaRssFeed.rss.Channel.Item.Count);
		GameObject leftPoster = GameObject.Find("leftPoster");
		StartCoroutine(loadImage(cinemaRssFeed.rss.Channel.Item[randomPoster].Enclosure.Url, leftPoster));

		cinemaRssFeed.rss.Channel.Item.Remove(cinemaRssFeed.rss.Channel.Item[randomPoster]);
		randomPoster = randomGenerator.Next(0, cinemaRssFeed.rss.Channel.Item.Count);
		GameObject rightPoster = GameObject.Find("rightPoster");
		StartCoroutine(loadImage(cinemaRssFeed.rss.Channel.Item[randomPoster].Enclosure.Url, rightPoster));
	}

	IEnumerator loadImage(string url, GameObject poster)
	{
		WWW www = new WWW(url);

        yield return www;

		if (www.texture != null)
		{
            poster.GetComponent<UnityEngine.UI.Image>().sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
            numberOfImage++;
		}

        if (numberOfImage==2) {
            GameObject status = GameObject.Find("Status");
            //status.GetComponent<SceneStatus>().readyToOpen = true;
        }

		yield return null;
	}
}
