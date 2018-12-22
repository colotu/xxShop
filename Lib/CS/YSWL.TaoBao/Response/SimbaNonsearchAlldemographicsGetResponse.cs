using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaNonsearchAlldemographicsGetResponse.
    /// </summary>
    public class SimbaNonsearchAlldemographicsGetResponse : TopResponse
    {
        /// <summary>
        /// 定向投放人群维度列表
        /// </summary>
        [XmlArray("demographic_list")]
        [XmlArrayItem("demographic")]
        public List<Demographic> DemographicList { get; set; }
    }
}
