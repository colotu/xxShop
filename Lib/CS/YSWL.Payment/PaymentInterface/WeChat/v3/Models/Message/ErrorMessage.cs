namespace YSWL.Payment.PaymentInterface.WeChat.v3.Models.Message
{
    public class ErrorMessage
    {
        //{"errcode":40001,"errmsg":"invalid credential"} AppId AppSecret   配置错误，或AccessToken 过期

        public string ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public bool TokenExpired
        {
            get { return ErrCode == "40001"; }
        }
    }
}
