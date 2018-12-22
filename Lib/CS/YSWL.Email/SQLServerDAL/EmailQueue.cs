using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using YSWL.DBUtility;
using YSWL.Email.IDAL;

namespace YSWL.Email.SQLServerDAL
{

    /// <summary>
    /// 数据访问类:EmailQueue
    /// </summary>
    public partial class EmailQueue : IEmailQueue
    {
        public EmailQueue()
        { }

        #region Method

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(YSWL.Email.Model.EmailQueue model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Accounts_EmailQueue(");
            strSql.Append("EmailPriority,IsBodyHtml,EmailTo,EmailCc,EmailBcc,EmailFrom,EmailSubject,EmailBody,NextTryTime,NumberOfTries)");
            strSql.Append(" values (");
            strSql.Append("@EmailPriority,@IsBodyHtml,@EmailTo,@EmailCc,@EmailBcc,@EmailFrom,@EmailSubject,@EmailBody,@NextTryTime,@NumberOfTries)");
            SqlParameter[] parameters = {
                    new SqlParameter("@EmailPriority", SqlDbType.Int,4),
                    new SqlParameter("@IsBodyHtml", SqlDbType.Bit,1),
                    new SqlParameter("@EmailTo", SqlDbType.NVarChar,2000),
                    new SqlParameter("@EmailCc", SqlDbType.NText),
                    new SqlParameter("@EmailBcc", SqlDbType.NText),
                    new SqlParameter("@EmailFrom", SqlDbType.NVarChar,256),
                    new SqlParameter("@EmailSubject", SqlDbType.NVarChar,1024),
                    new SqlParameter("@EmailBody", SqlDbType.NText),
                    new SqlParameter("@NextTryTime", SqlDbType.DateTime),
                    new SqlParameter("@NumberOfTries", SqlDbType.Int,4)};
            parameters[0].Value = model.EmailPriority;
            parameters[1].Value = model.IsBodyHtml;
            parameters[2].Value = model.EmailTo;
            parameters[3].Value = model.EmailCc;
            parameters[4].Value = model.EmailBcc;
            parameters[5].Value = model.EmailFrom;
            parameters[6].Value = model.EmailSubject;
            parameters[7].Value = model.EmailBody;
            parameters[8].Value = model.NextTryTime;
            parameters[9].Value = model.NumberOfTries;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.Email.Model.EmailQueue model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Accounts_EmailQueue set ");
            strSql.Append("EmailId=@EmailId,");
            strSql.Append("EmailPriority=@EmailPriority,");
            strSql.Append("IsBodyHtml=@IsBodyHtml,");
            strSql.Append("EmailTo=@EmailTo,");
            strSql.Append("EmailCc=@EmailCc,");
            strSql.Append("EmailBcc=@EmailBcc,");
            strSql.Append("EmailFrom=@EmailFrom,");
            strSql.Append("EmailSubject=@EmailSubject,");
            strSql.Append("EmailBody=@EmailBody,");
            strSql.Append("NextTryTime=@NextTryTime,");
            strSql.Append("NumberOfTries=@NumberOfTries");
            strSql.Append(" where ");
            SqlParameter[] parameters = {
                    new SqlParameter("@EmailId", SqlDbType.Int,4),
                    new SqlParameter("@EmailPriority", SqlDbType.Int,4),
                    new SqlParameter("@IsBodyHtml", SqlDbType.Bit,1),
                    new SqlParameter("@EmailTo", SqlDbType.NVarChar,2000),
                    new SqlParameter("@EmailCc", SqlDbType.NText),
                    new SqlParameter("@EmailBcc", SqlDbType.NText),
                    new SqlParameter("@EmailFrom", SqlDbType.NVarChar,256),
                    new SqlParameter("@EmailSubject", SqlDbType.NVarChar,1024),
                    new SqlParameter("@EmailBody", SqlDbType.NText),
                    new SqlParameter("@NextTryTime", SqlDbType.DateTime),
                    new SqlParameter("@NumberOfTries", SqlDbType.Int,4)};
            parameters[0].Value = model.EmailId;
            parameters[1].Value = model.EmailPriority;
            parameters[2].Value = model.IsBodyHtml;
            parameters[3].Value = model.EmailTo;
            parameters[4].Value = model.EmailCc;
            parameters[5].Value = model.EmailBcc;
            parameters[6].Value = model.EmailFrom;
            parameters[7].Value = model.EmailSubject;
            parameters[8].Value = model.EmailBody;
            parameters[9].Value = model.NextTryTime;
            parameters[10].Value = model.NumberOfTries;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Accounts_EmailQueue ");
            strSql.Append(" where ");
            SqlParameter[] parameters = {
            };

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.Email.Model.EmailQueue GetModel()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 EmailId,EmailPriority,IsBodyHtml,EmailTo,EmailCc,EmailBcc,EmailFrom,EmailSubject,EmailBody,NextTryTime,NumberOfTries from Accounts_EmailQueue ");
            strSql.Append(" where ");
            SqlParameter[] parameters = {
            };

            YSWL.Email.Model.EmailQueue model = new YSWL.Email.Model.EmailQueue();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["EmailId"] != null && ds.Tables[0].Rows[0]["EmailId"].ToString() != "")
                {
                    model.EmailId = int.Parse(ds.Tables[0].Rows[0]["EmailId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EmailPriority"] != null && ds.Tables[0].Rows[0]["EmailPriority"].ToString() != "")
                {
                    model.EmailPriority = int.Parse(ds.Tables[0].Rows[0]["EmailPriority"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsBodyHtml"] != null && ds.Tables[0].Rows[0]["IsBodyHtml"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["IsBodyHtml"].ToString() == "1") || (ds.Tables[0].Rows[0]["IsBodyHtml"].ToString().ToLower() == "true"))
                    {
                        model.IsBodyHtml = true;
                    }
                    else
                    {
                        model.IsBodyHtml = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["EmailTo"] != null && ds.Tables[0].Rows[0]["EmailTo"].ToString() != "")
                {
                    model.EmailTo = ds.Tables[0].Rows[0]["EmailTo"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmailCc"] != null && ds.Tables[0].Rows[0]["EmailCc"].ToString() != "")
                {
                    model.EmailCc = ds.Tables[0].Rows[0]["EmailCc"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmailBcc"] != null && ds.Tables[0].Rows[0]["EmailBcc"].ToString() != "")
                {
                    model.EmailBcc = ds.Tables[0].Rows[0]["EmailBcc"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmailFrom"] != null && ds.Tables[0].Rows[0]["EmailFrom"].ToString() != "")
                {
                    model.EmailFrom = ds.Tables[0].Rows[0]["EmailFrom"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmailSubject"] != null && ds.Tables[0].Rows[0]["EmailSubject"].ToString() != "")
                {
                    model.EmailSubject = ds.Tables[0].Rows[0]["EmailSubject"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmailBody"] != null && ds.Tables[0].Rows[0]["EmailBody"].ToString() != "")
                {
                    model.EmailBody = ds.Tables[0].Rows[0]["EmailBody"].ToString();
                }
                if (ds.Tables[0].Rows[0]["NextTryTime"] != null && ds.Tables[0].Rows[0]["NextTryTime"].ToString() != "")
                {
                    model.NextTryTime = DateTime.Parse(ds.Tables[0].Rows[0]["NextTryTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["NumberOfTries"] != null && ds.Tables[0].Rows[0]["NumberOfTries"].ToString() != "")
                {
                    model.NumberOfTries = int.Parse(ds.Tables[0].Rows[0]["NumberOfTries"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EmailId,EmailPriority,IsBodyHtml,EmailTo,EmailCc,EmailBcc,EmailFrom,EmailSubject,EmailBody,NextTryTime,NumberOfTries ");
            strSql.Append(" FROM Accounts_EmailQueue ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" EmailId,EmailPriority,IsBodyHtml,EmailTo,EmailCc,EmailBcc,EmailFrom,EmailSubject,EmailBody,NextTryTime,NumberOfTries ");
            strSql.Append(" FROM Accounts_EmailQueue ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM Accounts_EmailQueue ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrWhiteSpace(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T. desc");
            }
            strSql.Append(")AS Row, T.*  from Accounts_EmailQueue T ");
            if (!string.IsNullOrWhiteSpace(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }

        #endregion Method

        /// <summary>
        /// 将邮件加入邮件发送队列
        /// </summary>
        public bool PushEmailQueur(string uType, string uName, string EmailSubject, string EmailBody, string EmailFrom)
        {
            int rows = 0;
            SqlParameter[] parameters = {
                    new SqlParameter("@UserType", SqlDbType.NVarChar),
                    new SqlParameter("@UserName", SqlDbType.NVarChar),
                    new SqlParameter("@EmailSubject", SqlDbType.NVarChar),
                    new SqlParameter("@EmailBody", SqlDbType.NVarChar),
                    new SqlParameter("@EmailFrom", SqlDbType.NVarChar)
                    };
            parameters[0].Value = uType;
            parameters[1].Value = uName;
            parameters[2].Value = EmailSubject;
            parameters[3].Value = EmailBody;
            parameters[4].Value = EmailFrom;
            DbHelperSQL.RunProcedure("sp_Accounts_SendEmail", parameters, out rows);
            if (rows > 0)
                return true;
            return false;
        }

        public bool Exists(string emailSubject)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Accounts_EmailQueue");
            strSql.Append(" where EmailSubject=@emailSubject");
            SqlParameter[] parameters = {
					new SqlParameter("@EmailSubject", SqlDbType.NVarChar,200)
			};
            parameters[0].Value = emailSubject;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }
    }
}