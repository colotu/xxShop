using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// TicketItemsGetResponse.
    /// </summary>
    public class TicketItemsGetResponse : TopResponse
    {
        /// <summary>
        /// 商品信息
        /// </summary>
        [XmlArray("ticket_items")]
        [XmlArrayItem("ticket_item")]
        public List<TicketItem> TicketItems { get; set; }
    }
}
