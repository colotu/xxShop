using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaAdgroupsGetResponse.
    /// </summary>
    public class SimbaAdgroupsGetResponse : TopResponse
    {
        /// <summary>
        /// 返回的推广组分页对象
        /// </summary>
        [XmlElement("adgroups")]
        public ADGroupPage Adgroups { get; set; }
    }
}
