using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YSWL.Accounts.Bus;
using System.Web.Security;
using Webdiyer.WebControls.Mvc;
using YSWL.Json;
using YSWL.MALL.BLL.Members;
using YSWL.Common;
using YSWL.Web;
using YSWL.WeChat.Model.Activity;

namespace YSWL.MALL.Web.Areas.COM.Controllers
{
    public class WeChatController : COMControllerBase
    {
        //
        // GET: /COM/WeChat/
        #region 成员变量

        private YSWL.Accounts.Bus.User userBusManage = new YSWL.Accounts.Bus.User();
        private BLL.Members.Users userManage = new BLL.Members.Users();
         private BLL.Members.UsersExp userEXBll = new BLL.Members.UsersExp();
        private BLL.Members.PointsDetail detailBll = new BLL.Members.PointsDetail();
        #endregion

        #region 会员卡
        public ActionResult UserCard(int pageIndex = 1, string viewName = "UserCard")
        {

            ViewBag.MShop = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MShop);
          
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
                userEXBll.Delete(userid);
                return Content("0");
            }

            //绑定当前系统用户
            wUserModel.UserId = userid;
            wUserModel.NickName = name;
            if (!wUserBll.Update(wUserModel))
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

        #region 微信用户绑定

        public ActionResult UserBind(string viewName = "UserBind")
        {
            //清空现有帐号session
            Session.Remove(Globals.SESSIONKEY_USER);
            ViewBag.MShop = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MShop);
            return View(viewName);
        }
        [HttpPost]
        public ActionResult AjaxBind(string UserName, string UserPwd)
        {
            if (String.IsNullOrWhiteSpace(OpenId) || String.IsNullOrWhiteSpace(UserOpen))
            {
                return Content("-1");
            }
            YSWL.WeChat.BLL.Core.User wUserBll = new WeChat.BLL.Core.User();
            YSWL.WeChat.Model.Core.User wUserModel = wUserBll.GetUser(OpenId, UserOpen);
            if (wUserModel == null)
            {
                return Content("-1");
            }
            if (wUserModel.UserId>0)
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
                if (!wUserBll.UpdateUser(wUserModel))
                {
                    return Content("-1");
                }
                #region 建立关联关系
                YSWL.MALL.BLL.Members.UserInvite inviteBll=new UserInvite();
                inviteBll.AddInvite(wUserModel.OpenId, wUserModel.UserName, currentUser.UserID, currentUser.UserName, currentUser.NickName);
                #endregion

                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }

        public ActionResult RegBind(string viewName = "RegBind")
        {

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
                if (wUserModel.UserId <= 0)
                {
                    return View(viewName);
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
            if (userBusManage.HasUserByUserName(UserOpen))
            {
                YSWL.Accounts.Bus.User user = new YSWL.Accounts.Bus.User(UserOpen);
                if (user == null || user.UserID <= 0)
                {
                    return Content("0");
                }
                wUserModel.UserId = user.UserID;
                wUserModel.NickName = NickName;
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
            newUser.NickName = NickName; //昵称名称相同
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
                userEXBll.Delete(userid);
                return Content("0");
            }

            //绑定当前系统用户
            wUserModel.UserId = userid;
            wUserModel.NickName = NickName;
            if (!wUserBll.UpdateUser(wUserModel))
            {
                return Content("0");
            }

            #region 建立关联关系
            YSWL.MALL.BLL.Members.UserInvite inviteBll = new UserInvite();
            inviteBll.AddInvite(wUserModel.OpenId, wUserModel.UserName, wUserModel.UserId, newUser.UserName, newUser.NickName);
            #endregion

            return Content("1");
        }
        #endregion

        #region 微信活动
        private YSWL.WeChat.BLL.Activity.ActivityInfo infoBll = new WeChat.BLL.Activity.ActivityInfo();
        private YSWL.MALL.BLL.Shop.Coupon.CouponInfo couponBll = new BLL.Shop.Coupon.CouponInfo();
        public ActionResult Index()
        {
            return View();
        }

        #region 刮刮卡
        /// <summary>
        /// 刮刮卡
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Scratch(int id)
        {
            //获取活动信息
            YSWL.WeChat.Model.Activity.ActivityInfo infoModel = infoBll.GetActivity(id, 0);
            YSWL.WeChat.BLL.Activity.ActivityCode codeBll = new WeChat.BLL.Activity.ActivityCode();
            ViewBag.HasChange = codeBll.HasChance(id, UserOpen);
            return View(infoModel);
        }
        #endregion

        #region 大转盘
        public ActionResult BigWheel(int id)
        {
            //获取活动信息
            YSWL.WeChat.Model.Activity.ActivityInfo infoModel = infoBll.GetActivity(id, 1);
            YSWL.WeChat.BLL.Activity.ActivityCode codeBll = new WeChat.BLL.Activity.ActivityCode();
            ViewBag.HasChange = codeBll.HasChance(id, UserOpen);
            return View(infoModel);
        }
        #endregion
        /// <summary>
        /// 查看是否有机会获取
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult HasChange(FormCollection collection)
        {
            YSWL.WeChat.BLL.Activity.ActivityCode codeBll = new WeChat.BLL.Activity.ActivityCode();
            int activityId = Common.Globals.SafeInt(collection["ActivityId"], 0);
            if (activityId == 0)
            {
                return Content("False");
            }
            bool IsNeedBind = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("SyStem_WeChat_UserBind");
            YSWL.WeChat.BLL.Core.User wUserBll = new YSWL.WeChat.BLL.Core.User();
            YSWL.WeChat.Model.Core.User wUserModel = wUserBll.GetUser(OpenId, UserOpen);
            if (IsNeedBind & (wUserModel == null || wUserModel.UserId <= 0))
            {
                return Content("False");
            }
            //随机获取剩余优惠券
            string username = IsNeedBind ? wUserModel.UserId.ToString() : UserOpen;
            bool hasChange = codeBll.HasChance(activityId, username);
            return hasChange ? Content("True") : Content("False");
        }
        /// <summary>
        /// 获取奖品码
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSNCode(FormCollection collection)
        {
            YSWL.WeChat.BLL.Activity.ActivityCode codeBll = new WeChat.BLL.Activity.ActivityCode();
            int activityId = Common.Globals.SafeInt(collection["ActivityId"], 0);
            JsonObject json = new JsonObject();
            if (activityId == 0 || String.IsNullOrWhiteSpace(UserOpen))
            {
                json.Put("STATUS", "False");
                return Content(json.ToString());
            }
            json.Put("STATUS", "True");
          
            YSWL.WeChat.Model.Activity.ActivityInfo infoModel = infoBll.GetModelByCache(activityId);
            if (infoModel == null)
            {
                json.Put("Data", "NoData");
                return Content(json.ToString());
            }
            bool IsNeedBind = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("SyStem_WeChat_UserBind");
            //商城优惠券流程
            if (infoModel.AwardType ==1)
            {
                YSWL.WeChat.BLL.Core.User wUserBll = new YSWL.WeChat.BLL.Core.User();
                YSWL.WeChat.Model.Core.User wUserModel = wUserBll.GetUser(OpenId, UserOpen);
                if (IsNeedBind & (wUserModel == null || wUserModel.UserId <= 0))
                {
                    json.Put("Data", "NoData");
                    return Content(json.ToString());
                }
              //随机获取剩余优惠券
                string username = IsNeedBind ? wUserModel.UserId.ToString() : UserOpen;
                if (codeBll.HasChance(activityId, username))
            {
                //开始写入记录表
                YSWL.WeChat.BLL.Activity.ActivityDetail detailBll = new YSWL.WeChat.BLL.Activity.ActivityDetail();
                YSWL.WeChat.Model.Activity.ActivityDetail detailModel = new YSWL.WeChat.Model.Activity.ActivityDetail();
                detailModel.ActivityId = activityId;
                detailModel.ActivityName = infoModel.Name;
                detailModel.CreateDate = DateTime.Now;
                detailModel.UserName = IsNeedBind ? wUserModel.UserId.ToString() : UserOpen;
                detailBll.Add(detailModel);
                //根据几率获取
                Random rnd = new Random();
                int randValue = rnd.Next(100);
                //在这个几率内 就获取奖品
                if (randValue <= infoModel.Probability)
                {
                  YSWL.MALL.Model.Shop.Coupon.CouponInfo couponInfo= couponBll.GetAwardCode(activityId);
                  if (couponInfo == null)
                  {
                      json.Put("Data", "NoData");
                      return Content(json.ToString());
                  }
                  //获取奖品信息
                  YSWL.WeChat.BLL.Activity.ActivityAward awardBll = new WeChat.BLL.Activity.ActivityAward();
                  YSWL.WeChat.Model.Activity.ActivityAward awardModel = awardBll.GetAwardInfo(couponInfo.RuleId, activityId);
                  json.Put("Data", awardModel.AwardName);
                  json.Put("SNCode", couponInfo.CouponCode);
                  return Content(json.ToString());
                }
            }
            json.Put("Data", "NoData");
            return Content(json.ToString());
            }
            //之前的活动流程
            else
            {
                YSWL.WeChat.Model.Activity.ActivityCode codeModel = codeBll.GetAwardCode(activityId, UserOpen);
                if (codeModel == null)
                {
                    json.Put("Data", "NoData");
                    return Content(json.ToString());
                }
                json.Put("Data", codeModel.AwardName);
                json.Put("SNCode", codeModel.CodeName);
                return Content(json.ToString());
            }
        }
        /// <summary>
        /// 奖品绑定
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BindSNCode(FormCollection collection)
        {
            YSWL.WeChat.BLL.Activity.ActivityCode codeBll = new WeChat.BLL.Activity.ActivityCode();
            YSWL.MALL.BLL.Shop.Coupon.CouponInfo infoBll=new BLL.Shop.Coupon.CouponInfo();
            string code = collection["SNCode"];
            string phone = collection["Phone"];
            string username = collection["Name"];
            string Address = collection["Remark"];
            int type = Common.Globals.SafeInt(collection["Type"], 0);
            if (String.IsNullOrWhiteSpace(code) || String.IsNullOrWhiteSpace(UserOpen))
            {
                return Content("False");
            }

            if (type == 0)
            {
                return codeBll.UpdateUser(code, UserOpen, username, 1, phone, Address) ? Content("True") : Content("False");
            }
            else
            {
                  YSWL.WeChat.BLL.Core.User wUserBll = new YSWL.WeChat.BLL.Core.User();
                    YSWL.WeChat.Model.Core.User wUserModel = wUserBll.GetUser(OpenId, UserOpen);
                    if (wUserModel == null || wUserModel.UserId <= 0)
                    {
                          
                            return  Content("NoUser");
                    }
                    return infoBll.BindCoupon(code, wUserModel.UserId) ? Content("True") : Content("False");
            }
        }

        /// <summary>
        /// 获取活动礼品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public PartialViewResult AwardList(int id, string viewName = "_AwardList")
        {
            YSWL.WeChat.BLL.Activity.ActivityAward awardBll = new WeChat.BLL.Activity.ActivityAward();
            List<YSWL.WeChat.Model.Activity.ActivityAward> list = awardBll.GetAwardList(id);
            return PartialView(viewName, list);
        }
        #endregion

        #region 我要报名
        public ActionResult Apply(string viewName = "Apply")
        {
            return View(viewName);
        }

        [HttpPost]
        public ActionResult SubmitEntryForm(FormCollection fm)
        {
             BLL.Ms.EntryForm bll = new BLL.Ms.EntryForm();
            #region  微信用户名
            string cookieValue = "entryform";
            if (Session["WeChat_UserName"] != null)
            {
                cookieValue += "_" + Session["WeChat_UserName"].ToString();
            }
            #endregion

            if (Request.Cookies["entry"] != null)
            {
                if (cookieValue == Request.Cookies["entry"].Values["entry"])
                {
                    return Content("isnotnull");//ERROR  "您已经报过名，请不要重复报名！ 
                }
            }

            string username = fm["UserName"];
            if (String.IsNullOrWhiteSpace(username))
            {
                return Content("UserNameISNULL");
            }
            int age = Common.Globals.SafeInt(fm["Age"], -1);
            string email = Common.InjectionFilter.SqlFilter(fm["Email"]);
            string phone = Common.InjectionFilter.SqlFilter(fm["Phone"]);
            int region = Common.Globals.SafeInt(fm["Region"], -1);
            string houseaddress = Common.InjectionFilter.SqlFilter(fm["Houseaddress"]);
            string Sex = Common.InjectionFilter.SqlFilter(fm["Sex"]);
            string Description = Common.InjectionFilter.SqlFilter(fm["Description"]);
            Model.Ms.EntryForm model = new Model.Ms.EntryForm();
            if (age > 0)
            {
                model.Age = age;
            }
            if (region > 0)
            {
                model.RegionId = region;
            }
            model.UserName = username;
            model.Email = email;
            model.Phone = phone;
            model.HouseAddress = houseaddress;
            model.Sex = Sex;
            model.Description = Description;
            model.State = 0;
            if (bll.Add(model) > 0)
            {
                HttpCookie httpCookie = new HttpCookie("entry");
                httpCookie.Values.Add("entry", cookieValue);
                httpCookie.Expires = DateTime.Now.AddHours(240);
                Response.Cookies.Add(httpCookie);
                return Content("true");
            }
            return Content("false");
        }
        #endregion
        
        #region 失效链接地址
        public ActionResult FailLink()
        {
            return View();
        }

        #endregion 
    }
}
