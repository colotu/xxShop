using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// TbkMobileShopsConvertResponse.
    /// </summary>
    public class TbkMobileShopsConvertResponse : TopResponse
    {
        /// <summary>
        /// 淘宝客店铺对象列表
        /// </summary>
        [XmlArray("tbk_shops")]
        [XmlArrayItem("tbk_shop")]
        public List<TbkShop> TbkShops { get; set; }
    }
}
