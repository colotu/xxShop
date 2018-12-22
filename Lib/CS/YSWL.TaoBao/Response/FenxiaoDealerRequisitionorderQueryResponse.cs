using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// FenxiaoDealerRequisitionorderQueryResponse.
    /// </summary>
    public class FenxiaoDealerRequisitionorderQueryResponse : TopResponse
    {
        /// <summary>
        /// 经销采购单结果列表
        /// </summary>
        [XmlArray("dealer_orders")]
        [XmlArrayItem("dealer_order")]
        public List<DealerOrder> DealerOrders { get; set; }
    }
}
