using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// WlbWlborderGetResponse.
    /// </summary>
    public class WlbWlborderGetResponse : TopResponse
    {
        /// <summary>
        /// 物流宝订单详情
        /// </summary>
        [XmlElement("wlb_order")]
        public WlbOrder WlbOrder { get; set; }
    }
}
