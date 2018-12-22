using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// HotelOrderFaceCheckResponse.
    /// </summary>
    public class HotelOrderFaceCheckResponse : TopResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        [XmlElement("result")]
        public string Result { get; set; }
    }
}
