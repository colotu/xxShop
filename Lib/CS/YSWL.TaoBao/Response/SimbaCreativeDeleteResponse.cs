using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaCreativeDeleteResponse.
    /// </summary>
    public class SimbaCreativeDeleteResponse : TopResponse
    {
        /// <summary>
        /// 被删除的创意对象
        /// </summary>
        [XmlElement("creative")]
        public Creative Creative { get; set; }
    }
}
