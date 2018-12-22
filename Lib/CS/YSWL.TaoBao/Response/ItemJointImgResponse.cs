using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ItemJointImgResponse.
    /// </summary>
    public class ItemJointImgResponse : TopResponse
    {
        /// <summary>
        /// 商品图片信息
        /// </summary>
        [XmlElement("item_img")]
        public ItemImg ItemImg { get; set; }
    }
}
