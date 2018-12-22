using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using YSWL.DBUtility;
using YSWL.Email.IDAL;
using MySql.Data.MySqlClient;

namespace YSWL.Email.MySqlDAL
{
    public class EmailQueueProvider : IEmailQueueProvider
    {
        public int DeleteQueuedEmail(int emailId)
        {
            int rows = 0;
            MySqlParameter[] parameters = {
                    new MySqlParameter("?_EmailId", MySqlDbType.Int32,4)};
            parameters[0].Value = emailId;
            DbHelperMySQL.RunProcedure("sp_EmailQueue_Delete", parameters, out rows);
            return rows;
        }

        public IList<YSWL.Email.Model.EmailTemplate> DequeueEmail()
        {
            IList<YSWL.Email.Model.EmailTemplate> list = new List<YSWL.Email.Model.EmailTemplate>();
            MySqlParameter[] parameters = { };
            DataSet ds = DbHelperMySQL.RunProcedure("sp_Emails_Dequeue", parameters, "ds");
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
                MySqlParameter[] parameters = {
                    new MySqlParameter("?_EmailPriority", MySqlDbType.Int32,4),
                    new MySqlParameter("?_IsBodyHtml", MySqlDbType.Bit,1),
                    new MySqlParameter("?_EmailTo", MySqlDbType.VarChar,2000),
                    new MySqlParameter("?_EmailCc", MySqlDbType.Text),
                    new MySqlParameter("?_EmailBcc", MySqlDbType.Text),
                    new MySqlParameter("?_EmailFrom", MySqlDbType.VarChar,256),
                    new MySqlParameter("?_EmailSubject", MySqlDbType.VarChar,1024),
                    new MySqlParameter("?_EmailBody", MySqlDbType.Text)};
                parameters[0].Value = message.Priority;
                parameters[1].Value = message.IsBodyHtml;
                parameters[2].Value = YSWL.Common.Mail.EmailConvert.ToDelimitedString(message.To, ",");
                parameters[3].Value = YSWL.Common.Mail.EmailConvert.ToDelimitedString(message.CC, ",");
                parameters[4].Value = YSWL.Common.Mail.EmailConvert.ToDelimitedString(message.Bcc, ",");
                parameters[5].Value = message.From.Address;
                parameters[6].Value = message.Subject;
                parameters[7].Value = message.Body;
                DbHelperMySQL.RunProcedure("sp_Emails_Enqueue", parameters, out rows);
            }
        }

        public int QueueSendingFailure(IList<int> list, int failureInterval, int maxNumberOfTries)
        {
            int rows = 0;
            MySqlParameter[] parameters = {
                    new MySqlParameter("?_EmailId", MySqlDbType.Int32,4),
                    new MySqlParameter("?_FailureInterval", MySqlDbType.Int32),
                    new MySqlParameter("?_MaxNumberOfTries", MySqlDbType.Int32)};
            foreach (int guid in list)
            {
                parameters[0].Value = guid;
                parameters[1].Value = failureInterval;
                parameters[2].Value = maxNumberOfTries;
                DbHelperMySQL.RunProcedure("sp_EmailQueue_Failure", parameters, out rows);
            }
            return rows;
        }
    }
}