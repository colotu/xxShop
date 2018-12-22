using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// IncrementCustomerPermitResponse.
    /// </summary>
    public class IncrementCustomerPermitResponse : TopResponse
    {
        /// <summary>
        /// 当订阅成功后，返回的订阅情况。具体内容见AppCustomer数据结构。
        /// </summary>
        [XmlElement("app_customer")]
        public AppCustomer AppCustomer { get; set; }
    }
}
