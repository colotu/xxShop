using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaAdgroupDeletedcatmatchsGetResponse.
    /// </summary>
    public class SimbaAdgroupDeletedcatmatchsGetResponse : TopResponse
    {
        /// <summary>
        /// 一页类目出价对象
        /// </summary>
        [XmlElement("deleted_catmatchs")]
        public ADGroupCatMatchPage DeletedCatmatchs { get; set; }
    }
}
