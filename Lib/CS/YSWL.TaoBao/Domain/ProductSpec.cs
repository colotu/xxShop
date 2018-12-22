using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace YSWL.TaoBao.Domain
{
    /// <summary>
    /// ProductSpec Data Structure.
    /// </summary>
    [Serializable]
    public class ProductSpec : TopObject
    {
        /// <summary>
        /// 产品规格条形码，支持EAN-13格式。
        /// </summary>
        [XmlElement("barcode")]
        public string Barcode { get; set; }

        /// <summary>
        /// 认证图片列表
        /// </summary>
        [XmlArray("certified_pics")]
        [XmlArrayItem("cert_pic_info")]
        public List<CertPicInfo> CertifiedPics { get; set; }

        /// <summary>
        /// 产品的主图片地址。绝对地址，格式：http://host/image_path。
        /// </summary>
        [XmlElement("pic_url")]
        public string PicUrl { get; set; }

        /// <summary>
        /// 产品ID。
        /// </summary>
        [XmlElement("product_id")]
        public long ProductId { get; set; }

        /// <summary>
        /// 产品规格ID。
        /// </summary>
        [XmlElement("spec_id")]
        public long SpecId { get; set; }

        /// <summary>
        /// 产品规格的销售属性组合。格式为：pid:vid;pid:vid。其中：pid是销售属性id，vid是销售属性值id。如果该类目品牌下面没有销售属性，可以不用填写。销售属性通过tmall.brandcat.salespro.get接口获取。
        /// </summary>
        [XmlElement("spec_props")]
        public string SpecProps { get; set; }

        /// <summary>
        /// 销售属性值别名。格式为：pid1:vid1:别名1;pid2:vid2:别名2。其中：pid是销售属性id，vid是销售属性值id。别名长度不可以超过30个字符。目前只有颜色销售属性支持别名。
        /// </summary>
        [XmlElement("spec_props_alias")]
        public string SpecPropsAlias { get; set; }

        /// <summary>
        /// 1:表示可以使用的数据，也就是审核通过的。  3：表示等待小儿审核的产品规格，这个数据暂时还不能使用，要等待审核通过后，才能使用。
        /// </summary>
        [XmlElement("status")]
        public long Status { get; set; }
    }
}
