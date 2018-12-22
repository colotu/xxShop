using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaCampaignBudgetGetResponse.
    /// </summary>
    public class SimbaCampaignBudgetGetResponse : TopResponse
    {
        /// <summary>
        /// 推广计划日限额
        /// </summary>
        [XmlElement("campaign_budget")]
        public CampaignBudget CampaignBudget { get; set; }
    }
}
