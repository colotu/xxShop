using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// TradeSnapshotGetResponse.
    /// </summary>
    public class TradeSnapshotGetResponse : TopResponse
    {
        /// <summary>
        /// 只包含Trade中的tid和snapshot、子订单Order中的oid和snapshot
        /// </summary>
        [XmlElement("trade")]
        public Trade Trade { get; set; }
    }
}
