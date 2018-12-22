using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SellercenterRoleAddResponse.
    /// </summary>
    public class SellercenterRoleAddResponse : TopResponse
    {
        /// <summary>
        /// 子账号角色
        /// </summary>
        [XmlElement("role")]
        public Role Role { get; set; }
    }
}
