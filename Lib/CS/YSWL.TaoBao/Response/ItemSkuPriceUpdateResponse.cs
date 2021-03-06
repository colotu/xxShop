using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ItemSkuPriceUpdateResponse.
    /// </summary>
    public class ItemSkuPriceUpdateResponse : TopResponse
    {
        /// <summary>
        /// 商品SKU信息（只包含num_iid和modified）
        /// </summary>
        [XmlElement("sku")]
        public Sku Sku { get; set; }
    }
}
