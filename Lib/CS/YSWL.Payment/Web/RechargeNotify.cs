using YSWL.Payment.Model;

namespace YSWL.Payment.Web
{
    [System.Obsolete]
    public abstract class RechargeNotify<T> : RechargeReturnTemplatedPage<T> where T : class,IRechargeRequest, new()
    {
        public RechargeNotify() : base(true) { }

        protected override void DisplayMessage(string status) { }
    }
}

