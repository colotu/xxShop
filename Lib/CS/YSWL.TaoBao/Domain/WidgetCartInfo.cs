using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Domain
{
    /// <summary>
    /// WidgetCartInfo Data Structure.
    /// </summary>
    [Serializable]
    public class WidgetCartInfo : TopObject
    {
        /// <summary>
        /// 购物车记录的id，同BaseCartInfo中的cart_id
        /// </summary>
        [XmlElement("cart_id")]
        public long CartId { get; set; }

        /// <summary>
        /// 此条购物车记录的删除链接。服务端签名后的删除链接，isv在使用的时候前面加上“http://gw.api.taobao.com/widget?”即可生成完整的购物车记录删除链接。
        /// </summary>
        [XmlElement("delete_url")]
        public string DeleteUrl { get; set; }

        /// <summary>
        /// 购买的商品的商品数字id，同BaseCartInfo中的item_id,和Item中的num_iid
        /// </summary>
        [XmlElement("item_id")]
        public long ItemId { get; set; }

        /// <summary>
        /// 商品详情页连接地址，同Item的detail_url字段
        /// </summary>
        [XmlElement("item_url")]
        public string ItemUrl { get; set; }

        /// <summary>
        /// 购买的商品的图片地址，如果选择了sku并且sku有单独的图片，此地址为sku的图片地址
        /// </summary>
        [XmlElement("pic_url")]
        public string PicUrl { get; set; }

        /// <summary>
        /// 购买商品的价格，如果无sku，此价格为商品的当前价格。如果有sku，此价格为购买的sku的当前价格。如果此sku已经不存在，显示商品的价格
        /// </summary>
        [XmlElement("price")]
        public string Price { get; set; }

        /// <summary>
        /// 购买数量，同BaseCartInfo的quantity
        /// </summary>
        [XmlElement("quantity")]
        public long Quantity { get; set; }

        /// <summary>
        /// 商品的卖家昵称
        /// </summary>
        [XmlElement("seller_nick")]
        public string SellerNick { get; set; }

        /// <summary>
        /// sku的属性列表。如果购买的商品无sku，此字段为空。如果有sku，次字段返回sku的属性描述（属性名:属性值别名/属性值名，别名优先）。
        /// </summary>
        [XmlElement("sku")]
        public string Sku { get; set; }

        /// <summary>
        /// 购买的商品的title，同Item的title
        /// </summary>
        [XmlElement("title")]
        public string Title { get; set; }
    }
}
