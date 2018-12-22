using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// RefundMessageAddResponse.
    /// </summary>
    public class RefundMessageAddResponse : TopResponse
    {
        /// <summary>
        /// 退款信息。包含id和created
        /// </summary>
        [XmlElement("refund_message")]
        public RefundMessage RefundMessage { get; set; }
    }
}
