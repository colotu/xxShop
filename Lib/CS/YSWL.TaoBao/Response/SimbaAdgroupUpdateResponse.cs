using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaAdgroupUpdateResponse.
    /// </summary>
    public class SimbaAdgroupUpdateResponse : TopResponse
    {
        /// <summary>
        /// 被修改的推广组
        /// </summary>
        [XmlElement("adgroup")]
        public ADGroup Adgroup { get; set; }
    }
}
