using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// ItemJointPropimgResponse.
    /// </summary>
    public class ItemJointPropimgResponse : TopResponse
    {
        /// <summary>
        /// 属性图片对象信息
        /// </summary>
        [XmlElement("prop_img")]
        public PropImg PropImg { get; set; }
    }
}
