using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ProductImgDeleteResponse.
    /// </summary>
    public class ProductImgDeleteResponse : TopResponse
    {
        /// <summary>
        /// 返回productimg中的：id,product_id
        /// </summary>
        [XmlElement("product_img")]
        public ProductImg ProductImg { get; set; }
    }
}
