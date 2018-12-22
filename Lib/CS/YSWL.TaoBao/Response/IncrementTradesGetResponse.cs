using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// IncrementTradesGetResponse.
    /// </summary>
    public class IncrementTradesGetResponse : TopResponse
    {
        /// <summary>
        /// 获取的交易通知消息体 ，具体字段见NotifyTrade数据结构
        /// </summary>
        [XmlArray("notify_trades")]
        [XmlArrayItem("notify_trade")]
        public List<NotifyTrade> NotifyTrades { get; set; }

        /// <summary>
        /// 搜索到符合条件的结果总数。
        /// </summary>
        [XmlElement("total_results")]
        public long TotalResults { get; set; }
    }
}
