using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ItemSkuDeleteResponse.
    /// </summary>
    public class ItemSkuDeleteResponse : TopResponse
    {
        /// <summary>
        /// Sku结构
        /// </summary>
        [XmlElement("sku")]
        public Sku Sku { get; set; }
    }
}
