using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// LogisticsPartnersGetResponse.
    /// </summary>
    public class LogisticsPartnersGetResponse : TopResponse
    {
        /// <summary>
        /// 物流公司信息。
        /// </summary>
        [XmlArray("logistics_partners")]
        [XmlArrayItem("logistics_partner")]
        public List<LogisticsPartner> LogisticsPartners { get; set; }
    }
}
