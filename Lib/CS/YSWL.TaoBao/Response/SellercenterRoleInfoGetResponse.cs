using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SellercenterRoleInfoGetResponse.
    /// </summary>
    public class SellercenterRoleInfoGetResponse : TopResponse
    {
        /// <summary>
        /// 角色具体信息
        /// </summary>
        [XmlElement("role")]
        public Role Role { get; set; }
    }
}
