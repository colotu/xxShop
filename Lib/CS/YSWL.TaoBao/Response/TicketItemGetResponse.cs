using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// TicketItemGetResponse.
    /// </summary>
    public class TicketItemGetResponse : TopResponse
    {
        /// <summary>
        /// 参见TicketItem数据结构文档
        /// </summary>
        [XmlElement("ticket_item")]
        public TicketItem TicketItem { get; set; }
    }
}
