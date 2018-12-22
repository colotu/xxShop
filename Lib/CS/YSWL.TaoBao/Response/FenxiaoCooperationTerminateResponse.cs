using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// FenxiaoCooperationTerminateResponse.
    /// </summary>
    public class FenxiaoCooperationTerminateResponse : TopResponse
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        [XmlElement("is_success")]
        public bool IsSuccess { get; set; }
    }
}
