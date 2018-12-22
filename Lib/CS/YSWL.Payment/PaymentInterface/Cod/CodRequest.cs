using System.Globalization;
using YSWL.Payment.Core;
using YSWL.Payment.Model;

namespace YSWL.Payment.PaymentInterface.Cod
{
    internal class CodRequest : PaymentRequest
    {
        private string orderId;
        private GatewayInfo getGateway;

        public CodRequest(PayeeInfo payee, GatewayInfo gateway, TradeInfo trade)
        {
            this.orderId = trade.OrderId;
            this.getGateway = gateway;
        }

        public override void SendRequest()
        {
            Configuration.GatewayProvider provider = Configuration.PayConfiguration.GetConfig().Providers["cod"] as Configuration.GatewayProvider;
            if (provider != null)
            {
                this.RedirectToGateway(string.Format(CultureInfo.InvariantCulture, provider.Attributes["urlFormat"], new object[] { this.orderId, this.getGateway.Data }));
            }
        }
    }
}
