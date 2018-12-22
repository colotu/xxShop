using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// TradeShippingaddressUpdateResponse.
    /// </summary>
    public class TradeShippingaddressUpdateResponse : TopResponse
    {
        /// <summary>
        /// 交易结构
        /// </summary>
        [XmlElement("trade")]
        public Trade Trade { get; set; }
    }
}
