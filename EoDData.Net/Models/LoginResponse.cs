using System.Xml.Serialization;

namespace EoDData.Net
{
    [XmlRoot(ElementName="LOGINRESPONSE", Namespace = DATA_NAMESPACE)]
    public class LoginResponse : BaseResponse
    {
		[XmlAttribute(AttributeName = "Token")]
		public string Token { get; set; }

		[XmlAttribute(AttributeName = "DataFormat")]
		public string DataFormat { get; set; }

		[XmlAttribute(AttributeName = "Header")]
		public bool Header { get; set; }

		[XmlAttribute(AttributeName = "Suffix")]
		public bool Suffix { get; set; }
	}
}