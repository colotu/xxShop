/**
* EmailManage.cs
*
* 功 能： [N/A]
* 类 名： EmailManage
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/10/12 11:00:07  Rock    初版
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System.Collections.Generic;
using YSWL.Email.BLL;

namespace YSWL.Email
{
    public static class EmailManage
    {
        private static EmailQueue emailQueueManage = new EmailQueue();

        /// <summary>
        /// 加入邮件队列（单封邮件）
        /// </summary>
        /// <param name="model">单个邮件对象</param>
        /// <returns>是否成功加入队列</returns>
        public static bool PushQueue(Model.EmailQueue model)
        {
            return emailQueueManage.Add(model);
        }

        /// <summary>
        /// 加入邮件队列（群发邮件）
        /// </summary>
        /// <param name="list">邮件对象集合</param>
        public static void PushQueue(List<Model.EmailQueue> list)
        {
            if (list == null || list.Count < 1) return;
            list.ForEach(xx => emailQueueManage.Add(xx));
        }
        /// <summary>
        /// 加入邮件队列（群发邮件）
        /// </summary>
        /// <param name="userType">群组类型，例如：会员或管理员（群发）</param>
        /// <param name="emailSubject">邮件标题</param>
        /// <param name="emailBody">邮件内容</param>
        /// <param name="emailFrom">发件人</param>
        /// <returns>是否成功加入队列</returns>
        public static bool PushQueue(string userType, string emailFrom,
            string emailSubject, string emailBody)
        {
            return emailQueueManage.PushEmailQueur(userType, "", emailSubject, emailBody, emailFrom);
        }
        /// <summary>
        /// 加入邮件队列（群发邮件）
        /// </summary>
        /// <param name="userType">群组类型，例如：会员或管理员（群发）</param>
        /// <param name="userName">收件人（用户登录账户）</param>
        /// <param name="emailSubject">邮件标题</param>
        /// <param name="emailBody">邮件内容</param>
        /// <param name="emailFrom">发件人</param>
        /// <returns>是否成功加入队列</returns>
        public static bool PushQueue(string userType, string userName, string emailSubject,
            string emailBody, string emailFrom)
        {
            return emailQueueManage.PushEmailQueur(userType, userName, emailSubject, emailBody, emailFrom);
        }
    }
}