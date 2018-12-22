using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaToolsItemsTopGetResponse.
    /// </summary>
    public class SimbaToolsItemsTopGetResponse : TopResponse
    {
        /// <summary>
        /// 推广组信息列表
        /// </summary>
        [XmlArray("rankeditems")]
        [XmlArrayItem("ranked_item")]
        public List<RankedItem> Rankeditems { get; set; }
    }
}
