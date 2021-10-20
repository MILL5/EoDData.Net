using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace EoDData.Net
{
    [XmlRoot(ElementName = "RESPONSE", Namespace = DATA_NAMESPACE)]
    public class SplitListResponse : BaseResponse
    {
        [XmlElement(ElementName = "SPLITS")]
        public Splits Splits { get; set; }
    }

    [XmlRoot(ElementName = "SPLITS")]
    public class Splits
    {
        [XmlElement(ElementName = "SPLIT")]
        public List<Split> SplitList { get; set; }
    }

    [XmlRoot(ElementName = "SPLIT")]
    public class Split
    {
        [XmlAttribute(AttributeName = "Symbol")]
        public string Symbol { get; set; }

        [XmlAttribute(AttributeName = "Exchange")]
        public string Exchange { get; set; }

        [XmlAttribute(AttributeName = "DateTime")]
        public DateTime DateTime { get; set; }

        [XmlAttribute(AttributeName = "Ratio")]
        public string Ratio { get; set; }
    }
}
