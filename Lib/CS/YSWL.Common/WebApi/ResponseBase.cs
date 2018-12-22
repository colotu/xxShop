using System.ComponentModel;

namespace YSWL.Common.WebApi
{
    public class ResponseBase
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public ResponseCode ResponseCode { get; set; }
        /// <summary>
        /// 请求信息
        /// </summary>
        public string ResponseMsg { get; set; }
    }

    public enum ResponseCode
    {
        [Description("请求成功")]
        RequestSuccess = 200,
        [Description("请求失败")]
        RequestError = 500
    }
}