using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// UserGetResponse.
    /// </summary>
    public class UserGetResponse : TopResponse
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        [XmlElement("user")]
        public User User { get; set; }
    }
}
