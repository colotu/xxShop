using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// AlipayUserContractGetResponse.
    /// </summary>
    public class AlipayUserContractGetResponse : TopResponse
    {
        /// <summary>
        /// 支付宝用户订购信息
        /// </summary>
        [XmlElement("alipay_contract")]
        public AlipayContract AlipayContract { get; set; }
    }
}
