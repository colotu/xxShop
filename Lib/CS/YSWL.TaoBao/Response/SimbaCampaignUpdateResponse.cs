using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaCampaignUpdateResponse.
    /// </summary>
    public class SimbaCampaignUpdateResponse : TopResponse
    {
        /// <summary>
        /// 修改后的推广计划
        /// </summary>
        [XmlElement("campaign")]
        public Campaign Campaign { get; set; }
    }
}
