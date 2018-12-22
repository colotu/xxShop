using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ShopRemainshowcaseGetResponse.
    /// </summary>
    public class ShopRemainshowcaseGetResponse : TopResponse
    {
        /// <summary>
        /// 支持返回剩余橱窗数量，已用橱窗数量，总橱窗数量
        /// </summary>
        [XmlElement("shop")]
        public Shop Shop { get; set; }
    }
}
