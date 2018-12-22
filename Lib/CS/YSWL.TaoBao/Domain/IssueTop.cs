using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace YSWL.TaoBao.Domain
{
    /// <summary>
    /// IssueTop Data Structure.
    /// </summary>
    [Serializable]
    public class IssueTop : TopObject
    {
        /// <summary>
        /// 序号
        /// </summary>
        [XmlElement("index")]
        public long Index { get; set; }

        /// <summary>
        /// 是否被推荐
        /// </summary>
        [XmlElement("is_recommend")]
        public string IsRecommend { get; set; }

        /// <summary>
        /// 是否支持合买
        /// </summary>
        [XmlElement("is_support_united")]
        public string IsSupportUnited { get; set; }

        /// <summary>
        /// 彩期
        /// </summary>
        [XmlElement("issue")]
        public string Issue { get; set; }

        /// <summary>
        /// 彩期编号
        /// </summary>
        [XmlElement("issue_id")]
        public string IssueId { get; set; }

        /// <summary>
        /// 彩期状态
        /// </summary>
        [XmlElement("issue_status")]
        public string IssueStatus { get; set; }

        /// <summary>
        /// 最后购买时间
        /// </summary>
        [XmlElement("last_buy_time")]
        public string LastBuyTime { get; set; }

        /// <summary>
        /// 彩期描述
        /// </summary>
        [XmlElement("lottery_desc")]
        public string LotteryDesc { get; set; }

        /// <summary>
        /// 彩期id
        /// </summary>
        [XmlElement("lottery_type_id")]
        public long LotteryTypeId { get; set; }

        /// <summary>
        /// 开奖时间
        /// </summary>
        [XmlElement("open_award_time")]
        public string OpenAwardTime { get; set; }

        /// <summary>
        /// 列表类型
        /// </summary>
        [XmlArray("pursue_issue_top_list")]
        [XmlArrayItem("pursue_issue_top")]
        public List<PursueIssueTop> PursueIssueTopList { get; set; }

        /// <summary>
        /// 开始购买时间
        /// </summary>
        [XmlElement("start_time")]
        public string StartTime { get; set; }

        /// <summary>
        /// 总奖金池
        /// </summary>
        [XmlElement("total_award")]
        public string TotalAward { get; set; }
    }
}
