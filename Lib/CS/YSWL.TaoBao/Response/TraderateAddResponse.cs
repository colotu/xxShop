using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// TraderateAddResponse.
    /// </summary>
    public class TraderateAddResponse : TopResponse
    {
        /// <summary>
        /// 返回tid、oid、create
        /// </summary>
        [XmlElement("trade_rate")]
        public TradeRate TradeRate { get; set; }
    }
}
