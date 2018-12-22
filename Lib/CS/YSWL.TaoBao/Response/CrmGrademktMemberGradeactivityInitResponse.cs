using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// CrmGrademktMemberGradeactivityInitResponse.
    /// </summary>
    public class CrmGrademktMemberGradeactivityInitResponse : TopResponse
    {
        /// <summary>
        /// json格式
        /// </summary>
        [XmlElement("module")]
        public bool Module { get; set; }
    }
}
