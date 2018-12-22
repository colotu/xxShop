using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// CrmGradeGetResponse.
    /// </summary>
    public class CrmGradeGetResponse : TopResponse
    {
        /// <summary>
        /// 等级信息集合
        /// </summary>
        [XmlArray("grade_promotions")]
        [XmlArrayItem("grade_promotion")]
        public List<GradePromotion> GradePromotions { get; set; }
    }
}
