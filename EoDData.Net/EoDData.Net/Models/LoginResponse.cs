using System.Xml.Serialization;

namespace EoDData.Net
{
    [XmlRoot(ElementName = "LOGINRESPONSE", Namespace = DATA_NAMESPACE)]
    public class LoginResponse : BaseResponse
    {
        [XmlAttribute(AttributeName = "Token")]
        public string Token { get; set; }
    }
}