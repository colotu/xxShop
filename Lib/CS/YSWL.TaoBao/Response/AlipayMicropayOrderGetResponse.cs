using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// AlipayMicropayOrderGetResponse.
    /// </summary>
    public class AlipayMicropayOrderGetResponse : TopResponse
    {
        /// <summary>
        /// 冻结订单详情
        /// </summary>
        [XmlElement("micro_pay_order_detail")]
        public MicroPayOrderDetail MicroPayOrderDetail { get; set; }
    }
}
