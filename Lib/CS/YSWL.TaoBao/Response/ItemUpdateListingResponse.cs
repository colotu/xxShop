using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ItemUpdateListingResponse.
    /// </summary>
    public class ItemUpdateListingResponse : TopResponse
    {
        /// <summary>
        /// 上架后返回的商品信息：返回的结果就是:num_iid和modified
        /// </summary>
        [XmlElement("item")]
        public Item Item { get; set; }
    }
}
