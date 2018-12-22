using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// EbookItemsSearchResponse.
    /// </summary>
    public class EbookItemsSearchResponse : TopResponse
    {
        /// <summary>
        /// 查询的结果集
        /// </summary>
        [XmlArray("results")]
        [XmlArrayItem("ebook_item")]
        public List<EbookItem> Results { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        [XmlElement("total_results")]
        public long TotalResults { get; set; }
    }
}
