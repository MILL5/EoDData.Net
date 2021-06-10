using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace EoDData.Net
{
    [XmlRoot(ElementName = "RESPONSE", Namespace = DATA_NAMESPACE)]
    public class SymbolGetResponse : BaseResponse
    {
        [XmlElement(ElementName = "SYMBOL")]
        public Symbol Symbol { get; set; }
    }

    [XmlRoot(ElementName = "RESPONSE", Namespace = DATA_NAMESPACE)]
    public class SymbolListResponse : BaseResponse
    {
        [XmlElement(ElementName = "SYMBOLS")]
        public Symbols Symbols { get; set; }
    }

    [XmlRoot(ElementName = "SYMBOLS")]
    public class Symbols
    {
        [XmlElement(ElementName = "SYMBOL")]
        public List<Symbol> SymbolList { get; set; }
    }

    [XmlRoot(ElementName = "SYMBOL")]
    public class Symbol
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "LongName")]
        public string LongName { get; set; }

        [XmlAttribute(AttributeName = "DateTime")]
        public DateTime DateTime { get; set; }
    }
}