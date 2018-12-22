using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// PictureUploadResponse.
    /// </summary>
    public class PictureUploadResponse : TopResponse
    {
        /// <summary>
        /// 当前上传的一张图片信息
        /// </summary>
        [XmlElement("picture")]
        public Picture Picture { get; set; }
    }
}
