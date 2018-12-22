using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaKeywordsChangedGetResponse.
    /// </summary>
    public class SimbaKeywordsChangedGetResponse : TopResponse
    {
        /// <summary>
        /// 关键词分页对象
        /// </summary>
        [XmlElement("keywords")]
        public KeywordPage Keywords { get; set; }
    }
}
