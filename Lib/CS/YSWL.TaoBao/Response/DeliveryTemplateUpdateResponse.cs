using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// DeliveryTemplateUpdateResponse.
    /// </summary>
    public class DeliveryTemplateUpdateResponse : TopResponse
    {
        /// <summary>
        /// 表示修改是否成功
        /// </summary>
        [XmlElement("complete")]
        public bool Complete { get; set; }
    }
}
