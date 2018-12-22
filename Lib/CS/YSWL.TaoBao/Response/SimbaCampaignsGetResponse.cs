using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaCampaignsGetResponse.
    /// </summary>
    public class SimbaCampaignsGetResponse : TopResponse
    {
        /// <summary>
        /// 推广计划列表
        /// </summary>
        [XmlArray("campaigns")]
        [XmlArrayItem("campaign")]
        public List<Campaign> Campaigns { get; set; }
    }
}
