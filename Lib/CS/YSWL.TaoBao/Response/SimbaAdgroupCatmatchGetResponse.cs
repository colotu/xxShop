using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaAdgroupCatmatchGetResponse.
    /// </summary>
    public class SimbaAdgroupCatmatchGetResponse : TopResponse
    {
        /// <summary>
        /// 推广组的类目出价对象
        /// </summary>
        [XmlElement("adgroupcatmatch")]
        public ADGroupCatmatch Adgroupcatmatch { get; set; }
    }
}
