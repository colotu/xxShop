using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// TopatsItemcatsGetResponse.
    /// </summary>
    public class TopatsItemcatsGetResponse : TopResponse
    {
        /// <summary>
        /// 创建任务信息。里面只包含task_id和created
        /// </summary>
        [XmlElement("task")]
        public Task Task { get; set; }
    }
}