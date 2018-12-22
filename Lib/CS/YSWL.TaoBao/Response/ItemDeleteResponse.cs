using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ItemDeleteResponse.
    /// </summary>
    public class ItemDeleteResponse : TopResponse
    {
        /// <summary>
        /// 被删除商品的相关信息
        /// </summary>
        [XmlElement("item")]
        public Item Item { get; set; }
    }
}
