using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// LogisticsAddressModifyResponse.
    /// </summary>
    public class LogisticsAddressModifyResponse : TopResponse
    {
        /// <summary>
        /// 只返回修改时间modify_date
        /// </summary>
        [XmlElement("address_result")]
        public AddressResult AddressResult { get; set; }
    }
}
