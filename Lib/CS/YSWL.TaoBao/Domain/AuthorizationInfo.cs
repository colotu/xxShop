using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Domain
{
    /// <summary>
    /// AuthorizationInfo Data Structure.
    /// </summary>
    [Serializable]
    public class AuthorizationInfo : TopObject
    {
        /// <summary>
        /// 访问码
        /// </summary>
        [XmlElement("authorizationcode")]
        public string Authorizationcode { get; set; }

        /// <summary>
        /// 用户数字ＩＤ
        /// </summary>
        [XmlElement("uid")]
        public long Uid { get; set; }
    }
}
