using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace EoDData.Net
{
    [XmlRoot(ElementName = "RESPONSE", Namespace = DATA_NAMESPACE)]
    public class QuoteGetResponse : BaseResponse
    {
        [XmlElement(ElementName = "QUOTE")]
        public Quote Quote { get; set; }
    }

    [XmlRoot(ElementName = "RESPONSE", Namespace = DATA_NAMESPACE)]
    public class QuoteListResponse : BaseResponse
    {
        [XmlElement(ElementName = "QUOTES")]
        public Quotes Quotes { get; set; }
    }

    [XmlRoot(ElementName = "QUOTES")]
    public class Quotes
    {
        [XmlElement(ElementName = "QUOTE")]
        public List<Quote> QuoteList { get; set; }
    }

    [XmlRoot(ElementName="QUOTE")]
	public class Quote
    {
        [XmlAttribute(AttributeName="Symbol")] 
        public string Symbol { get; set; } 
    
        [XmlAttribute(AttributeName="Description")] 
        public string Description { get; set; }

        [XmlAttribute(AttributeName="Name")] 
        public string Name { get; set; }

        [XmlAttribute(AttributeName="DateTime")] 
        public DateTime DateTime { get; set; }

        [XmlAttribute(AttributeName="Open")] 
        public double Open { get; set; } 
    
        [XmlAttribute(AttributeName="High")] 
        public double High { get; set; } 
    
        [XmlAttribute(AttributeName="Low")] 
        public double Low { get; set; } 
    
        [XmlAttribute(AttributeName="Close")] 
        public double Close { get; set; }

        [XmlAttribute(AttributeName="Volume")] 
        public int Volume { get; set; } 
    
        [XmlAttribute(AttributeName="OpenInterest")] 
        public double OpenInterest { get; set; } 
    
        [XmlAttribute(AttributeName="Previous")] 
        public double Previous { get; set; } 
    
        [XmlAttribute(AttributeName="Change")] 
        public double Change { get; set; } 
    
        [XmlAttribute(AttributeName="Bid")] 
        public double Bid { get; set; }

        [XmlAttribute(AttributeName="Ask")] 
        public double Ask { get; set; } 
    
        [XmlAttribute(AttributeName="PreviousClose")] 
        public double PreviousClose { get; set; } 
    
        [XmlAttribute(AttributeName="NextOpen")] 
        public double NextOpen { get; set; } 
    
        [XmlAttribute(AttributeName="Modified")] 
        public DateTime Modified { get; set; }
    }
}