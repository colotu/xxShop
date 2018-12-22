using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// TopatsTasksGetResponse.
    /// </summary>
    public class TopatsTasksGetResponse : TopResponse
    {
        /// <summary>
        /// 符合查询条件内的定时任务的结果集
        /// </summary>
        [XmlArray("tasks")]
        [XmlArrayItem("task")]
        public List<Task> Tasks { get; set; }
    }
}
