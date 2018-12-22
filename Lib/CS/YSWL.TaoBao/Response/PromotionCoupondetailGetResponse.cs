using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// PromotionCoupondetailGetResponse.
    /// </summary>
    public class PromotionCoupondetailGetResponse : TopResponse
    {
        /// <summary>
        /// 优惠券详细信息
        /// </summary>
        [XmlArray("coupon_details")]
        [XmlArrayItem("coupon_detail")]
        public List<CouponDetail> CouponDetails { get; set; }

        /// <summary>
        /// 查询数量总数
        /// </summary>
        [XmlElement("total_results")]
        public long TotalResults { get; set; }
    }
}
