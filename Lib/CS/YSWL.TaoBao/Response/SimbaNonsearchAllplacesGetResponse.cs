using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaNonsearchAllplacesGetResponse.
    /// </summary>
    public class SimbaNonsearchAllplacesGetResponse : TopResponse
    {
        /// <summary>
        /// 定向推广投放位置列表
        /// </summary>
        [XmlArray("place_list")]
        [XmlArrayItem("place")]
        public List<Place> PlaceList { get; set; }
    }
}
