using System;
using System.Net.Mail;

namespace YSWL.Email.Model
{
    public class EmailTemplate : MailMessage
    {
        private string emailDescription;
        private int emailID;
        private string emailType;
        private int numberOfTries;
        private string tagDescription;
        private int templetId;
        private string emailBody;
        private string emailcc;
        private string emailbcc;

        /// <summary>
        /// 邮件内容
        /// </summary>
        public string EmailBody
        {
            get { return emailBody; }
            set { emailBody = value; }
        }

        private string emailTo;

        /// <summary>
        /// 邮件接收者
        /// </summary>
        public string EmailTo
        {
            get { return emailTo; }
            set { emailTo = value; }
        }

        /// <summary>
        /// 邮件描述信息
        /// </summary>
        public string EmailDescription
        {
            get
            {
                return this.emailDescription;
            }
            set
            {
                this.emailDescription = value;
            }
        }
        /// <summary>
        /// 抄送人
        /// </summary>
        public string EmailCc
        {
            set { emailcc = value; }
            get { return emailcc; }
        }
        /// <summary>
        /// 秘送人
        /// </summary>
        public string EmailBcc
        {
            set { emailbcc = value; }
            get { return emailbcc; }
        }

        /// <summary>
        /// 邮件ID
        /// </summary>
        public int EmailID
        {
            get
            {
                return this.emailID;
            }
            set
            {
                this.emailID = value;
            }
        }

        /// <summary>
        /// 邮件类型
        /// </summary>
        public string EmailType
        {
            get
            {
                return this.emailType;
            }
            set
            {
                this.emailType = value;
            }
        }

        /// <summary>
        /// 邮件发送失败尝试次数
        /// </summary>
        public int NumberOfTries
        {
            get
            {
                return this.numberOfTries;
            }
            set
            {
                this.numberOfTries = value;
            }
        }

        public string TagDescription
        {
            get
            {
                return this.tagDescription;
            }
            set
            {
                this.tagDescription = value;
            }
        }

        public int TempletId
        {
            get
            {
                return this.templetId;
            }
            set
            {
                this.templetId = value;
            }
        }
    }
}