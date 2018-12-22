using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// LogisticsConsignResendResponse.
    /// </summary>
    public class LogisticsConsignResendResponse : TopResponse
    {
        /// <summary>
        /// 返回发货是否成功is_success
        /// </summary>
        [XmlElement("shipping")]
        public Shipping Shipping { get; set; }
    }
}
