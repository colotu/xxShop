using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ProductUpdateResponse.
    /// </summary>
    public class ProductUpdateResponse : TopResponse
    {
        /// <summary>
        /// 返回product数据结构中的：product_id,modified
        /// </summary>
        [XmlElement("product")]
        public Product Product { get; set; }
    }
}
