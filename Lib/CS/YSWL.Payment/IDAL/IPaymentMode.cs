using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using YSWL.Payment.Model;

namespace YSWL.Payment.IDAL
{
  public  interface IPaymentMode
    {
        PaymentModeActionStatus CreateUpdateDeletePaymentMode(PaymentModeInfo paymentMode, DataProviderAction action);

        void SortPaymentMode(int modeId, SortAction action);

        string GetWeChatUser(int orderId);

        PaymentModeInfo GetPaymentMode(int modeId);
        PaymentModeInfo GetPaymentMode(string gateway);
        List<PaymentModeInfo> GetPaymentModes(int type,string keyword);
        PaymentModeInfo PopupPayment(IDataRecord reader);

        AccountSummaryInfo PopupAccountSummary(IDataRecord reader);

        RechargeRequestInfo GetRechargeRequest(long rechargeId);
 
        int RemoveRechargeRequest(long rechargeId);
        bool AddBalanceDetail(BalanceDetailInfo balanceDetails);

        RechargeRequestInfo PopulateRechargeRequest(IDataRecord reader);

        CurrencyInfo GetCurrency(string code);
        IList<CurrencyInfo> GetCurrencys();

        int DeleteCurrency(IList<string> code);
        CurrencyInfo PopulateCurrencyFromDataReader(IDataRecord reader);

    }
}
