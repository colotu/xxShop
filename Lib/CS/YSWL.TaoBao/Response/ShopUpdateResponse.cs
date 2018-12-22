using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ShopUpdateResponse.
    /// </summary>
    public class ShopUpdateResponse : TopResponse
    {
        /// <summary>
        /// 店铺信息
        /// </summary>
        [XmlElement("shop")]
        public Shop Shop { get; set; }
    }
}
