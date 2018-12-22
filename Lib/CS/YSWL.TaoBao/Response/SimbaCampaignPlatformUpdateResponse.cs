using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaCampaignPlatformUpdateResponse.
    /// </summary>
    public class SimbaCampaignPlatformUpdateResponse : TopResponse
    {
        /// <summary>
        /// 修改后的推广计划投放平台设置
        /// </summary>
        [XmlElement("campaign_platform")]
        public CampaignPlatform CampaignPlatform { get; set; }
    }
}
