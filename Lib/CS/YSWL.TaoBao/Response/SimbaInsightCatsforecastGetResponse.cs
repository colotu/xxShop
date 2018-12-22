using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaInsightCatsforecastGetResponse.
    /// </summary>
    public class SimbaInsightCatsforecastGetResponse : TopResponse
    {
        /// <summary>
        /// 类目对象列表
        /// </summary>
        [XmlArray("in_category_tops")]
        [XmlArrayItem("i_n_category_top")]
        public List<INCategoryTop> InCategoryTops { get; set; }
    }
}
