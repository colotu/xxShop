using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaInsightToplevelcatsGetResponse.
    /// </summary>
    public class SimbaInsightToplevelcatsGetResponse : TopResponse
    {
        /// <summary>
        /// 得到一级类目
        /// </summary>
        [XmlArray("in_category_tops")]
        [XmlArrayItem("i_n_category_top")]
        public List<INCategoryTop> InCategoryTops { get; set; }
    }
}
