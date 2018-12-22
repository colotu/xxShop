using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// PictureReplaceResponse.
    /// </summary>
    public class PictureReplaceResponse : TopResponse
    {
        /// <summary>
        /// 图片替换是否成功
        /// </summary>
        [XmlElement("done")]
        public bool Done { get; set; }
    }
}
