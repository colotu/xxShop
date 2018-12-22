using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SellercenterSubuserPermissionsRolesGetResponse.
    /// </summary>
    public class SellercenterSubuserPermissionsRolesGetResponse : TopResponse
    {
        /// <summary>
        /// 子账号被所拥有的权限
        /// </summary>
        [XmlElement("subuser_permission")]
        public SubUserPermission SubuserPermission { get; set; }
    }
}
