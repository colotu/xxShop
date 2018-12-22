using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// FenxiaoProductSkusGetResponse.
    /// </summary>
    public class FenxiaoProductSkusGetResponse : TopResponse
    {
        /// <summary>
        /// sku信息
        /// </summary>
        [XmlArray("skus")]
        [XmlArrayItem("fenxiao_sku")]
        public List<FenxiaoSku> Skus { get; set; }

        /// <summary>
        /// 记录数量
        /// </summary>
        [XmlElement("total_results")]
        public long TotalResults { get; set; }
    }
}
