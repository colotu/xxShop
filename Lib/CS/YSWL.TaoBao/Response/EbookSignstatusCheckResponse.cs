using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// EbookSignstatusCheckResponse.
    /// </summary>
    public class EbookSignstatusCheckResponse : TopResponse
    {
        /// <summary>
        /// 签约状态
        /// </summary>
        [XmlElement("ebook_sign_status")]
        public EbookSignStatus EbookSignStatus { get; set; }
    }
}
