using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// UmpPromotionGetResponse.
    /// </summary>
    public class UmpPromotionGetResponse : TopResponse
    {
        /// <summary>
        /// 优惠详细信息
        /// </summary>
        [XmlElement("promotions")]
        public PromotionDisplayTop Promotions { get; set; }
    }
}
