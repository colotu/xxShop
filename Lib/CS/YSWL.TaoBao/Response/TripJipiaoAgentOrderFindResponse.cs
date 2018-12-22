using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// TripJipiaoAgentOrderFindResponse.
    /// </summary>
    public class TripJipiaoAgentOrderFindResponse : TopResponse
    {
        /// <summary>
        /// 国内机票订单搜索返回结果对象
        /// </summary>
        [XmlElement("search_result")]
        public SearchOrderResult SearchResult { get; set; }
    }
}
