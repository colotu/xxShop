using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// CaipiaoIssueGetResponse.
    /// </summary>
    public class CaipiaoIssueGetResponse : TopResponse
    {
        /// <summary>
        /// 彩期结构
        /// </summary>
        [XmlArray("results")]
        [XmlArrayItem("issue_top")]
        public List<IssueTop> Results { get; set; }

        /// <summary>
        /// 系统时间
        /// </summary>
        [XmlElement("server_time")]
        public string ServerTime { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        [XmlElement("status")]
        public bool Status { get; set; }
    }
}
