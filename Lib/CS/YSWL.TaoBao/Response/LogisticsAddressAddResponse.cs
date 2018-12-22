using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// LogisticsAddressAddResponse.
    /// </summary>
    public class LogisticsAddressAddResponse : TopResponse
    {
        /// <summary>
        /// 只返回修改日期modify_date
        /// </summary>
        [XmlElement("address_result")]
        public AddressResult AddressResult { get; set; }
    }
}
