using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ProductsGetResponse.
    /// </summary>
    public class ProductsGetResponse : TopResponse
    {
        /// <summary>
        /// 返回具体信息为入参fields请求的字段信息
        /// </summary>
        [XmlArray("products")]
        [XmlArrayItem("product")]
        public List<Product> Products { get; set; }
    }
}
