using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ItemrecommendItemsGetResponse.
    /// </summary>
    public class ItemrecommendItemsGetResponse : TopResponse
    {
        /// <summary>
        /// 返回的推荐商品列表结果集
        /// </summary>
        [XmlArray("values")]
        [XmlArrayItem("favorite_item")]
        public List<FavoriteItem> Values { get; set; }
    }
}
