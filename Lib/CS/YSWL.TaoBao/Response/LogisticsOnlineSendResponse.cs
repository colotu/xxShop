using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// LogisticsOnlineSendResponse.
    /// </summary>
    public class LogisticsOnlineSendResponse : TopResponse
    {
        /// <summary>
        /// 返回发货是否成功is_success
        /// </summary>
        [XmlElement("shipping")]
        public Shipping Shipping { get; set; }
    }
}
