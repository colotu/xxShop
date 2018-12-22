using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// AftersaleGetResponse.
    /// </summary>
    public class AftersaleGetResponse : TopResponse
    {
        /// <summary>
        /// 售后服务返回对象
        /// </summary>
        [XmlArray("after_sales")]
        [XmlArrayItem("after_sale")]
        public List<AfterSale> AfterSales { get; set; }
    }
}
