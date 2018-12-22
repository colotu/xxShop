using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaAdgroupOnlineitemsGetResponse.
    /// </summary>
    public class SimbaAdgroupOnlineitemsGetResponse : TopResponse
    {
        /// <summary>
        /// 带分页的淘宝商品
        /// </summary>
        [XmlElement("page_item")]
        public SimbaItemPartition PageItem { get; set; }
    }
}
