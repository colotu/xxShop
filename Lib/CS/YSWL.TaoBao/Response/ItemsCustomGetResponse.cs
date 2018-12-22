using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ItemsCustomGetResponse.
    /// </summary>
    public class ItemsCustomGetResponse : TopResponse
    {
        /// <summary>
        /// 商品列表，具体返回字段以fields决定
        /// </summary>
        [XmlArray("items")]
        [XmlArrayItem("item")]
        public List<Item> Items { get; set; }
    }
}
