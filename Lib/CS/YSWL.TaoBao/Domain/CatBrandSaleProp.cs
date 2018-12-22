using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Domain
{
    /// <summary>
    /// CatBrandSaleProp Data Structure.
    /// </summary>
    [Serializable]
    public class CatBrandSaleProp : TopObject
    {
        /// <summary>
        /// 被管控的品牌的Id
        /// </summary>
        [XmlElement("brand_id")]
        public long BrandId { get; set; }

        /// <summary>
        /// 被管控的类目ID
        /// </summary>
        [XmlElement("cat_id")]
        public long CatId { get; set; }

        /// <summary>
        /// true表示：不是产品的规格属性  false表示：是产品的规格属性。
        /// </summary>
        [XmlElement("is_not_spec")]
        public bool IsNotSpec { get; set; }

        /// <summary>
        /// 被管控的销售属性ID
        /// </summary>
        [XmlElement("property_id")]
        public long PropertyId { get; set; }
    }
}
