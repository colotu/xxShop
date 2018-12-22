using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// WlbItemGetResponse.
    /// </summary>
    public class WlbItemGetResponse : TopResponse
    {
        /// <summary>
        /// 商品信息
        /// </summary>
        [XmlElement("item")]
        public WlbItem Item { get; set; }
    }
}
