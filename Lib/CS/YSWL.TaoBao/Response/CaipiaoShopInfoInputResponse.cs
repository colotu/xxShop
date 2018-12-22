using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// CaipiaoShopInfoInputResponse.
    /// </summary>
    public class CaipiaoShopInfoInputResponse : TopResponse
    {
        /// <summary>
        /// 录入操作是否成功
        /// </summary>
        [XmlElement("input_result")]
        public bool InputResult { get; set; }
    }
}
