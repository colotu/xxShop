using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ItemRecommendAddResponse.
    /// </summary>
    public class ItemRecommendAddResponse : TopResponse
    {
        /// <summary>
        /// 被推荐的商品的信息
        /// </summary>
        [XmlElement("item")]
        public Item Item { get; set; }
    }
}
