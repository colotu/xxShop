using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// UmpToolAddResponse.
    /// </summary>
    public class UmpToolAddResponse : TopResponse
    {
        /// <summary>
        /// 工具id
        /// </summary>
        [XmlElement("tool_id")]
        public long ToolId { get; set; }
    }
}
