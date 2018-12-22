using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaAdgroupAdgroupcatmatchsGetResponse.
    /// </summary>
    public class SimbaAdgroupAdgroupcatmatchsGetResponse : TopResponse
    {
        /// <summary>
        /// 类目出价列表
        /// </summary>
        [XmlArray("adgroup_catmatch_list")]
        [XmlArrayItem("a_d_group_catmatch")]
        public List<ADGroupCatmatch> AdgroupCatmatchList { get; set; }
    }
}
