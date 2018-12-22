using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaInsightCatsbaseGetResponse.
    /// </summary>
    public class SimbaInsightCatsbaseGetResponse : TopResponse
    {
        /// <summary>
        /// 类目基础数据列表
        /// </summary>
        [XmlArray("in_category_bases")]
        [XmlArrayItem("i_n_category_base")]
        public List<INCategoryBase> InCategoryBases { get; set; }
    }
}
