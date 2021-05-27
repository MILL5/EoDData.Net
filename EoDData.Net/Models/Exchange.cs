using System;
using System.Xml.Serialization;

namespace EoDData.Net
{
	[XmlRoot(ElementName = "RESPONSE", Namespace = "http://ws.eoddata.com/Data")]
	public class ExchangeResponse : BaseResponse
	{
		[XmlElement(ElementName = "EXCHANGE")]
		public Exchange Exchange { get; set; }
	}

	[XmlRoot(ElementName = "EXCHANGE")]
	public class Exchange
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
		public string Suffix { get; set; }

		[XmlAttribute(AttributeName = "TimeZone")]
		public string TimeZone { get; set; }

		[XmlAttribute(AttributeName = "IsIntraday")]
		public bool IsIntraday { get; set; }

		[XmlAttribute(AttributeName = "IntradayStartDate")]
		public DateTime IntradayStartDate { get; set; }

		[XmlAttribute(AttributeName = "HasIntradayProduct")]
		public bool HasIntradayProduct { get; set; }
	}
}