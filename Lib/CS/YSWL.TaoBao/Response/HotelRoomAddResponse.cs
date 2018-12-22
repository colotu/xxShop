using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// HotelRoomAddResponse.
    /// </summary>
    public class HotelRoomAddResponse : TopResponse
    {
        /// <summary>
        /// 商品结构
        /// </summary>
        [XmlElement("room")]
        public Room Room { get; set; }
    }
}
