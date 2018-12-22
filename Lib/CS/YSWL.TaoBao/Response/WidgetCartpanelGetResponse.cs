using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// WidgetCartpanelGetResponse.
    /// </summary>
    public class WidgetCartpanelGetResponse : TopResponse
    {
        /// <summary>
        /// 当前用户通过当前app加入购物车的商品记录列表。
        /// </summary>
        [XmlArray("cart_info")]
        [XmlArrayItem("widget_cart_info")]
        public List<WidgetCartInfo> CartInfo { get; set; }

        /// <summary>
        /// 当前用户通过当前app加入购物车的商品记录条数。
        /// </summary>
        [XmlElement("total_results")]
        public long TotalResults { get; set; }
    }
}
