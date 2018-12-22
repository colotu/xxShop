using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// FenxiaoGradeAddResponse.
    /// </summary>
    public class FenxiaoGradeAddResponse : TopResponse
    {
        /// <summary>
        /// 等级ID
        /// </summary>
        [XmlElement("grade_id")]
        public long GradeId { get; set; }

        /// <summary>
        /// 操作是否成功
        /// </summary>
        [XmlElement("is_success")]
        public bool IsSuccess { get; set; }
    }
}
