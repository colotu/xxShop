using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// TripJipiaoAgentItinerarySendResponse.
    /// </summary>
    public class TripJipiaoAgentItinerarySendResponse : TopResponse
    {
        /// <summary>
        /// 成功或者失败
        /// </summary>
        [XmlElement("is_success")]
        public bool IsSuccess { get; set; }
    }
}
