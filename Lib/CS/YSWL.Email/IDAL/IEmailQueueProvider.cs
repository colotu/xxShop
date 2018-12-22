/**
* IEmailQueueProvider.cs
*
* 功 能： [N/A]
* 类 名： IEmailQueueProvider
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/3/4 19:45:13  Ben    初版
*
* Copyright (c) 2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System.Collections.Generic;
using System.Net.Mail;

namespace YSWL.Email.IDAL
{

    public interface IEmailQueueProvider
    {
        int DeleteQueuedEmail(int emailId);
        IList<YSWL.Email.Model.EmailTemplate> DequeueEmail();
        void QueueEmail(MailMessage message);
        int QueueSendingFailure(IList<int> list, int failureInterval, int maxNumberOfTries);
    }

}
