using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaAdgroupChangedcatmatchsGetResponse.
    /// </summary>
    public class SimbaAdgroupChangedcatmatchsGetResponse : TopResponse
    {
        /// <summary>
        /// 一页类目出价对象
        /// </summary>
        [XmlElement("changed_catmatchs")]
        public ADGroupCatMatchPage ChangedCatmatchs { get; set; }
    }
}
