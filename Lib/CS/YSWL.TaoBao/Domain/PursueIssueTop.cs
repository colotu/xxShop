using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Domain
{
    /// <summary>
    /// PursueIssueTop Data Structure.
    /// </summary>
    [Serializable]
    public class PursueIssueTop : TopObject
    {
        /// <summary>
        /// 序号位置
        /// </summary>
        [XmlElement("index")]
        public long Index { get; set; }

        /// <summary>
        /// 彩期名称
        /// </summary>
        [XmlElement("issue")]
        public string Issue { get; set; }

        /// <summary>
        /// 彩期编号
        /// </summary>
        [XmlElement("issue_id")]
        public string IssueId { get; set; }

        /// <summary>
        /// stop_sale停售  issue_overdue本期截至  normal正常
        /// </summary>
        [XmlElement("issue_status")]
        public string IssueStatus { get; set; }
    }
}
