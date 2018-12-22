using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// TradeMemoUpdateResponse.
    /// </summary>
    public class TradeMemoUpdateResponse : TopResponse
    {
        /// <summary>
        /// 更新交易的备注信息后返回的Trade，其中可用字段为tid和modified
        /// </summary>
        [XmlElement("trade")]
        public Trade Trade { get; set; }
    }
}
