using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// AlipayMicropayOrderDirectPayResponse.
    /// </summary>
    public class AlipayMicropayOrderDirectPayResponse : TopResponse
    {
        /// <summary>
        /// 单笔直接支付返回结果
        /// </summary>
        [XmlElement("single_pay_detail")]
        public SinglePayDetail SinglePayDetail { get; set; }
    }
}
