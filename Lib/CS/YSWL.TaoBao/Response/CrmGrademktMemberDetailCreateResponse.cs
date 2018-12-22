using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// CrmGrademktMemberDetailCreateResponse.
    /// </summary>
    public class CrmGrademktMemberDetailCreateResponse : TopResponse
    {
        /// <summary>
        /// json格式
        /// </summary>
        [XmlElement("module")]
        public bool Module { get; set; }
    }
}
