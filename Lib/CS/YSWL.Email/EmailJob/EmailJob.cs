/**
* EmailJob.cs
*
* 功 能： [队列发送邮件]
* 类 名： EmailJob
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/5/23 12:54:12  Rock    初版
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Xml;
using YSWL.Email.EmailJob;

namespace YSWL.Email.EmailJob
{
    public class EmailJob : IJob
    {
        private int _failureInterval = 15;
        private int _numberOfTries = 5;

        public void Execute(XmlNode node)
        {
            if (node != null)
            {
                XmlAttribute attribute = node.Attributes["failureInterval"];
                XmlAttribute attribute2 = node.Attributes["numberOfTries"];
                if (attribute != null)
                {
                    try
                    {
                        this._failureInterval = int.Parse(attribute.Value, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        this._failureInterval = 15;
                    }
                }
                if (attribute2 != null)
                {
                    try
                    {
                        this._numberOfTries = int.Parse(attribute2.Value, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        this._numberOfTries = 5;
                    }
                }
                this.SendQueuedEmailJob();
            }
        }

        public void SendQueuedEmailJob()
        {
            SendQueuedEmails(this._failureInterval, this._numberOfTries);
        }

        private void SendQueuedEmails(int failureInterval, int maxNumberOfTries)
        {
            Model.MailConfig site = new BLL.MailConfig().GetModel();

            if ((site != null))
            {
                IList<YSWL.Email.Model.EmailTemplate> list = YSWL.Email.BLL.EmailQueueProvider.DequeueEmail();// EmailQueueProvider.Instance().DequeueEmail();
                IList<int> list2 = new List<int>();

                //SmtpClient client = new SmtpClient(site.SMTPServer, site.SMTPPort)
                //{
                //    UseDefaultCredentials = false
                //};
                //client.Credentials = new NetworkCredential(site.Username, Common.DEncrypt.DESEncrypt.Decrypt(site.Password));
                //client.EnableSsl = false;// HiContext.Current.SiteSettings.SmtpEnableSsl;
                //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                int num = 0;
                int num2 = 0;
                short smtpServerConnectionLimit = 0;// config.SmtpServerConnectionLimit;
                foreach (YSWL.Email.Model.EmailTemplate template in list)
                {
                    try
                    {
                        template.From = new MailAddress(site.Mailaddress);
                        template.BodyEncoding = Encoding.UTF8;
                        template.Body = template.Body.Replace("\n", "\r\n");

                        //client.Send(template);
#warning YSWL.Common.MailSender.Send - isAuthentication 参数未公开, SMTP 服务器要求安全连接或客户端未通过身份验证 To 孙鹏 BEN ADD 2012-12-11
                        YSWL.Common.MailSender.Send(site.SMTPServer, site.Username, YSWL.Common.DEncrypt.DESEncrypt.Decrypt(site.Password), site.Mailaddress, 
                            template.EmailTo, template.EmailCc,template.EmailBcc, template.Subject, template.Body, true, Encoding.Default, true,site.SMTPSSL, null);
                       //YSWL.Common.MailSender.sendEmail(site.Mailaddress, site.Password,"",,template.Emailcc,template.EmailBcc,template.Subject, template.Body);
                        YSWL.Email.BLL.EmailQueueProvider.DeleteQueuedEmail(template.EmailID);
                        if ((smtpServerConnectionLimit != -1) && (++num >= smtpServerConnectionLimit))
                        {
                            Thread.Sleep(new TimeSpan(0, 0, 0, 15, 0));
                            num = 0;
                        }
                    }
                    catch(Exception ex)
                    {
                        StringBuilder log=new StringBuilder();
                        log.AppendFormat("发送邮件失败，错误信息为：{0},错误堆栈信息为{1}", ex.Message, ex.StackTrace);
                        YSWL.Common.FileManage.WriteText(log);
                        list2.Add(template.EmailID);
                    }
                    num2++;
                    if ((num2 >= 5))
                    {
                        YSWL.Email.BLL.EmailQueueProvider.DeleteQueuedEmail(template.EmailID);
                    }
                }
                if (list2.Count > 0)
                {
                    YSWL.Email.BLL.EmailQueueProvider.QueueSendingFailure(list2, failureInterval, maxNumberOfTries);
                }
            }
        }
    }
}