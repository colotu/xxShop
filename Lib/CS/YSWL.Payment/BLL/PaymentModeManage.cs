/**
* PaymentModeManage.cs
*
* 功 能： 支付模块业务数据交互类
* 类 名： PaymentModeManage
*
* Ver   变更日期    部门      担当者 变更内容
* ─────────────────────────────────
* V0.01 2012/01/13  研发部    姚远   初版
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌─────────────────────────────────┐
*│ 此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露． │
*│ 版权所有：云商未来（北京）科技有限公司                           │
*└─────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using YSWL.Payment.IDAL;
using YSWL.Payment.Model;
using YSWL.Payment.DALFactory;
using YSWL.Payment.Core;

namespace YSWL.Payment.BLL
{
    public static class PaymentModeManage
    {
        private static IPaymentMode service = YSWL.DBUtility.PubConstant.IsSQLServer? (IPaymentMode)new SQLServerDAL.PaymentModeService(): new MySqlDAL.PaymentModeService();

        #region Payment
        public static List<PaymentModeInfo> GetPaymentModes()
        {
            return service.GetPaymentModes((int)Model.DriveEnum.ALL,"");
        }
        public static List<PaymentModeInfo> GetPaymentModes(Model.DriveEnum driveEnum = Model.DriveEnum.ALL,string keyword = "")
        {
            return service.GetPaymentModes((int)driveEnum, keyword);
        }


        public static string GetWeChatUser(int orderId)
        {
            return service.GetWeChatUser(orderId);
        }

        public static PaymentModeInfo GetPaymentModeById(int modeId)
        {
            PaymentModeInfo paymentMode = service.GetPaymentMode(modeId);
            DecryptPaymentMode(paymentMode);
            return paymentMode;
        }

        [Obsolete]
        public static PaymentModeInfo GetPaymentModeByName(string gateway)
        {
            PaymentModeInfo paymentMode = service.GetPaymentMode(gateway);
            DecryptPaymentMode(paymentMode);
            return paymentMode;
        }

        [Obsolete]
        public static PaymentModeInfo GetPaymentMode(int modeId)
        {
            PaymentModeInfo paymentMode = service.GetPaymentMode(modeId);
            DecryptPaymentMode(paymentMode);
            return paymentMode;
        }

        [Obsolete]
        public static PaymentModeInfo GetPaymentMode(string gateway)
        {
            PaymentModeInfo paymentMode = service.GetPaymentMode(gateway);
            DecryptPaymentMode(paymentMode);
            return paymentMode;
        }

        public static PaymentModeActionStatus CreatePaymentMode(PaymentModeInfo paymentMode)
        {
            if (paymentMode == null)
            {
                return PaymentModeActionStatus.UnknowError;
            }
            EncryptPaymentMode(paymentMode);
            return service.CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Create);
        }

        public static PaymentModeActionStatus UpdatePaymentMode(PaymentModeInfo paymentMode)
        {
            if (paymentMode == null)
            {
                return PaymentModeActionStatus.UnknowError;
            }
            EncryptPaymentMode(paymentMode);
            return service.CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Update);
        }

        public static bool DeletePaymentMode(int modeId)
        {
            PaymentModeActionStatus unknowError = PaymentModeActionStatus.UnknowError;
            PaymentModeInfo paymentMode = new PaymentModeInfo
            {
                ModeId = modeId
            };
            unknowError = service.CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Delete);
            return (unknowError == PaymentModeActionStatus.Success);
        }

        //public static void AscPaymentMode(int modeId)
        //{
        //    service.SortPaymentMode(modeId, SortAction.ASC);
        //}
        //public static void DescPaymentMode(int modeId)
        //{
        //    service.SortPaymentMode(modeId, SortAction.Desc);
        //}

        internal static void DecryptPaymentMode(PaymentModeInfo paymentMode)
        {
            if (paymentMode != null)
            {
                using (MsCryptographer cryptographer = new MsCryptographer(true, false))
                {
                    if (!string.IsNullOrEmpty(paymentMode.SecretKey))
                    {
                        paymentMode.SecretKey = cryptographer.Decrypt(paymentMode.SecretKey);
                    }
                    if (!string.IsNullOrEmpty(paymentMode.SecondKey))
                    {
                        paymentMode.SecondKey = cryptographer.Decrypt(paymentMode.SecondKey);
                    }
                    if (!string.IsNullOrEmpty(paymentMode.Password))
                    {
                        paymentMode.Password = cryptographer.Decrypt(paymentMode.Password);
                    }
                    if (!string.IsNullOrEmpty(paymentMode.Partner))
                    {
                        paymentMode.Partner = cryptographer.Decrypt(paymentMode.Partner);
                    }
                }
            }
        }

        internal static void EncryptPaymentMode(PaymentModeInfo paymentMode)
        {
            if (paymentMode != null)
            {
                using (MsCryptographer cryptographer = new MsCryptographer(false, true))
                {
                    if (!string.IsNullOrEmpty(paymentMode.SecretKey))
                    {
                        paymentMode.SecretKey = cryptographer.Encrypt(paymentMode.SecretKey);
                    }
                    if (!string.IsNullOrEmpty(paymentMode.SecondKey))
                    {
                        paymentMode.SecondKey = cryptographer.Encrypt(paymentMode.SecondKey);
                    }
                    if (!string.IsNullOrEmpty(paymentMode.Password))
                    {
                        paymentMode.Password = cryptographer.Encrypt(paymentMode.Password);
                    }
                    if (!string.IsNullOrEmpty(paymentMode.Partner))
                    {
                        paymentMode.Partner = cryptographer.Encrypt(paymentMode.Partner);
                    }
                }
            }
        }
        #endregion 
        #region Recharge
        public static RechargeRequestInfo GetRechargeRequest(long rechargeId)
        {
            return service.GetRechargeRequest(rechargeId);
        }

        public static bool RemoveRechargeRequest(long rechargeId)
        {
            return (service.RemoveRechargeRequest(rechargeId) > 0);
        }
        public static bool AddBalanceDetail(BalanceDetailInfo balanceDetails)
        {
            return service.AddBalanceDetail(balanceDetails);
        }
        #endregion
    }
}