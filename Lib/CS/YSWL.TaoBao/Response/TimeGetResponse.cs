using System;
using System.Xml.Serialization;

namespace YSWL.TaoBao.Response
{
    /// <summary>
    /// TimeGetResponse.
    /// </summary>
    public class TimeGetResponse : TopResponse
    {
        /// <summary>
        /// 淘宝系统当前时间。格式:yyyy-MM-dd HH:mm:ss
        /// </summary>
        [XmlElement("time")]
        public string Time { get; set; }
    }
}
