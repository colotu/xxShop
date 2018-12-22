using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SkusCustomGetResponse.
    /// </summary>
    public class SkusCustomGetResponse : TopResponse
    {
        /// <summary>
        /// Sku对象，具体字段以fields决定
        /// </summary>
        [XmlArray("skus")]
        [XmlArrayItem("sku")]
        public List<Sku> Skus { get; set; }
    }
}
