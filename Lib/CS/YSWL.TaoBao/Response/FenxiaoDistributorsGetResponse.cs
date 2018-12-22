using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// FenxiaoDistributorsGetResponse.
    /// </summary>
    public class FenxiaoDistributorsGetResponse : TopResponse
    {
        /// <summary>
        /// 分销商详细信息
        /// </summary>
        [XmlArray("distributors")]
        [XmlArrayItem("distributor")]
        public List<Distributor> Distributors { get; set; }
    }
}
