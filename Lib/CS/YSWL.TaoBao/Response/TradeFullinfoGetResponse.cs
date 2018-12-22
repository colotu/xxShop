using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// TradeFullinfoGetResponse.
    /// </summary>
    public class TradeFullinfoGetResponse : TopResponse
    {
        /// <summary>
        /// 搜索到的交易信息列表，返回的Trade和Order中包含的具体信息为入参fields请求的字段信息
        /// </summary>
        [XmlElement("trade")]
        public Trade Trade { get; set; }
    }
}
