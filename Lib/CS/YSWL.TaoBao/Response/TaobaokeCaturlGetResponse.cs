using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// TaobaokeCaturlGetResponse.
    /// </summary>
    public class TaobaokeCaturlGetResponse : TopResponse
    {
        /// <summary>
        /// 只返回taobaoke_cat_click_url
        /// </summary>
        [XmlElement("taobaoke_item")]
        public TaobaokeItem TaobaokeItem { get; set; }
    }
}
