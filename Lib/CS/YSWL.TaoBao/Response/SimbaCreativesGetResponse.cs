using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaCreativesGetResponse.
    /// </summary>
    public class SimbaCreativesGetResponse : TopResponse
    {
        /// <summary>
        /// 创意对象列表
        /// </summary>
        [XmlArray("creatives")]
        [XmlArrayItem("creative")]
        public List<Creative> Creatives { get; set; }
    }
}
