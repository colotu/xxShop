using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// UmpActivityAddResponse.
    /// </summary>
    public class UmpActivityAddResponse : TopResponse
    {
        /// <summary>
        /// 活动id
        /// </summary>
        [XmlElement("act_id")]
        public long ActId { get; set; }
    }
}
