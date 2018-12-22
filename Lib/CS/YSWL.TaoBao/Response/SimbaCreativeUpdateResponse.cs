using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaCreativeUpdateResponse.
    /// </summary>
    public class SimbaCreativeUpdateResponse : TopResponse
    {
        /// <summary>
        /// 创意修改记录对象
        /// </summary>
        [XmlElement("creativerecord")]
        public CreativeRecord Creativerecord { get; set; }
    }
}
