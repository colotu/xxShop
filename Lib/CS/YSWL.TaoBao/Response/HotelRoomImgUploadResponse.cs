using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// HotelRoomImgUploadResponse.
    /// </summary>
    public class HotelRoomImgUploadResponse : TopResponse
    {
        /// <summary>
        /// 商品图片结构
        /// </summary>
        [XmlElement("room_image")]
        public RoomImage RoomImage { get; set; }
    }
}
