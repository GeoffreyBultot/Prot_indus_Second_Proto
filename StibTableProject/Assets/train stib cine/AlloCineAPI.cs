using System;

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AlloCineAPI
{
	public class RSSFeed
	{
		public Rss rss;

		public RSSFeed(String content)
		{
			// Create an XmlSerializer for rss Class.
			XmlSerializer serializer = new XmlSerializer(typeof(Rss));

			// Create an XmlUrlResolver with default credentials.
			XmlUrlResolver resolver = new XmlUrlResolver();
			resolver.Credentials = CredentialCache.DefaultCredentials;

			// Create the reader.
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.XmlResolver = resolver;

			// Serialize.
            XmlReader reader = XmlReader.Create(new StringReader(content), settings);
			rss = (Rss)serializer.Deserialize(reader);
		}
	}

	[XmlRoot(ElementName="image")]
	public class Image {
		[XmlElement(ElementName="title")]
		public string Title { get; set; }
		[XmlElement(ElementName="link")]
		public string Link { get; set; }
		[XmlElement(ElementName="url")]
		public string Url { get; set; }
	}

	[XmlRoot(ElementName="link", Namespace="http://www.w3.org/2005/Atom")]
	public class Link {
		[XmlAttribute(AttributeName="atom10", Namespace="http://www.w3.org/2000/xmlns/")]
		public string Atom10 { get; set; }
		[XmlAttribute(AttributeName="rel")]
		public string Rel { get; set; }
		[XmlAttribute(AttributeName="type")]
		public string Type { get; set; }
		[XmlAttribute(AttributeName="href")]
		public string Href { get; set; }
	}

	[XmlRoot(ElementName="info", Namespace="http://rssnamespace.org/feedburner/ext/1.0")]
	public class Info {
		[XmlAttribute(AttributeName="uri")]
		public string Uri { get; set; }
	}

	[XmlRoot(ElementName="guid")]
	public class Guid {
		[XmlAttribute(AttributeName="isPermaLink")]
		public string IsPermaLink { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="enclosure")]
	public class Enclosure {
		[XmlAttribute(AttributeName="url")]
		public string Url { get; set; }
		[XmlAttribute(AttributeName="type")]
		public string Type { get; set; }
		[XmlAttribute(AttributeName="length")]
		public string Length { get; set; }
	}

	[XmlRoot(ElementName="item")]
	public class Item {
		[XmlElement(ElementName="pubDate")]
		public string PubDate { get; set; }
		[XmlElement(ElementName="title")]
		public string Title { get; set; }
		[XmlElement(ElementName="link")]
		public string Link2 { get; set; }
		[XmlElement(ElementName="description")]
		public string Description { get; set; }
		[XmlElement(ElementName="guid")]
		public Guid Guid { get; set; }
		[XmlElement(ElementName="enclosure")]
		public Enclosure Enclosure { get; set; }
		[XmlElement(ElementName="origLink", Namespace="http://rssnamespace.org/feedburner/ext/1.0")]
		public string OrigLink { get; set; }
	}

	[XmlRoot(ElementName="channel")]
	public class Channel {
		[XmlElement(ElementName="title")]
		public string Title { get; set; }
		[XmlElement(ElementName="link")]
		public List<string> Link { get; set; }
		[XmlElement(ElementName="description")]
		public string Description { get; set; }
		[XmlElement(ElementName="copyright")]
		public string Copyright { get; set; }
		[XmlElement(ElementName="category")]
		public string Category { get; set; }
		[XmlElement(ElementName="language")]
		public string Language { get; set; }
		[XmlElement(ElementName="lastBuildDate")]
		public string LastBuildDate { get; set; }
		[XmlElement(ElementName="pubDate")]
		public string PubDate { get; set; }
		[XmlElement(ElementName="ttl")]
		public string Ttl { get; set; }
		[XmlElement(ElementName="image")]
		public Image Image { get; set; }
		[XmlElement(ElementName="info", Namespace="http://rssnamespace.org/feedburner/ext/1.0")]
		public Info Info { get; set; }
		[XmlElement(ElementName="item")]
		public List<Item> Item { get; set; }
	}

	[XmlRoot(ElementName="rss")]
	public class Rss {
		[XmlElement(ElementName="channel")]
		public Channel Channel { get; set; }
		[XmlAttribute(AttributeName="feedburner", Namespace="http://www.w3.org/2000/xmlns/")]
		public string Feedburner { get; set; }
		[XmlAttribute(AttributeName="version")]
		public string Version { get; set; }
	}

}