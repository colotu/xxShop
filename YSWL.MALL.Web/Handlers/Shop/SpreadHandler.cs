using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using YSWL.Web;

namespace YSWL.MALL.Web.Handlers.Shop
{
    public class SpreadHandler : IHttpHandler, IRequiresSessionState
    {
        #region IHttpHandler 成员

        public bool IsReusable
        {
            get { return false; }
        }
        public void ProcessRequest(HttpContext context)
        {
            string code = context.Request.Params["s"];
            string area = context.Request.Params["area"];
            //推广地址
            if (!String.IsNullOrWhiteSpace(code))
            {
                //还要解码
                code = YSWL.Common.UrlOper.Base64Decrypt(code);
                int index = code.IndexOf("&");
                if (index == -1)
                {
                    Redirect(context, "/");
                }
                string p = code.Substring(0, index);
                string r = code.Substring(index + 1);
                long productId = Common.Globals.SafeLong(p, 0);
                string agent = context.Request.UserAgent;
                string tag = string.Empty;
                
                if (productId > 0)
                {
                    // 处理ReturnUrl
                    string returnUrl = MvcApplication.GetCurrentRoutePath(AreaRoute.Shop)+"Product/Detail/" + productId + "?"+tag+"r=" + YSWL.Common.UrlOper.Base64Encrypt(r);
                    if (MatchAgent(agent))
                    {
                        if (!String.IsNullOrWhiteSpace(area) && area.ToLower() == AreaRoute.MB.ToString().ToLower())
                        {
                            returnUrl = MvcApplication.GetCurrentRoutePath(AreaRoute.MB) + "Product/Detail/" + productId + "?" + tag + "r=" + YSWL.Common.UrlOper.Base64Encrypt(r);
                        }
                        else
                        {
                            returnUrl = MvcApplication.GetCurrentRoutePath(AreaRoute.MShop) + "Product/Detail/" + productId + "?" + tag + "r=" + YSWL.Common.UrlOper.Base64Encrypt(r);
                        }
                    }
                    Redirect(context, returnUrl);
                }
            }
            Redirect(context, "/");
        }

        private void Redirect(HttpContext context, string url)
        {
            context.Response.Clear();
            context.Response.Write(
                string.Format("<script type=\"text/javascript\">window.location.replace('{0}');</script>", url));
            context.Response.End();
        }

        /// <summary>
        /// 是否是移动设备
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        private bool MatchAgent(string agent)
        {
            string[] keywords = { "Android", "iPhone", "iPod", "iPad", "Windows Phone", "MQQBrowser" };
            bool flag = false;
            foreach (string item in keywords)
            {
                if (agent.Contains(item))
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        #endregion
    }
}