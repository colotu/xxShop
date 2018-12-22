namespace YSWL.Payment.PaymentInterface.WeChat.v3.Models
{
    public class PayModel
    {
        public string AppId { get; set; }
        public string Package { get; set; }
        public string Timestamp { get; set; }
        public string Noncestr { get; set; }
        public string PaySign { get; set; }
        public string SignType { get { return "MD5"; } }
    }
}