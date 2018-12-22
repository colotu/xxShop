using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// UmpRangeDeleteResponse.
    /// </summary>
    public class UmpRangeDeleteResponse : TopResponse
    {
        /// <summary>
        /// 调用是否成功
        /// </summary>
        [XmlElement("is_success")]
        public bool IsSuccess { get; set; }
    }
}
