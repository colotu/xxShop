using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// VasSubscSearchResponse.
    /// </summary>
    public class VasSubscSearchResponse : TopResponse
    {
        /// <summary>
        /// 订购关系对象
        /// </summary>
        [XmlArray("article_subs")]
        [XmlArrayItem("article_sub")]
        public List<ArticleSub> ArticleSubs { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        [XmlElement("total_item")]
        public long TotalItem { get; set; }
    }
}
