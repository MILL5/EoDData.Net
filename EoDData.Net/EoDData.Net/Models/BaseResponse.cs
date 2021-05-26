using System.Xml.Serialization;

namespace EoDData.Net
{
    public class BaseResponse
    {
        protected const string DATA_NAMESPACE = "http://ws.eoddata.com/Data";

        [XmlAttribute(AttributeName = "xsd")]
        public string Xsd { get; set; }

        [XmlAttribute(AttributeName = "xsi")]
        public string Xsi { get; set; }

        [XmlAttribute(AttributeName = "Message")]
        public string Message { get; set; }

        [XmlAttribute(AttributeName = "DataFormat")]
        public string DataFormat { get; set; }

        [XmlAttribute(AttributeName = "Header")]
        public bool Header { get; set; }

        [XmlAttribute(AttributeName = "Suffix")]
        public bool Suffix { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
}