using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaAdgroupAddResponse.
    /// </summary>
    public class SimbaAdgroupAddResponse : TopResponse
    {
        /// <summary>
        /// 新增加的推广组
        /// </summary>
        [XmlElement("adgroup")]
        public ADGroup Adgroup { get; set; }
    }
}
