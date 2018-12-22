using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ItemPropimgUploadResponse.
    /// </summary>
    public class ItemPropimgUploadResponse : TopResponse
    {
        /// <summary>
        /// PropImg属性图片结构
        /// </summary>
        [XmlElement("prop_img")]
        public PropImg PropImg { get; set; }
    }
}
