using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using YSWL.Accounts.Bus;
using YSWL.Common;
using YSWL.Components.Setting;
using YSWL.MALL.Model.SysManage;
using YSWL.MALL.ViewModel.Shop;
using YSWL.MALL.Web.Components.Setting.Shop;
using Webdiyer.WebControls.Mvc;
using CapCRL.Common.Bus.Jsons;
using System.Security.Cryptography;
using System.Net;
using System.Text;
using System.Configuration;
using System.IO;
using YSWL.Web;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{
    public class AccountController : MShopControllerBase
    {
        //private string strZjbXzLeave = ConfigurationSettings.AppSettings["zhicheng"].ToString();//会员限制职称
        //private string strZjbToQianbao = ConfigurationSettings.AppSettings["ZjbToQb"].ToString();//会员限制职称
        shopCom spcom = new shopCom();

        BLL.Pay.BalanceDetails balanceManage = new BLL.Pay.BalanceDetails();//转进钱包

        BLL.Members.PointsDetail pointBLL = new BLL.Members.PointsDetail();

        //
        // GET: /Mobile/Account/
        private YSWL.Accounts.Bus.User userBusManage = new YSWL.Accounts.Bus.User();
        private BLL.Members.Users userManage = new BLL.Members.Users();
        private BLL.Members.UsersExp userExpManage = new BLL.Members.UsersExp();
        private YSWL.MALL.BLL.Members.UserInvite inviteBll = new YSWL.MALL.BLL.Members.UserInvite();
        public ActionResult Index()
        {
            return View();
        }
        #region 登录
        public ActionResult Login(string returnUrl)
        {
            ViewBag.RegisterToggle = BLL.SysManage.ConfigSystem.GetValueByCache("Shop_RegisterToggle");//注册方式     
            bool IsCloseLogin = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_Close_Login");
            ViewBag.SMSIsOpen = BLL.SysManage.ConfigSystem.GetBoolValueByCache("Emay_SMS_IsOpen");//是否开启手机验证
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                ViewBag.returnUrl = returnUrl;
            }
         
            if (IsCloseLogin)
            {
                return Redirect(ViewBag.BasePath + "Error/TurnOff");
                // return RedirectToAction("TurnOff", "Error", new { id = 1, viewname = "url" });
            }
            bool IsNeedBind = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("SyStem_WeChat_UserBind");
            if (IsNeedBind)
            {
                YSWL.WeChat.BLL.Core.User wUserBll = new WeChat.BLL.Core.User();
                if (String.IsNullOrWhiteSpace(OpenId) || String.IsNullOrWhiteSpace(UserOpen))
                {
                    return View();
                }
                YSWL.WeChat.Model.Core.User wUserModel = wUserBll.GetUser(OpenId, UserOpen);
                if (wUserModel.UserId <= 0)
                {
                    return View();
                }
                AccountsPrincipal userPrincipal = new AccountsPrincipal(wUserModel.UserId);
                User currentUser = new YSWL.Accounts.Bus.User(userPrincipal);
                if (!currentUser.Activity)
                {
                    return View();
                }
                HttpContext.User = userPrincipal;
                Session[YSWL.Common.Globals.SESSIONKEY_USER] = currentUser;
                FormsAuthentication.SetAuthCookie(currentUser.UserName, true);
                return String.IsNullOrWhiteSpace(returnUrl) ? Redirect(ViewBag.BasePath + "u") : Redirect(returnUrl);
            }
            //string returnUrl = Request.QueryString["returnUrl"];

            if (HttpContext.User.Identity.IsAuthenticated && CurrentUser != null && CurrentUser.UserType != "AA")
                return Redirect(ViewBag.BasePath + "u");
            //return RedirectToAction("Index", "UserCenter", new { id = 1, viewname = "url" });

            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "登录" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View();
        }

        [HttpPost]
        public ActionResult Login(LogOnModel model, string returnUrl)
        {
            ViewBag.RegisterToggle = BLL.SysManage.ConfigSystem.GetValueByCache("Shop_RegisterToggle");//注册方式     
            bool IsCloseLogin = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_Close_Login");
            ViewBag.SMSIsOpen = BLL.SysManage.ConfigSystem.GetBoolValueByCache("Emay_SMS_IsOpen");//是否开启手机验证
            if (IsCloseLogin)
            {
                return Redirect(ViewBag.BasePath + "Error/TurnOff");
                //return RedirectToAction("TurnOff", "Error", new { id = 1, viewname = "url" });
            }

            if (ModelState.IsValid)
            {
                #region SEO 优化设置
                IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
                ViewBag.Title = "登录" + pageSetting.Title;
                ViewBag.Keywords = pageSetting.Keywords;
                ViewBag.Description = pageSetting.Description;
                #endregion
                AccountsPrincipal userPrincipal = AccountsPrincipal.ValidateLogin(model.UserName, model.Password);
                if (userPrincipal == null)
                {
                    //判断输入的用户名在会员系统里面是否存在                                        
                    if (spcom.IsHaveUsername(model.UserName, model.Password))
                    {
                        string strUserLave = spcom.GetUserLeave(model.UserName);//获取会员的职称

                        if (RegisterVIPuser(model.UserName, model.Password))
                        {
                            userPrincipal = AccountsPrincipal.ValidateLogin(model.UserName, model.Password);


                        }
                        else
                        {
                            ModelState.AddModelError("Message", "用户名或密码不正确, 请重新输入!");
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Message", "用户名或密码不正确, 请重新输入!");
                        return View(model);
                    }
                }
                User currentUser = new YSWL.Accounts.Bus.User(userPrincipal);
                if (!currentUser.Activity)
                {
                    ModelState.AddModelError("Message", "对不起，该帐号已被冻结或未激活，请联系管理员！");
                    return View(model);
                }
                HttpContext.User = userPrincipal;
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                Session[YSWL.Common.Globals.SESSIONKEY_USER] = currentUser;
                //登录成功加积分
                YSWL.MALL.BLL.Members.PointsDetail pointBll = new BLL.Members.PointsDetail();

                int pointers = pointBll.AddPoints(1, currentUser.UserID, "登录操作");
                int rankScore = BLL.Members.RankDetail.AddScore(1, currentUser.UserID, "登录操作");
                BLL.Shop.Products.ShoppingCartHelper.LoadShoppingCart(currentUser.UserID);




                ////--会员登录后，自动把会员商城积分转入商城本身的积分账户--------

                //if (spcom.GetUserTrueName(model.UserName).Length > 0)//判断是VIP会员
                //{

                //    YSWL.MALL.Model.Members.UsersExpModel userexpVIPmodel = userExpManage.GetModelByCache(currentUser.UserID);
                //    userexpVIPmodel.BodilyForm = "VIP";
                //    userExpManage.UpdateEx(userexpVIPmodel);


                //    int vipShopjifen = spcom.GetVipShopjfByusername(model.UserName);
                //    if (vipShopjifen > 0)//VIP会员积分大于0的话，把会员系统的商城积分转入商城的商城积分
                //    {
                //        spcom.UpVIPjfByuser(model.UserName, "Mall", vipShopjifen.ToString());//把会员系统的商城积分扣掉

                //        pointBLL.PointsHuzhuan(100, currentUser.UserID, "VIP会员把VIP商城积分转入商城的积分账户", Convert.ToInt32(vipShopjifen.ToString()), "", 0);//把会员系统的商城积分转入商城的商城积分
                //    }
                //}
                ////-----------------------------------------------------------------------


                bool IsNeedBind = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("SyStem_WeChat_UserBind");
                YSWL.Log.LogHelper.AddInfoLog("SyStem_WeChat_UserBind", IsNeedBind.ToString());
                if (IsNeedBind)
                {
                    YSWL.WeChat.BLL.Core.User wUserBll = new WeChat.BLL.Core.User();
                    YSWL.WeChat.Model.Core.User wUserModel = wUserBll.GetUser(OpenId, UserOpen);
                    YSWL.Log.LogHelper.AddInfoLog("SyStem_WeChat_UserBind-->OpenId,UserOpen", OpenId+"----"+ UserOpen);
                    if (wUserModel != null && wUserModel.UserId <= 0)
                    {
                        //绑定当前系统用户
                        wUserModel.UserId = currentUser.UserID;
                        wUserBll.UpdateUser(wUserModel);
                        #region 建立关联关系
                        YSWL.MALL.BLL.Members.UserInvite inviteBll = new YSWL.MALL.BLL.Members.UserInvite();
                        inviteBll.AddInvite(wUserModel.OpenId, wUserModel.UserName, currentUser.UserID, currentUser.UserName, currentUser.NickName);
                        #endregion
                    }
                }
                returnUrl = Server.UrlDecode(returnUrl);
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    //TempData["pointer"] = pointers;
                    return Redirect(ViewBag.BasePath + "u");
                    //return RedirectToAction("Index", "UserCenter", new { id=1,viewname = "url"});  

                }
            }
            return View(model);
        }

        public bool RegisterVIPuser(string strUsername, string strPwd)
        {
            bool returnRegB = false;
            int uid = -1;

            string strPassJM = FormsAuthentication.HashPasswordForStoringInConfigFile(strPwd.Replace("'", ""), "MD5");

            bool IsCloseRegisterSendEmail = BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_Close_RegisterEmailCheck");
            User newUser = new User();
            //DONE: 警告DB字段未对应: Email 字段 varchar(100) UserName 字段 varchar(50) 已完成 BEN DONE 2012-11-22
            newUser.UserName = strUsername;
            newUser.NickName = spcom.GetUserTrueName(strUsername);  //昵称名称相同

            newUser.TrueName = newUser.NickName;//model.TrueName; //真实名字
            newUser.Password = AccountsPrincipal.EncryptPassword(strPwd);

            newUser.Activity = true;
            newUser.UserType = "UU";
            newUser.Style = 1;
            newUser.User_dateCreate = DateTime.Now;
            newUser.User_cLang = "zh-CN";

            int userid = newUser.Create();

            #region
            //添加用户扩展表数据     
            BLL.Members.UsersExp ue = new BLL.Members.UsersExp();
            ue.UserID = userid;
            ue.BirthdayVisible = 0;
            ue.BirthdayIndexVisible = false;
            ue.Gravatar = string.Format("/{0}/User/Gravatar/{1}", MvcApplication.UploadFolder, userid);
            ue.ConstellationVisible = 0;
            ue.ConstellationIndexVisible = false;
            ue.NativePlaceVisible = 0;
            ue.NativePlaceIndexVisible = false;
            ue.RegionId = 0;
            //ue.RegionId = Common.Globals.SafeInt(model.RegionId, 0); //用户地点区域
            ue.AddressVisible = 0;
            ue.AddressIndexVisible = false;
            ue.BodilyFormVisible = 0;
            ue.BodilyFormIndexVisible = false;
            ue.BloodTypeVisible = 0;
            ue.BloodTypeIndexVisible = false;
            ue.MarriagedVisible = 0;
            ue.MarriagedIndexVisible = false;
            ue.PersonalStatusVisible = 0;
            ue.PersonalStatusIndexVisible = false;
            ue.LastAccessIP = "";
            ue.LastAccessTime = DateTime.Now;
            ue.LastLoginTime = DateTime.Now;
            ue.LastPostTime = DateTime.Now;
            ue.NickName = strUsername;//用户昵称

            // ue.PersonalDomain = U;//个人身份证号码
            ue.BodilyForm = "VIP";

            ue.MSN = strPwd;//把用户的明文密码填写到这个字段保存
            #endregion
            //注册来源
            ue.SourceType = (int)YSWL.MALL.Model.Members.Enum.SourceType.Cust;
            if (!ue.AddExp(ue, uid))
            {
                userManage.Delete(userid);
                userExpManage.Delete(userid);
                returnRegB = false;
            }
            else
            {
                returnRegB = true;
            }
            //清除Session 
            Session["SMSCode"] = null;
            Session["SMS_DATE"] = DateTime.MinValue;
            #region
            string DefaultGravatar = BLL.SysManage.ConfigSystem.GetValueByCache("DefaultGravatar");
            DefaultGravatar = string.IsNullOrEmpty(DefaultGravatar) ? "/Upload/User/Gravatar/Default.jpg" : DefaultGravatar;
            string TargetGravatarFile = BLL.SysManage.ConfigSystem.GetValueByCache("TargetGravatarFile");
            TargetGravatarFile = string.IsNullOrEmpty(TargetGravatarFile) ? "/Upload/User/Gravatar/" : TargetGravatarFile;
            string path = ControllerContext.HttpContext.Server.MapPath("/");
            if (System.IO.File.Exists(path + DefaultGravatar))
            {
                System.IO.File.Copy(path + DefaultGravatar, path + TargetGravatarFile + userid + ".jpg", true);
            }

            //注册加积分
            YSWL.MALL.BLL.Members.PointsDetail pointBll = new BLL.Members.PointsDetail();
            pointBll.AddPoints(2, userid, "注册成功");

            #endregion

            return returnRegB;
        }


        public ActionResult MyQRtest(string id, string viewName = "MyQRCode")
        {
            try
            {
                string IUserIdI = "123";

                return View(viewName);
            }
            catch
            {
                viewName = "";
            }

            return View(viewName);
        }


        [HttpPost]
        public ActionResult AjaxLogin(string UserName, string UserPwd)
        {
            bool IsCloseLogin = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_Close_Login");
            ViewBag.SMSIsOpen = BLL.SysManage.ConfigSystem.GetBoolValueByCache("Emay_SMS_IsOpen");//是否开启手机验证
            if (IsCloseLogin)
            {
                return Content("-1");
            }
            if (ModelState.IsValid)
            {
                AccountsPrincipal userPrincipal = AccountsPrincipal.ValidateLogin(UserName, UserPwd);
                if (userPrincipal != null)
                {
                    User currentUser = new YSWL.Accounts.Bus.User(userPrincipal);
                    if (!currentUser.Activity)
                    {
                        ModelState.AddModelError("Message", "对不起，该帐号已被冻结，请联系管理员！");
                    }
                    //if (currentUser.UserType == "AA")
                    //{
                    //    ModelState.AddModelError("Message", "您是管理员用户，您没有权限登录后台系统！") ;                        
                    //}
                    HttpContext.User = userPrincipal;
                    FormsAuthentication.SetAuthCookie(UserName, true);
                    Session[YSWL.Common.Globals.SESSIONKEY_USER] = currentUser;
                    //登录成功加积分
                    YSWL.MALL.BLL.Members.PointsDetail pointBll = new BLL.Members.PointsDetail();
                    int pointers = pointBll.AddPoints(1, currentUser.UserID, "登录操作");
                    int rankScore = BLL.Members.RankDetail.AddScore(1, currentUser.UserID, "登录操作");
                    BLL.Shop.Products.ShoppingCartHelper.LoadShoppingCart(currentUser.UserID);

                    bool IsNeedBind = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("SyStem_WeChat_UserBind");
                    YSWL.Log.LogHelper.AddInfoLog("SyStem_WeChat_UserBind", IsNeedBind.ToString());
                    if (IsNeedBind)
                    {
                        YSWL.WeChat.BLL.Core.User wUserBll = new WeChat.BLL.Core.User();
                        YSWL.WeChat.Model.Core.User wUserModel = wUserBll.GetUser(OpenId, UserOpen);
                        YSWL.Log.LogHelper.AddInfoLog("SyStem_WeChat_UserBind-->OpenId,UserOpen", OpenId + "----" + UserOpen);
                        if (wUserModel != null && wUserModel.UserId <= 0)
                        {
                            //绑定当前系统用户
                            wUserModel.UserId = currentUser.UserID;
                            wUserBll.UpdateUser(wUserModel);
                            #region 建立关联关系
                            YSWL.MALL.BLL.Members.UserInvite inviteBll = new YSWL.MALL.BLL.Members.UserInvite();
                            inviteBll.AddInvite(wUserModel.OpenId, wUserModel.UserName, currentUser.UserID, currentUser.UserName, currentUser.NickName);
                            #endregion
                        }
                    }


                    return Content("1|" + pointers.ToString());
                }
                else
                {
                    if (spcom.IsHaveUsername(UserName, UserPwd))
                    {
                        if (RegisterVIPuser(UserName, UserPwd))
                        {
                            userPrincipal = AccountsPrincipal.ValidateLogin(UserName, UserPwd);

                            HttpContext.User = userPrincipal;
                            FormsAuthentication.SetAuthCookie(UserName, true);
                            Session[YSWL.Common.Globals.SESSIONKEY_USER] = currentUser;
                            //登录成功加积分
                            YSWL.MALL.BLL.Members.PointsDetail pointBll = new BLL.Members.PointsDetail();
                            int pointers = pointBll.AddPoints(1, currentUser.UserID, "登录操作");
                            int rankScore = BLL.Members.RankDetail.AddScore(1, currentUser.UserID, "登录操作");
                            BLL.Shop.Products.ShoppingCartHelper.LoadShoppingCart(currentUser.UserID);
                            return Content("1|" + pointers.ToString());
                        }
                        else
                        {
                            return Content("0");
                        }
                    }
                    else
                    {
                        return Content("0");
                    }
                }
            }
            return Content("0");
        } 
        #region 退出
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Remove(Globals.SESSIONKEY_USER);
            Session.Clear();
            Session.Abandon();
            return Redirect(ViewBag.BasePath);
            // return RedirectToAction("Index", "Home", new { id=1,viewname = "url"});
        }
        #endregion
        #endregion

        #region 注册
        // public ActionResult Register()
        public ActionResult Register(string viewName, string id = "-1")
        {
            if (id == "-1")
            {
                id = viewName;
            }
            bool IsCloseRegister = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_Close_Register");

            ViewBag.SMSIsOpen = BLL.SysManage.ConfigSystem.GetBoolValueByCache("Emay_SMS_IsOpen");//是否开启手机验证
            if (IsCloseRegister)
            {
                return Redirect(ViewBag.BasePath + "Error/TurnOff");
                //return RedirectToAction("TurnOff", "Error", new { id=1,viewname = "url"});
            }
            if (CurrentUser != null && CurrentUser.UserType != "AA")
                return Redirect(ViewBag.BasePath + "u");
            //return RedirectToAction("Index", "UserCenter", new { id = 1, viewname = "url" });
            ViewBag.RegisterToggle = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_RegisterToggle");//注册方式
            RegisterModel regModel = new RegisterModel();

            ViewBag.Seconds = 0;
            if (Session["SMS_DATE"] != null && !String.IsNullOrWhiteSpace(Session["SMS_DATE"].ToString()))
            {
                DateTime smsDate = Globals.SafeDateTime(Session["SMS_DATE"].ToString(), DateTime.MinValue);
                if (smsDate != DateTime.MinValue)
                {
                    TimeSpan smsSeconds = smsDate.AddSeconds(60) - DateTime.Now;
                    ViewBag.Seconds = (int)smsSeconds.TotalSeconds;
                }
            }

            if (!String.IsNullOrWhiteSpace(id)&& id!="-1")
            {
                string InviteUserIdI = Common.DEncrypt.Hex16.Decode(id);
                int InviteuidI = Globals.SafeInt(InviteUserIdI, -1);//获取邀请人用户ID
                BLL.Members.Users userdd = new BLL.Members.Users();
                ViewBag.TjrName = userdd.GetUserName(InviteuidI) + " " + userdd.GetUserTrueNameByUsername(userdd.GetUserName(InviteuidI));

                regModel.InviteUserId = userdd.GetUserName(InviteuidI);

                ViewBag.hfSIsTjr = true;
            }
            else
            {
                ViewBag.hfSIsTjr = false;
            }


            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "注册" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View(regModel);
        }
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "注册" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            bool IsOpen = BLL.SysManage.ConfigSystem.GetBoolValueByCache("Emay_SMS_IsOpen");//是否开启手机验证
            ViewBag.SMSIsOpen = IsOpen;//是否开启手机验证
            bool IsCloseRegister = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_Close_Register");
            if (IsCloseRegister)
            {
                return Redirect(ViewBag.BasePath + "Error/TurnOff");
                //return RedirectToAction("TurnOff", "Error", new { id = 1, viewname = "url" });
            }

            string regStr = BLL.SysManage.ConfigSystem.GetValueByCache("Shop_RegisterToggle");//注册方式         
            ViewBag.RegisterToggle = regStr;

            //判断昵称是否已存在
            if (userBusManage.HasUserByNickName(model.NickName))
            {
                ViewBag.hasnickname = "昵称已被抢先使用，换一个试试";
                return View(model);
            }
            if (userBusManage.HasUserByUserName(model.UserName))
            {
                if (regStr == "Phone")
                {
                    ViewBag.hasemail = "该手机已被注册";
                }
                else
                {
                    ViewBag.hasemail = "该邮箱已被注册";
                }
                return View(model);
            }
            if (regStr == "Phone" && IsOpen && !VerifiySMSCode(model.UserName, model.SMSCode))
            {
                ViewBag.SCodeError = "短信校验码有误";
                return View(model);
            }
    

            // bool IsCloseRegisterSendEmail = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_Close_RegisterEmailCheck");
            User newUser = new User();
            //DONE: 警告DB字段未对应: Email 字段 varchar(100) UserName 字段 varchar(50) 已完成 BEN DONE 2012-11-22
            newUser.UserName = model.UserName;
            newUser.NickName = model.NickName;  //昵称名称相同
            newUser.Password = AccountsPrincipal.EncryptPassword(model.Password);
            if (regStr == "Phone")
            {
                newUser.Phone = model.UserName;
            }
            else
            {
                newUser.Email = model.UserName;
            }

            int uid = -1;

            if (!String.IsNullOrWhiteSpace(Request.Form["inviteid"]))
            {
                string id = Common.DEncrypt.Hex16.Decode(Request.Form["inviteid"]);
                uid = Globals.SafeInt(id, -1);
            }

            //判断邀请人是否在
            if (model.InviteUserId != null && model.InviteUserId.Length > 0)//输入邀请人
            {
                if (!userBusManage.HasUser(model.InviteUserId))//邀请人不存在
                {
                    ViewBag.SCodeError = "邀请人不存在，请换一个，或者不填写邀请人";
                    ViewBag.hfSIsTjr = false;

                    return View(model);
                }
                else//邀请人存在
                {
                    BLL.Members.Users userdd = new BLL.Members.Users();
                    uid = userdd.GetUserIdByUserName(model.InviteUserId);
                }
            }
            else
            {
            }

            //if (IsCloseRegisterSendEmail) //关闭
            newUser.Activity = true;
            //else //开启
            //    newUser.Activity = false;
            newUser.UserType = "UU";
            newUser.Style = 1;
            newUser.User_dateCreate = DateTime.Now;
            newUser.User_cLang = "zh-CN";
            newUser.Phone = model.Phone;

            //获取业务员ID
            bool IsOpenSales = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_Register_OpenSales");
            //int salesId = Common.Globals.SafeInt(model.EmployeeID, 0);
            //if (IsOpenSales)
            //{
            //   //YSWL.MALL.Model.Members.UsersExpModel salesModel = userExpManage.GetSalesModel(salesId);
            //   //newUser.EmployeeID = salesModel == null ? 0 : salesModel.UserID;
            //}

            newUser.EmployeeID = userManage.GetEmployeeIDByUserid(uid.ToString());//会员归属哪个店铺

            int userid = newUser.Create();
            if (userid == -100)
            {
                ModelState.AddModelError("Message", ErrorCodeToString(MembershipCreateStatus.DuplicateUserName));
            }
            else
            {
                //添加用户扩展表数据
                BLL.Members.UsersExp ue = new BLL.Members.UsersExp();
                ue.UserID = userid;
                ue.NickName = model.NickName; //昵称名称相同

                ue.BirthdayVisible = 0;
                ue.BirthdayIndexVisible = false;
                ue.Gravatar = string.Format("/{0}/User/Gravatar/{1}", MvcApplication.UploadFolder, userid);
                ue.ConstellationVisible = 0;
                ue.ConstellationIndexVisible = false;
                ue.NativePlaceVisible = 0;
                ue.NativePlaceIndexVisible = false;
                ue.RegionId = 0;
                ue.AddressVisible = 0;
                ue.AddressIndexVisible = false;
                ue.BodilyFormVisible = 0;
                ue.BodilyFormIndexVisible = false;
                ue.BloodTypeVisible = 0;
                ue.BloodTypeIndexVisible = false;
                ue.MarriagedVisible = 0;
                ue.MarriagedIndexVisible = false;
                ue.PersonalStatusVisible = 0;
                ue.PersonalStatusIndexVisible = false;
                ue.LastAccessIP = "";
                ue.LastAccessTime = DateTime.Now;
                ue.LastLoginTime = DateTime.Now;
                ue.LastPostTime = DateTime.Now;

                ue.NickName = model.NickName; //昵称名称相同
                ue.MSN = model.Password;
                //注册来源
                ue.SourceType = (int)YSWL.MALL.Model.Members.Enum.SourceType.WeChat;
                ////绑定业务员ID
                //if (newUser.EmployeeID > 0)
                //{
                //    ue.SalesId = salesId;
                //}
                if (!ue.AddExp(ue, uid))
                {
                    userManage.Delete(userid);
                    userExpManage.Delete(userid);
                    ModelState.AddModelError("Message", "注册失败！");
                    return View(model);
                }
                //清除Session 
                Session["SMSCode"] = null;
                Session["SMS_DATE"] = DateTime.MinValue;

                #region 默认数据
                string DefaultGravatar = BLL.SysManage.ConfigSystem.GetValueByCache("DefaultGravatar");
                DefaultGravatar = string.IsNullOrEmpty(DefaultGravatar) ? "/Upload/User/Gravatar/Default.jpg" : DefaultGravatar;
                string TargetGravatarFile = BLL.SysManage.ConfigSystem.GetValueByCache("TargetGravatarFile");
                TargetGravatarFile = string.IsNullOrEmpty(TargetGravatarFile) ? "/Upload/User/Gravatar/" : TargetGravatarFile;
                string path = ControllerContext.HttpContext.Server.MapPath("/");
                if (System.IO.File.Exists(path + DefaultGravatar))
                {
                    System.IO.File.Copy(path + DefaultGravatar, path + TargetGravatarFile + userid + ".jpg", true);
                }
                //if (!IsCloseRegisterSendEmail) //开启了发送邮件的功能
                //{
                //SendEmail(model.Email, model.Email, 0);
                //  return RedirectToAction("RegisterSuccess", "Account", new { email = model.Email });// return Redirect(ViewBag.BasePath + "Account/RegisterSuccess");
                //}
                //else
                //{
                FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                //注册加积分

                #region 生 成 会员 二维码 

                string area = BLL.SysManage.ConfigSystem.GetValueByCache("MainArea");
                string basepath = "/";
                if (area.ToLower() != AreaRoute.MShop.ToString().ToLower())
                {
                    basepath = "/MShop/";
                }
                string _uploadFolder = string.Format("/{0}/QRcode/", MvcApplication.UploadFolder);
                string filename = string.Format("{0}.png", userid);
                string mapPath = Request.MapPath(_uploadFolder);
                string mapPathQRImgUrl = mapPath + filename;

                string baseURL = string.Format("/tools/qr/gen.aspx?margin={0}&size={1}&level={2}&format={3}&content={4}", 0, 180, "30%", "png", "{0}");
                string websiteUrl = "http://" + Globals.DomainFullName + basepath + "Account/Register/" + Common.DEncrypt.Hex16.Encode(userid.ToString());
                websiteUrl = "http://" + Globals.DomainFullName + string.Format(baseURL, Common.Globals.UrlEncode(websiteUrl));
                if (!Directory.Exists(mapPath))
                {
                    Directory.CreateDirectory(mapPath);
                }
                using (var webClient = new System.Net.WebClient())
                {
                    try
                    {
                        webClient.DownloadFile(websiteUrl, mapPathQRImgUrl);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                #endregion

                YSWL.MALL.BLL.Members.PointsDetail pointBll = new BLL.Members.PointsDetail();
                int pointers = pointBll.AddPoints(2, userid, "注册成功");
                int rankScore = BLL.Members.RankDetail.AddScore(2, userid, "注册成功");

                #region 注册送优惠券
                YSWL.MALL.BLL.Members.Users.RegForCoupon(newUser);
                #endregion
                // }

                #endregion
                return Redirect(ViewBag.BasePath);
                //return RedirectToAction("Personal", "UserCenter", new { id = 1, viewname = "url" });

            }
            return View(model);
        }
        #endregion

        #region 用户协议
        public ActionResult UserAgreement()
        {

            BLL.SysManage.WebSiteSet WebSiteSet = new BLL.SysManage.WebSiteSet(ApplicationKeyType.System);
            RegisterModel regModel = new RegisterModel();
            regModel.UserAgreement = WebSiteSet.RegistStatement;

            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "注册协议" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View(regModel);
        }
        #endregion

        #region Ajax验证

        /// <summary>
        /// 验证用户名是否已存在
        /// </summary>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsExistUserName(string userName)
        {
            bool valid = !(userBusManage.HasUserByUserName(userName));
            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 验证昵称是否已存在
        /// </summary>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsExistNickName(string nickName)
        {
            bool valid = !(userBusManage.HasUserByNickName(nickName));
            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 验证Phone是否已存在
        /// </summary>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsExistPhone(string phone)
        {
            bool valid = !(userBusManage.HasUserByPhone(phone));
            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 验证Email是否已存在
        /// </summary>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsExistEmail(string email)
        {
            bool valid = !(userBusManage.HasUserByEmail(email));
            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Ajax发送短信
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        public ActionResult SendSMS(FormCollection Fm)
        {
            YSWL.Json.JsonObject result = new YSWL.Json.JsonObject();
            string Phone = Fm["Phone"];
            string ImageCode = Fm["ImageCode"];
            if (String.IsNullOrWhiteSpace(Phone))
            {
                result.Accumulate("STATUS", "PHONEISNULL");//手机号为空
                return Content(result.ToString());
            }

            #region 验证图形验证码
            if (String.IsNullOrWhiteSpace(ImageCode))
            {
                result.Accumulate("STATUS", "IMAGECODEISINULL");//图形验证码为空
                return Content(result.ToString());
            }
            if (Session["CheckCode"] == null || Session["CheckCode"].ToString() == "")
            {
                result.Accumulate("STATUS", "IMAGECODEISEXPIRED");//图形验证码已失效
                return Content(result.ToString());
            }
            if (Session["CheckCode"].ToString().ToLower() != ImageCode.ToLower())
            {
                result.Accumulate("STATUS", "IMAGECODEISERROR");//验证码错误
                return Content(result.ToString());
            }
            #endregion

            //是否验证发送短信频繁
            bool IsOpenFrequentVerified = Common.Globals.SafeBool(BLL.SysManage.ConfigSystem.GetValueByCache("Emay_SMS_IsOpen_FrequentVerified"), true);
            if (IsOpenFrequentVerified)
            {
                if (!YSWL.MALL.Web.Components.SMSHelper.CheckSendNum(Request.UserHostAddress, Phone))
                {
                    result.Accumulate("STATUS", "SENDSMSFREQUENT");//发送短信频繁
                    return Content(result.ToString());
                }
            }
            Random rnd = new Random();
            int rand = rnd.Next(100000, 999999);
            string content = BLL.SysManage.ConfigSystem.GetValueByCache("Emay_SMS_Content");
            content = content.Replace("{SMSCode}", rand.ToString());
            Session["SMSCode"] = rand;
            Session["SMS_DATE"] = DateTime.Now;
            Session["SMSPhone"] = Phone;
            string[] numbers = new string[] { Phone };
            bool isSuccess =YSWL.MALL.Web.Components.SMSHelper.SendSMS(content, numbers);
            if (isSuccess)
            {
                if (IsOpenFrequentVerified)
                {
                    //增加发送短信次数
                    YSWL.MALL.Web.Components.SMSHelper.AddSendNum(Request.UserHostAddress, Phone);
                }
                result.Accumulate("STATUS", "SUCCESS");
                result.Accumulate("DATA", Phone);
                //result.Accumulate("rand", rand.ToString());
                Session["CheckCode"] = null;
            }
            else
            {
                result.Accumulate("STATUS", "FAILED");
            }
            return Content(result.ToString());
        }

        /// <summary>
        /// 验证效验码
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        public ActionResult VerifiyCode(FormCollection Fm)
        {
            string code = Fm["SMSCode"];
            string phoneNumber = Fm["Phone"];
            return VerifiySMSCode(phoneNumber,code) ? Content("True") : Content("False");
        }
        /// <summary>
        /// 验证短信校验码是否正确
        /// </summary>
        /// <param name="smscode"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        private bool VerifiySMSCode(string phone,string smscode)
        {
            if (Session["SMSCode"] == null || String.IsNullOrWhiteSpace(Session["SMSCode"].ToString()))
            {
                return false;
            }
            return smscode == Session["SMSCode"].ToString() && Session["SMSPhone"].ToString() == phone ? true : false;
        }

        #endregion

        #region 微信用户绑定

        public ActionResult UserBind(string viewName = "UserBind")
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Mobile);
            ViewBag.Title = "微信用户绑定" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            ViewBag.User = Request.Params["user"];
            ViewBag.OpenId = Request.Params["open"];
            return View(viewName);
        }
        [HttpPost]
        public ActionResult AjaxBind(string UserName, string UserPwd, string User, string OpenId)
        {
            if (String.IsNullOrWhiteSpace(OpenId) || String.IsNullOrWhiteSpace(User))
            {
                return Content("-1");
            }
            YSWL.WeChat.BLL.Core.User wUserBll = new WeChat.BLL.Core.User();
            YSWL.WeChat.Model.Core.User wUserModel = wUserBll.GetUser(OpenId, User);
            if (wUserModel == null)
            {
                return Content("-1");
            }
            if (!String.IsNullOrWhiteSpace(wUserModel.NickName))
            {
                return Content("3");
            }
            AccountsPrincipal userPrincipal = AccountsPrincipal.ValidateLogin(UserName, UserPwd);
            if (userPrincipal != null)
            {
                User currentUser = new YSWL.Accounts.Bus.User(userPrincipal);
                if (!currentUser.Activity)
                {
                    return Content("2");
                }
                HttpContext.User = userPrincipal;
                FormsAuthentication.SetAuthCookie(UserName, true);
                //绑定当前系统用户
                wUserModel.UserId = currentUser.UserID;
                wUserModel.NickName = currentUser.NickName;
                if (!wUserBll.Update(wUserModel))
                {
                    return Content("-1");
                }
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }

        public ActionResult RegBind(string viewName = "RegBind")
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Mobile);
            ViewBag.Title = "新用户绑定" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion

            string returnUrl = Common.Globals.UrlDecode(Request.Params["returnUrl"]);
            if (String.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = ViewBag.BasePath + "u";
            }
            ViewBag.ReturnUrl = returnUrl;
            if (currentUser != null)
            {
                return Redirect(returnUrl);
            }
            //直接登录
            if (userBusManage.HasUserByUserName(UserOpen))
            {
                #region  自动登陆
                YSWL.WeChat.BLL.Core.User wUserBll = new WeChat.BLL.Core.User();
                YSWL.WeChat.Model.Core.User wUserModel = wUserBll.GetUser(OpenId, UserOpen);
                if (wUserModel.UserId <= 0)//如果用户中存在此用户则与微信用户建立关联关系
                {
                    YSWL.Accounts.Bus.User user = new YSWL.Accounts.Bus.User(UserOpen);
                    if (user == null || user.UserID <= 0)
                    {
                        return View(viewName);
                    }
                    //建立关联关系
                    wUserModel.UserId = user.UserID;
                    if (!wUserBll.UpdateUser(wUserModel))
                    {
                        return View(viewName);
                    }
                    #region 建立关联关系
                    inviteBll.AddInvite(wUserModel.OpenId, wUserModel.UserName, user.UserID, user.UserName, user.NickName);
                    #endregion
                }
                AccountsPrincipal userPrincipal = new AccountsPrincipal(wUserModel.UserId);
                if (userPrincipal == null)
                {
                    return View(viewName);
                }
                currentUser = new YSWL.Accounts.Bus.User(userPrincipal);
                if (!currentUser.Activity)
                {
                    return View(viewName);
                }
                HttpContext.User = userPrincipal;
                Session[YSWL.Common.Globals.SESSIONKEY_USER] = currentUser;
                FormsAuthentication.SetAuthCookie(currentUser.UserName, true);
                return Redirect(returnUrl);
                #endregion
            }
           
            return View(viewName);
        }

        [HttpPost]
        public ActionResult AjaxRegBind(string NickName)
        {
            if (String.IsNullOrWhiteSpace(UserOpen))
            {
                return Content("0");
            }
            YSWL.WeChat.BLL.Core.User wUserBll = new WeChat.BLL.Core.User();
            YSWL.WeChat.Model.Core.User wUserModel = wUserBll.GetUser(OpenId, UserOpen);
            //如果存在该用户名，先直接绑定，然后登录

            if (wUserModel == null)
            {
                return Content("0");
            }

            YSWL.Accounts.Bus.User user = new YSWL.Accounts.Bus.User(wUserModel.UserId);
            if (user != null && user.UserID > 0)
            {
                wUserModel.UserId = user.UserID;
                wUserModel.NickName = NickName;
                if (!wUserBll.UpdateUser(wUserModel))
                {
                    return Content("0");
                }

                #region 建立关联关系
                inviteBll.AddInvite(wUserModel.OpenId, wUserModel.UserName, user.UserID, user.UserName, user.NickName);
                #endregion

                return Content("1");
            }
            User newUser = new User();
            //DONE: 警告DB字段未对应: Email 字段 varchar(100) UserName 字段 varchar(50) 已完成 BEN DONE 2012-11-22
            int nextUserId=userManage.GetMaxId()+1;
            newUser.UserName = UserOpen;// "wx" + nextUserId + new Random().Next(10, 99);
            newUser.NickName = NickName; //昵称名称相同
            newUser.Password = AccountsPrincipal.EncryptPassword(newUser.UserName);
            newUser.Email = "";
            newUser.Activity = true;
            newUser.UserType = "UU";
            newUser.Style = 1;
            newUser.User_dateCreate = DateTime.Now;
            newUser.User_cLang = "zh-CN";
            int userid = newUser.Create();
            if (userid == -100)
            {
                return Content("0");
            }

            //添加用户扩展表数据
            BLL.Members.UsersExp ue = new BLL.Members.UsersExp();
            ue.UserID = userid;
            ue.BirthdayVisible = 0;
            ue.BirthdayIndexVisible = false;
            ue.Gravatar = string.Format("/{0}/User/Gravatar/{1}", MvcApplication.UploadFolder, userid);
            ue.ConstellationVisible = 0;
            ue.ConstellationIndexVisible = false;
            ue.NativePlaceVisible = 0;
            ue.NativePlaceIndexVisible = false;
            ue.RegionId = 0;
            ue.AddressVisible = 0;
            ue.AddressIndexVisible = false;
            ue.BodilyFormVisible = 0;
            ue.BodilyFormIndexVisible = false;
            ue.BloodTypeVisible = 0;
            ue.BloodTypeIndexVisible = false;
            ue.MarriagedVisible = 0;
            ue.MarriagedIndexVisible = false;
            ue.PersonalStatusVisible = 0;
            ue.PersonalStatusIndexVisible = false;
            ue.LastAccessIP = "";
            ue.LastAccessTime = DateTime.Now;
            ue.LastLoginTime = DateTime.Now;
            ue.LastPostTime = DateTime.Now;
            if (!ue.Add(ue))
            {
                userManage.Delete(userid);
                userExpManage.Delete(userid);
                return Content("0");
            }

            #region 默认数据

            string DefaultGravatar = BLL.SysManage.ConfigSystem.GetValueByCache("DefaultGravatar");
            DefaultGravatar = string.IsNullOrEmpty(DefaultGravatar)
                                  ? "/Upload/User/Gravatar/Default.jpg"
                                  : DefaultGravatar;
            string TargetGravatarFile = BLL.SysManage.ConfigSystem.GetValueByCache("TargetGravatarFile");
            TargetGravatarFile = string.IsNullOrEmpty(TargetGravatarFile)
                                     ? "/Upload/User/Gravatar/"
                                     : TargetGravatarFile;
            string path = ControllerContext.HttpContext.Server.MapPath("/");
            if (System.IO.File.Exists(path + DefaultGravatar))
            {
                System.IO.File.Copy(path + DefaultGravatar, path + TargetGravatarFile + userid + ".jpg", true);
            }

            #endregion

            //绑定当前系统用户
            wUserModel.UserId = userid;
            wUserModel.NickName = NickName;
            if (!wUserBll.UpdateUser(wUserModel))
            {
                return Content("0");
            }

            #region 建立关联关系
           
            inviteBll.AddInvite(wUserModel.OpenId, wUserModel.UserName, newUser.UserID, newUser.UserName, newUser.NickName);
            #endregion

            return Content("1");
        }
        #endregion

        #region 展示二维码

        public ActionResult MyQRCode(string id, string viewName = "MyQRCode")
        {
            Bitmap b = null;
            System.Drawing.Image i = null;
            try
            {
                if (id == null)
                {
                    id = viewName;
                }
                string IUserIdI = "";
                if (!String.IsNullOrWhiteSpace(id))
                {
                    IUserIdI = Common.DEncrypt.Hex16.Decode(id);
                }

                ViewBag.Url = "Account/Register/" + IUserIdI;

                string QRPath = Server.MapPath("/Upload/QRcode/" + IUserIdI + ".png");
                string QRPathbj = Server.MapPath("/Upload/QRcode/QRBJ/" + IUserIdI + "bj.jpg");

                ViewBag.IsExist = System.IO.File.Exists(QRPath);
                ViewBag.UserId = IUserIdI;

                if (!System.IO.File.Exists(QRPathbj))
                {
                    string bgPath = Server.MapPath("/Upload/QRcode/qrewmbj.png");
                    string qrPath = QRPath;
                    ////调整图像大小
                    b = new Bitmap(bgPath);
                    i = resizeImage(b, new Size(459, 898));
                    string rePath = Server.MapPath("/Upload/QRcode/QRBJ/") + "rebg.png";
                    i.Save(rePath);
                    i.Dispose();
                    MergeImage(rePath, qrPath, IUserIdI.ToString());
                    QRPathbj = "/Upload/QRcode/QRBJ/" + IUserIdI + "bj.jpg";
                }
                else
                {
                    QRPathbj = "/Upload/QRcode/QRBJ/" + IUserIdI + "bj.jpg";
                }
                ViewBag.UserQRurl = QRPathbj;
            }
            catch (Exception ex)
            {
                ViewBag.UserQRurl = "/Images/QR_notExist.png";
            }
            finally
            {

                if (b != null)
                {
                    b.Dispose();
                }
                if (i != null)
                {
                    i.Dispose();
                }
            }
            viewName = "MyQRCode";
            return View(viewName);
        }

        //拼图函数
        private void MergeImage(string strBg, string strQr, string stuserid)
        {
             
            // 数组元素个数(即要拼图的图片个数)
            int lenth = 2;
            // 图片集合
            Bitmap[] maps = new Bitmap[lenth];
            //图片对应纵坐标集合
            int[] pointY = new int[lenth];
            //读取本地图片初始化Bitmap
            Bitmap map = null;
            Bitmap bitMap = null;
            Graphics g1 = null;

            if (!System.IO.File.Exists(strQr))
            {
                return;
            }

            try
            {
                //第一个图片对象，背景图片
                map = new Bitmap(strBg);
                maps[0] = map;
                pointY[0] = 0;
                //第二个图片对象，二维码
                map = new Bitmap(strQr);
                maps[1] = map;
                pointY[1] = 695;
               
                // 初始化背景图片的宽高
                bitMap = new Bitmap(459, 898);
                // 初始化画板
               g1 = Graphics.FromImage(bitMap);
                ////设置画布背景颜色为白色
                //g1.FillRectangle(Brushes.White, new Rectangle(80, 45, 160, 125));
                //绘制第一个图片，背景图
                for (int i = 0; i < maps[0].Width; i++)
                {
                    for (int j = 0; j < maps[0].Height; j++)
                    {
                        // 以像素点形式绘制(将要拼图的图片上的每个坐标点绘制到拼图对象的指定位置，直至所有点都绘制完成)
                        var temp = maps[0].GetPixel(i, j);
                        // 将图片画布的点绘制到整体画布的指定位置
                        bitMap.SetPixel(i, pointY[0] + j, temp);
                    }
                }
                maps[0].Dispose();
                //绘制第二个图片，一个白色边框
                g1.FillRectangle(Brushes.LightGreen, new Rectangle(60, 694, 182, 182));
                //绘制第三个图片，二维码
                for (int i = 0; i < maps[1].Width; i++)
                {
                    for (int j = 0; j < maps[1].Height; j++)
                    {
                        var temp = maps[1].GetPixel(i, j);
                        bitMap.SetPixel(61 + i, pointY[1] + j, temp);
                    }
                }
                maps[1].Dispose();
                // 保存输出到本地
                bitMap.Save(Server.MapPath("/Upload/QRcode/QRBJ") + "/" + stuserid + "bj.jpg");
                g1.Dispose();
                bitMap.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally{
                if (g1 != null)
                {
                    g1.Dispose();
                }
                if (bitMap != null)
                {
                    bitMap.Dispose();
                }
                if (map != null)
                {
                    map.Dispose();
                }
                
            }  
        }

        //调整图像大小
        private static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {
            //获取图片宽度
            int sourceWidth = imgToResize.Width;
            //获取图片高度
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //计算宽度的缩放比例
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //计算高度的缩放比例
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //期望的宽度
            int destWidth = (int)(sourceWidth * nPercent);
            //期望的高度
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //绘制图像
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            imgToResize.Dispose();
            return (System.Drawing.Image)b;
        }


        #endregion

        #region 会员卡
        public ActionResult UserCard(int pageIndex = 1, string viewName = "UserCard")
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "会员卡";// + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            BLL.Members.UsersExp userEXBll = new BLL.Members.UsersExp();
            YSWL.MALL.BLL.Members.UserCard cardBll = new BLL.Members.UserCard();
            BLL.Pay.BalanceDetails balanDetaBll = new BLL.Pay.BalanceDetails();
            int userid = 0;
            if (CurrentUser == null)
            {
                YSWL.WeChat.BLL.Core.User wUserBll = new WeChat.BLL.Core.User();
                if (String.IsNullOrWhiteSpace(OpenId) || String.IsNullOrWhiteSpace(UserOpen))
                {
                    return View();
                }
                YSWL.WeChat.Model.Core.User wUserModel = wUserBll.GetUser(OpenId, UserOpen);
                if (wUserModel.UserId <= 0)
                {
                    return View();
                }
                AccountsPrincipal userPrincipal = new AccountsPrincipal(wUserModel.UserId);
                if (userPrincipal == null)
                {
                    return View();
                }
                User currentUser = new YSWL.Accounts.Bus.User(userPrincipal);
                if (!currentUser.Activity)
                {
                    return View();
                }
                HttpContext.User = userPrincipal;
                Session[YSWL.Common.Globals.SESSIONKEY_USER] = currentUser;
                userid = currentUser.UserID;
                FormsAuthentication.SetAuthCookie(currentUser.UserName, true);
            }
            else
            {
                userid = CurrentUser.UserID;
            }
            //首页用户数据
            YSWL.MALL.Model.Members.UsersExpModel userEXModel = userEXBll.GetUsersModel(userid);
            if (userEXModel != null)
            {
                ViewBag.UserInfo = userEXModel;
                YSWL.MALL.Model.Members.UserCard cardModel = cardBll.GetModel(userEXModel.UserCardCode);
                ViewBag.Status = cardModel == null ? -1 : cardModel.Status;
            }

            int _pageSize = 8;
            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;
            int toalCount = 0;

            //获取总条数
            toalCount = balanDetaBll.GetRecordCount(" UserId =" + userid);//获取总条数 
            if (toalCount < 1)
            {
                return PartialView(viewName);//NO DATA
            }
            List<YSWL.MALL.Model.Pay.BalanceDetails> list = balanDetaBll.GetListByPage(" UserId = " + userid, startIndex, endIndex);
            PagedList<YSWL.MALL.Model.Pay.BalanceDetails> lists = new PagedList<YSWL.MALL.Model.Pay.BalanceDetails>(list, pageIndex, _pageSize, toalCount);
            return View(lists);
        }

        [HttpPost]
        public ActionResult GetUserCard()
        {
            YSWL.MALL.BLL.Members.UserCard cardBll = new BLL.Members.UserCard();

            if (CurrentUser == null)
            {
                if (String.IsNullOrWhiteSpace(OpenId) || String.IsNullOrWhiteSpace(UserOpen))
                {
                    return Content("NoOpen");
                }
                return Content("NoLogin");
            }
            if (cardBll.AddCard(currentUser.UserID))
            {
                return Content("True");
            }
            return Content("False");
        }

        [HttpPost]
        public ActionResult BindUserCard(FormCollection collection)
        {
            YSWL.MALL.BLL.Members.UserCard cardBll = new BLL.Members.UserCard();
            string name = collection["Name"];
            string phone = collection["Phone"];
            if (String.IsNullOrWhiteSpace(UserOpen))
            {
                return Content("0");
            }
            YSWL.WeChat.BLL.Core.User wUserBll = new WeChat.BLL.Core.User();
            YSWL.WeChat.Model.Core.User wUserModel = wUserBll.GetUser(OpenId, UserOpen);
            //如果存在该用户名，先直接绑定，然后登录
            if (userBusManage.HasUserByUserName(UserOpen))
            {
                YSWL.Accounts.Bus.User user = new YSWL.Accounts.Bus.User(UserOpen);
                if (user == null || user.UserID <= 0)
                {
                    return Content("0");
                }
                wUserModel.UserId = user.UserID;
                wUserModel.NickName = name;
                if (!wUserBll.Update(wUserModel))
                {
                    return Content("0");
                }
                return Content("1");
            }

            if (wUserModel == null)
            {
                return Content("0");
            }

            User newUser = new User();
            //DONE: 警告DB字段未对应: Email 字段 varchar(100) UserName 字段 varchar(50) 已完成 BEN DONE 2012-11-22
            newUser.UserName = UserOpen;
            newUser.NickName = name; //昵称名称相同
            newUser.TrueName = name;
            newUser.Phone = phone;
            newUser.Password = AccountsPrincipal.EncryptPassword(UserOpen);
            newUser.Email = "";
            newUser.Activity = true;
            newUser.UserType = "UU";
            newUser.Style = 1;
            newUser.User_dateCreate = DateTime.Now;
            newUser.User_cLang = "zh-CN";
            int userid = newUser.Create();
            if (userid == -100)
            {
                return Content("0");
            }

            //添加用户扩展表数据
            BLL.Members.UsersExp ue = new BLL.Members.UsersExp();
            ue.UserID = userid;
            ue.BirthdayVisible = 0;
            ue.BirthdayIndexVisible = false;
            ue.Gravatar = string.Format("/{0}/User/Gravatar/{1}", MvcApplication.UploadFolder, userid);
            ue.ConstellationVisible = 0;
            ue.ConstellationIndexVisible = false;
            ue.NativePlaceVisible = 0;
            ue.NativePlaceIndexVisible = false;
            ue.RegionId = 0;
            ue.AddressVisible = 0;
            ue.AddressIndexVisible = false;
            ue.BodilyFormVisible = 0;
            ue.BodilyFormIndexVisible = false;
            ue.BloodTypeVisible = 0;
            ue.BloodTypeIndexVisible = false;
            ue.MarriagedVisible = 0;
            ue.MarriagedIndexVisible = false;
            ue.PersonalStatusVisible = 0;
            ue.PersonalStatusIndexVisible = false;
            ue.LastAccessIP = "";
            ue.LastAccessTime = DateTime.Now;
            ue.LastLoginTime = DateTime.Now;
            ue.LastPostTime = DateTime.Now;
            if (!ue.Add(ue))
            {
                userManage.Delete(userid);
                userExpManage.Delete(userid);
                return Content("0");
            }

            #region 默认数据

            string DefaultGravatar = BLL.SysManage.ConfigSystem.GetValueByCache("DefaultGravatar");
            DefaultGravatar = string.IsNullOrEmpty(DefaultGravatar)
                                  ? "/Upload/User/Gravatar/Default.jpg"
                                  : DefaultGravatar;
            string TargetGravatarFile = BLL.SysManage.ConfigSystem.GetValueByCache("TargetGravatarFile");
            TargetGravatarFile = string.IsNullOrEmpty(TargetGravatarFile)
                                     ? "/Upload/User/Gravatar/"
                                     : TargetGravatarFile;
            string path = ControllerContext.HttpContext.Server.MapPath("/");
            if (System.IO.File.Exists(path + DefaultGravatar))
            {
                System.IO.File.Copy(path + DefaultGravatar, path + TargetGravatarFile + userid + ".jpg", true);
            }

            #endregion

            //绑定当前系统用户
            wUserModel.UserId = userid;
            wUserModel.NickName = name;
            if (!wUserBll.UpdateUser(wUserModel))
            {
                return Content("0");
            }
            //添加会员卡
            if (!cardBll.AddCard(userid))
            {
                return Content("0");
            }

            return Content("1");
        }
        #endregion

        //邮箱是否存在
        [HttpPost]
        public void HasEmail(FormCollection collection)
        {
            YSWL.Accounts.Bus.User user = new User();
            if (!String.IsNullOrWhiteSpace(collection["Email"]))
            {
                Response.ContentType = "application/text";
                if (user.HasUserByEmail(collection["Email"].Trim()))
                {
                    Response.Write("true");
                }
                else
                {
                    Response.Write("false");
                }
            }
        }

        [HttpPost]
        public void HasSSUser(FormCollection collection)
        {
            if (!String.IsNullOrWhiteSpace(collection["UserId"]))
            {
                Response.ContentType = "application/text";
                int userId = Common.Globals.SafeInt(collection["UserId"], 0);
                if (userExpManage.HasSales(userId))
                {
                    Response.Write("true");
                }
                else
                {
                    Response.Write("false");
                }
            }
        }


        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // 请参见 http://go.microsoft.com/fwlink/?LinkID=177550 以查看
            // 状态代码的完整列表。
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "用户名已存在。请输入不同的用户名。";

                case MembershipCreateStatus.DuplicateEmail:
                    return "该电子邮件地址的用户名已存在。请输入不同的电子邮件地址。";

                case MembershipCreateStatus.InvalidPassword:
                    return "提供的密码无效。请输入有效的密码值。";

                case MembershipCreateStatus.InvalidEmail:
                    return "提供的电子邮件地址无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidAnswer:
                    return "提供的密码取回答案无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidQuestion:
                    return "提供的密码取回问题无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidUserName:
                    return "提供的用户名无效。请检查该值并重试。";

                case MembershipCreateStatus.ProviderError:
                    return "身份验证提供程序返回了错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                case MembershipCreateStatus.UserRejected:
                    return "已取消用户创建请求。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                default:
                    return "发生未知错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";
            }
        }
        #endregion


        /// <summary>
        /// Ajax 判断是否登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AjaxIsLogin()
        {
            if (currentUser != null)
            {
                if (currentUser.UserType == "AA")
                {
                    return Content("AA");
                }
                return Content("True");
            }
            return Content("False");
        }


        /// <summary>
        /// 验证推荐人是否已存在 zhou20160102xiugai
        /// </summary>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsExistInvite(string Invite)
        {
            Invite = Invite.Replace("'", "");
            bool valid = userBusManage.HasUserByUserName(Invite);
            string sTrueName = userManage.GetUserTrueNameByUsername(Invite);
            string strJSDATA = valid.ToString().ToLower().Trim() + sTrueName;
            return Json(strJSDATA, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 显示推荐人  zhou20160102xiugai
        /// </summary>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult GetTrueName(string Invite)
        {
            Invite = Invite.Replace("'", "");
            string sTrueName = userManage.GetUserTrueNameByUsername(Invite);
            return Json(sTrueName, JsonRequestBehavior.AllowGet);
        }
    }
}
