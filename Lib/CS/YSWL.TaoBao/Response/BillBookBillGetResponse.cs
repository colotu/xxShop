using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// BillBookBillGetResponse.
    /// </summary>
    public class BillBookBillGetResponse : TopResponse
    {
        /// <summary>
        /// 虚拟账户账单
        /// </summary>
        [XmlElement("bookbill")]
        public BookBill Bookbill { get; set; }
    }
}
