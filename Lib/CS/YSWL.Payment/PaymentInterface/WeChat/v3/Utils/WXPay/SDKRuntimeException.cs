using System;

namespace YSWL.Payment.PaymentInterface.WeChat.v3.Utils.WXPay
{
    [Serializable]
    public class SDKRuntimeException : Exception
    {

        private const long serialVersionUID = 1L;

        public SDKRuntimeException(String str)
            : base(str)
        {

        }
    }
}
