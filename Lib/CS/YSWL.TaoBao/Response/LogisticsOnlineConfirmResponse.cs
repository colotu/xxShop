using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// LogisticsOnlineConfirmResponse.
    /// </summary>
    public class LogisticsOnlineConfirmResponse : TopResponse
    {
        /// <summary>
        /// 只返回is_success：是否成功。
        /// </summary>
        [XmlElement("shipping")]
        public Shipping Shipping { get; set; }
    }
}
