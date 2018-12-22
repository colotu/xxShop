using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using YSWL.DBUtility;
using YSWL.Email.IDAL;
using System.Text;

namespace YSWL.Email.SQLServerDAL
{

    public class EmailQueueProvider : IEmailQueueProvider
    {
        public int DeleteQueuedEmail(int emailId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Accounts_EmailQueue ");
            strSql.Append(" where EmailId=@EmailId");
            SqlParameter[] parameters = {
					new SqlParameter("@EmailId", SqlDbType.Int,4)
			};
            parameters[0].Value = emailId;
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            return rows;
        }

        public IList<YSWL.Email.Model.EmailTemplate> DequeueEmail()
        {
            IList<YSWL.Email.Model.EmailTemplate> list = new List<YSWL.Email.Model.EmailTemplate>();
            SqlParameter[] parameters = { };
            DataSet ds = DbHelperSQL.RunProcedure("sp_Emails_Dequeue", parameters, "ds");
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    YSWL.Email.Model.EmailTemplate model = new Model.EmailTemplate();
                    LoadEntityData(ref model, dr);
                    list.Add(model);
                }
            }
            return list;
        }

        private void LoadEntityData(ref YSWL.Email.Model.EmailTemplate model, DataRow dr)
        {
            if (dr["EmailId"].ToString() != "")
            {
                model.EmailID = int.Parse(dr["EmailId"].ToString());
            }
            if (dr["EmailTo"] != null && dr["EmailTo"].ToString() != "")
            {
                model.EmailTo = dr["EmailTo"].ToString();
            }
            if (dr["EmailCc"] != null && dr["EmailCc"].ToString() != "")
            {
                model.EmailCc = dr["EmailCc"].ToString();
            }
            if (dr["EmailBcc"] != null && dr["EmailBcc"].ToString() != "")
            {
                model.EmailBcc = dr["EmailBcc"].ToString();
            }
            if (dr["EmailSubject"] != null && dr["EmailSubject"].ToString() != "")
            {
                model.Subject = dr["EmailSubject"].ToString();
            }
            if (dr["EmailBody"] != null && dr["EmailBody"].ToString() != "")
            {
                model.Body = dr["EmailBody"].ToString();
            }
        }

        public void QueueEmail(MailMessage message)
        {
            if (message != null)
            {
                int rows = 0;
                SqlParameter[] parameters = {
                    new SqlParameter("@EmailPriority", SqlDbType.Int,4),
                    new SqlParameter("@IsBodyHtml", SqlDbType.Bit,1),
                    new SqlParameter("@EmailTo", SqlDbType.NVarChar,2000),
                    new SqlParameter("@EmailCc", SqlDbType.NText),
                    new SqlParameter("@EmailBcc", SqlDbType.NText),
                    new SqlParameter("@EmailFrom", SqlDbType.NVarChar,256),
                    new SqlParameter("@EmailSubject", SqlDbType.NVarChar,1024),
                    new SqlParameter("@EmailBody", SqlDbType.NText)};
                parameters[0].Value = message.Priority;
                parameters[1].Value = message.IsBodyHtml;
                parameters[2].Value = YSWL.Common.Mail.EmailConvert.ToDelimitedString(message.To, ",");
                parameters[3].Value = YSWL.Common.Mail.EmailConvert.ToDelimitedString(message.CC, ",");
                parameters[4].Value = YSWL.Common.Mail.EmailConvert.ToDelimitedString(message.Bcc, ",");
                parameters[5].Value = message.From.Address;
                parameters[6].Value = message.Subject;
                parameters[7].Value = message.Body;
                DbHelperSQL.RunProcedure("sp_Emails_Enqueue", parameters, out rows);
            }
        }

        public int QueueSendingFailure(IList<int> list, int failureInterval, int maxNumberOfTries)
        {
            int rows = 0;
            SqlParameter[] parameters = {
                    new SqlParameter("@EmailId", SqlDbType.Int,4),
                    new SqlParameter("@FailureInterval", SqlDbType.Int),
                    new SqlParameter("@MaxNumberOfTries", SqlDbType.Int)};
            foreach (int guid in list)
            {
                parameters[0].Value = guid;
                parameters[1].Value = failureInterval;
                parameters[2].Value = maxNumberOfTries;
                DbHelperSQL.RunProcedure("sp_EmailQueue_Failure", parameters, out rows);
            }
            return rows;
        }
    }
}