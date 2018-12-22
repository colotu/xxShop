using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mail;
using System.Xml;
using System.Configuration;
using System.Web;
using System.IO;
using System.Net;
using System.Net.Mail;
using MailMessage = System.Net.Mail.MailMessage;

namespace YSWL.Common
{
    /// <summary>
    ///  邮件发送类
    /// </summary>
    public class MailSender
    {

        public static void Send(string tomail, string bccmail, string subject, string body, params string[] files)
        {
            Send(SmtpConfig.Create().SmtpSetting.Sender, tomail, bccmail, subject, body, true, Encoding.Default, true, files);
        }        


        public static void Send(string frommail, string tomail,string bccmail, string subject,
                        string body, bool isBodyHtml, Encoding encoding, bool isAuthentication, params string[] files)
        {            
            Send(SmtpConfig.Create().SmtpSetting.Server, SmtpConfig.Create().SmtpSetting.UserName, SmtpConfig.Create().SmtpSetting.Password, frommail,
                tomail,"", bccmail, subject, body, isBodyHtml, encoding, isAuthentication,false, files);
        }



        public static void Send(string server,string username,string password, string frommail, string tomail, string ccmail,string bccmail, string subject,
                        string body, bool isBodyHtml, Encoding encoding, bool isAuthentication,bool isSsl, params string[] files)
        {


           // SmtpClient smtpClient = new SmtpClient(server);
            //MailAddress from = new MailAddress("ben@contoso.com", "Ben Miller");
            //MailAddress to = new MailAddress("jane@contoso.com", "Jane Clayton");
            MailMessage message = new MailMessage();

            if (!String.IsNullOrWhiteSpace(tomail))
            {
                message = new MailMessage(frommail, tomail);
            }
            else
            {
                message.From = new MailAddress(frommail);
            }
            System.Net.Mail.SmtpClient smtpClient = null;
            //163邮箱特殊处理
            if (server.Contains("163.com"))
            {
                smtpClient = new System.Net.Mail.SmtpClient("smtp.163.com");
                smtpClient.Credentials = new System.Net.NetworkCredential(message.From.Address, password);
            }
            else
            {
                smtpClient = new SmtpClient
                {
                    Host = server,
                    EnableSsl = isSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(message.From.Address, password)
                };
            }
            

           
            //if (isAuthentication == true)
            //{
            //    smtpClient.Credentials = new NetworkCredential(username, password);
            //    smtpClient.EnableSsl = true;
            //}

            if (!String.IsNullOrWhiteSpace(bccmail))
            {
                string[] maillist = YSWL.Common.StringPlus.GetStrArray(bccmail);
                foreach (string m in maillist)
                {
                    if (m.Trim() != "")
                    {
                        MailAddress bcc = new MailAddress(m.Trim());
                        message.Bcc.Add(bcc);
                    }
                }
            }
            if (!String.IsNullOrWhiteSpace(ccmail))
            {
                string[] maillist = YSWL.Common.StringPlus.GetStrArray(ccmail);
                foreach (string m in maillist)
                {
                    if (m.Trim() != "")
                    {
                        MailAddress cc = new MailAddress(m.Trim());
                        message.CC.Add(cc);
                    }
                }
            }            
            message.IsBodyHtml = isBodyHtml;
            message.SubjectEncoding = encoding;
            message.BodyEncoding = encoding;

            message.Subject = subject;
            message.Body = body;

            message.Attachments.Clear();
            if (files != null && files.Length != 0)
            {
                for (int i = 0; i < files.Length; ++i)
                {
                    Attachment attach = new Attachment(files[i]);
                    message.Attachments.Add(attach);
                }
            }

       
            smtpClient.Send(message);
            message.Attachments.Dispose();

        }


        public static void sendEmail(string frommail, string password, string tomail, string ccmail, string bccmail, string subject,
                        string body)
        {
            try
            {
                System.Web.Mail.MailMessage myMail = new System.Web.Mail.MailMessage();
                myMail.Fields.Add
                    ("http://schemas.microsoft.com/cdo/configuration/smtpserver",
                                  "smtp.exmail.qq.com");
                myMail.Fields.Add
                    ("http://schemas.microsoft.com/cdo/configuration/smtpserverport",
                                  "465");
                myMail.Fields.Add
                    ("http://schemas.microsoft.com/cdo/configuration/sendusing",
                                  "2");
                //sendusing: cdoSendUsingPort, value 2, for sending the message using 
                //the network.

                //smtpauthenticate: Specifies the mechanism used when authenticating 
                //to an SMTP 
                //service over the network. Possible values are:
                //- cdoAnonymous, value 0. Do not authenticate.
                //- cdoBasic, value 1. Use basic clear-text authentication. 
                //When using this option you have to provide the user name and password 
                //through the sendusername and sendpassword fields.
                //- cdoNTLM, value 2. The current process security context is used to 
                // authenticate with the service.
                myMail.Fields.Add
                ("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
                //Use 0 for anonymous
                myMail.Fields.Add
                ("http://schemas.microsoft.com/cdo/configuration/sendusername",
                    frommail);
                myMail.Fields.Add
                ("http://schemas.microsoft.com/cdo/configuration/sendpassword",
                     password);
                myMail.Fields.Add
                ("http://schemas.microsoft.com/cdo/configuration/smtpusessl",
                     "true");
                myMail.From = frommail;

                //if (!String.IsNullOrWhiteSpace(bccmail))
                //{
                //    string[] maillist = YSWL.Common.StringPlus.GetStrArray(bccmail);
                //    foreach (string m in maillist)
                //    {
                //        if (m.Trim() != "")
                //        {
                //            MailAddress bcc = new MailAddress(m.Trim());
                //            myMail.Bcc
                //        }
                //    }
                //}
                //if (!String.IsNullOrWhiteSpace(ccmail))
                //{
                //    string[] maillist = YSWL.Common.StringPlus.GetStrArray(ccmail);
                //    foreach (string m in maillist)
                //    {
                //        if (m.Trim() != "")
                //        {
                //            MailAddress cc = new MailAddress(m.Trim());
                //            message.CC.Add(cc);
                //        }
                //    }
                //}            
                myMail.Bcc = bccmail;
                myMail.Subject = subject;
                myMail.BodyFormat =new MailFormat();
                myMail.Body = body;
                //if (pAttachmentPath.Trim() != "")
                //{
                //    MailAttachment MyAttachment =
                //            new MailAttachment(pAttachmentPath);
                //    myMail.Attachments.Add(MyAttachment);
                //    myMail.Priority = System.Web.Mail.MailPriority.High;
                //}

                System.Web.Mail.SmtpMail.SmtpServer = "smtp.exmail.qq.com:465";
                System.Web.Mail.SmtpMail.Send(myMail);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 重载发送邮件的方法（增加邮件的回复地址）
        /// </summary>
        /// <param name="server"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="frommail"></param>
        /// <param name="tomail"></param>
        /// <param name="ccmail"></param>
        /// <param name="bccmail"></param>
        /// <param name="replymail"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isBodyHtml"></param>
        /// <param name="encoding"></param>
        /// <param name="isAuthentication"></param>
        /// <param name="isSsl"></param>
        /// <param name="files"></param>
        public static void Send(string server,string username,string password, string frommail, string tomail, string ccmail,string bccmail, string replymail,string subject,
                        string body, bool isBodyHtml, Encoding encoding, bool isAuthentication,bool isSsl, params string[] files)
        {


            //SmtpClient smtpClient = new SmtpClient(server);
            //MailAddress from = new MailAddress("ben@contoso.com", "Ben Miller");
            //MailAddress to = new MailAddress("jane@contoso.com", "Jane Clayton");
             MailMessage message=new MailMessage();
            if (!String.IsNullOrWhiteSpace(tomail))
            {
                message = new MailMessage(frommail, tomail);
            }
            else
            {
                message.From = new MailAddress(frommail);
            }
            System.Net.Mail.SmtpClient smtpClient = null;
            //163邮箱特殊处理
            if (server.Contains("163.com"))
            {
                smtpClient = new System.Net.Mail.SmtpClient("smtp.163.com");
                smtpClient.Credentials = new System.Net.NetworkCredential(message.From.Address, password);
            }
            else
            {
                smtpClient = new SmtpClient
                {
                    Host = server,
                    EnableSsl = isSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(message.From.Address, password)
                };
            }
          

            if (!String.IsNullOrWhiteSpace(bccmail))
            {
                string[] maillist = YSWL.Common.StringPlus.GetStrArray(bccmail);
                foreach (string m in maillist)
                {
                    if (m.Trim() != "")
                    {
                        MailAddress bcc = new MailAddress(m.Trim());
                        message.Bcc.Add(bcc);
                    }
                }
            }
            if (!String.IsNullOrWhiteSpace(ccmail))
            {
                string[] maillist = YSWL.Common.StringPlus.GetStrArray(ccmail);
                foreach (string m in maillist)
                {
                    if (m.Trim() != "")
                    {
                        MailAddress cc = new MailAddress(m.Trim());
                        message.CC.Add(cc);
                    }
                }
            }
            if (!String.IsNullOrWhiteSpace(replymail))
            {
                string[] maillist = YSWL.Common.StringPlus.GetStrArray(replymail);
                foreach (string m in maillist)
                {
                    if (m.Trim() != "")
                    {
                        MailAddress cc = new MailAddress(m.Trim());
                        message.ReplyToList.Add(cc);
                    }
                }
            }
            message.IsBodyHtml = isBodyHtml;
            message.SubjectEncoding = encoding;
            message.BodyEncoding = encoding;

            message.Subject = subject;
            message.Body = body;

            message.Attachments.Clear();
            if (files != null && files.Length != 0)
            {
                for (int i = 0; i < files.Length; ++i)
                {
                    Attachment attach = new Attachment(files[i]);
                    message.Attachments.Add(attach);
                }
            }
          
            smtpClient.Send(message);
            message.Attachments.Dispose();

        }

        public static void Send(string recipient, string subject, string body)
        {
            Send( SmtpConfig.Create().SmtpSetting.Sender, recipient,"", subject, body, true, Encoding.Default, true, null);
        }

        public static void Send(string Recipient, string Sender, string Subject, string Body)
        {
            Send(Sender, Recipient, "",Subject, Body, true, Encoding.UTF8, true, null);
        }
                
    }

    public class SmtpSetting
    {
        private string _server;

        public string Server
        {
            get { return _server; }
            set { _server = value; }
        }
        private bool _authentication;

        public bool Authentication
        {
            get { return _authentication; }
            set { _authentication = value; }
        }
        private string _username;

        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }
        private string _sender;

        public string Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }
        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
    }

    public class SmtpConfig
    {
        private static SmtpConfig _smtpConfig;
        private string ConfigFile
        {
            get
            {
                string configPath = ConfigurationManager.AppSettings["SmtpConfigPath"];
                if (string.IsNullOrEmpty(configPath) || configPath.Trim().Length == 0)
                {
                    configPath = HttpContext.Current.Request.MapPath("/Config/SmtpSetting.config");
                }
                else
                {
                    if (!Path.IsPathRooted(configPath))
                        configPath = HttpContext.Current.Request.MapPath(Path.Combine(configPath, "SmtpSetting.config"));
                    else
                        configPath = Path.Combine(configPath, "SmtpSetting.config");
                }
                return configPath;
            }
        }
        public SmtpSetting SmtpSetting
        {
            get
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(this.ConfigFile);
                SmtpSetting smtpSetting = new SmtpSetting();
                smtpSetting.Server = doc.DocumentElement.SelectSingleNode("Server").InnerText;
                smtpSetting.Authentication = Convert.ToBoolean(doc.DocumentElement.SelectSingleNode("Authentication").InnerText);
                smtpSetting.UserName = doc.DocumentElement.SelectSingleNode("User").InnerText;
                smtpSetting.Password = doc.DocumentElement.SelectSingleNode("Password").InnerText;
                smtpSetting.Sender = doc.DocumentElement.SelectSingleNode("Sender").InnerText;

                return smtpSetting;
            }
        }
        private SmtpConfig()
        {

        }
        public static SmtpConfig Create()
        {
            if (_smtpConfig == null)
            {
                _smtpConfig = new SmtpConfig();
            }
            return _smtpConfig;
        }
    }
}
