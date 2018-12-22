using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// HotelRoomGetResponse.
    /// </summary>
    public class HotelRoomGetResponse : TopResponse
    {
        /// <summary>
        /// 商品结构。是否返回酒店信息、房型信息、房态列表、宝贝描述根据参数决定
        /// </summary>
        [XmlElement("room")]
        public Room Room { get; set; }
    }
}
