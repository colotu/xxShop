using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using YSWL.DBUtility;
using YSWL.Email.IDAL;
using MySql.Data.MySqlClient;

namespace YSWL.Email.MySqlDAL
{
    /// <summary>
    /// MailConfig
    /// </summary>
    public class MailConfig : IMailConfig
    {
        public MailConfig()
        { }

        #region

        /// <summary>
        ///
        /// </summary>
        public bool Exists(int UserID, string Mailaddress)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Accounts_MailConfig");
            strSql.Append(" where UserID=?UserID and  Mailaddress=?Mailaddress");
            MySqlParameter[] parameters = {
                    new MySqlParameter("?UserID", MySqlDbType.Int32,4),
                    new MySqlParameter("?Mailaddress", MySqlDbType.VarChar,100)};
            parameters[0].Value = UserID;
            parameters[1].Value = Mailaddress;
            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        ///
        /// </summary>
        public int Add(YSWL.Email.Model.MailConfig model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Accounts_MailConfig(");
            strSql.Append("UserID,Mailaddress,Username,Password,SMTPServer,SMTPPort,SMTPSSL,POPServer,POPPort,POPSSL)");
            strSql.Append(" values (");
            strSql.Append("?UserID,?Mailaddress,?Username,?Password,?SMTPServer,?SMTPPort,?SMTPSSL,?POPServer,?POPPort,?POPSSL)");
            strSql.Append(";select ??IDENTITY");
            MySqlParameter[] parameters = {
                    new MySqlParameter("?UserID", MySqlDbType.Int32,4),
                    new MySqlParameter("?Mailaddress", MySqlDbType.VarChar,100),
                    new MySqlParameter("?Username", MySqlDbType.VarChar,50),
                    new MySqlParameter("?Password", MySqlDbType.VarChar,50),
                    new MySqlParameter("?SMTPServer", MySqlDbType.VarChar,50),
                    new MySqlParameter("?SMTPPort", MySqlDbType.Int32,4),
                    new MySqlParameter("?SMTPSSL", MySqlDbType.Int16,2),
                    new MySqlParameter("?POPServer", MySqlDbType.VarChar,50),
                    new MySqlParameter("?POPPort", MySqlDbType.Int32,4),
                    new MySqlParameter("?POPSSL", MySqlDbType.Int16,2)};
            parameters[0].Value = model.UserID;
            parameters[1].Value = model.Mailaddress;
            parameters[2].Value = model.Username;
            parameters[3].Value = model.Password;
            parameters[4].Value = model.SMTPServer;
            parameters[5].Value = model.SMTPPort;
            parameters[6].Value = model.SMTPSSL ? 1 : 0;
            parameters[7].Value = model.POPServer;
            parameters[8].Value = model.POPPort;
            parameters[9].Value = model.POPSSL ? 1 : 0;

            object obj = DbHelperMySQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void Update(YSWL.Email.Model.MailConfig model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Accounts_MailConfig set ");
            strSql.Append("UserID=?UserID,");
            strSql.Append("Mailaddress=?Mailaddress,");
            strSql.Append("Username=?Username,");
            strSql.Append("Password=?Password,");
            strSql.Append("SMTPServer=?SMTPServer,");
            strSql.Append("SMTPPort=?SMTPPort,");
            strSql.Append("SMTPSSL=?SMTPSSL,");
            strSql.Append("POPServer=?POPServer,");
            strSql.Append("POPPort=?POPPort,");
            strSql.Append("POPSSL=?POPSSL");
            strSql.Append(" where ID=?ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("?ID", MySqlDbType.Int32,4),
                    new MySqlParameter("?UserID", MySqlDbType.Int32,4),
                    new MySqlParameter("?Mailaddress", MySqlDbType.VarChar,100),
                    new MySqlParameter("?Username", MySqlDbType.VarChar,50),
                    new MySqlParameter("?Password", MySqlDbType.VarChar,50),
                    new MySqlParameter("?SMTPServer", MySqlDbType.VarChar,50),
                    new MySqlParameter("?SMTPPort", MySqlDbType.Int32,4),
                    new MySqlParameter("?SMTPSSL", MySqlDbType.Int16,2),
                    new MySqlParameter("?POPServer", MySqlDbType.VarChar,50),
                    new MySqlParameter("?POPPort", MySqlDbType.Int32,4),
                    new MySqlParameter("?POPSSL", MySqlDbType.Int16,2)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.Mailaddress;
            parameters[3].Value = model.Username;
            parameters[4].Value = model.Password;
            parameters[5].Value = model.SMTPServer;
            parameters[6].Value = model.SMTPPort;
            parameters[7].Value = model.SMTPSSL ? 1 : 0;
            parameters[8].Value = model.POPServer;
            parameters[9].Value = model.POPPort;
            parameters[10].Value = model.POPSSL ? 1 : 0;

            DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Accounts_MailConfig ");
            strSql.Append(" where ID=?ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("?ID", MySqlDbType.Int32,4)};
            parameters[0].Value = ID;

            DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        ///
        /// </summary>
        public YSWL.Email.Model.MailConfig GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select   ID,UserID,Mailaddress,Username,Password,SMTPServer,SMTPPort,SMTPSSL,POPServer,POPPort,POPSSL from Accounts_MailConfig ");
            strSql.Append(" where ID=?ID LIMIT 1");
            MySqlParameter[] parameters = {
                    new MySqlParameter("?ID", MySqlDbType.Int32,4)};
            parameters[0].Value = ID;

            YSWL.Email.Model.MailConfig model = new YSWL.Email.Model.MailConfig();
            DataSet ds = DbHelperMySQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ID"].ToString() != "")
                {
                    model.ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UserID"].ToString() != "")
                {
                    model.UserID = int.Parse(ds.Tables[0].Rows[0]["UserID"].ToString());
                }
                model.Mailaddress = ds.Tables[0].Rows[0]["Mailaddress"].ToString();
                model.Username = ds.Tables[0].Rows[0]["Username"].ToString();
                model.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                model.SMTPServer = ds.Tables[0].Rows[0]["SMTPServer"].ToString();
                if (ds.Tables[0].Rows[0]["SMTPPort"].ToString() != "")
                {
                    model.SMTPPort = int.Parse(ds.Tables[0].Rows[0]["SMTPPort"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SMTPSSL"].ToString() != "")
                {
                    model.SMTPSSL = ds.Tables[0].Rows[0]["SMTPSSL"].ToString() == "1" ? true : false;
                }
                model.POPServer = ds.Tables[0].Rows[0]["POPServer"].ToString();
                if (ds.Tables[0].Rows[0]["POPPort"].ToString() != "")
                {
                    model.POPPort = int.Parse(ds.Tables[0].Rows[0]["POPPort"].ToString());
                }
                if (ds.Tables[0].Rows[0]["POPSSL"].ToString() != "")
                {
                    model.POPSSL = ds.Tables[0].Rows[0]["POPSSL"].ToString() == "1" ? true : false;
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public YSWL.Email.Model.MailConfig GetModel()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select   ID,UserID,Mailaddress,Username,Password,SMTPServer,SMTPPort,SMTPSSL,POPServer,POPPort,POPSSL from Accounts_MailConfig  LIMIT 1");

            YSWL.Email.Model.MailConfig model = new YSWL.Email.Model.MailConfig();
            DataSet ds = DbHelperMySQL.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ID"].ToString() != "")
                {
                    model.ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UserID"].ToString() != "")
                {
                    model.UserID = int.Parse(ds.Tables[0].Rows[0]["UserID"].ToString());
                }
                model.Mailaddress = ds.Tables[0].Rows[0]["Mailaddress"].ToString();
                model.Username = ds.Tables[0].Rows[0]["Username"].ToString();
                model.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                model.SMTPServer = ds.Tables[0].Rows[0]["SMTPServer"].ToString();
                if (ds.Tables[0].Rows[0]["SMTPPort"].ToString() != "")
                {
                    model.SMTPPort = int.Parse(ds.Tables[0].Rows[0]["SMTPPort"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SMTPSSL"].ToString() != "")
                {
                    model.SMTPSSL = ds.Tables[0].Rows[0]["SMTPSSL"].ToString() == "1" ? true : false;
                }
                model.POPServer = ds.Tables[0].Rows[0]["POPServer"].ToString();
                if (ds.Tables[0].Rows[0]["POPPort"].ToString() != "")
                {
                    model.POPPort = int.Parse(ds.Tables[0].Rows[0]["POPPort"].ToString());
                }
                if (ds.Tables[0].Rows[0]["POPSSL"].ToString() != "")
                {
                    model.POPSSL = ds.Tables[0].Rows[0]["POPSSL"].ToString() == "1" ? true : false;
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,UserID,Mailaddress,Username,Password,SMTPServer,SMTPPort,SMTPSSL,POPServer,POPPort,POPSSL ");
            strSql.Append(" FROM Accounts_MailConfig ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQL.Query(strSql.ToString());
        }

        /// <summary>
        ///
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
       
            strSql.Append(" ID,UserID,Mailaddress,Username,Password,SMTPServer,SMTPPort,SMTPSSL,POPServer,POPPort,POPSSL ");
            strSql.Append(" FROM Accounts_MailConfig ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            if (Top > 0)
            {
                strSql.Append(" LIMIT " + Top.ToString());
            }
            return DbHelperMySQL.Query(strSql.ToString());
        }

        #endregion  成员方法
    }
}