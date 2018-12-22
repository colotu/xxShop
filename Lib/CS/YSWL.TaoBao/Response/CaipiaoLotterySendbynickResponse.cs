using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// CaipiaoLotterySendbynickResponse.
    /// </summary>
    public class CaipiaoLotterySendbynickResponse : TopResponse
    {
        /// <summary>
        /// 赠送是否成功，成功为true, 否则为false
        /// </summary>
        [XmlElement("send_result")]
        public bool SendResult { get; set; }
    }
}
