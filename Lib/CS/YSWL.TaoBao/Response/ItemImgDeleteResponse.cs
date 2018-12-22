using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ItemImgDeleteResponse.
    /// </summary>
    public class ItemImgDeleteResponse : TopResponse
    {
        /// <summary>
        /// 商品图片结构
        /// </summary>
        [XmlElement("item_img")]
        public ItemImg ItemImg { get; set; }
    }
}
