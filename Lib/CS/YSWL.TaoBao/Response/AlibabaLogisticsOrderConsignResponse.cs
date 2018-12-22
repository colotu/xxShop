using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// AlibabaLogisticsOrderConsignResponse.
    /// </summary>
    public class AlibabaLogisticsOrderConsignResponse : TopResponse
    {
        /// <summary>
        /// 发货结果
        /// </summary>
        [XmlElement("consign_result")]
        public ConsignResult ConsignResult { get; set; }
    }
}
