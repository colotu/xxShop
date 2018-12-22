using System;
using System.Xml.Serialization;
using YSWL.TaoBao.Domain;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// JipiaoPolicyGetResponse.
    /// </summary>
    public class JipiaoPolicyGetResponse : TopResponse
    {
        /// <summary>
        /// 机票政策查询返回结果对象
        /// </summary>
        [XmlElement("policy")]
        public Policy Policy { get; set; }
    }
}
