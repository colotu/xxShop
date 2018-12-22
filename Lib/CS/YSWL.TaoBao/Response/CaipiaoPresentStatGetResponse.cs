using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// CaipiaoPresentStatGetResponse.
    /// </summary>
    public class CaipiaoPresentStatGetResponse : TopResponse
    {
        /// <summary>
        /// 查询的结果集
        /// </summary>
        [XmlArray("results")]
        [XmlArrayItem("lottery_wangcai_present_stat")]
        public List<LotteryWangcaiPresentStat> Results { get; set; }

        /// <summary>
        /// 查询的结果集大小
        /// </summary>
        [XmlElement("total_results")]
        public long TotalResults { get; set; }
    }
}
