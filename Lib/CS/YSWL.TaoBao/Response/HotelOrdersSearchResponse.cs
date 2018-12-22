using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// HotelOrdersSearchResponse.
    /// </summary>
    public class HotelOrdersSearchResponse : TopResponse
    {
        /// <summary>
        /// 多个订单结构，是否返回入住人列表根据参数决定
        /// </summary>
        [XmlArray("hotel_orders")]
        [XmlArrayItem("hotel_order")]
        public List<HotelOrder> HotelOrders { get; set; }

        /// <summary>
        /// 符合条件的结果总数
        /// </summary>
        [XmlElement("total_results")]
        public long TotalResults { get; set; }
    }
}
