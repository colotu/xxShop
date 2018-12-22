using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ItemSkusGetResponse.
    /// </summary>
    public class ItemSkusGetResponse : TopResponse
    {
        /// <summary>
        /// Sku列表
        /// </summary>
        [XmlArray("skus")]
        [XmlArrayItem("sku")]
        public List<Sku> Skus { get; set; }
    }
}
