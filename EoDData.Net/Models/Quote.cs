using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace EoDData.Net
{
    [XmlRoot(ElementName = "QUOTES")]
    public class Quotes
    {
        [XmlElement(ElementName = "QUOTE")]
        public List<Quote> QuoteList { get; set; }
    }
    
    [XmlRoot(ElementName = "RESPONSE", Namespace = DATA_NAMESPACE)]
    public class QuoteListResponse : BaseResponse
    {
        [XmlElement(ElementName = "QUOTES")]
        public Quotes Quotes { get; set; }
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
        public int OpenInterest { get; set; } 
    
        [XmlAttribute(AttributeName="Previous")] 
        public int Previous { get; set; } 
    
        [XmlAttribute(AttributeName="Change")] 
        public int Change { get; set; } 
    
        [XmlAttribute(AttributeName="Bid")] 
        public int Bid { get; set; } 
    
        [XmlAttribute(AttributeName="Ask")] 
        public int Ask { get; set; } 
    
        [XmlAttribute(AttributeName="PreviousClose")] 
        public int PreviousClose { get; set; } 
    
        [XmlAttribute(AttributeName="NextOpen")] 
        public int NextOpen { get; set; } 
    
        [XmlAttribute(AttributeName="Modified")] 
        public DateTime Modified { get; set; }
    }
}