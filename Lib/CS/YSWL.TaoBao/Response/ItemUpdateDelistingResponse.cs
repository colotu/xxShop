using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ItemUpdateDelistingResponse.
    /// </summary>
    public class ItemUpdateDelistingResponse : TopResponse
    {
        /// <summary>
        /// 返回商品更新信息：返回的结果是:num_iid和modified
        /// </summary>
        [XmlElement("item")]
        public Item Item { get; set; }
    }
}
