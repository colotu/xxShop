using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaCreativesChangedGetResponse.
    /// </summary>
    public class SimbaCreativesChangedGetResponse : TopResponse
    {
        /// <summary>
        /// 广告创意分页对象
        /// </summary>
        [XmlElement("creatives")]
        public CreativePage Creatives { get; set; }
    }
}
