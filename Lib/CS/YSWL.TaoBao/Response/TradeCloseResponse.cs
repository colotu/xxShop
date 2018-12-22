using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// TradeCloseResponse.
    /// </summary>
    public class TradeCloseResponse : TopResponse
    {
        /// <summary>
        /// 关闭交易时返回的Trade信息，可用字段有tid和modified
        /// </summary>
        [XmlElement("trade")]
        public Trade Trade { get; set; }
    }
}
