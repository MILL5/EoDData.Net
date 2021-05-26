using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace EoDData.Net
{
	[XmlRoot(ElementName = "EXCHANGE")]
	public class EXCHANGE
	{
		[XmlAttribute(AttributeName = "Code")]
		public string Code { get; set; }

		[XmlAttribute(AttributeName = "Name")]
		public string Name { get; set; }

		[XmlAttribute(AttributeName = "LastTradeDateTime")]
		public DateTime LastTradeDateTime { get; set; }

		[XmlAttribute(AttributeName = "Country")]
		public string Country { get; set; }

		[XmlAttribute(AttributeName = "Currency")]
		public string Currency { get; set; }

		[XmlAttribute(AttributeName = "Advances")]
		public int Advances { get; set; }

		[XmlAttribute(AttributeName = "Declines")]
		public int Declines { get; set; }

		[XmlAttribute(AttributeName = "Suffix")]
		public object Suffix { get; set; }

		[XmlAttribute(AttributeName = "TimeZone")]
		public string TimeZone { get; set; }

		[XmlAttribute(AttributeName = "IsIntraday")]
		public bool IsIntraday { get; set; }

		[XmlAttribute(AttributeName = "IntradayStartDate")]
		public DateTime IntradayStartDate { get; set; }

		[XmlAttribute(AttributeName = "HasIntradayProduct")]
		public bool HasIntradayProduct { get; set; }
	}

	[XmlRoot(ElementName = "RESPONSE", Namespace = "http://ws.eoddata.com/Data")]
	public class RESPONSE
	{
		[XmlElement(ElementName = "EXCHANGE")]
		public EXCHANGE EXCHANGE { get; set; }

		[XmlAttribute(AttributeName = "xsd")]
		public string Xsd { get; set; }

		[XmlAttribute(AttributeName = "xsi")]
		public string Xsi { get; set; }

		[XmlAttribute(AttributeName = "Source")]
		public string Source { get; set; }

		[XmlAttribute(AttributeName = "Message")]
		public string Message { get; set; }

		[XmlAttribute(AttributeName = "Date")]
		public DateTime Date { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}
}