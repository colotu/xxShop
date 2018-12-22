using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// TmallBrandcatSalesproGetResponse.
    /// </summary>
    public class TmallBrandcatSalesproGetResponse : TopResponse
    {
        /// <summary>
        /// 获取被管控品牌和类目下的销售属性，注意，一个管控品牌和类目下可能有多个销售属性。
        /// </summary>
        [XmlArray("cat_brand_sale_props")]
        [XmlArrayItem("cat_brand_sale_prop")]
        public List<CatBrandSaleProp> CatBrandSaleProps { get; set; }
    }
}
