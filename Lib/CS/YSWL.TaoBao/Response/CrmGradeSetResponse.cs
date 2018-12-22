using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// CrmGradeSetResponse.
    /// </summary>
    public class CrmGradeSetResponse : TopResponse
    {
        /// <summary>
        /// true：成功 false：失败
        /// </summary>
        [XmlElement("is_success")]
        public bool IsSuccess { get; set; }
    }
}
