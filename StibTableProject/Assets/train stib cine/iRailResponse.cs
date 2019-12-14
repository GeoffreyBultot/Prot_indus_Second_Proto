using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


namespace iRailResponse {



	public class iRailFeed
	{
		

		public Liveboard liveBoard;


		public iRailFeed(String content)
		{
			
			// Create an XmlSerializer for rss Class.
			XmlSerializer serializer = new XmlSerializer(typeof(Liveboard));

			// Create an XmlUrlResolver with default credentials.
			XmlUrlResolver resolver = new XmlUrlResolver();
			resolver.Credentials = CredentialCache.DefaultCredentials;

			// Create the reader.
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.XmlResolver = resolver;

			// Serialize.
			XmlReader reader = XmlReader.Create(new StringReader(content), settings);
			liveBoard = (Liveboard)serializer.Deserialize(reader);
		}

	[XmlRoot(ElementName="station")]
	public class Station {
		[XmlAttribute(AttributeName="id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName="locationX")]
		public string LocationX { get; set; }
		[XmlAttribute(AttributeName="locationY")]
		public string LocationY { get; set; }
		[XmlAttribute(AttributeName="URI")]
		public string URI { get; set; }
		[XmlAttribute(AttributeName="standardname")]
		public string Standardname { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="time")]
	public class Time {
		[XmlAttribute(AttributeName="formatted")]
		public string Formatted { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="vehicle")]
	public class Vehicle {
		[XmlAttribute(AttributeName="URI")]
		public string URI { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="platform")]
	public class Platform {
		[XmlAttribute(AttributeName="normal")]
		public string Normal { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="occupancy")]
	public class Occupancy {
		[XmlAttribute(AttributeName="URI")]
		public string URI { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="departure")]
	public class Departure {
		[XmlElement(ElementName="station")]
		public Station Station { get; set; }
		[XmlElement(ElementName="time")]
		public Time Time { get; set; }
		[XmlElement(ElementName="vehicle")]
		public Vehicle Vehicle { get; set; }
		[XmlElement(ElementName="platform")]
		public Platform Platform { get; set; }
		[XmlElement(ElementName="departureConnection")]
		public string DepartureConnection { get; set; }
		[XmlElement(ElementName="occupancy")]
		public Occupancy Occupancy { get; set; }
		[XmlAttribute(AttributeName="id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName="delay")]
		public string Delay { get; set; }
		[XmlAttribute(AttributeName="canceled")]
		public string Canceled { get; set; }
		[XmlAttribute(AttributeName="left")]
		public string Left { get; set; }
	}

	[XmlRoot(ElementName="departures")]
	public class Departures {
		[XmlElement(ElementName="departure")]
		public List<Departure> Departure { get; set; }
		[XmlAttribute(AttributeName="number")]
		public string Number { get; set; }
	}

	[XmlRoot(ElementName="liveboard")]
	public class Liveboard {
		[XmlElement(ElementName="station")]
		public Station Station { get; set; }
		[XmlElement(ElementName="departures")]
		public Departures Departures { get; set; }
		[XmlAttribute(AttributeName="version")]
		public string Version { get; set; }
		[XmlAttribute(AttributeName="timestamp")]
		public string Timestamp { get; set; }
	}
}
}

