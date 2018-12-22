using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// HotelTypeAddResponse.
    /// </summary>
    public class HotelTypeAddResponse : TopResponse
    {
        /// <summary>
        /// 房型结构
        /// </summary>
        [XmlElement("room_type")]
        public RoomType RoomType { get; set; }
    }
}
