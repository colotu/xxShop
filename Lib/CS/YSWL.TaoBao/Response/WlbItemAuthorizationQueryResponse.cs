using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// WlbItemAuthorizationQueryResponse.
    /// </summary>
    public class WlbItemAuthorizationQueryResponse : TopResponse
    {
        /// <summary>
        /// 授权关系列表
        /// </summary>
        [XmlArray("authorization_list")]
        [XmlArrayItem("wlb_authorization")]
        public List<WlbAuthorization> AuthorizationList { get; set; }

        /// <summary>
        /// 结果总数
        /// </summary>
        [XmlElement("total_count")]
        public long TotalCount { get; set; }
    }
}
