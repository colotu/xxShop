using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Domain
{
    /// <summary>
    /// SimbaItem Data Structure.
    /// </summary>
    [Serializable]
    public class SimbaItem : TopObject
    {
        /// <summary>
        /// 主人昵称
        /// </summary>
        [XmlElement("nick")]
        public string Nick { get; set; }

        /// <summary>
        /// 商品信息在外部系统(淘宝主站)的数字id
        /// </summary>
        [XmlElement("num_id")]
        public long NumId { get; set; }

        /// <summary>
        /// 商品信息在外部系统（淘宝主站）的价格
        /// </summary>
        [XmlElement("price")]
        public string Price { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        [XmlElement("publish_time")]
        public string PublishTime { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        [XmlElement("quantity")]
        public long Quantity { get; set; }

        /// <summary>
        /// 销量
        /// </summary>
        [XmlElement("sales_count")]
        public long SalesCount { get; set; }

        /// <summary>
        /// 商品信息在外部系统（淘宝主站）的标题
        /// </summary>
        [XmlElement("title")]
        public string Title { get; set; }
    }
}
