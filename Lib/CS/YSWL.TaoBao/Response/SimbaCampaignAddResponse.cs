using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaCampaignAddResponse.
    /// </summary>
    public class SimbaCampaignAddResponse : TopResponse
    {
        /// <summary>
        /// 创建的推广计划
        /// </summary>
        [XmlElement("campaign")]
        public Campaign Campaign { get; set; }
    }
}
