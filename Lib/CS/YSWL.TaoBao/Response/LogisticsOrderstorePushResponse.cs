using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// LogisticsOrderstorePushResponse.
    /// </summary>
    public class LogisticsOrderstorePushResponse : TopResponse
    {
        /// <summary>
        /// 返回结果是否成功is_success
        /// </summary>
        [XmlElement("shipping")]
        public Shipping Shipping { get; set; }
    }
}
