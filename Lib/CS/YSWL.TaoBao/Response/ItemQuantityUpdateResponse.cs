using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ItemQuantityUpdateResponse.
    /// </summary>
    public class ItemQuantityUpdateResponse : TopResponse
    {
        /// <summary>
        /// iid、numIid、num和modified，skus中每个sku的skuId、quantity和modified
        /// </summary>
        [XmlElement("item")]
        public Item Item { get; set; }
    }
}
