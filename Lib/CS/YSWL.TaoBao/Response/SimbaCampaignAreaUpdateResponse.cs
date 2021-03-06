using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaCampaignAreaUpdateResponse.
    /// </summary>
    public class SimbaCampaignAreaUpdateResponse : TopResponse
    {
        /// <summary>
        /// 修改后的推广计划投放地域
        /// </summary>
        [XmlElement("campaign_area")]
        public CampaignArea CampaignArea { get; set; }
    }
}
