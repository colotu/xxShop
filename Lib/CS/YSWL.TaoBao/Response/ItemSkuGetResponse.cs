using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ItemSkuGetResponse.
    /// </summary>
    public class ItemSkuGetResponse : TopResponse
    {
        /// <summary>
        /// Sku
        /// </summary>
        [XmlElement("sku")]
        public Sku Sku { get; set; }
    }
}
