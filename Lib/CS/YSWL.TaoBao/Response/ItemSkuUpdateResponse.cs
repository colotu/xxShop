using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ItemSkuUpdateResponse.
    /// </summary>
    public class ItemSkuUpdateResponse : TopResponse
    {
        /// <summary>
        /// 商品Sku
        /// </summary>
        [XmlElement("sku")]
        public Sku Sku { get; set; }
    }
}
