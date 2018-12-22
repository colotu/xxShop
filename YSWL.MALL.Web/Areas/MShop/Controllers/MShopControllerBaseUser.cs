using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YSWL.Accounts.Bus;
using System.Web.Security;

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{
    [MShopError]
    public class MShopControllerBaseUser : YSWL.MALL.Web.Controllers.ControllerBaseUser
    {
        //
        // GET: /Mobile/MobileControllerBaseUser/

        #region 覆盖父类的  ViewResult View 方法 用于ViewName动态判空
        protected new ViewResult View(string viewName, object model)
        {
            return !string.IsNullOrWhiteSpace(viewName) ? base.View(viewName, model) : View(model);
        }

        protected new ViewResult View(string viewName)
        {
            return !string.IsNullOrWhiteSpace(viewName) ? base.View(viewName) : View();
        }
        #endregion

        #region UserName
        public string UserOpen
        {
            get
            {
                if (Session["WeChat_UserName"] != null)
                {
                    return Session["WeChat_UserName"].ToString();
                }
                return String.Empty;
            }
        }
        #endregion

        #region  OpenId
        public string OpenId
        {
            get
            {
                if (Session["WeChat_OpenId"] != null)
                {
                    return Session["WeChat_OpenId"].ToString();
                }
                return String.Empty;
            }
        }
        #endregion

        public override ActionResult RedirectToLogin(ActionExecutingContext filterContext)
        {
            string rawurl = Request.RawUrl;
            bool IsAutoLogin = Common.Globals.SafeBool(YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_AutoLogin", -1, "AA"), false);

            #region  自动登陆
            bool IsNeedBind = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("SyStem_WeChat_UserBind");
            //不读Seession 里面的信息，防止信息错乱
            //if (Session[YSWL.Common.Globals.SESSIONKEY_USER] != null && CurrentUser != null && CurrentUser.UserType != "AA")
            //{
            //    BLL.Shop.Products.ShoppingCartHelper.LoadShoppingCart(currentUser.UserID);//加载购物车
            //    return String.IsNullOrWhiteSpace(rawurl) ? Redirect(ViewBag.BasePath + "u") : Redirect(rawurl);
            //}
            YSWL.WeChat.BLL.Core.User wUserBll = new WeChat.BLL.Core.User();
            if (String.IsNullOrWhiteSpace(OpenId) || String.IsNullOrWhiteSpace(UserOpen))
            {
                return Redirect(ViewBag.BasePath + "a/l/?returnUrl=" + Server.UrlEncode(rawurl));
            }
            YSWL.WeChat.Model.Core.User wUserModel = wUserBll.GetUser(OpenId, UserOpen);
            if (IsNeedBind)
            {
                if (wUserModel.UserId <= 0)
                {
                    return Redirect(ViewBag.BasePath + "a/l/?returnUrl=" + Server.UrlEncode(rawurl));
                }
                AccountsPrincipal userPrincipal = new AccountsPrincipal(wUserModel.UserId);
                currentUser = new YSWL.Accounts.Bus.User(userPrincipal);
                if (!currentUser.Activity)
                {
                    return Redirect(ViewBag.BasePath + "a/l/?returnUrl=" + Server.UrlEncode(rawurl));
                }
                HttpContext.User = userPrincipal;
                Session[YSWL.Common.Globals.SESSIONKEY_USER] = currentUser;
                FormsAuthentication.SetAuthCookie(currentUser.UserName, true);
                BLL.Shop.Products.ShoppingCartHelper.LoadShoppingCart(currentUser.UserID);//加载购物车
                return String.IsNullOrWhiteSpace(rawurl) ? Redirect(ViewBag.BasePath + "u") : Redirect(rawurl);
            }
            if (IsAutoLogin)
            {
                string AutoLoginUrl = ViewBag.BasePath + "Account/RegBind?returnUrl=" + Server.UrlEncode(rawurl);
                if (wUserModel.UserId <= 0)
                {
                    return Redirect(AutoLoginUrl);
                }
                AccountsPrincipal userPrincipal = new AccountsPrincipal(wUserModel.UserId);
                User currentUser = new YSWL.Accounts.Bus.User(userPrincipal);
                if (!currentUser.Activity)
                {
                    return Redirect(AutoLoginUrl);
                }
                HttpContext.User = userPrincipal;
                Session[YSWL.Common.Globals.SESSIONKEY_USER] = currentUser;
                FormsAuthentication.SetAuthCookie(currentUser.UserName, true);
                BLL.Shop.Products.ShoppingCartHelper.LoadShoppingCart(currentUser.UserID);//加载购物车
                return String.IsNullOrWhiteSpace(rawurl) ? Redirect(ViewBag.BasePath + "u") : Redirect(rawurl);
            }

            #endregion
            return Redirect(ViewBag.BasePath + "a/l/?returnUrl=" + Server.UrlEncode(rawurl));

            //return RedirectToAction("Login", "Account", new {id=1, area = "MShop", returnUrl = Server.UrlEncode(rawurl),viewname="url"});
        }
    }
}
