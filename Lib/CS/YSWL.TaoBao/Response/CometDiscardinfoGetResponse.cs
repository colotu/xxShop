using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// CometDiscardinfoGetResponse.
    /// </summary>
    public class CometDiscardinfoGetResponse : TopResponse
    {
        /// <summary>
        /// 丢弃消息的列表
        /// </summary>
        [XmlArray("discard_info_list")]
        [XmlArrayItem("discard_info")]
        public List<DiscardInfo> DiscardInfoList { get; set; }
    }
}
