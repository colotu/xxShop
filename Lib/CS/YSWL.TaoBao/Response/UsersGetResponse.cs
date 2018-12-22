using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// UsersGetResponse.
    /// </summary>
    public class UsersGetResponse : TopResponse
    {
        /// <summary>
        /// 用户信息列表
        /// </summary>
        [XmlArray("users")]
        [XmlArrayItem("user")]
        public List<User> Users { get; set; }
    }
}
