using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// HotelRoomUpdateResponse.
    /// </summary>
    public class HotelRoomUpdateResponse : TopResponse
    {
        /// <summary>
        /// 商品结构
        /// </summary>
        [XmlElement("room")]
        public Room Room { get; set; }
    }
}
