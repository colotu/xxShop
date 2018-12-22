using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// HotelOrderFaceDealResponse.
    /// </summary>
    public class HotelOrderFaceDealResponse : TopResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        [XmlElement("result")]
        public string Result { get; set; }
    }
}
