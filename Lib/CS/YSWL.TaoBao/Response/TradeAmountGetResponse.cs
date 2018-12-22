using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// TradeAmountGetResponse.
    /// </summary>
    public class TradeAmountGetResponse : TopResponse
    {
        /// <summary>
        /// 主订单的财务信息详情
        /// </summary>
        [XmlElement("trade_amount")]
        public TradeAmount TradeAmount { get; set; }
    }
}
