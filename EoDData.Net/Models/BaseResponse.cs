using System;
using System.Xml.Serialization;

namespace EoDData.Net
{
    [XmlRoot(ElementName = "RESPONSE", Namespace = DATA_NAMESPACE)]
    public class BaseResponse
    {
        protected const string DATA_NAMESPACE = "http://ws.eoddata.com/Data";

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