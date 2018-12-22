using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using YSWL.MALL.BLL.Ms;
using YSWL.Components.Setting;
using YSWL.MALL.Model.SysManage;
using YSWL.OAuth;
using YSWL.OAuth.Http.Converters;
using YSWL.OAuth.Http.Converters.Json;
using YSWL.OAuth.Json;
using YSWL.OAuth.Rest.Client;
using YSWL.OAuth.v2;
using YSWL.Accounts.Bus;
using YSWL.Common;
using YSWL.MALL.ViewModel.Shop;
using YSWL.MALL.Web.Components.Setting.Shop;
using System.Text;
using YSWL.Web;
using CapCRL.Common.Bus.Jsons;
using System.Security.Cryptography;
using System.Net;
using System.Configuration;
using System.IO;

namespace YSWL.MALL.Web.Areas.Shop.Controllers
{
    public class AccountController : Web.Controllers.ControllerBase
    {
        #region 成员变量

        private string strZjbXzLeave = ConfigurationSettings.AppSettings["zhicheng"].ToString();//会员限制职称
        private string strZjbToQianbao = ConfigurationSettings.AppSettings["ZjbToQb"].ToString();//会员限制职称
        shopCom spcom = new shopCom();

        BLL.Pay.BalanceDetails balanceManage = new BLL.Pay.BalanceDetails();//转进钱包

        BLL.Members.PointsDetail pointBLL = new BLL.Members.PointsDetail();

        private YSWL.Accounts.Bus.User userBusManage = new YSWL.Accounts.Bus.User();
        private BLL.Members.Users userManage = new BLL.Members.Users();
        private BLL.Members.UsersExp userExpManage = new BLL.Members.UsersExp();


       

        #endregion
        public ActionResult Index(FormCollection form)
        {
            return RedirectToAction("Login", "Account", new { area = "Shop" });
        }

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
                return  Content(result.ToString());
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
            bool isSuccess = YSWL.MALL.Web.Components.SMSHelper.SendSMS(content, numbers);
            if (isSuccess)
            {
                if (IsOpenFrequentVerified) {
                    //增加发送短信次数
                    YSWL.MALL.Web.Components.SMSHelper.AddSendNum(Request.UserHostAddress, Phone);
                }
                result.Accumulate("STATUS", "SUCCESS");
                result.Accumulate("DATA", Phone);
                //result.Accumulate("rand", rand.ToString());
                //Session["CheckCode"] = null;
            }else {
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
        /// <param name="phone"></param>
        /// <param name="smscode"></param>
        /// <returns></returns>
        private bool VerifiySMSCode(string phone,string smscode)
        {
            if (Session["SMSCode"] == null || String.IsNullOrWhiteSpace(Session["SMSCode"].ToString()) || Session["SMSPhone"] == null || String.IsNullOrWhiteSpace(Session["SMSPhone"].ToString()))
            {
                return false;
            }
            return (smscode == Session["SMSCode"].ToString() && Session["SMSPhone"].ToString() == phone ? true : false);
        }

        #endregion

        #region 登录
        public ActionResult Login(string returnUrl)
        {
            ViewBag.RegisterToggle = BLL.SysManage.ConfigSystem.GetValueByCache("Shop_RegisterToggle");//注册方式         
            BLL.SysManage.WebSiteSet webSiteSet = new BLL.SysManage.WebSiteSet(ApplicationKeyType.Shop);
            ViewBag.WebName = webSiteSet.WebName;
            bool IsCloseLogin = BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_Close_Login");
            if (IsCloseLogin)
            {
                return RedirectToAction("TurnOff", "Error");
            }
            if (!string.IsNullOrWhiteSpace(returnUrl))
                ViewBag.returnUrl = returnUrl;
            if (HttpContext.User.Identity.IsAuthenticated && CurrentUser != null && CurrentUser.UserType != "AA")
            {
                //使用returnUrl跳转
                if (!string.IsNullOrWhiteSpace(returnUrl)) return Redirect(returnUrl);
                return RedirectToAction("Index", "UserCenter");
            }
            if (MvcApplication.MainAreaRoute != AreaRoute.Shop && BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_Unique_Login")) return Redirect("/Account/Login?returnUrl="+ returnUrl);
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", ApplicationKeyType.Shop);
            ViewBag.Title = "登录" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View();
        }

        [HttpPost]
        public ActionResult Login(LogOnModel model, string returnUrl)
        {

            YSWL.MALL.BLL.Members.PointsDetail pointBll = new BLL.Members.PointsDetail();

            ViewBag.RegisterToggle = BLL.SysManage.ConfigSystem.GetValueByCache("Shop_RegisterToggle");//注册方式         
            bool IsCloseLogin = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_Close_Login");
            if (IsCloseLogin)
            {
                return RedirectToAction("TurnOff", "Error");
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
                        if (RegisterVIPuser(model.UserName, model.Password))
                        {
                            userPrincipal = AccountsPrincipal.ValidateLogin(model.UserName, model.Password);
                        }
                        else
                        {
                            ModelState.AddModelError("Message", "用户名或密码不正确, 请重新输入!");
                            return View(model);
                        }

                        //string strUserLave = spcom.GetUserLeave(model.UserName);//获取会员的职称

                        //if (strZjbXzLeave.IndexOf(strUserLave) < 0)//会员的职称在限制列表中没有找到，会员可以继续注册
                        //{

                        //}
                        //else
                        //{
                        //    ModelState.AddModelError("Message", "此会员的职称为:"+ strUserLave + ",不允许登录, 请联系公司!");
                        //    return View(model);
                        //}
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
                int pointers = pointBll.AddPoints(1, currentUser.UserID, "登录操作");


                //--会员登录后，自动把会员商城积分转入商城本身的积分账户--------

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
                //-----------------------------------------------------------------------


                //加成长值
                int rankScore = BLL.Members.RankDetail.AddScore(1, currentUser.UserID, "登录操作");
                //加载Shop模块的购物车
                if (MvcApplication.MainAreaRoute == AreaRoute.Shop)
                {
                    BLL.Shop.Products.ShoppingCartHelper.LoadShoppingCart(currentUser.UserID);
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
                    return RedirectToAction("Index", "UserCenter");
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

        [HttpPost]
        public ActionResult AjaxLogin(string UserName, string UserPwd)
        {
            bool IsCloseLogin = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_Close_Login");
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
                        return Content("NotActivity");
                    }
                    else
                    {
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
                        //加载Shop模块的购物车
                        if (MvcApplication.MainAreaRoute == AreaRoute.Shop)
                        {
                            BLL.Shop.Products.ShoppingCartHelper.LoadShoppingCart(currentUser.UserID);
                        }
                        return Content(string.Format("1|{0}|{1}", pointers, rankScore));
                    }    
                }
                else
                {
                    return Content("0");
                }
            }
            return Content("0");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Remove(Globals.SESSIONKEY_USER);
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region 注册
        public ActionResult Register(string id, string viewName = "Register")
        {
            ViewBag.InviteID = id;
            BLL.SysManage.WebSiteSet webSiteSet = new BLL.SysManage.WebSiteSet(Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.WebName = webSiteSet.WebName;
            bool IsCloseRegister =  BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_Close_Register");
            ViewBag.SMSIsOpen= BLL.SysManage.ConfigSystem.GetBoolValueByCache("Emay_SMS_IsOpen");//是否开启手机验证
            if (IsCloseRegister)
            {
                return RedirectToAction("TurnOff", "Error");
            }
            if (CurrentUser != null && CurrentUser.UserType != "AA") return RedirectToAction("Index", "UserCenter");

            if (MvcApplication.MainAreaRoute != AreaRoute.Shop) return Redirect("/Account/Register");

            ViewBag.RegisterToggle= YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_RegisterToggle");//注册方式

            BLL.SysManage.WebSiteSet WebSiteSet = new BLL.SysManage.WebSiteSet(ApplicationKeyType.System);
            RegisterModel regModel = new RegisterModel();
            regModel.UserAgreement = WebSiteSet.RegistStatement;
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

            // if (!String.IsNullOrWhiteSpace(id))
            if (!String.IsNullOrWhiteSpace(id) && id != "-1")
            {
                string InviteUserIdI = Common.DEncrypt.Hex16.Decode(id);
                int InviteuidI = Globals.SafeInt(InviteUserIdI, -1);//获取邀请人用户ID
                BLL.Members.Users userManage = new BLL.Members.Users();
                ViewBag.TjrName = userManage.GetUserName(InviteuidI) + " " + userManage.GetUserTrueNameByUsername(userManage.GetUserName(InviteuidI));

                regModel.InviteUserId = userManage.GetUserName(InviteuidI);

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
            return View(viewName,regModel);
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
            int uid = -1;
            if (!String.IsNullOrWhiteSpace(Request.Form["inviteid"]))
            {
                string id = Common.DEncrypt.Hex16.Decode(Request.Form["inviteid"]);
                uid = Globals.SafeInt(id, -1);
            }
            bool IsCloseRegister =  BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_Close_Register");
            if (IsCloseRegister)
            {
                return RedirectToAction("TurnOff", "Error");
            }

            string regStr = BLL.SysManage.ConfigSystem.GetValueByCache("Shop_RegisterToggle");//注册方式         
            ViewBag.RegisterToggle = regStr;
            bool isOpen=BLL.SysManage.ConfigSystem.GetBoolValueByCache("Emay_SMS_IsOpen");//手机验证
            ViewBag.SMSIsOpen = isOpen;
            if (ModelState.IsValid)
            {
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
                if (regStr == "Phone" && isOpen && !VerifiySMSCode(model.UserName, model.SMSCode))
                {
                    ViewBag.SCodeError = "短信校验码有误";
                    return View(model);
                }

                //判断邀请人是否在
                if (model.InviteUserId != null && model.InviteUserId.Trim().Length > 0)//输入邀请人
                {
                    if (!userBusManage.HasUser(model.InviteUserId))//邀请人不存在
                    {
                        ViewBag.hasIDCardNum = "邀请人不存在，请换一个，或者不填写邀请人!";
                        return View(model);
                    }
                    else//邀请人存在
                    {
                        uid = userManage.GetUserIdByUserName(model.InviteUserId);
                    }
                }
                else
                {
                }



                bool IsCloseRegisterSendEmail = BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_Close_RegisterEmailCheck");
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
                if (regStr == "Phone" ||  IsCloseRegisterSendEmail) //手机号码注册   或者  关闭邮箱验证
                    newUser.Activity = true;
                else //开启
                    newUser.Activity = false;
                newUser.UserType = "UU";
                newUser.Style = 1;
                newUser.User_dateCreate = DateTime.Now;
                newUser.User_cLang = "zh-CN";

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
                    ue.NickName = model.NickName;

                    ue.MSN = model.Password;

                    //注册来源
                    ue.SourceType = (int)YSWL.MALL.Model.Members.Enum.SourceType.PC;
                    if (!ue.AddExp(ue,uid))
                    {
                        userManage.Delete(userid);
                        userExpManage.Delete(userid);
                        ModelState.AddModelError("Message", "注册失败！");
                        return View(model);
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
                    BLL.Members.RankDetail.AddScore(2, userid, "注册成功");

                    #region 注册送优惠券
                    YSWL.MALL.BLL.Members.Users.RegForCoupon(newUser);
                    #endregion

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

                    string baseURL = string.Format("/tools/qr/gen.aspx?margin={0}&size={1}&level={2}&format={3}&content={4}", 5, 180, "30%", "png", "{0}");
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

                    if (regStr  == "Phone" ||  IsCloseRegisterSendEmail)  //手机号码注册   或者  关闭邮箱验证
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    }
                    else
                    {
                        try
                        {
                            SendEmail(model.UserName, model.UserName, 0);
                            return RedirectToAction("RegisterSuccess", "Account", new { email = model.UserName });// return Redirect(ViewBag.BasePath + "Account/RegisterSuccess");
                        }
                        catch (Exception ex)
                        {
                            Model.SysManage.ErrorLog Errormodel = new Model.SysManage.ErrorLog();
                            Errormodel.Loginfo = ex.Message;
                            Errormodel.StackTrace = ex.StackTrace;
                            Errormodel.Url = Request.Url.AbsoluteUri;
                            BLL.SysManage.ErrorLog.Add(Errormodel);
                            ModelState.AddModelError("", "邮件发送过程中出现网络异常，请稍后再试！");
                        }
                    }
                    #endregion
                    return Redirect(ViewBag.BasePath+"UserCenter/Personal");
                }
            }
            return View(model);
        }
        #endregion

        #region 注册邮件验证
        //注册邮件验证页面
        public ActionResult ValidateEmail()
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "邮箱验证成功" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion

            string SecretKey = Request.QueryString["SecretKey"];
            YSWL.MALL.BLL.SysManage.VerifyMail bll = new YSWL.MALL.BLL.SysManage.VerifyMail();
            YSWL.MALL.Model.SysManage.VerifyMail model = bll.GetModel(SecretKey);
            if (!string.IsNullOrEmpty(SecretKey) && bll.Exists(SecretKey) && model != null &&
                model.ValidityType.HasValue && model.ValidityType.Value == 0)
            {
                switch (model.Status)
                {
                    case 0:
                        TimeSpan ts = DateTime.Now - model.CreatedDate;
                        if (ts.TotalHours > 24)
                        {
                            model.Status = 2; // 0:邮箱验证未通过1：邮箱验证通过2：已过期
                            bll.Update(model);
                            ViewBag.Msg = "注册验证已过期！";
                            // ModelState.AddModelError("Error", "注册验证已过期！");
                        }
                        User user = new User(model.UserName);
                        if (user != null)
                        {
                            //更新用户状态
                            user.UpdateActivity(user.UserID, true);
                            ViewBag.Email = user.Email;
                        }

                        model.Status = 1; // 0:邮箱验证未通过1：邮箱验证通过2：已过期
                        bll.Update(model);
                        ViewBag.Msg = "Success";
                        ViewBag.email = model.UserName;
                        break;
                    case 1:
                        model.Status = 2;
                        bll.Update(model);
                        ViewBag.Msg = "注册验证已通过！";
                        //  ModelState.AddModelError("Error", "注册验证已通过！");
                        break;
                    case 2:
                        ViewBag.Msg = "注册验证已过期！";
                        // ModelState.AddModelError("Error", "注册验证已过期！");
                        break;
                    default:
                        ViewBag.Msg = "无效的邮箱验证码！";
                        //ModelState.AddModelError("Error", "无效的邮箱验证码！");
                        break;
                }
            }
            else
            {
                ViewBag.Msg = "无效的邮箱验证码！";
            }
            return View();
        }

        public ActionResult RegisterSuccess(string email)
        {
            if (String.IsNullOrWhiteSpace(email)) {
                return Redirect(ViewBag.BasePath);
            }
            ViewBag.Email = email;
            ViewBag.EmailUrl = EmailUrl(email);
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "注册成功" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View();
        }

        #endregion



        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
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


        #region 忽略SSL签名验证
        internal class AcceptAllCertificatePolicy : System.Net.ICertificatePolicy
        {
            public AcceptAllCertificatePolicy()
            {
            }

            public bool CheckValidationResult(System.Net.ServicePoint sPoint,
               System.Security.Cryptography.X509Certificates.X509Certificate cert, System.Net.WebRequest wRequest, int certProb)
            {
                // Always accept
                return true;
            }
        }
        #endregion

        


        #region 找回密码
        public ActionResult FindPwd()
        {
            if (CurrentUser != null && CurrentUser.UserType != "AA") return RedirectToAction("Index", "UserCenter");

            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "找回密码" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View();
        }

        [HttpPost]
        public ActionResult FindPwd(FormCollection collection)
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "找回密码" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion

            string username = collection["UserName"].Trim();
            ViewData["UserName"] = username;
            if (!new User().HasUserByUserName(username))
            {
                ModelState.AddModelError("Error", "该用户不存在！");
                return View(ViewData["UserName"]);
            }
            YSWL.Accounts.Bus.User user = new User(username);
            if (user == null || user.UserID <=0)
            {
                return View(ViewData["UserName"]);
            }
            ViewData["Email"] = user.Email;
            if ((Session["CheckCode"] != null) && (Session["CheckCode"].ToString() != ""))
            {
                if (Session["CheckCode"].ToString().ToLower() != collection["CheckCode"].Trim().ToLower())
                {
                    ModelState.AddModelError("Error", "验证码错误！");
                    Session["CheckCode"] = null;
                    return View(ViewData["UserName"]);
                }
                else
                {
                    Session["CheckCode"] = null;
                }
            }
            else
            {
                return View(ViewData["UserName"]);
            }
            //YSWL.Accounts.Bus.User userinfo = new User(username);
            if (String.IsNullOrWhiteSpace(user.Email))
            {
                ModelState.AddModelError("Error", "该邮箱用户不存在！");
                return View(ViewData["UserName"]);
            }
            //if (!(bool)userinfo.Activity)
            //{
            //    ModelState.AddModelError("Error", "您的帐号尚未通过邮箱验证,请重新发送确认邮件或者登录邮箱查看邮件！");
            //    return RedirectToAction("RegisterEmail", "Account", new { id = userinfo.UserID });
            //}
            try
            {
                SendEmail(user.UserName, user.Email, 1);
                return RedirectToAction("FindPwdEmail", "Account", new { email = ViewData["Email"] });
            }
            catch (Exception)
            {
                ModelState.AddModelError("Error", "邮件发送过程中出现网络异常，请稍后再试！");
            }
            finally
            {

            }
            return View(ViewData["UserName"]);
        }

        /// 找回密码邮箱验证页面
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ActionResult FindPwdEmail(string email)
        {
            ViewBag.Email = email;
            ViewBag.EmailUrl = EmailUrl(email);
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "找回密码邮箱验证" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View();
        }

        /// <summary>
        /// 激活密码找回
        /// </summary>
        /// <returns></returns>
        public ActionResult VerifyPassword()
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "找回密码" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion

            string SecretKey = ViewBag.SecretKey = Request.QueryString["SecretKey"];

            if (!string.IsNullOrEmpty(SecretKey))
            {
                YSWL.MALL.BLL.SysManage.VerifyMail bll = new YSWL.MALL.BLL.SysManage.VerifyMail();
                if (bll.Exists(SecretKey))
                {
                    YSWL.MALL.Model.SysManage.VerifyMail model = bll.GetModel(SecretKey);
                    if (model != null && model.ValidityType.HasValue)
                    {
                        if (model.ValidityType.Value == 1)
                        {
                            //0:邮箱验证未通过1：邮箱验证通过2：已过期
                            if (model.Status == 0)
                            {
                                TimeSpan ts = DateTime.Now - model.CreatedDate;
                                if (ts.TotalHours > 24)
                                {
                                    model.Status = 2;// 0:邮箱验证未通过1：邮箱验证通过2：已过期
                                    bll.Update(model);
                                    ViewBag.Msg = "找回密码的验证码已过期！";
                                    ModelState.AddModelError("Error", "找回密码的验证码已过期！");

                                }
                                
                                User user = new User(model.UserName);
                                if (user != null)
                                {
                                    ViewBag.Email = user.Email;
                                }
                                model.Status = 1;// 0:邮箱验证未通过1：邮箱验证通过2：已过期
                                bll.Update(model);
                                ViewBag.Msg = "Success";
                            }
                            else if (model.Status == 1)
                            {
                                model.Status = 2;
                                bll.Update(model);
                                ViewBag.Msg = "找回密码的验证码已通过邮箱验证！";
                                ModelState.AddModelError("Error", "找回密码的验证码已通过邮箱验证！");

                            }
                            else if (model.Status == 2)
                            {
                                ViewBag.Msg = "找回密码的验证码已过期！";
                                ModelState.AddModelError("Error", "找回密码的验证码已过期！");

                            }
                            else
                            {
                                ViewBag.Msg = "无效的邮箱验证码！";
                                ModelState.AddModelError("Error", "无效的邮箱验证码！");
                            }
                        }
                    }

                }
            }
            return View();
        }

        //邮箱是否存在
        [HttpPost]
        public void HasEmail(FormCollection collection)
        {
            string username = collection["UserName"];
            if (!String.IsNullOrWhiteSpace(username))
            {
                Response.ContentType = "application/text";
                if (!new User().HasUserByUserName(username))
                {
                    Response.Write("false");
                    return;
                }
                YSWL.Accounts.Bus.User user =new User(username);
                if (!String.IsNullOrWhiteSpace(user.Email))
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
        public ActionResult VerifyPassword(FormCollection collection)
        {
            if (!String.IsNullOrWhiteSpace(collection["Email"]) && !String.IsNullOrWhiteSpace(collection["NewPwd"]))
            {
                string secretKey = collection["SecretKey"];
                string password = collection["NewPwd"];
               
                YSWL.MALL.BLL.SysManage.VerifyMail bll = new YSWL.MALL.BLL.SysManage.VerifyMail();

                YSWL.MALL.Model.SysManage.VerifyMail model = bll.GetModel(secretKey);
                if (model == null || !model.ValidityType.HasValue || model.ValidityType.Value != 1 )
                {
                    //非法修改密码
                    LogHelp.AddInvadeLog("Areas.Shop.Controllers-HttpPost-VerifyPassword", System.Web.HttpContext.Current.Request);
                    return HttpNotFound();
                }
                string username = model.UserName;
                User user = new User();
                if (!user.HasUserByUserName(username)) {
                    ModelState.AddModelError("Error", "该用户不存在！");
                    return View();
                }
                User currentUser = new User(username);
                if (String.IsNullOrWhiteSpace(password))
                {
                    ModelState.AddModelError("Error", "密码不能为空！");
                    return View();
                }
                currentUser.Password = AccountsPrincipal.EncryptPassword(YSWL.Common.PageValidate.InputText(password, 30));
                if (!currentUser.Update())
                {
                    ModelState.AddModelError("Error", "密码重置失败，请检查输入的信息是否正确或者联系管理员！");
                    return View();
                }
                else
                {
                    AccountsPrincipal newUser = AccountsPrincipal.ValidateLogin(username, password);
                    FormsAuthentication.SetAuthCookie(username, false);
                    Session[Globals.SESSIONKEY_USER] = currentUser;
                    Session["Style"] = currentUser.Style;
                    YSWL.MALL.BLL.Members.PointsDetail pointBll = new BLL.Members.PointsDetail();
                    pointBll.AddPoints(1, currentUser.UserID, "登录操作");
                    BLL.Members.RankDetail.AddScore(1, currentUser.UserID, "登录操作");
                    if (Session["returnPage"] != null)
                    {
                        string returnpage = Session["returnPage"].ToString();
                        Session["returnPage"] = null;
                        return Redirect(returnpage);
                    }
                    else
                    {
                        return RedirectToAction("Index", "UserCenter");
                    }
                }
            }
            return View();
        }
        /// <summary>
        /// 重新发送邮件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void SendEmail(FormCollection collection)
        {
            if (!String.IsNullOrWhiteSpace(collection["Email"]))
            {
                User user = new User(collection["Email"]);
                int type = -1;//int.Parse(collection["Type"].ToString());
                //string emailtype = "RegisterEmail";
                if (!String.IsNullOrWhiteSpace(collection["Type"]) && Common.PageValidate.IsNumber(collection["Type"]))
                {
                    type = Common.Globals.SafeInt(collection["Type"], -1);
                    // emailtype = type == 0 ? "RegisterEmail" : "FindPwdEmail";
                }
                if (!String.IsNullOrWhiteSpace(user.NickName))
                {
                    SendEmail(user.UserName, user.Email, type);
                    Response.ContentType = "application/text";
                    Response.Write("success");
                }
            }
        }
        #endregion

        #region 发送邮件
        /// <summary>
        /// 根据用户id得到用户的邮箱
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        protected string EmailSuffix(string sUserID)
        {
            int userID = Globals.SafeInt(Globals.UrlDecode(sUserID), 0);
            BLL.Members.Users bll = new BLL.Members.Users();
            Model.Members.Users model = bll.GetModel(userID);
            if (model != null)
            {
                return model.Email;
            }
            else
                return "";
        }
        public string EmailUrl(string email)
        {
            string emailUrl = "";
            string emailStr = email.Substring(email.LastIndexOf('@') + 1);
            //谷歌邮箱特殊处理
            if (emailStr.Contains("gmail"))
            {
                emailStr = "google.com";
            }
            emailUrl = "http://mail." + emailStr;
            return emailUrl;
        }
        /// <summary>
        ///         发送邮件 type 0:表示注册激活邮件，1：表示找回密码邮件
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="type"></param>
        protected bool SendEmail(string username, string email, int type)
        {
            YSWL.MALL.BLL.Ms.EmailTemplet emailBll = new EmailTemplet();
            switch (type)
            {
                case 0:
                    return emailBll.SendRegisterEmail(username, email);
                case 1:
                    return emailBll.SendFindPwdEmail(username, email);
            }
            return false;
        }
        #endregion

        #region 微博帐号绑定
        public ActionResult ToBind()
        {
            string pName = Request["pName"];
            if (!String.IsNullOrWhiteSpace(pName))
            {
                String url = ViewBag.BasePath + "social/qq";
                switch (pName)
                {
                    case "QZone":
                        url = ViewBag.BasePath + "social/qq";
                        break;
                    case "Sina":
                        url = ViewBag.BasePath + "social/sina";
                        break;
                    default:
                        url = ViewBag.BasePath + "social/sina";
                        break;
                }
                System.Web.HttpContext.Current.Response.Redirect(url);
            }
            return RedirectToAction("UserBind", "UserCenter");
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


        #region Ajax 注册

        [HttpPost]
        public ActionResult AjaxRegister(string NickName, string UserName, string Pwd)
        {
            bool IsCloseRegister = BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_Close_Register");
            if (IsCloseRegister)
            {
                return RedirectToAction("TurnOff", "Error");
            }

            string regStr = BLL.SysManage.ConfigSystem.GetValueByCache("Shop_RegisterToggle");//注册方式         
            ViewBag.RegisterToggle = regStr;
            bool isOpen = BLL.SysManage.ConfigSystem.GetBoolValueByCache("Emay_SMS_IsOpen");//手机验证
            ViewBag.SMSIsOpen = isOpen;
                //判断昵称是否已存在
               if (userBusManage.HasUserByNickName(NickName))
                {
                    return Content("False");
                }
              
                bool IsCloseRegisterSendEmail = BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_Close_RegisterEmailCheck");
                User newUser = new User();
                //DONE: 警告DB字段未对应: Email 字段 varchar(100) UserName 字段 varchar(50) 已完成 BEN DONE 2012-11-22
                newUser.UserName = UserName;
                newUser.NickName =  NickName;  //昵称名称相同
                newUser.Password = AccountsPrincipal.EncryptPassword(Pwd);
                if (regStr == "Phone")
                {
                    newUser.Phone = UserName;
                }
                else
                {
                    newUser.Email = UserName;
                }
                if (regStr == "Phone" || IsCloseRegisterSendEmail) //手机号码注册   或者  关闭邮箱验证
                    newUser.Activity = true;
                else //开启
                    newUser.Activity = false;
                newUser.UserType = "UU";
                newUser.Style = 1;
                newUser.User_dateCreate = DateTime.Now;
                newUser.User_cLang = "zh-CN";
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
                    ue.BirthdayVisible = 0;
                    ue.BirthdayIndexVisible = false;
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
                    ue.NickName =  NickName;
                    //注册来源
                    ue.SourceType = (int)YSWL.MALL.Model.Members.Enum.SourceType.PC;
                    if (!ue.AddExp(ue,-1))
                    {
                        userManage.Delete(userid);
                        userExpManage.Delete(userid);
                        return Content("False");
                    }
                    //清除Session 
                    Session["SMSCode"] = null;
                    Session["SMS_DATE"] = DateTime.MinValue;
                    FormsAuthentication.SetAuthCookie(newUser.UserName, false /* createPersistentCookie */);
                    return Content("True");
                }
                return Content("False");
        }

        #endregion


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
