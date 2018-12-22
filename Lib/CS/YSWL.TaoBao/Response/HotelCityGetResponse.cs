using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// HotelCityGetResponse.
    /// </summary>
    public class HotelCityGetResponse : TopResponse
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        [XmlElement("result")]
        public string Result { get; set; }
    }
}
