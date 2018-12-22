using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ShopGetResponse.
    /// </summary>
    public class ShopGetResponse : TopResponse
    {
        /// <summary>
        /// 店铺信息
        /// </summary>
        [XmlElement("shop")]
        public Shop Shop { get; set; }
    }
}
