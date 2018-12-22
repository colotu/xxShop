using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// PictureUserinfoGetResponse.
    /// </summary>
    public class PictureUserinfoGetResponse : TopResponse
    {
        /// <summary>
        /// 用户使用图片空间的信息
        /// </summary>
        [XmlElement("user_info")]
        public UserInfo UserInfo { get; set; }
    }
}
