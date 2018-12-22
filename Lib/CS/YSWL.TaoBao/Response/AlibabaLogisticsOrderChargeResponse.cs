using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// AlibabaLogisticsOrderChargeResponse.
    /// </summary>
    public class AlibabaLogisticsOrderChargeResponse : TopResponse
    {
        /// <summary>
        /// 订单费用
        /// </summary>
        [XmlElement("order_charge")]
        public OrderCharge OrderCharge { get; set; }
    }
}
