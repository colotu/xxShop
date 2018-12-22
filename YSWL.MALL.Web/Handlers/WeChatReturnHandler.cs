using System;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace YSWL.MALL.Web.Handlers
{
    public class WeChatReturnHandler : IHttpHandler, IRequiresSessionState
    {
        #region IHttpHandler 成员

        public bool IsReusable
        {
            get { return false; }
        }
        public void ProcessRequest(HttpContext context)
        {
            
            string code = context.Request.Params["code"];
            //微信授权
            if (!String.IsNullOrWhiteSpace(code))
            {
                string state = Common.Globals.UrlDecode(context.Request.Params["state"]);
                string openId = state.Substring(0, state.IndexOf("|"));
                string returnUrl = state.Substring(state.IndexOf("|") + 1);

                string appId = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_AppId", openId);
                string appSercet = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_AppSercet", openId);
                if (!String.IsNullOrWhiteSpace(appId) && !String.IsNullOrWhiteSpace(appSercet))
                {
                    //网页授权
                    string userOpenId = YSWL.WeChat.BLL.Core.Utils.GetUserOpenId(appId, appSercet, code);
                    // 处理ReturnUrl
                    context.Session["WeChat_UserName"] = userOpenId; 
                    context.Session["WeChat_OpenId"] = openId;

                    YSWL.Log.LogHelper.AddInfoLog("WeChat_UserName", userOpenId);
                    YSWL.Log.LogHelper.AddInfoLog("WeChat_OpenId", openId);

                    #region 记录用户浏览页面
                    YSWL.WeChat.BLL.Core.OPLog.AddViewLog(userOpenId, openId, returnUrl);
                    #endregion
                    Redirect(context, returnUrl);
                }
            }
            else
            {
                string Domain = "http://"+Common.Globals.DomainFullName;
                string mpKey = context.Request.QueryString["mp"];
                string returnUrl = Common.Globals.UrlDecode(context.Request.QueryString["returnUrl"]);
                bool isRepeat = Common.Globals.SafeBool(context.Request.QueryString["rep"], true);
                if (!string.IsNullOrWhiteSpace(mpKey))
                {
                    string baseUrl = "http://" + Common.Globals.DomainFullName + "/wcreturn.aspx?returnUrl={0}&mp={1}&rep={2}";
                    
                    string link = String.Format(baseUrl, Common.Globals.UrlEncode(returnUrl), mpKey, isRepeat);
                    //解码
                    string mpKey_D = YSWL.Common.DEncrypt.DESEncrypt.Decrypt(mpKey);
                    returnUrl = returnUrl.Replace(Domain, "");
                    if (String.IsNullOrWhiteSpace(mpKey_D))
                    {

                        if (returnUrl.Contains("http://"))
                        {
                            Redirect(context, returnUrl);
                        }
                        else
                        {
                              context.Server.TransferRequest(returnUrl, false);
                        }
                      
                        return;
                    }
                    var arrrKey = mpKey_D.Split('|');
                    if (arrrKey.Count() < 2 || String.IsNullOrWhiteSpace(arrrKey[0]) || String.IsNullOrWhiteSpace(arrrKey[1]))
                    {
                        if (returnUrl.Contains("http://"))
                        {
                            Redirect(context, returnUrl);
                        }
                        else
                        {
                            context.Server.TransferRequest(returnUrl, false);
                        }
                        return;
                    }
                    #region 记录用户浏览页面
                    YSWL.WeChat.BLL.Core.OPLog.AddViewLog(arrrKey[0], arrrKey[1], returnUrl);
                    #endregion
                    //允许重复访问
                    if (isRepeat)
                    {
                        context.Session["WeChat_UserName"] = arrrKey[0];
                        context.Session["WeChat_OpenId"] = arrrKey[1];
                        if (returnUrl.Contains("http://"))
                        {
                            Redirect(context, returnUrl);
                        }
                        else
                        {
                            context.Server.TransferRequest(returnUrl, false);
                        }
                        return;
                    }
                    if (YSWL.WeChat.BLL.Core.LinkLog.ExistsEx(link))
                    {
                        context.Session["WeChat_UserName"] = arrrKey[0];
                        context.Session["WeChat_OpenId"] = arrrKey[1];
                        YSWL.WeChat.BLL.Core.LinkLog.DeleteEx(link);
                        if (returnUrl.Contains("http://"))
                        {
                            Redirect(context, returnUrl);
                        }
                        else
                        {
                            context.Server.TransferRequest(returnUrl, false);
                        }
                        return;
                    }
                }
            }
            context.Server.TransferRequest("/COM/WeChat/FailLink", true);
        }

        private void Redirect(HttpContext context, string url)
        {
            context.Response.Clear();
            context.Response.Write(
                string.Format("<script type=\"text/javascript\">window.location.replace('{0}');</script>", url));
            context.Response.End();
        }

        #endregion
    }
}