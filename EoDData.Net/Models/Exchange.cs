using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace EoDData.Net
{
	[XmlRoot(ElementName = "RESPONSE", Namespace = DATA_NAMESPACE)]
	public class ExchangeGetResponse : BaseResponse
	{
		[XmlElement(ElementName = "EXCHANGE")]
		public Exchange Exchange { get; set; }
	}

	[XmlRoot(ElementName = "RESPONSE", Namespace = DATA_NAMESPACE)]
	public class ExchangeListResponse : BaseResponse
	{
		[XmlElement(ElementName = "EXCHANGES")]
		public Exchanges Exchanges { get; set; }
	}

	[XmlRoot(ElementName = "EXCHANGES")]
	public class Exchanges
	{
		[XmlElement(ElementName = "EXCHANGE")]
		public List<Exchange> ExchangeList { get; set; }
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