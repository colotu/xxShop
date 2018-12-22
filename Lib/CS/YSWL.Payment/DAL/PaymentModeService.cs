/**
* PaymentModeService.cs
*
* 功 能： 支付模块DB数据交互类
* 类 名： PaymentModeService
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
using System.Data;
using System.Data.Common;
using System.Text;
using YSWL.Payment.Model;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YSWL.Payment.DAL
{
    [Obsolete]
    internal class PaymentModeService
    {
        private Database database = DatabaseFactory.CreateDatabase();

        #region Payment
        public PaymentModeActionStatus CreateUpdateDeletePaymentMode(PaymentModeInfo paymentMode, DataProviderAction action)
        {
            if (paymentMode == null)
            {
                return PaymentModeActionStatus.UnknowError;
            }

            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sp_Pay_PaymentMode_CreateUpdateDelete");
            this.database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int)action);
            this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            if (action == DataProviderAction.Create)
            {
                this.database.AddOutParameter(storedProcCommand, "ModeId", DbType.Int32, 4);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "ModeId", DbType.Int32, paymentMode.ModeId);
            }
            if (action != DataProviderAction.Delete)
            {
                this.database.AddInParameter(storedProcCommand, "MerchantCode", DbType.String, paymentMode.MerchantCode);
                this.database.AddInParameter(storedProcCommand, "EmailAddress", DbType.String, paymentMode.EmailAddress);
                this.database.AddInParameter(storedProcCommand, "SecretKey", DbType.String, paymentMode.SecretKey);
                this.database.AddInParameter(storedProcCommand, "SecondKey", DbType.String, paymentMode.SecondKey);
                this.database.AddInParameter(storedProcCommand, "Password", DbType.String, paymentMode.Password);
                this.database.AddInParameter(storedProcCommand, "Partner", DbType.String, paymentMode.Partner);
                this.database.AddInParameter(storedProcCommand, "Name", DbType.String, paymentMode.Name);
                this.database.AddInParameter(storedProcCommand, "Description", DbType.String, paymentMode.Description);
                this.database.AddInParameter(storedProcCommand, "AllowRecharge", DbType.Boolean, paymentMode.AllowRecharge);
                this.database.AddInParameter(storedProcCommand, "Gateway", DbType.String, paymentMode.Gateway);
                this.database.AddInParameter(storedProcCommand, "DrivePath", DbType.String, paymentMode.DrivePath);
                if (paymentMode.DisplaySequence > 0)
                {
                    this.database.AddInParameter(storedProcCommand, "DisplaySequence", DbType.Int32, paymentMode.DisplaySequence);
                }
                this.database.AddInParameter(storedProcCommand, "Charge", DbType.Currency, paymentMode.Charge);
                this.database.AddInParameter(storedProcCommand, "IsPercent", DbType.Boolean, paymentMode.IsPercent);
            }
            PaymentModeActionStatus unknowError = PaymentModeActionStatus.UnknowError;
            if ((action != DataProviderAction.Delete) && (paymentMode.SupportedCurrencys.Count > 0))
            {
                using (DbConnection connection = this.database.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        this.database.ExecuteNonQuery(storedProcCommand, transaction);
                        unknowError = (PaymentModeActionStatus)((int)this.database.GetParameterValue(storedProcCommand, "Status"));
                        int num = (action == DataProviderAction.Create) ? ((int)this.database.GetParameterValue(storedProcCommand, "ModeId")) : paymentMode.ModeId;
                        if (unknowError == PaymentModeActionStatus.Success)
                        {
                            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("  ");
                            this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, num);
                            StringBuilder builder = new StringBuilder();
                            if (action == DataProviderAction.Update)
                            {
                                builder.Append("DELETE  From  Pay_PaymentCurrencys Where ModeId=@ModeId");
                            }
                            builder.Append(" DECLARE @intErrorCode INT;SET @intErrorCode = 0;");
                            int num2 = 0;
                            foreach (string str in paymentMode.SupportedCurrencys)
                            {
                                builder.Append("INSERT INTO Pay_PaymentCurrencys(ModeId,Code) Values(").Append("@ModeId").Append(",@Code").Append(num2).Append(");SET @intErrorCode = @intErrorCode + @@ERROR;");
                                this.database.AddInParameter(sqlStringCommand, "Code" + num2, DbType.String, str);
                                num2++;
                            }
                            sqlStringCommand.CommandText = builder.Append("SELECT @intErrorCode;").ToString();
                            if (((int)this.database.ExecuteScalar(sqlStringCommand, transaction)) == 0)
                            {
                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();
                                unknowError = PaymentModeActionStatus.UnknowError;
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        unknowError = PaymentModeActionStatus.UnknowError;
                    }
                    connection.Close();
                    return unknowError;
                }
            }
            this.database.ExecuteNonQuery(storedProcCommand);
            return (PaymentModeActionStatus)((int)this.database.GetParameterValue(storedProcCommand, "Status"));
        }

        public void SortPaymentMode(int modeId, SortAction action)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sp_Pay_PaymentMode_Sequence");
            this.database.AddInParameter(storedProcCommand, "ModeId", DbType.Int32, modeId);
            this.database.AddInParameter(storedProcCommand, "Sort", DbType.Int32, (int)action);
            this.database.ExecuteNonQuery(storedProcCommand);
        }

        public PaymentModeInfo GetPaymentMode(int modeId)
        {
            PaymentModeInfo info = new PaymentModeInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Pay_PaymentTypes WHERE ModeId = @ModeId;SELECT Code FROM Pay_PaymentCurrencys WHERE ModeId = @ModeId");
            this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = this.PopupPayment(reader);
                }
                if (!reader.NextResult())
                {
                    return info;
                }
                while (reader.Read())
                {
                    info.SupportedCurrencys.Add(reader.GetString(0));
                }
            }
            return info;
        }

        public PaymentModeInfo GetPaymentMode(string gateway)
        {
            PaymentModeInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM Pay_PaymentTypes WHERE Gateway = @Gateway;SELECT Code FROM Pay_PaymentCurrencys WHERE ModeId = (SELECT top 1 ModeId FROM Pay_PaymentTypes WHERE Gateway = @Gateway)");
            this.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, gateway);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = this.PopupPayment(reader);
                }
                if (!reader.NextResult())
                {
                    return info;
                }
                while (reader.Read())
                {
                    info.SupportedCurrencys.Add(reader.GetString(0));
                }
            }
            return info;
        }

        public List<PaymentModeInfo> GetPaymentModes(int type)
        {
            List<PaymentModeInfo> list = new List<PaymentModeInfo>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Pay_PaymentTypes WHERE Gateway <> 'adminaction'  ");
            if (type > 0)
            {
                strSql.AppendFormat(" And DrivePath like '%|{0}|%'", type);
            }
            strSql.Append("  Order by DisplaySequence");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(strSql.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(this.PopupPayment(reader));
                }
            }
            return list;
        }

        #region PopupPayment
        public PaymentModeInfo PopupPayment(IDataRecord reader)
        {
            if (reader == null)
            {
                return null;
            }
            PaymentModeInfo info = new PaymentModeInfo
            {
                ModeId = (int)reader["ModeId"],
                Name = (string)reader["Name"],
                MerchantCode = (string)reader["MerchantCode"],
                DisplaySequence = (int)reader["DisplaySequence"],
                Charge = (decimal)reader["Charge"],
                IsPercent = (bool)reader["IsPercent"],
                AllowRecharge = (bool)reader["AllowRecharge"]
            };
            if (reader["EmailAddress"] != DBNull.Value)
            {
                info.EmailAddress = (string)reader["EmailAddress"];
            }
            if (reader["SecretKey"] != DBNull.Value)
            {
                info.SecretKey = (string)reader["SecretKey"];
            }
            if (reader["SecondKey"] != DBNull.Value)
            {
                info.SecondKey = (string)reader["SecondKey"];
            }
            if (reader["Password"] != DBNull.Value)
            {
                info.Password = (string)reader["Password"];
            }
            if (reader["Partner"] != DBNull.Value)
            {
                info.Partner = (string)reader["Partner"];
            }
            if (reader["Description"] != DBNull.Value)
            {
                info.Description = (string)reader["Description"];
            }
            if (reader["Gateway"] != DBNull.Value)
            {
                info.Gateway = (string)reader["Gateway"];
            }
            if (reader["Logo"] != DBNull.Value)
            {
                info.Logo = reader["Logo"].ToString();
            }

            if (reader["DrivePath"] != DBNull.Value)
            {
                info.DrivePath = reader["DrivePath"].ToString();
            }
            
            return info;
        }
        #endregion
        #endregion

        #region AccountSummary
        public AccountSummaryInfo GetAccountSummary(int userId)
        {
            AccountSummaryInfo info = null;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sp_Pay_AccountSummary_Get");
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, userId);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                while (reader.Read())
                {
                    info = this.PopupAccountSummary(reader);
                }
            }
            return info;
        }
        #region PopupAccountSummary
        public AccountSummaryInfo PopupAccountSummary(IDataRecord reader)
        {
            if (reader == null)
            {
                return null;
            }
            AccountSummaryInfo info = new AccountSummaryInfo();
            if (reader["AccountAmount"] != DBNull.Value)
            {
                info.AccountAmount = (decimal)reader["AccountAmount"];
            }
            if (reader["FreezeBalance"] != DBNull.Value)
            {
                info.FreezeBalance = (decimal)reader["FreezeBalance"];
            }
            info.UseableBalance = info.AccountAmount - info.FreezeBalance;
            return info;
        }
        #endregion
        #endregion

        #region Recharge
        public RechargeRequestInfo GetRechargeRequest(long rechargeId)
        {
            RechargeRequestInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Pay_RechargeRequest WHERE RechargeId = @RechargeId");
            this.database.AddInParameter(sqlStringCommand, "RechargeId", DbType.Int64, rechargeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    info = this.PopulateRechargeRequest(reader);
                }
            }
            return info;
        }

        public long AddRechargeBlance(RechargeRequestInfo rechargeRequest)
        {
            if (rechargeRequest != null)
            {
                DbCommand storedProcCommand = this.database.GetStoredProcCommand("sp_cf_rechargeRequest_Create");
                this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
                this.database.AddOutParameter(storedProcCommand, "RechargeId", DbType.Int64, 10);
                this.database.AddInParameter(storedProcCommand, "TradeDate", DbType.DateTime, rechargeRequest.TradeDate);
                this.database.AddInParameter(storedProcCommand, "RechargeBlance", DbType.Currency, rechargeRequest.RechargeBlance);
                this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, rechargeRequest.UserId);
                this.database.AddInParameter(storedProcCommand, "PaymentGateway", DbType.String, rechargeRequest.PaymentGateway);
                this.database.ExecuteNonQuery(storedProcCommand);
                if (((int)this.database.GetParameterValue(storedProcCommand, "Status")) == 0)
                {
                    return (long)this.database.GetParameterValue(storedProcCommand, "RechargeId");
                }
            }
            return 0L;
        }

        public int RemoveRechargeRequest(long rechargeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Pay_RechargeRequest WHERE rechargeId = @rechargeId");
            this.database.AddInParameter(sqlStringCommand, "rechargeId", DbType.Int64, rechargeId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool AddBalanceDetail(BalanceDetailInfo balanceDetails)
        {
            if (balanceDetails == null)
            {
                return false;
            }
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sp_Pay_BalanceDetails_Create");
            this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, balanceDetails.UserId);
            this.database.AddInParameter(storedProcCommand, "TradeDate", DbType.DateTime, balanceDetails.TradeDate);
            this.database.AddInParameter(storedProcCommand, "TradeType", DbType.Int32, (int)balanceDetails.TradeType);
            this.database.AddInParameter(storedProcCommand, "Income", DbType.Currency, balanceDetails.Income);
            this.database.AddInParameter(storedProcCommand, "Balance", DbType.Currency, balanceDetails.Balance);
            this.database.AddInParameter(storedProcCommand, "Remark", DbType.String, balanceDetails.Remark);
            this.database.AddInParameter(storedProcCommand, "rechargeId", DbType.Int64, balanceDetails.JournalNumber);
            this.database.ExecuteNonQuery(storedProcCommand);
            return (((int)this.database.GetParameterValue(storedProcCommand, "Status")) == 0);
        }
        #region PopupRecharge
        public RechargeRequestInfo PopulateRechargeRequest(IDataRecord reader)
        {
            if (reader == null)
            {
                return null;
            }
            return new RechargeRequestInfo
                       {
                           RechargeId = (long)reader["RechargeId"],
                           TradeDate = (DateTime)reader["TradeDate"],
                           UserId = (int)reader["UserId"],
                           PaymentTypeId = (int)reader["PaymentTypeId"],
                           PaymentGateway = (string)reader["PaymentGateway"],
                           RechargeBlance = (decimal)reader["RechargeBlance"],
                           PaymentStatus = (PaymentStatus)reader["Status"]
                       };
        }
        #endregion
        #endregion

        #region Currency
        public CurrencyInfo GetCurrency(string code)
        {
            CurrencyInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Pay_Currencys WHERE LOWER(Code) = LOWER(@Code)");
            this.database.AddInParameter(sqlStringCommand, "Code", DbType.String, code);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = this.PopulateCurrencyFromDataReader(reader);
                }
            }
            return info;
        }
        public IList<CurrencyInfo> GetCurrencys()
        {
            IList<CurrencyInfo> list = new List<CurrencyInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Pay_Currencys");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(this.PopulateCurrencyFromDataReader(reader));
                }
            }
            return list;
        }
        public int DeleteCurrency(IList<string> code)
        {
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Pay_Currencys WHERE Code = @Code");
            this.database.AddInParameter(sqlStringCommand, "Code", DbType.String);
            foreach (string str in code)
            {
                this.database.SetParameterValue(sqlStringCommand, "Code", str);
                this.database.ExecuteNonQuery(sqlStringCommand);
                num++;
            }
            return num;
        }
        #region PopupCurrency
        public CurrencyInfo PopulateCurrencyFromDataReader(IDataRecord reader)
        {
            if (reader == null) return null;

            return new CurrencyInfo
                       {
                           Code = (string)reader["Code"],
                           Name = (string)reader["Name"],
                           Symbol = (string)reader["Symbol"],
                           ExchangeRate = (decimal)reader["ExchangeRate"]
                       };
        }
        #endregion
        #endregion
    }
}