using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ItemUpdateResponse.
    /// </summary>
    public class ItemUpdateResponse : TopResponse
    {
        /// <summary>
        /// 商品结构里的num_iid，modified
        /// </summary>
        [XmlElement("item")]
        public Item Item { get; set; }
    }
}
