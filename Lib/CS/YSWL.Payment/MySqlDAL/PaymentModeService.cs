using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using YSWL.DBUtility;
using YSWL.Payment.IDAL;
using YSWL.Payment.Model;

namespace YSWL.MySqlDAL
{
    public partial class PaymentModeService : IPaymentMode
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

        private PaymentModeActionStatus DeletePaymentMode(PaymentModeInfo paymentMode)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();

            StringBuilder strSql1 = new StringBuilder();
            strSql1.AppendFormat(" DELETE FROM Pay_PaymentCurrencys WHERE ModeId= {0} ;",paymentMode.ModeId);
            MySqlParameter[] parameters1 = {
			};
            sqllist.Add(new CommandInfo(strSql1.ToString(), parameters1, EffentNextType.ExcuteEffectRows));

            StringBuilder strSql2 = new StringBuilder();
            strSql2.AppendFormat(" DELETE FROM Pay_PaymentTypes WHERE ModeId = {0} ;", paymentMode.ModeId);
            MySqlParameter[] parameters2 = {
			};
            sqllist.Add(new CommandInfo(strSql2.ToString(), parameters2, EffentNextType.ExcuteEffectRows));

            int count = DbHelperMySQL.ExecuteSqlTran(sqllist);
            if (count != 2) {
                return PaymentModeActionStatus.UnknowError;
            }
            return PaymentModeActionStatus.Success;
        }

        public string GetWeChatUser(int orderId)
        {

            throw new NotImplementedException();
        }

        private PaymentModeActionStatus UpdatePaymentMode(PaymentModeInfo paymentMode)
        {
            PaymentModeActionStatus status = PaymentModeActionStatus.UnknowError;
            //连接MySql数据库
            using (MySqlConnection connection = DbHelperMySQL.GetConnection)
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string strSql = string.Format("Select DisplaySequence  From Pay_PaymentTypes where ModeId={0}", paymentMode.ModeId);
                        MySqlParameter[] pars = { };
                        object objSeq = DbHelperMySQL.GetSingle4Trans(new CommandInfo(strSql, pars), transaction);
                        int displaySequence = Common.Globals.SafeInt(objSeq, -1);
                        if (displaySequence != paymentMode.DisplaySequence)
                        {
                            List<CommandInfo> sqllist = new List<CommandInfo>();
                            StringBuilder strSql1 = new StringBuilder();
                            strSql1.AppendFormat("UPDATE Pay_PaymentTypes set DisplaySequence = DisplaySequence + 1 where DisplaySequence >= {0} ;", paymentMode.DisplaySequence);
                            MySqlParameter[] parameters1 = { };
                            sqllist.Add(new CommandInfo(strSql1.ToString(), parameters1, EffentNextType.ExcuteEffectRows));
                            DbHelperMySQL.ExecuteSqlTran4Indentity(sqllist, transaction);
                        }

                        List<CommandInfo> sqllist2 = new List<CommandInfo>();
                        string str2 = @"UPDATE Pay_PaymentTypes SET MerchantCode=?MerchantCode, EmailAddress=?EmailAddress, SecretKey=?SecretKey, SecondKey=?SecondKey, Password=?Password,Partner=?Partner, Name=?Name, Description=?Description, Gateway=?Gateway, DisplaySequence=?DisplaySequence, Charge=?Charge, IsPercent=?IsPercent, AllowRecharge=?AllowRecharge,Logo=?Logo,DrivePath=?DrivePath WHERE ModeId = ?ModeId;";
                        MySqlParameter[] pars2 = { 
                                                    new MySqlParameter("?MerchantCode",MySqlDbType.VarChar,300),
                                                    new MySqlParameter("?EmailAddress",MySqlDbType.VarChar,255),
                                                    new MySqlParameter("?SecretKey",MySqlDbType.VarChar,4000),
                                                    new MySqlParameter("?SecondKey",MySqlDbType.VarChar,4000),
                                                    new MySqlParameter("?Password",MySqlDbType.VarChar,4000),
                                                    new MySqlParameter("?Partner",MySqlDbType.VarChar,300),
                                                    new MySqlParameter("?Name",MySqlDbType.VarChar,100),
                                                    new MySqlParameter("?Description",MySqlDbType.Text),
                                                    new MySqlParameter("?Gateway",MySqlDbType.VarChar,200),
                                                    new MySqlParameter("?DisplaySequence",MySqlDbType.Int32,4),
                                                    new MySqlParameter("?Charge",MySqlDbType.Decimal,8),
                                                    new MySqlParameter("?IsPercent",MySqlDbType.Int16,2),
                                                    new MySqlParameter("?AllowRecharge",MySqlDbType.Int16,2),
                                                    new MySqlParameter("?Logo",MySqlDbType.VarChar,255),
                                                    new MySqlParameter("?DrivePath",MySqlDbType.VarChar,255),
                                                    new MySqlParameter("?ModeId",MySqlDbType.Int32,4)
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
                        sqllist2.Add(new CommandInfo(str2, pars2,EffentNextType.ExcuteEffectRows));

                        string strDel = string.Format("DELETE  From  Pay_PaymentCurrencys Where ModeId={0}", paymentMode.ModeId);
                        sqllist2.Add(new CommandInfo(strDel, pars2,EffentNextType.None));

                        DbHelperMySQL.ExecuteSqlTran4Indentity(sqllist2, transaction);

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
                            MySqlParameter[] parameters3 = { };
                            sqllist3.Add(new CommandInfo(strSql3.ToString(), parameters3, EffentNextType.ExcuteEffectRows));
                            DbHelperMySQL.ExecuteSqlTran4Indentity(sqllist3, transaction);
                        }

                        transaction.Commit();
                        status = PaymentModeActionStatus.Success;
                    }
                    catch (MySqlException)
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
            using (MySqlConnection connection = DbHelperMySQL.GetConnection)
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        List<CommandInfo> sqllist = new List<CommandInfo>();
                        StringBuilder strSql1 = new StringBuilder();
                        strSql1.AppendFormat("UPDATE Pay_PaymentTypes set DisplaySequence = DisplaySequence + 1 where DisplaySequence >= {0} ;",paymentMode.DisplaySequence);
                        MySqlParameter[] parameters1 = {};
                        sqllist.Add(new CommandInfo(strSql1.ToString(), parameters1, EffentNextType.ExcuteEffectRows));
                        DbHelperMySQL.ExecuteSqlTran4Indentity(sqllist, transaction);

                        string str2 = @"INSERT INTO 
			                            Pay_PaymentTypes(MerchantCode, EmailAddress, SecretKey, SecondKey, Password,Partner, Name, Description, Gateway, DisplaySequence, Charge, IsPercent, AllowRecharge,Logo,DrivePath) VALUES (?MerchantCode, ?EmailAddress, ?SecretKey, ?SecondKey, ?Password,?Partner, ?Name, ?Description, ?Gateway, ?DisplaySequence, ?Charge, ?IsPercent, ?AllowRecharge,?Logo,?DrivePath);
		                             select @@IDENTITY;";

                        MySqlParameter[] pars2 = { 
                                                    new MySqlParameter("?MerchantCode",MySqlDbType.VarChar,300),
                                                    new MySqlParameter("?EmailAddress",MySqlDbType.VarChar,255),
                                                    new MySqlParameter("?SecretKey",MySqlDbType.VarChar,4000),
                                                    new MySqlParameter("?SecondKey",MySqlDbType.VarChar,4000),
                                                    new MySqlParameter("?Password",MySqlDbType.VarChar,4000),
                                                    new MySqlParameter("?Partner",MySqlDbType.VarChar,300),
                                                    new MySqlParameter("?Name",MySqlDbType.VarChar,100),
                                                    new MySqlParameter("?Description",MySqlDbType.Text),
                                                    new MySqlParameter("?Gateway",MySqlDbType.VarChar,200),
                                                    new MySqlParameter("?DisplaySequence",MySqlDbType.Int32,4),
                                                    new MySqlParameter("?Charge",MySqlDbType.Decimal,8),
                                                    new MySqlParameter("?IsPercent",MySqlDbType.Int16,2),
                                                    new MySqlParameter("?AllowRecharge",MySqlDbType.Int16,2),
                                                    new MySqlParameter("?Logo",MySqlDbType.VarChar,255),
                                                    new MySqlParameter("?DrivePath",MySqlDbType.VarChar,255)
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
                        object objId = DbHelperMySQL.GetSingle4Trans(new CommandInfo(str2, pars2), transaction);

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
                            MySqlParameter[] parameters2 = { };
                            sqllist2.Add(new CommandInfo(strSql2.ToString(), parameters2, EffentNextType.ExcuteEffectRows));
                            DbHelperMySQL.ExecuteSqlTran4Indentity(sqllist2, transaction);
                        }

                        transaction.Commit();
                        if (modeId > 0)
                        {
                            status = PaymentModeActionStatus.Success;
                        }
                    }
                    catch (MySqlException)
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
            //MySqlParameter[] parameters = {
            //                            new MySqlParameter("?ModeId",MySqlDbType.Int32),
            //                            new MySqlParameter("?Sort",MySqlDbType.Int32)
            //                            };
            //parameters[0].Value = modeId;
            //parameters[1].Value = (int)action;
            //DbHelperMySQL.RunProcedure("sp_Pay_PaymentMode_Sequence", parameters);
        }

        public PaymentModeInfo GetPaymentMode(int modeId)
        {
            PaymentModeInfo info = new PaymentModeInfo();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Pay_PaymentTypes WHERE ModeId = ?ModeId;SELECT Code FROM Pay_PaymentCurrencys WHERE ModeId = ?ModeId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?ModeId", MySqlDbType.Int32)
			};
            parameters[0].Value = modeId;
            using (IDataReader reader = DbHelperMySQL.ExecuteReader(strSql.ToString(), parameters))
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
            strSql.Append("SELECT * FROM Pay_PaymentTypes WHERE Gateway = @Gateway LIMIT 1;SELECT Code FROM Pay_PaymentCurrencys WHERE ModeId = (SELECT ModeId FROM Pay_PaymentTypes WHERE Gateway = @Gateway LIMIT 1) ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@Gateway", MySqlDbType.VarChar)
			};
            parameters[0].Value = gateway;
            using (IDataReader reader = DbHelperMySQL.ExecuteReader(strSql.ToString(), parameters))
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

        public List<PaymentModeInfo> GetPaymentModes(int type,string keyword="")
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

            using (IDataReader reader = DbHelperMySQL.ExecuteReader(strSql.ToString()))
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

            MySqlParameter[] parameters = {
                                        new MySqlParameter("?UserId",SqlDbType.Int)
                                        };
            parameters[0].Value = userId;
            //TODO: 存储过程
            using (IDataReader reader = DbHelperSQL.RunProcedure("sp_Pay_AccountSummary_Get", parameters))
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Pay_RechargeRequest WHERE RechargeId = ?RechargeId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?RechargeId", MySqlDbType.Int64)
			};
            parameters[0].Value = rechargeId;

            using (IDataReader reader = DbHelperMySQL.ExecuteReader(strSql.ToString(), parameters))
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
                MySqlParameter[] parameters = {
                                        new MySqlParameter("?TradeDate",MySqlDbType.Int32),
                                        new MySqlParameter("?RechargeBlance",MySqlDbType.DateTime),
                                        new MySqlParameter("?UserId",MySqlDbType.Int32),
                                        new MySqlParameter("?PaymentGateway",MySqlDbType.Decimal),
                                           (MySqlParameter)DbHelperMySQL.CreateReturnParam("Status", MySqlDbType.Int32, 4),
                                           (MySqlParameter)DbHelperMySQL.CreateReturnParam("RechargeId", MySqlDbType.Int32, 4)
                                        };
                parameters[0].Value = rechargeRequest.TradeDate;
                parameters[1].Value = rechargeRequest.RechargeBlance;
                parameters[2].Value = rechargeRequest.UserId;
                parameters[3].Value = rechargeRequest.PaymentGateway;
                //TODO: 存储过程
                DbHelperSQL.RunProcedure("sp_cf_rechargeRequest_Create", parameters);

                if ((int)parameters[4].Value == 0)
                {
                    return (long)parameters[5].Value;
                }
            }
            return 0L;
        }

        public int RemoveRechargeRequest(long rechargeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM Pay_RechargeRequest WHERE rechargeId = ?rechargeId ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?rechargeId", MySqlDbType.Int64)
			};
            parameters[0].Value = rechargeId;
            return DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
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
        VALUES  ( ?UserId ,
                  ?TradeDate ,
                  ?TradeType ,
                  ?Income ,
                  ?Balance ,
                  ?Remark
                )");
            MySqlParameter[] parameters1 = {
                                                new MySqlParameter("?UserId",MySqlDbType.Int32),
                                                new MySqlParameter("?TradeDate",MySqlDbType.DateTime),
                                                new MySqlParameter("?TradeType",MySqlDbType.Int32),
                                                new MySqlParameter("?Income",MySqlDbType.Decimal),
                                                new MySqlParameter("?Balance",MySqlDbType.Int32),
                                                new MySqlParameter("?Remark",MySqlDbType.Int32)
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
            MySqlParameter[] parameters2 = {
			};
            sqllist.Add(new CommandInfo(strSql2.ToString(), parameters2, EffentNextType.ExcuteEffectRows));

            StringBuilder strSql3 = new StringBuilder();
            //删除一条充值记录
            strSql2.AppendFormat(@"DELETE  FROM Pay_RechargeRequest
                WHERE   RechargeId ={0};", balanceDetails.JournalNumber);
            MySqlParameter[] parameters3 = {
			};
            sqllist.Add(new CommandInfo(strSql3.ToString(), parameters3, EffentNextType.ExcuteEffectRows));
            
            int count = DbHelperMySQL.ExecuteSqlTran(sqllist);
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
            strSql.Append("SELECT * FROM Pay_Currencys WHERE LOWER(Code) = LOWER(?Code) ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?Code", MySqlDbType.VarChar)
			};
            parameters[0].Value = code;

            using (IDataReader reader = DbHelperMySQL.ExecuteReader(strSql.ToString(), parameters))
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
            using (IDataReader reader = DbHelperMySQL.ExecuteReader(strSql.ToString()))
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
                DbHelperMySQL.ExecuteSql(strSql.ToString());
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
