using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// LogisticsAddressSearchResponse.
    /// </summary>
    public class LogisticsAddressSearchResponse : TopResponse
    {
        /// <summary>
        /// 一组地址库数据，
        /// </summary>
        [XmlArray("addresses")]
        [XmlArrayItem("address_result")]
        public List<AddressResult> Addresses { get; set; }
    }
}
