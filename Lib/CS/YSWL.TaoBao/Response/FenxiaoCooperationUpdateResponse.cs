using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// FenxiaoCooperationUpdateResponse.
    /// </summary>
    public class FenxiaoCooperationUpdateResponse : TopResponse
    {
        /// <summary>
        /// 更新结果成功失败
        /// </summary>
        [XmlElement("is_success")]
        public bool IsSuccess { get; set; }
    }
}
