using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// BillBillGetResponse.
    /// </summary>
    public class BillBillGetResponse : TopResponse
    {
        /// <summary>
        /// 账单明细
        /// </summary>
        [XmlElement("bill")]
        public Bill Bill { get; set; }
    }
}
