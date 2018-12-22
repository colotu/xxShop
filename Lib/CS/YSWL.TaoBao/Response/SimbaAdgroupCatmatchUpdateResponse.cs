using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// SimbaAdgroupCatmatchUpdateResponse.
    /// </summary>
    public class SimbaAdgroupCatmatchUpdateResponse : TopResponse
    {
        /// <summary>
        /// 推广组的类目出价对象
        /// </summary>
        [XmlElement("adgroupcatmatch")]
        public ADGroupCatmatch Adgroupcatmatch { get; set; }
    }
}
