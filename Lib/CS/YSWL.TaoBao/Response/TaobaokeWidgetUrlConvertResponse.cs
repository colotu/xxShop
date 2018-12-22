using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// TaobaokeWidgetUrlConvertResponse.
    /// </summary>
    public class TaobaokeWidgetUrlConvertResponse : TopResponse
    {
        /// <summary>
        /// 只返回click_url
        /// </summary>
        [XmlElement("taobaoke_item")]
        public TaobaokeItem TaobaokeItem { get; set; }
    }
}
