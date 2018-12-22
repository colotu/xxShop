using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaAdgroupNonsearchpricesUpdateResponse.
    /// </summary>
    public class SimbaAdgroupNonsearchpricesUpdateResponse : TopResponse
    {
        /// <summary>
        /// 推广组列表
        /// </summary>
        [XmlArray("adgroup_list")]
        [XmlArrayItem("a_d_group")]
        public List<ADGroup> AdgroupList { get; set; }
    }
}
