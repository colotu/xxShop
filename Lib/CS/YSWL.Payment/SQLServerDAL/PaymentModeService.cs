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
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using YSWL.DBUtility;
using YSWL.Payment.IDAL;
using YSWL.Payment.Model;

namespace YSWL.SQLServerDAL
{
  public  partial class PaymentModeService : IPaymentMode
    {
        #region Payment
        public PaymentModeActionStatus CreateUpdateDeletePaymentMode(PaymentModeInfo paymentMode, DataProviderAction action)
        {
            if (paymentMode == null)
            {
                return PaymentModeActionStatus.UnknowError;
            }

            PaymentModeActionStatus status = PaymentModeActionStatus.UnknowError;
            switch (action)
            {
                case DataProviderAction.Create:
                    status = CreatePaymentMode(paymentMode);
                    break;
                case DataProviderAction.Update:
                    status = UpdatePaymentMode(paymentMode);
                    break;
                case DataProviderAction.Delete:
                    status = DeletePaymentMode(paymentMode);
                    break;
                default:
                    break;
            }
            return status;
        }


        public string GetWeChatUser(int orderId)
        {
            
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  UserName  from WeChat_User where exists(select  1  from  OMS_Orders where OrderId=@OrderId and BuyerID=UserId)");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderId", SqlDbType.Int)
            };
            parameters[0].Value = orderId;
            object obj= DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
            return obj == null ? "" : obj.ToString();
        }

        private PaymentModeActionStatus DeletePaymentMode(PaymentModeInfo paymentMode)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();

            StringBuilder strSql1 = new StringBuilder();
            strSql1.AppendFormat(" DELETE FROM Pay_PaymentCurrencys WHERE ModeId= {0} ;", paymentMode.ModeId);
            SqlParameter[] parameters1 = {
			};
            sqllist.Add(new CommandInfo(strSql1.ToString(), parameters1, EffentNextType.ExcuteEffectRows));

            StringBuilder strSql2 = new StringBuilder();
            strSql2.AppendFormat(" DELETE FROM Pay_PaymentTypes WHERE ModeId = {0} ;", paymentMode.ModeId);
            SqlParameter[] parameters2 = {
			};
            sqllist.Add(new CommandInfo(strSql2.ToString(), parameters2, EffentNextType.ExcuteEffectRows));

            int count = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            if (count != 2)
            {
                return PaymentModeActionStatus.UnknowError;
            }
            return PaymentModeActionStatus.Success;
        }

        private PaymentModeActionStatus UpdatePaymentMode(PaymentModeInfo paymentMode)
        {
            PaymentModeActionStatus status = PaymentModeActionStatus.UnknowError;
            //连接MySql数据库
            using (SqlConnection connection = DBHelper.DefaultDBHelper.GetDBObject().GetConnection)
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string strSql = string.Format("Select DisplaySequence  From Pay_PaymentTypes where ModeId={0}", paymentMode.ModeId);
                        SqlParameter[] pars = { };
                        object objSeq = DBHelper.DefaultDBHelper.GetSingle4Trans(new CommandInfo(strSql, pars), transaction);
                        int displaySequence = Common.Globals.SafeInt(objSeq, -1);
                        if (displaySequence != paymentMode.DisplaySequence)
                        {
                            List<CommandInfo> sqllist = new List<CommandInfo>();
                            StringBuilder strSql1 = new StringBuilder();
                            strSql1.AppendFormat("UPDATE Pay_PaymentTypes set DisplaySequence = DisplaySequence + 1 where DisplaySequence >= {0} ;", paymentMode.DisplaySequence);
                            SqlParameter[] parameters1 = { };
                            sqllist.Add(new CommandInfo(strSql1.ToString(), parameters1, EffentNextType.ExcuteEffectRows));
                            DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(sqllist, transaction);
                        }

                        List<CommandInfo> sqllist2 = new List<CommandInfo>();
                        string str2 = @"UPDATE Pay_PaymentTypes SET MerchantCode=@MerchantCode, EmailAddress=@EmailAddress, SecretKey=@SecretKey, SecondKey=@SecondKey, Password=@Password,Partner=@Partner, Name=@Name, Description=@Description, Gateway=@Gateway, DisplaySequence=@DisplaySequence, Charge=@Charge, IsPercent=@IsPercent, AllowRecharge=@AllowRecharge,Logo=@Logo,DrivePath=@DrivePath WHERE ModeId = @ModeId;";
                        SqlParameter[] pars2 = { 
                                                    new SqlParameter("@MerchantCode",SqlDbType.NVarChar,300),
                                                    new SqlParameter("@EmailAddress",SqlDbType.NVarChar,255),
                                                    new SqlParameter("@SecretKey",SqlDbType.NVarChar,4000),
                                                    new SqlParameter("@SecondKey",SqlDbType.NVarChar,4000),
                                                    new SqlParameter("@Password",SqlDbType.NVarChar,4000),
                                                    new SqlParameter("@Partner",SqlDbType.NVarChar,300),
                                                    new SqlParameter("@Name",SqlDbType.NVarChar,100),
                                                    new SqlParameter("@Description",SqlDbType.Text),
                                                    new SqlParameter("@Gateway",SqlDbType.NVarChar,200),
                                                    new SqlParameter("@DisplaySequence",SqlDbType.Int,4),
                                                    new SqlParameter("@Charge",SqlDbType.Money,8),
                                                    new SqlParameter("@IsPercent",SqlDbType.BigInt,8),
                                                    new SqlParameter("@AllowRecharge",SqlDbType.BigInt,8),
                                                    new SqlParameter("@Logo",SqlDbType.NVarChar,255),
                                                    new SqlParameter("@DrivePath",SqlDbType.NVarChar,255),
                                                    new SqlParameter("@ModeId",SqlDbType.Int,4)
                                                };
                        pars2[0].Value = paymentMode.MerchantCode;
                        pars2[1].Value = paymentMode.EmailAddress;
                        pars2[2].Value = paymentMode.SecretKey;
                        pars2[3].Value = paymentMode.SecondKey;
                        pars2[4].Value = paymentMode.Password;
                        pars2[5].Value = paymentMode.Partner;
                        pars2[6].Value = paymentMode.Name;
                        pars2[7].Value = paymentMode.Description;
                        pars2[8].Value = paymentMode.Gateway;
                        pars2[9].Value = paymentMode.DisplaySequence;
                        pars2[10].Value = paymentMode.Charge;
                        pars2[11].Value = paymentMode.IsPercent;
                        pars2[12].Value = paymentMode.AllowRecharge;
                        pars2[13].Value = paymentMode.Logo;
                        pars2[14].Value = paymentMode.DrivePath;
                        pars2[15].Value = paymentMode.ModeId;
                        sqllist2.Add(new CommandInfo(str2, pars2, EffentNextType.ExcuteEffectRows));

                        string strDel = string.Format("DELETE  From  Pay_PaymentCurrencys Where ModeId={0}", paymentMode.ModeId);
                        sqllist2.Add(new CommandInfo(strDel, pars2, EffentNextType.None));

                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(sqllist2, transaction);

                        List<CommandInfo> sqllist3 = new List<CommandInfo>();
                        StringBuilder strSql3 = new StringBuilder();
                        int num = 0;
                        foreach (string str in paymentMode.SupportedCurrencys)
                        {
                            strSql3.AppendFormat("INSERT INTO Pay_PaymentCurrencys(ModeId,Code) Values({0},'{1}');", paymentMode.ModeId, str);
                            num++;
                        }
                        if (strSql3.Length > 0)
                        {
                            SqlParameter[] parameters3 = { };
                            sqllist3.Add(new CommandInfo(strSql3.ToString(), parameters3, EffentNextType.ExcuteEffectRows));
                            DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(sqllist3, transaction);
                        }

                        transaction.Commit();
                        status = PaymentModeActionStatus.Success;
                    }
                    catch (SqlException)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return status;
        }

        private PaymentModeActionStatus CreatePaymentMode(PaymentModeInfo paymentMode)
        {
            PaymentModeActionStatus status = PaymentModeActionStatus.UnknowError;
            //连接MySql数据库
            using (SqlConnection connection = DBHelper.DefaultDBHelper.GetDBObject().GetConnection)
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        //List<CommandInfo> sqllist = new List<CommandInfo>();
                        //StringBuilder strSql1 = new StringBuilder();
                        //strSql1.AppendFormat("UPDATE Pay_PaymentTypes set DisplaySequence = DisplaySequence + 1 where DisplaySequence >= {0} ;", paymentMode.DisplaySequence);
                        //SqlParameter[] parameters1 = { };
                        //sqllist.Add(new CommandInfo(strSql1.ToString(), parameters1, EffentNextType.ExcuteEffectRows));
                        //DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(sqllist, transaction);

                        string str2 = @"INSERT INTO 
			                            Pay_PaymentTypes(MerchantCode, EmailAddress, SecretKey, SecondKey, Password,Partner, Name, Description, Gateway, DisplaySequence, Charge, IsPercent, AllowRecharge,Logo,DrivePath) VALUES (@MerchantCode, @EmailAddress, @SecretKey, @SecondKey, @Password,@Partner, @Name, @Description, @Gateway, @DisplaySequence, @Charge, @IsPercent, @AllowRecharge,@Logo,@DrivePath);
		                             select @@IDENTITY;";

                        SqlParameter[] pars2 = { 
                                                    new SqlParameter("@MerchantCode",SqlDbType.NVarChar,300),
                                                    new SqlParameter("@EmailAddress",SqlDbType.NVarChar,255),
                                                    new SqlParameter("@SecretKey",SqlDbType.NVarChar,4000),
                                                    new SqlParameter("@SecondKey",SqlDbType.NVarChar,4000),
                                                    new SqlParameter("@Password",SqlDbType.NVarChar,4000),
                                                    new SqlParameter("@Partner",SqlDbType.NVarChar,300),
                                                    new SqlParameter("@Name",SqlDbType.NVarChar,100),
                                                    new SqlParameter("@Description",SqlDbType.Text),
                                                    new SqlParameter("@Gateway",SqlDbType.NVarChar,200),
                                                    new SqlParameter("@DisplaySequence",SqlDbType.Int,4),
                                                    new SqlParameter("@Charge",SqlDbType.Money,8),
                                                    new SqlParameter("@IsPercent",SqlDbType.BigInt,8),
                                                    new SqlParameter("@AllowRecharge",SqlDbType.BigInt,8),
                                                    new SqlParameter("@Logo",SqlDbType.NVarChar,255),
                                                    new SqlParameter("@DrivePath",SqlDbType.NVarChar,255)
                                                };
                        pars2[0].Value = paymentMode.MerchantCode;
                        pars2[1].Value = paymentMode.EmailAddress;
                        pars2[2].Value = paymentMode.SecretKey;
                        pars2[3].Value = paymentMode.SecondKey;
                        pars2[4].Value = paymentMode.Password;
                        pars2[5].Value = paymentMode.Partner;
                        pars2[6].Value = paymentMode.Name;
                        pars2[7].Value = paymentMode.Description;
                        pars2[8].Value = paymentMode.Gateway;
                        pars2[9].Value = paymentMode.DisplaySequence;
                        pars2[10].Value = paymentMode.Charge;
                        pars2[11].Value = paymentMode.IsPercent;
                        pars2[12].Value = paymentMode.AllowRecharge;
                        pars2[13].Value = paymentMode.Logo;
                        pars2[14].Value = paymentMode.DrivePath;
                        object objId = DBHelper.DefaultDBHelper.GetSingle4Trans(new CommandInfo(str2, pars2), transaction);

                        int modeId = Common.Globals.SafeInt(objId, 0);
                        List<CommandInfo> sqllist2 = new List<CommandInfo>();
                        StringBuilder strSql2 = new StringBuilder();
                        int num = 0;
                        foreach (string str in paymentMode.SupportedCurrencys)
                        {
                            strSql2.AppendFormat("INSERT INTO Pay_PaymentCurrencys(ModeId,Code) Values({0},'{1}');", modeId, str);
                            num++;
                        }
                        if (strSql2.Length > 0)
                        {
                            SqlParameter[] parameters2 = { };
                            sqllist2.Add(new CommandInfo(strSql2.ToString(), parameters2, EffentNextType.ExcuteEffectRows));
                            DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(sqllist2, transaction);
                        }

                        transaction.Commit();
                        if (modeId > 0)
                        {
                            status = PaymentModeActionStatus.Success;
                        }
                    }
                    catch (SqlException)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return status;
        }

        public void SortPaymentMode(int modeId, SortAction action)
        {
            //SqlParameter[] parameters = {
            //                            new SqlParameter("@ModeId",SqlDbType.Int),
            //                            new SqlParameter("@Sort",SqlDbType.Int)
            //                            };
            //parameters[0].Value = modeId;
            //parameters[1].Value = (int)action;
            //DBHelper.DefaultDBHelper.RunProcedure("sp_Pay_PaymentMode_Sequence", parameters);
        }

        public PaymentModeInfo GetPaymentMode(int modeId)
        {
            PaymentModeInfo info = new PaymentModeInfo();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Pay_PaymentTypes WHERE ModeId = @ModeId;SELECT Code FROM Pay_PaymentCurrencys WHERE ModeId = @ModeId");
            SqlParameter[] parameters = {
					new SqlParameter("@ModeId", SqlDbType.Int)
			};
            parameters[0].Value = modeId;
            using (IDataReader reader = DBHelper.DefaultDBHelper.ExecuteReader(strSql.ToString(), parameters))
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT top 1 * FROM Pay_PaymentTypes WHERE Gateway = @Gateway ;SELECT Top 1  Code FROM Pay_PaymentCurrencys WHERE ModeId = (SELECT ModeId FROM Pay_PaymentTypes WHERE Gateway = @Gateway ) ");
            SqlParameter[] parameters = {
					new SqlParameter("@Gateway", SqlDbType.NVarChar)
			};
            parameters[0].Value = gateway;
            using (IDataReader reader = DBHelper.DefaultDBHelper.ExecuteReader(strSql.ToString(), parameters))
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

        public List<PaymentModeInfo> GetPaymentModes(int type,string keyword)
        {
            List<PaymentModeInfo> list = new List<PaymentModeInfo>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Pay_PaymentTypes WHERE Gateway <> 'adminaction'  ");
            if (type > 0)
            {
                strSql.AppendFormat(" And DrivePath like '%|{0}|%'",  type);
            }

            if (!String.IsNullOrWhiteSpace(keyword))
            {
                strSql.AppendFormat(" And Name like '%{0}%'", Common.InjectionFilter.SqlFilter(keyword));
            }
            strSql.Append("  Order by DisplaySequence");

            using (IDataReader reader = DBHelper.DefaultDBHelper.ExecuteReader(strSql.ToString()))
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Pay_RechargeRequest WHERE RechargeId = @RechargeId");
            SqlParameter[] parameters = {
					new SqlParameter("@RechargeId", SqlDbType.BigInt)
			};
            parameters[0].Value = rechargeId;

            using (IDataReader reader = DBHelper.DefaultDBHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                while (reader.Read())
                {
                    info = this.PopulateRechargeRequest(reader);
                }
            }
            return info;
        }


        public int RemoveRechargeRequest(long rechargeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM Pay_RechargeRequest WHERE rechargeId = @rechargeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@rechargeId",SqlDbType.BigInt)
			};
            parameters[0].Value = rechargeId;
            return DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        public bool AddBalanceDetail(BalanceDetailInfo balanceDetails)
        {
            if (balanceDetails == null)
            {
                return false;
            }

            List<CommandInfo> sqllist = new List<CommandInfo>();
            // 添加一条预付款明细
            StringBuilder strSql1 = new StringBuilder();
            strSql1.AppendFormat(@"INSERT  INTO Pay_BalanceDetails
                ( UserId ,
                  TradeDate ,
                  TradeType ,
                  Income ,
                  Balance ,
                  Remark
                )
        VALUES  ( @UserId ,
                  @TradeDate ,
                  @TradeType ,
                  @Income ,
                  @Balance ,
                  @Remark
                )");
            SqlParameter[] parameters1 = {
                                                new SqlParameter("@UserId",SqlDbType.Int),
                                                new SqlParameter("@TradeDate",SqlDbType.DateTime),
                                                new SqlParameter("@TradeType",SqlDbType.Int),
                                                new SqlParameter("@Income",SqlDbType.Money),
                                                new SqlParameter("@Balance",SqlDbType.Int),
                                                new SqlParameter("@Remark",SqlDbType.Int)
			};
            parameters1[0].Value = balanceDetails.UserId;
            parameters1[1].Value = balanceDetails.TradeDate;
            parameters1[2].Value = (int)balanceDetails.TradeType;
            parameters1[3].Value = balanceDetails.Income;
            parameters1[4].Value = balanceDetails.Balance;
            parameters1[5].Value = balanceDetails.Remark;
            parameters1[6].Value = balanceDetails.JournalNumber;
            sqllist.Add(new CommandInfo(strSql1.ToString(), parameters1, EffentNextType.ExcuteEffectRows));

            StringBuilder strSql2 = new StringBuilder();
            //更新个人信息的预付款记录
            strSql2.AppendFormat(@"UPDATE  Accounts_UsersExp
        SET     balance = _Balance
        WHERE   UserId = {0};", balanceDetails.UserId);
            SqlParameter[] parameters2 = {
			};
            sqllist.Add(new CommandInfo(strSql2.ToString(), parameters2, EffentNextType.ExcuteEffectRows));

            StringBuilder strSql3 = new StringBuilder();
            //删除一条充值记录
            strSql2.AppendFormat(@"DELETE  FROM Pay_RechargeRequest
                WHERE   RechargeId ={0};", balanceDetails.JournalNumber);
            SqlParameter[] parameters3 = {
			};
            sqllist.Add(new CommandInfo(strSql3.ToString(), parameters3, EffentNextType.ExcuteEffectRows));

            int count = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            if (count != 3)
            {
                return false;
            }
            return true;
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Pay_Currencys WHERE LOWER(Code) = LOWER(@Code) ");
            SqlParameter[] parameters = {
					new SqlParameter("@Code", SqlDbType.NVarChar)
			};
            parameters[0].Value = code;

            using (IDataReader reader = DBHelper.DefaultDBHelper.ExecuteReader(strSql.ToString(), parameters))
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

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * FROM Pay_Currencys ");
            using (IDataReader reader = DBHelper.DefaultDBHelper.ExecuteReader(strSql.ToString()))
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
            StringBuilder strSql = new StringBuilder();
            foreach (string str in code)
            {
                strSql.Append(" DELETE FROM Pay_Currencys ");
                strSql.AppendFormat(" WHERE Code = '{0}'", str);
                DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
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
