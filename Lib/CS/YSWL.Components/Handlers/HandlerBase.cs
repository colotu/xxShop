/**
* HandlerBase.cs
*
* 功 能： Handler基类
* 类 名： HandlerBase
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/11/13 20:04:09  Ben    初版
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System.Web;
using YSWL.Common;

namespace YSWL.Components.Handlers
{
    public abstract class HandlerBase : IHttpHandler
    {
        public const string KEY_STATUS = "STATUS";
        public const string KEY_DATA = "DATA";

        public const string STATUS_SUCCESS = "SUCCESS";
        public const string STATUS_FAILED = "FAILED";
        public const string STATUS_ERROR = "ERROR";

        public const string STATUS_NODATA = "NODATA";
        public const string STATUS_NOLOGIN = "NOLOGIN";
        public const string STATUS_UNAUTHORIZED = "UNAUTHORIZED";

        public abstract bool IsReusable { get; }
        public abstract void ProcessRequest(HttpContext context);

        #region 当前登录用户
        /// <summary>
        /// 当前登录用户
        /// </summary>
        protected YSWL.Accounts.Bus.User CurrentUser
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context == null || !context.User.Identity.IsAuthenticated)
                {
                    return null;
                }
                YSWL.Accounts.Bus.User currentUser;
                if (context.Session[Globals.SESSIONKEY_USER] == null)
                {
                    currentUser = new YSWL.Accounts.Bus.User(
                        new YSWL.Accounts.Bus.AccountsPrincipal(context.User.Identity.Name));
                    context.Session[Globals.SESSIONKEY_USER] = currentUser;
                }
                else
                {
                    currentUser = (YSWL.Accounts.Bus.User)context.Session[Globals.SESSIONKEY_USER];
                }
                return currentUser;
            }
        }

        #endregion

    }
}