using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// RefundGetResponse.
    /// </summary>
    public class RefundGetResponse : TopResponse
    {
        /// <summary>
        /// 搜索到的交易信息列表
        /// </summary>
        [XmlElement("refund")]
        public Refund Refund { get; set; }
    }
}
