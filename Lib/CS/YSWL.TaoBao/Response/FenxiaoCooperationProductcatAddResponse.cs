using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// FenxiaoCooperationProductcatAddResponse.
    /// </summary>
    public class FenxiaoCooperationProductcatAddResponse : TopResponse
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        [XmlElement("is_success")]
        public bool IsSuccess { get; set; }
    }
}
