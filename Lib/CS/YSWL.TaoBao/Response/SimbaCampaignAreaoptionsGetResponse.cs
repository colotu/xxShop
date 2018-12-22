using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaCampaignAreaoptionsGetResponse.
    /// </summary>
    public class SimbaCampaignAreaoptionsGetResponse : TopResponse
    {
        /// <summary>
        /// 推广计划所有可设置的投放地域
        /// </summary>
        [XmlArray("area_options")]
        [XmlArrayItem("area_option")]
        public List<AreaOption> AreaOptions { get; set; }
    }
}
