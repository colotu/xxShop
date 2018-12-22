using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Domain
{
    /// <summary>
    /// FoodSecurity Data Structure.
    /// </summary>
    [Serializable]
    public class FoodSecurity : TopObject
    {
        /// <summary>
        /// 厂家联系方式
        /// </summary>
        [XmlElement("contact")]
        public string Contact { get; set; }

        /// <summary>
        /// 产品标准号
        /// </summary>
        [XmlElement("design_code")]
        public string DesignCode { get; set; }

        /// <summary>
        /// 厂名
        /// </summary>
        [XmlElement("factory")]
        public string Factory { get; set; }

        /// <summary>
        /// 厂址
        /// </summary>
        [XmlElement("factory_site")]
        public string FactorySite { get; set; }

        /// <summary>
        /// 食品添加剂
        /// </summary>
        [XmlElement("food_additive")]
        public string FoodAdditive { get; set; }

        /// <summary>
        /// 配料表
        /// </summary>
        [XmlElement("mix")]
        public string Mix { get; set; }

        /// <summary>
        /// 保质期
        /// </summary>
        [XmlElement("period")]
        public string Period { get; set; }

        /// <summary>
        /// 储藏方法
        /// </summary>
        [XmlElement("plan_storage")]
        public string PlanStorage { get; set; }

        /// <summary>
        /// 生产许可证号
        /// </summary>
        [XmlElement("prd_license_no")]
        public string PrdLicenseNo { get; set; }

        /// <summary>
        /// 生产结束日期
        /// </summary>
        [XmlElement("product_date_end")]
        public string ProductDateEnd { get; set; }

        /// <summary>
        /// 生产开始日期
        /// </summary>
        [XmlElement("product_date_start")]
        public string ProductDateStart { get; set; }

        /// <summary>
        /// 进货结束日期，要在生产日期之后
        /// </summary>
        [XmlElement("stock_date_end")]
        public string StockDateEnd { get; set; }

        /// <summary>
        /// 进货开始日期，要在生产日期之后
        /// </summary>
        [XmlElement("stock_date_start")]
        public string StockDateStart { get; set; }

        /// <summary>
        /// 供货商
        /// </summary>
        [XmlElement("supplier")]
        public string Supplier { get; set; }
    }
}
