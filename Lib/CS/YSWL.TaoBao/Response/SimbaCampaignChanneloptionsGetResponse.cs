using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaCampaignChanneloptionsGetResponse.
    /// </summary>
    public class SimbaCampaignChanneloptionsGetResponse : TopResponse
    {
        /// <summary>
        /// 所有推广计划可投放的频道
        /// </summary>
        [XmlArray("channel_options")]
        [XmlArrayItem("channel_option")]
        public List<ChannelOption> ChannelOptions { get; set; }
    }
}
