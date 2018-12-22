using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ItemPropimgDeleteResponse.
    /// </summary>
    public class ItemPropimgDeleteResponse : TopResponse
    {
        /// <summary>
        /// 属性图片结构
        /// </summary>
        [XmlElement("prop_img")]
        public PropImg PropImg { get; set; }
    }
}
