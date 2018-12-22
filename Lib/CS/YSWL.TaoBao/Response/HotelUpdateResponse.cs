using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// HotelUpdateResponse.
    /// </summary>
    public class HotelUpdateResponse : TopResponse
    {
        /// <summary>
        /// 酒店结构
        /// </summary>
        [XmlElement("hotel")]
        public Hotel Hotel { get; set; }
    }
}
