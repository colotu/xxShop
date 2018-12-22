using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// PromotionMealGetResponse.
    /// </summary>
    public class PromotionMealGetResponse : TopResponse
    {
        /// <summary>
        /// 搭配套餐列表。
        /// </summary>
        [XmlArray("meal_list")]
        [XmlArrayItem("meal")]
        public List<Meal> MealList { get; set; }
    }
}
