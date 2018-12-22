using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// TaobaokeListurlGetResponse.
    /// </summary>
    public class TaobaokeListurlGetResponse : TopResponse
    {
        /// <summary>
        /// 只返回keyword_click_url
        /// </summary>
        [XmlElement("taobaoke_item")]
        public TaobaokeItem TaobaokeItem { get; set; }
    }
}
