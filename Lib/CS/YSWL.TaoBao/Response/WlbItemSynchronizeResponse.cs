using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// WlbItemSynchronizeResponse.
    /// </summary>
    public class WlbItemSynchronizeResponse : TopResponse
    {
        /// <summary>
        /// 修改时间
        /// </summary>
        [XmlElement("gmt_modified")]
        public string GmtModified { get; set; }
    }
}
