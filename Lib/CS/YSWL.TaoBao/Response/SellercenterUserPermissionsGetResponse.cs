using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SellercenterUserPermissionsGetResponse.
    /// </summary>
    public class SellercenterUserPermissionsGetResponse : TopResponse
    {
        /// <summary>
        /// 权限列表
        /// </summary>
        [XmlArray("permissions")]
        [XmlArrayItem("permission")]
        public List<Permission> Permissions { get; set; }
    }
}
