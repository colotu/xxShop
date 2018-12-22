using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// AlipayMicropayOrderFreezepayurlGetResponse.
    /// </summary>
    public class AlipayMicropayOrderFreezepayurlGetResponse : TopResponse
    {
        /// <summary>
        /// 支付冻结金的地址
        /// </summary>
        [XmlElement("pay_freeze_url")]
        public string PayFreezeUrl { get; set; }
    }
}
