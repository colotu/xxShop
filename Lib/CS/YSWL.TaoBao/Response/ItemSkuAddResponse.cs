using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ItemSkuAddResponse.
    /// </summary>
    public class ItemSkuAddResponse : TopResponse
    {
        /// <summary>
        /// sku
        /// </summary>
        [XmlElement("sku")]
        public Sku Sku { get; set; }
    }
}
