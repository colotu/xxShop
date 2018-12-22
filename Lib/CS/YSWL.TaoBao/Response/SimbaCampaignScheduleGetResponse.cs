using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaCampaignScheduleGetResponse.
    /// </summary>
    public class SimbaCampaignScheduleGetResponse : TopResponse
    {
        /// <summary>
        /// 修改后的推广计划分时折扣
        /// </summary>
        [XmlElement("campaign_schedule")]
        public CampaignSchedule CampaignSchedule { get; set; }
    }
}
