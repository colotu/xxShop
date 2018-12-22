using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// UserBuyerGetResponse.
    /// </summary>
    public class UserBuyerGetResponse : TopResponse
    {
        /// <summary>
        /// 只返回user_id,nick,sex,buyer_credit,avatar,has_shop,vip_info参数
        /// </summary>
        [XmlElement("user")]
        public User User { get; set; }
    }
}
