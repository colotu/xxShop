using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// WlbItemMapGetResponse.
    /// </summary>
    public class WlbItemMapGetResponse : TopResponse
    {
        /// <summary>
        /// 外部商品实体列表
        /// </summary>
        [XmlArray("out_entity_item_list")]
        [XmlArrayItem("out_entity_item")]
        public List<OutEntityItem> OutEntityItemList { get; set; }
    }
}
