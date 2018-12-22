using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaKeywordsAddResponse.
    /// </summary>
    public class SimbaKeywordsAddResponse : TopResponse
    {
        /// <summary>
        /// 关键词列表
        /// </summary>
        [XmlArray("keywords")]
        [XmlArrayItem("keyword")]
        public List<Keyword> Keywords { get; set; }
    }
}
