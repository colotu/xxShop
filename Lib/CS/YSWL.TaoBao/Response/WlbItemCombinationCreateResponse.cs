using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// WlbItemCombinationCreateResponse.
    /// </summary>
    public class WlbItemCombinationCreateResponse : TopResponse
    {
        /// <summary>
        /// 组合关系创建时间
        /// </summary>
        [XmlElement("gmt_create")]
        public string GmtCreate { get; set; }
    }
}
