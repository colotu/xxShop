using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// WlbItemMapGetByExtentityResponse.
    /// </summary>
    public class WlbItemMapGetByExtentityResponse : TopResponse
    {
        /// <summary>
        /// 物流宝商品id
        /// </summary>
        [XmlElement("item_id")]
        public long ItemId { get; set; }
    }
}
