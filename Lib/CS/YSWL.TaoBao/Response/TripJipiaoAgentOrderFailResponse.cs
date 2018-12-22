using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// TripJipiaoAgentOrderFailResponse.
    /// </summary>
    public class TripJipiaoAgentOrderFailResponse : TopResponse
    {
        /// <summary>
        /// 失败订单操作成功失败
        /// </summary>
        [XmlElement("is_success")]
        public bool IsSuccess { get; set; }
    }
}
