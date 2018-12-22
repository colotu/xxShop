using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// CrmGrademktMemberQueryResponse.
    /// </summary>
    public class CrmGrademktMemberQueryResponse : TopResponse
    {
        /// <summary>
        /// json格式
        /// </summary>
        [XmlElement("module")]
        public string Module { get; set; }
    }
}
