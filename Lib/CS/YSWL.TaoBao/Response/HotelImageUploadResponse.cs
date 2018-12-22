using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// HotelImageUploadResponse.
    /// </summary>
    public class HotelImageUploadResponse : TopResponse
    {
        /// <summary>
        /// 酒店图片
        /// </summary>
        [XmlElement("hotel_image")]
        public HotelImage HotelImage { get; set; }
    }
}
