using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Domain
{
    /// <summary>
    /// CertPicInfo Data Structure.
    /// </summary>
    [Serializable]
    public class CertPicInfo : TopObject
    {
        /// <summary>
        /// 认证图片类型的数值id
        /// </summary>
        [XmlElement("cert_type")]
        public long CertType { get; set; }

        /// <summary>
        /// 认证图片的url地址
        /// </summary>
        [XmlElement("pic_url")]
        public string PicUrl { get; set; }
    }
}
