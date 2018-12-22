using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// UmpRangeGetResponse.
    /// </summary>
    public class UmpRangeGetResponse : TopResponse
    {
        /// <summary>
        /// 营销范围类列表！
        /// </summary>
        [XmlArray("ranges")]
        [XmlArrayItem("range")]
        public List<Range> Ranges { get; set; }
    }
}
