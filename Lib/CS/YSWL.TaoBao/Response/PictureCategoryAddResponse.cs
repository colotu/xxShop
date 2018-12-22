using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// PictureCategoryAddResponse.
    /// </summary>
    public class PictureCategoryAddResponse : TopResponse
    {
        /// <summary>
        /// 图片分类信息
        /// </summary>
        [XmlElement("picture_category")]
        public PictureCategory PictureCategory { get; set; }
    }
}
