using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Web.Mvc;
using YSWL.Accounts.Bus;
using YSWL.MALL.BLL.Members;
using YSWL.MALL.BLL.Shop.Commission;
using YSWL.MALL.BLL.Shop.Gift;
using YSWL.MALL.BLL.Shop.Order;
using YSWL.Common;
using YSWL.Components.Setting;
using YSWL.Json;
using YSWL.Json.Conversion;
using YSWL.MALL.Model.Shop;
using YSWL.MALL.Model.Shop.Coupon;
using YSWL.MALL.Model.Shop.Order;
using YSWL.MALL.Model.SysManage;
using YSWL.MALL.Web.Components.Setting.Shop;
using Webdiyer.WebControls.Mvc;
using System.Linq;
using CouponInfo = YSWL.MALL.BLL.Shop.Coupon.CouponInfo;
using CouponRule = YSWL.MALL.BLL.Shop.Coupon.CouponRule;
using EnumHelper = YSWL.MALL.Model.Shop.Order.EnumHelper;
using OrderAction = YSWL.MALL.BLL.Shop.Order.OrderAction;
using System.Security.Cryptography;
using System.Net;
using CapCRL.Common.Bus.Jsons;
using System.Configuration;

namespace YSWL.MALL.Web.Areas.Shop.Controllers
{
    public class UserCenterController : ShopControllerBaseUser
    {
        //private readonly string strleyinka = ConfigurationSettings.AppSettings["yinka"].ToString();
        //private readonly string strlejinka = ConfigurationSettings.AppSettings["jinka"].ToString();
        //private readonly string strlezuanka = ConfigurationSettings.AppSettings["zuanka"].ToString();
        //private readonly string strlefeicunka = ConfigurationSettings.AppSettings["feicun"].ToString();

        private readonly string strVipUrl = ConfigurationSettings.AppSettings["VipUrl"].ToString();
        shopCom spcom = new shopCom();

        private readonly BLL.Members.Users userBll = new BLL.Members.Users();

        private readonly BLL.Members.PointsDetail detailBll = new BLL.Members.PointsDetail();
        private readonly BLL.Members.SiteMessage bllSM = new BLL.Members.SiteMessage();
        private readonly BLL.Members.UsersExp userEXBll = new BLL.Members.UsersExp();
        private readonly BLL.Members.UserBind userBind = new BLL.Members.UserBind();
        private readonly Orders _orderManage = new Orders();
        private readonly BLL.Pay.RechargeRequest rechargeBll = new BLL.Pay.RechargeRequest();
        private readonly BLL.Pay.BalanceDrawRequest balanDrawBll = new BLL.Pay.BalanceDrawRequest();
        private readonly BLL.Pay.BalanceDetails balanDetaBll = new BLL.Pay.BalanceDetails();
        private readonly BLL.Members.UserInvite inviteBll = new BLL.Members.UserInvite();
        private readonly YSWL.MALL.BLL.Shop.Coupon.CouponRule ruleBll = new CouponRule();
        private YSWL.MALL.BLL.Shop.Order.OrderAction actionBll = new BLL.Shop.Order.OrderAction();
        private readonly BLL.Shop.Gift.ExchangeDetail exchangeBll = new ExchangeDetail();
        private readonly BLL.Shop.Coupon.CouponInfo infoBll = new CouponInfo();
        private YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll = new YSWL.MALL.BLL.Shop.Products.SKUInfo();
        BLL.Shop.Products.ProductInfo prodBll = new BLL.Shop.Products.ProductInfo();
        private BLL.Shop.ReturnOrder.ReturnOrders retuOrderBLL = new BLL.Shop.ReturnOrder.ReturnOrders();
        private readonly BLL.Shop.Commission.CommissionDetail comdetailBll = new BLL.Shop.Commission.CommissionDetail();
        private YSWL.MALL.BLL.Shop.Favorite favoBll = new BLL.Shop.Favorite();
        BLL.Shop.Supplier.SupplierInfo suppinfobll = new BLL.Shop.Supplier.SupplierInfo();
        #region  首页
        //
        // GET: /Shop/UserCenter/
        public ActionResult Index(string viewName = "Index")
        {
            BLL.SysManage.WebSiteSet webSiteSet = new BLL.SysManage.WebSiteSet(Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.WebName = webSiteSet.WebName;
            YSWL.MALL.Model.Members.UsersExpModel usersModel = userEXBll.GetUsersModel(CurrentUser.UserID);
            if (usersModel != null)
            {
                BLL.Members.UserRank userRankBll = new UserRank();
                //会员等级
                usersModel.UserRank = userRankBll.GetRankInfo(usersModel.Grade.HasValue? usersModel.Grade.Value:0);
                //是否开启会员等级
                ViewBag.RankScoreIsEnable = BLL.SysManage.ConfigSystem.GetBoolValueByCache("RankScoreEnable");
                BLL.Members.SiteMessage msgBll = new BLL.Members.SiteMessage();
                ViewBag.privatecount = msgBll.GetReceiveMsgNotReadCount(CurrentUser.UserID, -1);//未读私信的条数
                Orders orderBll = new Orders();
                ViewBag.Unpaid = orderBll.GetUnPaidCounts(CurrentUser.UserID);//未支付订单数
                #region SEO 优化设置
                IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
                ViewBag.Title = "个人中心" + pageSetting.Title;
                ViewBag.Keywords = pageSetting.Keywords;
                ViewBag.Description = pageSetting.Description;

                //获取所有店铺的会员编号
                string strsuppEmpid = userBll.GetEmployeeIDByUserid(currentUser.UserID.ToString()).ToString();
                //店铺会员编号获得商家名称
                string suppName = suppinfobll.GetSuppNameBywhere(" UserId='" + strsuppEmpid + "'");

                ViewBag.suppName = suppName;

                //获取商城积分
                ///----------------------
                ViewBag.PointTotal = usersModel.Points.ToString();

                string strTureNmae= spcom.GetUserTrueName(currentUser.UserName);
                //ViewBag.XJJF = spcom.GetVipXJjfByusername(currentUser.UserName);
                //ViewBag.Gwjifen = spcom.GetVipGwjfByusername(currentUser.UserName);
                ViewBag.mfjyurl = strVipUrl;
                ///----------------------

                //--会员登录后，自动把会员商城积分转入商城本身的积分账户--------

                //if (spcom.GetUserTrueName(currentUser.UserName).Length > 0)//判断是VIP会员
                //{

                //    usersModel.BodilyForm = "VIP";
                //    userEXBll.UpdateEx(usersModel);


                //    int vipShopjifen = spcom.GetVipShopjfByusername(currentUser.UserName);
                //    if (vipShopjifen > 0)//VIP会员积分大于0的话，把会员系统的商城积分转入商城的商城积分
                //    {
                //        spcom.UpVIPjfByuser(currentUser.UserName, "Mall", vipShopjifen.ToString());//把会员系统的商城积分扣掉

                //        detailBll.PointsHuzhuan(100, currentUser.UserID, "VIP会员把VIP商城积分转入商城的积分账户", Convert.ToInt32(vipShopjifen.ToString()), "", 0);//把会员系统的商城积分转入商城的商城积分
                //    }
                //}
                //-----------------------------------------------------------------------


                ///
                #endregion
                return View(viewName, usersModel);
            }
            return RedirectToAction("Login", "Account");//去登录
        }
        #endregion

        #region 用户个人资料

        public ActionResult Personal()
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "个人资料" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion

            if (HttpContext.User.Identity.IsAuthenticated && CurrentUser != null && CurrentUser.UserType != "AA")
            {
                Model.Members.UsersExpModel model = userEXBll.GetUsersModel(CurrentUser.UserID);
                if (null != model)
                {
                    return View(model);
                }
            }

            return RedirectToAction("Login", "Account");//去登录
        }
        #endregion 用户个人资料

        #region 更新用户信息

        /// <summary>
        /// 更新用户信息
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public void UpdateUserInfo(FormCollection collection)
        {
            if (!HttpContext.User.Identity.IsAuthenticated || CurrentUser == null || CurrentUser.UserType == "AA")
            {
                RedirectToAction(ViewBag.BasePath + "Account/Login");//去登录
            }
            else
            {
                JsonObject json = new JsonObject();
                Model.Members.UsersExpModel model = userEXBll.GetUsersModel(CurrentUser.UserID);
                if (null == model)
                {
                    RedirectToAction("Login", "Account");//去登录
                }
                else
                {
                    model.TelPhone = collection["TelPhone"];
                    string birthday = collection["Birthday"];
                    if (!string.IsNullOrWhiteSpace(birthday) && PageValidate.IsDateTime(birthday))
                    {
                        model.Birthday = Globals.SafeDateTime(birthday, DateTime.Now);
                    }
                    else
                    {
                        model.Birthday = null;
                    }
                    model.Constellation = collection["Constellation"];//星座
                    model.PersonalStatus = collection["PersonalStatus"];//职业
                    model.Singature = collection["Singature"];
                    model.RegionId = Common.Globals.SafeInt(collection["Address"], 0);
                    User currentUser = new YSWL.Accounts.Bus.User(CurrentUser.UserID);
                    currentUser.Sex = collection["Sex"];
                    currentUser.Email = collection["Email"];
                    currentUser.NickName = collection["NickName"];
                    currentUser.Phone = collection["Phone"];
                    
                    if (currentUser.Update() && userEXBll.Update(model))
                    {
                        json.Accumulate("STATUS", "SUCC");
                    }
                    else
                    {
                        json.Accumulate("STATUS", "FAIL");
                    }
                    Response.Write(json.ToString());
                }
            }
        }

        #endregion 更新用户信息

        #region 用户头像

        public ActionResult Gravatar()
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "修改头像" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            if (CurrentUser == null)
            {
                return RedirectToAction("Login", "Account");//去登录
            }
            else
            {
                ViewBag.UserID = CurrentUser.UserID;
                return View("Gravatar");
            }
        }

        #endregion 用户头像

        #region 修改用户头像

        /// <summary>
        /// 修改用户头像
        /// </summary>
        [HttpPost]
        public ActionResult Gravatar(FormCollection collection)
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "修改头像" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion

            try
            {
                if (CurrentUser == null)
                {
                    return RedirectToAction("Login", "Account");//去登录
                }
                else
                {
                    Model.Members.UsersExpModel model = userEXBll.GetUsersModel(CurrentUser.UserID);
                    if (null != model)
                    {
                        model.Gravatar = collection["Gravatar"];
                        if (userEXBll.Update(model))//更新扩展信息  ,后期将更新头像独立一个方法。
                        {
                            return RedirectToAction("Personal");
                        }
                        else
                        {
                            return RedirectToAction("Personal");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");//去登录
                    }
                }
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public void CutGravatar(FormCollection collection)
        {
            string savePath = "/" + MvcApplication.UploadFolder + "/User/Gravatar/";  //头像保存路径
            // string savePath = "/Upload/User/Gravatar/";
            if (String.IsNullOrWhiteSpace(collection["x"]) || String.IsNullOrWhiteSpace(collection["y"]) || String.IsNullOrWhiteSpace(collection["w"]) || String.IsNullOrWhiteSpace(collection["h"]) || String.IsNullOrWhiteSpace(collection["filename"]))
                return;
            int x = (int)Common.Globals.SafeDecimal(collection["x"], 0);//坐标点有可能是浮点型 比如 27.5
            int y = (int)Common.Globals.SafeDecimal(collection["y"], 0);
            int w = (int)Common.Globals.SafeDecimal(collection["w"], 0);
            int h = (int)Common.Globals.SafeDecimal(collection["h"], 0);
            string filename = collection["filename"];//Request["UploadPhoto"];
            int UserId = currentUser.UserID;
            try
            {
                byte[] image = Crop(filename, w, h, x, y);
                if (!Directory.Exists(Server.MapPath(savePath)))
                {
                    Directory.CreateDirectory(Server.MapPath(savePath));
                }
                FileStream f = new FileStream(Server.MapPath(savePath + UserId + ".jpg"), FileMode.Create);
                f.Write(image, 0, image.Length);
                f.Close();
                Response.Write("success");
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }
        public byte[] Crop(string Img, int Width, int Height, int X, int Y)
        {
            try
            {
                using (var OriginalImage = new Bitmap(Server.MapPath(Img)))
                {
                    using (var bmp = new Bitmap(Width, Height, OriginalImage.PixelFormat))
                    {
                        bmp.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution);
                        using (Graphics Graphic = Graphics.FromImage(bmp))
                        {
                            Graphic.SmoothingMode = SmoothingMode.AntiAlias;
                            Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            Graphic.DrawImage(OriginalImage, new Rectangle(0, 0, Width, Height), X, Y, Width, Height,
                                              GraphicsUnit.Pixel);
                            var ms = new MemoryStream();
                            bmp.Save(ms, OriginalImage.RawFormat);
                            return ms.GetBuffer();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw (Ex);
            }
        }
        #endregion 修改用户头像

        #region 用户密码

        public ActionResult ChangePassword()
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "修改密码" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View();
        }

        #endregion 用户密码

        #region 检查用户原密码

        /// <summary>
        ///检查用户原密码
        /// </summary>
        [HttpPost]
        public void CheckPassword(FormCollection collection)
        {
            if (!HttpContext.User.Identity.IsAuthenticated || CurrentUser == null)
            {
                RedirectToAction(ViewBag.BasePath + "Account/Login");//去登录
            }
            else
            {
                JsonObject json = new JsonObject();
                string password = collection["Password"];
                if (!string.IsNullOrWhiteSpace(password))
                {
                    SiteIdentity SID = new SiteIdentity(CurrentUser.UserName);
                    if (SID.TestPassword(password.Trim()) == 0)
                    {
                        json.Accumulate("STATUS", "ERROR");
                    }
                    else
                    {
                        json.Accumulate("STATUS", "OK");
                    }
                }
                else
                {
                    json.Accumulate("STATUS", "UNDEFINED");
                }
                Response.Write(json.ToString());
            }
        }

        #endregion 检查用户原密码

        #region 更新用户密码

        /// <summary>
        /// 更新用户密码
        /// </summary>
        [HttpPost]
        public void UpdateUserPassword(FormCollection collection)
        {
            if (!HttpContext.User.Identity.IsAuthenticated || CurrentUser == null)
            {
                RedirectToAction(ViewBag.BasePath + "Account/Login");//去登录
            }
            else
            {
                JsonObject json = new JsonObject();
                string newpassword = collection["NewPassword"];
                string confirmpassword = collection["ConfirmPassword"];
                if (!string.IsNullOrWhiteSpace(newpassword) && !string.IsNullOrWhiteSpace(confirmpassword))
                {
                    if (newpassword.Trim() != confirmpassword.Trim())
                    {
                        json.Accumulate("STATUS", "FAIL");
                    }
                    else
                    {
                        currentUser.Password = AccountsPrincipal.EncryptPassword(newpassword);
                        if (currentUser.Update())
                        {
                            json.Accumulate("STATUS", "UPDATESUCC");
                        }
                        else
                        {
                            json.Accumulate("STATUS", "UPDATEFAIL");
                        }
                    }
                }
                else
                {
                    json.Accumulate("STATUS", "UNDEFINED");
                }
                Response.Write(json.ToString());
            }
        }

        #endregion 更新用户密码

        #region 检查用户输入的昵称是否被其他用户使用

        /// <summary>
        ///检查用户输入的昵称是否被其他用户使用
        /// </summary>
        [HttpPost]
        public void CheckNickName(FormCollection collection)
        {
            JsonObject json = new JsonObject();
            if (HttpContext.User.Identity.IsAuthenticated && CurrentUser != null && CurrentUser.UserType != "AA")
            {
                string nickname = collection["NickName"];
                if (!string.IsNullOrWhiteSpace(nickname))
                {
                    BLL.Members.Users bll = new BLL.Members.Users();
                    if (bll.ExistsNickName(CurrentUser.UserID, nickname))
                    {
                        json.Accumulate("STATUS", "EXISTS");
                    }
                    else
                    {
                        json.Accumulate("STATUS", "OK");
                    }
                }
                else
                {
                    json.Accumulate("STATUS", "NOTNULL");
                }
                Response.Write(json.ToString());
            }
            else
            {
                json.Accumulate("STATUS", "NOTNULL");
                Response.Write(json.ToString());
            }
        }

        #endregion 检查用户输入的昵称是否被其他用户使用

        #region 积分明细

        public ActionResult PointsDetail(int pageIndex = 1)
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "积分明细" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            //首页用户数据
            YSWL.MALL.Model.Members.UsersExpModel userEXModel = userEXBll.GetUsersModel(CurrentUser.UserID);
            if (userEXModel != null)
            {
                ViewBag.UserInfo = userEXModel;
            }
            int _pageSize = 15;

            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;
            int toalCount = 0;

            //获取总条数
            toalCount = detailBll.GetRecordCount(" UserID=" + CurrentUser.UserID);
            if (toalCount < 1)
            {
                return View();//NO DATA
            }
            List<YSWL.MALL.Model.Members.PointsDetail> detailList = detailBll.GetListByPageEX("UserID=" + CurrentUser.UserID, " ", startIndex, endIndex);
            if (detailList != null && detailList.Count > 0)
            {
                foreach (var item in detailList)
                {
                    item.RuleName = GetRuleName(item.RuleId);
                }
            }
            PagedList<YSWL.MALL.Model.Members.PointsDetail> lists = new PagedList<YSWL.MALL.Model.Members.PointsDetail>(detailList, pageIndex, _pageSize, toalCount);
            return View(lists);
        }

        public string GetRuleName(int RuleId)
        {
            if (RuleId == -1)
            {
                return "积分消费";
            }
            YSWL.MALL.BLL.Members.PointsRule ruleBll = new BLL.Members.PointsRule();
            return ruleBll.GetRuleName(RuleId);
        }

        public ActionResult Points()
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "积分明细" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            //首页用户数据
            YSWL.MALL.Model.Members.UsersExpModel userEXModel = userEXBll.GetUsersModel(CurrentUser.UserID);
            if (userEXModel != null)
            {
                ViewBag.UserInfo = userEXModel;
            }
            return View();
        }
        public PartialViewResult PointsList(int pageIndex = 1, string viewName = "_PointsList")
        {
            int _pageSize = 8;

            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;
            int toalCount = 0;

            //获取总条数
            toalCount = detailBll.GetRecordCount(" UserID=" + CurrentUser.UserID);
            if (toalCount < 1)
            {
                return  PartialView(viewName);//NO DATA
            }
            List<YSWL.MALL.Model.Members.PointsDetail> detailList = detailBll.GetListByPageEX("UserID=" + CurrentUser.UserID, " ", startIndex, endIndex);
            if (detailList != null && detailList.Count > 0)
            {
                foreach (var item in detailList)
                {
                    item.RuleName = GetRuleName(item.RuleId);
                }
            }
            PagedList<YSWL.MALL.Model.Members.PointsDetail> lists = new PagedList<YSWL.MALL.Model.Members.PointsDetail>(detailList, pageIndex, _pageSize, toalCount);
            return PartialView(viewName,lists);
        }
        #endregion 积分明细

        #region 积分兑换明细

        public ActionResult Exchanges(int pageIndex = 1)
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "积分兑换明细" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            //首页用户数据
            YSWL.MALL.Model.Members.UsersExpModel userEXModel = userEXBll.GetUsersModel(CurrentUser.UserID);
            if (userEXModel != null)
            {
                ViewBag.UserInfo = userEXModel;
            }
            int _pageSize = 15;

            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;
            int toalCount = 0;

            //获取总条数
            toalCount = exchangeBll.GetRecordCount(" UserID=" + CurrentUser.UserID);
            if (toalCount < 1)
            {
                return View();//NO DATA
            }
            List<YSWL.MALL.Model.Shop.Gift.ExchangeDetail> detailList = exchangeBll.GetListByPageEX("UserID=" + CurrentUser.UserID, " CreatedDate desc", startIndex, endIndex);

            PagedList<YSWL.MALL.Model.Shop.Gift.ExchangeDetail> lists = new PagedList<YSWL.MALL.Model.Shop.Gift.ExchangeDetail>(detailList, pageIndex, _pageSize, toalCount);
            return View(lists);
        }


        public PartialViewResult CouponRule(int top = 4, string viewName = "_CouponRule")
        {

            List<YSWL.MALL.Model.Shop.Coupon.CouponRule> ruleList = ruleBll.GetModelList(" Type=1 and Status=1");
            return PartialView(viewName, ruleList);
        }
        [HttpPost]
        public ActionResult AjaxExchange(int RuleId)
        {
            YSWL.MALL.Model.Shop.Coupon.CouponRule ruleModel = ruleBll.GetModel(RuleId);
            YSWL.MALL.Model.Members.UsersExpModel userEXModel = userEXBll.GetUsersModel(CurrentUser.UserID);
            if (ruleModel == null)
            {
                return Content("False");
            }
            if (ruleModel.NeedPoint > userEXModel.Points)
            {
                return Content("NoPoints");
            }
            if (ruleBll.GenCoupon(ruleModel, currentUser.UserID))
            {
                return Content("True");
            }
            return Content("False");
        }

        #endregion 积分兑换明细

        #region 我的优惠券

        public ActionResult MyCoupon(int pageIndex = 1)
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "我的优惠券" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion

            int status = Common.Globals.SafeInt(Request.Params["Status"], 1);  
            int _pageSize = 10;

            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;
            int toalCount = 0;

            //获取总条数
            toalCount = infoBll.GetRecordCount(String.Format(" UserID={0} and Status={1}", currentUser.UserID, status));
            if (toalCount < 1)
            {
                return View();//NO DATA
            }
            List<YSWL.MALL.Model.Shop.Coupon.CouponInfo> infoList = infoBll.GetListByPageEX(String.Format(" UserID={0} and Status={1}", currentUser.UserID, status), " GenerateTime desc", startIndex, endIndex);
            YSWL.MALL.BLL.Shop.Coupon.CouponClass classBll = new YSWL.MALL.BLL.Shop.Coupon.CouponClass();
            foreach (var Info in infoList)
            {
                YSWL.MALL.Model.Shop.Coupon.CouponClass classModel = classBll.GetModelByCache(Info.ClassId);
                Info.ClassName = classModel == null ? "" : classModel.Name;
            }
            PagedList<YSWL.MALL.Model.Shop.Coupon.CouponInfo> lists = new PagedList<YSWL.MALL.Model.Shop.Coupon.CouponInfo>(infoList, pageIndex, _pageSize, toalCount);
            return View(lists);
        }


        #endregion

        #region 检查用户输入的昵称是否存在

        /// <summary>
        ///检查用户输入的昵称是否存在
        /// </summary>
        [HttpPost]
        public void ExistsNickName(FormCollection collection)
        {
            if (!HttpContext.User.Identity.IsAuthenticated || CurrentUser == null)
            {
                RedirectToAction(ViewBag.BasePath + "Account/Login");//去登录
            }
            else
            {
                JsonObject json = new JsonObject();
                string nickname = collection["NickName"];
                if (!string.IsNullOrWhiteSpace(nickname))
                {
                    BLL.Members.Users bll = new BLL.Members.Users();
                    if (bll.ExistsNickName(nickname))
                    {
                        json.Accumulate("STATUS", "EXISTS");
                    }
                    else
                    {
                        json.Accumulate("STATUS", "NOTEXISTS");
                    }
                }
                else
                {
                    json.Accumulate("STATUS", "NOTNULL");
                }
                Response.Write(json.ToString());
            }
        }

        #endregion 检查用户输入的昵称是否存在

        #region 发站内信

        /// <summary>
        /// 发站内信
        /// </summary>
        /// <returns></returns>
        public ActionResult SendMessage()
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "发信息" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            ViewBag.Name = Request.Params["name"];
            return View("SendMessage");
        }

        #endregion 发站内信

        #region 发送站内信息

        /// <summary>
        /// 发送站内信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void SendMsg(FormCollection collection)
        {

            JsonObject json = new JsonObject();
            string nickname = Common.InjectionFilter.Filter(collection["NickName"]);
            string title = Common.InjectionFilter.Filter(collection["Title"]);
            string content = Common.InjectionFilter.Filter(collection["Content"]);
            if (string.IsNullOrWhiteSpace(nickname))
            {
                json.Accumulate("STATUS", "NICKNAMENULL");
            }
            else if (string.IsNullOrWhiteSpace(title))
            {
                json.Accumulate("STATUS", "TITLENULL");
            }
            else if (string.IsNullOrWhiteSpace(content))
            {
                json.Accumulate("STATUS", "CONTENTNULL");
            }
            else
            {
                BLL.Members.Users bll = new BLL.Members.Users();
                if (bll.ExistsNickName(nickname))
                {
                    int ReceiverID = bll.GetUserIdByNickName(nickname);
                    YSWL.MALL.Model.Members.SiteMessage modeSiteMessage = new YSWL.MALL.Model.Members.SiteMessage();
                    modeSiteMessage.Title = title;
                    modeSiteMessage.Content = content;
                    modeSiteMessage.SenderID = CurrentUser.UserID;
                    modeSiteMessage.ReaderIsDel = false;
                    modeSiteMessage.ReceiverIsRead = false;
                    modeSiteMessage.SenderIsDel = false;
                    modeSiteMessage.ReceiverID = ReceiverID;
                    modeSiteMessage.SendTime = DateTime.Now;
                    if (bllSM.Add(modeSiteMessage) > 0)
                    {
                        json.Accumulate("STATUS", "SUCC");
                    }
                    else
                    {
                        json.Accumulate("STATUS", "FAIL");
                    }
                }
                else
                {
                    json.Accumulate("STATUS", "NICKNAMENOTEXISTS");
                }
            }
            Response.Write(json.ToString());
        }

        #endregion 发送站内信息

        #region 回复站内信息

        /// <summary>
        /// 回复站内信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void ReplyMsg(int ReceiverID, string Title, string Content)
        {
            JsonObject json = new JsonObject();
            YSWL.MALL.Model.Members.SiteMessage modeSiteMessage = new YSWL.MALL.Model.Members.SiteMessage();
            modeSiteMessage.Title = Title;
            modeSiteMessage.Content = Content;
            modeSiteMessage.SenderID = CurrentUser.UserID;
            modeSiteMessage.ReaderIsDel = false;
            modeSiteMessage.ReceiverIsRead = false;
            modeSiteMessage.SenderIsDel = false;
            modeSiteMessage.ReceiverID = ReceiverID;
            modeSiteMessage.SendTime = DateTime.Now;
            if (bllSM.Add(modeSiteMessage) > 0)
                json.Accumulate("STATUS", "SUCC");
            else
                json.Accumulate("STATUS", "FAIL");
            Response.Write(json.ToString());
        }

        #endregion 回复站内信息

        #region 删除站内信息

        /// <summary>
        /// 删除收到的站内信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void DelReceiveMsg(int MsgID)
        {
            JsonObject json = new JsonObject();
            if (bllSM.SetReceiveMsgToDelById(MsgID,currentUser.UserID) > 0)
                json.Accumulate("STATUS", "SUCC");
            else
                json.Accumulate("STATUS", "FAIL");
            Response.Write(json.ToString());
        }

        #endregion 删除站内信息

        #region 读取站内信息
        /// <summary>
        /// 读取站内信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ReadMsg(int? MsgID)
        {
            if (MsgID.HasValue)
            {
                Model.Members.SiteMessage siteModel = bllSM.GetModelByCache(MsgID.Value);
                if (siteModel != null &&
                    ((siteModel.ReceiverID.HasValue && siteModel.ReceiverID.Value == currentUser.UserID) ||
                     (siteModel.SenderID.HasValue && siteModel.SenderID.Value == currentUser.UserID)))
                {
                    if (siteModel.ReceiverIsRead == false)
                        bllSM.SetReceiveMsgAlreadyRead(siteModel.ID);
                    return View(siteModel);
                }
            }
            return RedirectToAction("Inbox");
        }
        #endregion 读取站内信息

        #region 收件箱

        /// <summary>
        /// 收件箱
        /// </summary>
        /// <returns></returns>
        public ActionResult Inbox()
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "收件箱" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View();
        }

        /// <summary>
        /// 收件箱
        /// </summary>
        /// <returns></returns>
        public PartialViewResult InboxList(int? page, string viewName = "_InboxList")
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "发件箱" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            page = page.HasValue && page.Value > 1 ? page.Value : 1;
            ViewBag.inboxpage = page;
            int pagesize = 7;
            PagedList<YSWL.MALL.Model.Members.SiteMessage> list = bllSM.GetAllReceiveMsgListByMvcPage(CurrentUser.UserID, -1, pagesize, page.Value);
            //foreach (YSWL.MALL.Model.Members.SiteMessage item in list)
            //{
            //    if (item.ReceiverIsRead == false)
            //    {
            //        bllSM.SetReceiveMsgAlreadyRead(item.ID);
            //    }
            //}
            if (Request.IsAjaxRequest())
                return PartialView(viewName, list);
            return PartialView(viewName, list);
        }

        #endregion 收件箱

        #region 发件箱

        /// <summary>
        /// 发件箱
        /// </summary>
        /// <returns></returns>
        public ActionResult Outbox(int? page)
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "发件箱" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            page = page.HasValue && page.Value > 1 ? page.Value : 1;
            int pagesize = 10;
            PagedList<YSWL.MALL.Model.Members.SiteMessage> list = bllSM.GetAllSendMsgListByMvcPage(CurrentUser.UserID, pagesize, page.Value);
            if (Request.IsAjaxRequest())
                return PartialView("_OutboxList", list);
            return View("OutBox", list);
        }

        #endregion 发件箱

        #region 系统信息

        /// <summary>
        /// 系统信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SysInfo(int? page)
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "系统消息" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion

            page = page.HasValue && page.Value > 1 ? page.Value : 1;
            int pagesize = 10;
            PagedList<YSWL.MALL.Model.Members.SiteMessage> list = bllSM.GetAllSystemMsgListByMvcPage(CurrentUser.UserID, -1, CurrentUser.UserType, pagesize, page.Value);
            foreach (YSWL.MALL.Model.Members.SiteMessage item in list)
            {
                if (item.ReceiverIsRead == false)
                    bllSM.SetSystemMsgStateToAlreadyRead(item.ID, CurrentUser.UserID, CurrentUser.UserType);
            }
            if (Request.IsAjaxRequest())
                return PartialView("_SysInfoList", list);
            return View("SysInfo", list);
        }

        #endregion 系统信息

        #region 收货地址
        public ActionResult ShippAddressList(string viewName = "ShippAddressList")
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "我的收货地址" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion

            BLL.Shop.Shipping.ShippingAddress addressManage = new BLL.Shop.Shipping.ShippingAddress();
            List<Model.Shop.Shipping.ShippingAddress> list = addressManage.GetModelList(" UserId=" + CurrentUser.UserID);

            return View(viewName, list);
        }

        public ActionResult ShippAddress(int id = -1, string viewName = "ShippAddress")
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "我的收货地址" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion

            BLL.Shop.Shipping.ShippingAddress addressManage = new BLL.Shop.Shipping.ShippingAddress();
            Model.Shop.Shipping.ShippingAddress model = new Model.Shop.Shipping.ShippingAddress();
            if (id > 0)
            {
                model = addressManage.GetModel(id);
                if (model != null && model.UserId != CurrentUser.UserID)
                {
                    LogHelp.AddInvadeLog(
                        string.Format(
                            "非法获取收货人数据|当前用户:{0}|获取收货地址:{1}|_YSWL.Web.Areas.Shop.Controllers.UserCenterController.ShippAddress",
                            CurrentUser.UserID, id), System.Web.HttpContext.Current.Request);
                    return View(viewName, new Model.Shop.Shipping.ShippingAddress());
                }
            }
            return View(viewName, model);
        }

        [HttpPost]
        public ActionResult SubmitShippAddress(Model.Shop.Shipping.ShippingAddress model)
        {
            if (CurrentUser == null || model == null) return Content("NO");

            BLL.Shop.Shipping.ShippingAddress addressManage = new BLL.Shop.Shipping.ShippingAddress();
            //Update
            if (model.ShippingId > 0)
            {
                Model.Shop.Shipping.ShippingAddress baseModel = addressManage.GetModel(model.ShippingId);
                if (baseModel == null || baseModel.UserId!=currentUser.UserID)
                {
                    return Content("NO");
                }
                baseModel.ShipName = model.ShipName;
                baseModel.RegionId = model.RegionId;
                baseModel.Address = model.Address;
                baseModel.CelPhone = model.CelPhone;
                baseModel.Zipcode = model.Zipcode;

                if (addressManage.Update(baseModel))
                {
                    
                    return Content("OK");
                }
                return Content("NO");
            }
            //Add
            model.UserId = CurrentUser.UserID;
            model.ShippingId = addressManage.Add(model);
            if (model.ShippingId > 0)
            {
                return Content("OK");
            }
            return Content("NO");
        }

        [HttpPost]
        public ActionResult DelShippAddress(int id)
        {
            if (id < 1) return Content("NOID");
            BLL.Shop.Shipping.ShippingAddress addressManage = new BLL.Shop.Shipping.ShippingAddress();
            Model.Shop.Shipping.ShippingAddress model = addressManage.GetModel(id);
            if (model != null && CurrentUser.UserID == model.UserId)
            {
                if (addressManage.Delete(id))
                {
                    
                    return Content("OK");                    
                }
                return Content("NO");

            }
            return Content("ERROR");
        }

        [HttpPost]
        public ActionResult SetDefaultAddress(int id)
        {
            if (id < 1) return Content("NOID");
            BLL.Shop.Shipping.ShippingAddress addressManage = new BLL.Shop.Shipping.ShippingAddress();
            return Content(addressManage.SetDefaultShipAddress(CurrentUser.UserID, id) ? "OK" : "NO");
        }
        #endregion

        #region 订单列表
        public ActionResult Orders(string viewName = "Orders")
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "我的订单-订单明细" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View(viewName);
        }
        public PartialViewResult OrderList(int pageIndex = 1, string viewName = "_OrderList")
        {
            Orders orderBll = new Orders();
            BLL.Shop.Order.OrderItems itemBll = new BLL.Shop.Order.OrderItems();

            int _pageSize = 5;

            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;
            int toalCount = 0;

            string where = " BuyerID=" + CurrentUser.UserID +
#if true //方案二 统一提取主订单, 然后加载子订单信息 在View中根据订单支付状态和是否有子单对应展示
                //主订单
                           " AND OrderType=1";
#else   //方案一 提取数据时 过滤主/子单数据 View中无需对应 [由于不够灵活此方案作废]
                           //主订单 无子订单
                           " AND ((OrderType = 1 AND HasChildren = 0) " +
                           //子订单 已支付 或 货到付款/银行转账 子订单
                           "OR (OrderType = 2 AND (PaymentStatus > 1 OR (PaymentGateway='cod' OR PaymentGateway='bank')) ) " +
                           //主订单 有子订单 未支付的主订单 非 货到付款/银行转账 子订单
                           "OR (OrderType = 1 AND HasChildren = 1 AND PaymentStatus < 2 AND PaymentGateway<>'cod' AND PaymentGateway<>'bank'))";
#endif

            //获取总条数
            toalCount = orderBll.GetRecordCount(where);
            if (toalCount < 1)
            {
                return PartialView(viewName);//NO DATA
            }
            List<OrderInfo> orderList = orderBll.GetListByPageEX(where, "", startIndex, endIndex);
            if (orderList != null && orderList.Count > 0)
            {
                foreach (OrderInfo item in orderList)
                {
                    //有子订单 已支付 或 货到付款/银行转账 子订单 - 加载子单
                    if (item.HasChildren && (item.PaymentStatus > 1 || (item.PaymentGateway == "cod" || item.PaymentGateway == "bank")))
                    {
                        item.SubOrders = orderBll.GetModelList(" ParentOrderId=" + item.OrderId);
                        item.SubOrders.ForEach(
                            info => info.OrderItems = itemBll.GetModelList(" OrderId=" + info.OrderId));
                    }
                    else
                    {
                        item.OrderItems = itemBll.GetModelList(" OrderId=" + item.OrderId);
                    }
                }
            }
            PagedList<OrderInfo> lists = new PagedList<OrderInfo>(orderList, pageIndex, _pageSize, toalCount);
            if (Request.IsAjaxRequest())
                return PartialView(viewName, lists);
            return PartialView(viewName, lists);
        }

        #region 辅助方法

        public static string GetOrderType(string paymentGateway, int orderStatus, int paymentStatus, int shippingStatus)
        {
            YSWL.MALL.BLL.Shop.Order.Orders orderBll = new Orders();
            return orderBll.GetOrderTypeStr(paymentGateway,
                                    orderStatus,
                                    paymentStatus,
                                    shippingStatus);
        }
        public static EnumHelper.OrderMainStatus GetOrderMainStatus(string paymentGateway, int orderStatus, int paymentStatus, int shippingStatus)
        {
            Orders orderBll = new Orders();
            return orderBll.GetOrderType(paymentGateway,
                                    orderStatus,
                                    paymentStatus,
                                    shippingStatus);
        }
        #endregion

        #region Ajax方法
        [HttpPost]//取消订单
        public ActionResult CancelOrder(FormCollection Fm)
        {
            Orders orderBll = new Orders();
            long orderId = Globals.SafeLong(Fm["OrderId"], 0);
            OrderInfo orderInfo = orderBll.GetModelInfo(orderId);
            if (orderInfo == null || orderInfo.BuyerID != CurrentUser.UserID)
                return Content("False");

            if (OrderManage.CancelOrder(orderInfo, currentUser))
                return Content("True");
            return Content("False");
        }
        [HttpPost]//完成订单
        public ActionResult CompleteOrder(FormCollection Fm)
        {
            Orders orderBll = new Orders();
            long orderId = Globals.SafeLong(Fm["OrderId"], 0);
            OrderInfo orderInfo = orderBll.GetModelInfo(orderId);
            if (orderInfo == null || orderInfo.BuyerID != CurrentUser.UserID)
                return Content("False");
            if (BLL.Shop.Order.OrderManage.CompleteOrder(orderInfo, CurrentUser))
            {
                return Content("True");
            }
            return Content("False");
        }
        #endregion
        #endregion
 
        #region 商品评论
        public ActionResult PReview(long id = -1, string viewName = "PReview")
        {
            OrderInfo orderModel = _orderManage.GetModelInfo(id);
            if (orderModel == null ||
                orderModel.BuyerID != CurrentUser.UserID || orderModel.IsReviews || orderModel.OrderStatus != (int)EnumHelper.OrderStatus.Complete
                ) return Redirect(ViewBag.BasePath + "UserCenter/Orders");


            List<Model.Shop.Order.OrderItems> list = orderModel.OrderItems;

            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "评论" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View(viewName, list);
        }
        /// <summary>
        /// 提交商品评论
        /// </summary>
        /// <param name="fm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AjAxPReview(FormCollection fm)
        {
            string data = fm["PReviewjson"];
            if (String.IsNullOrWhiteSpace(data))
            {
                return Content("false");
            }
            BLL.Shop.Products.ProductReviews prodRevBll = new BLL.Shop.Products.ProductReviews();
            List<Model.Shop.Products.ProductReviews> modelList = new List<Model.Shop.Products.ProductReviews>();
            Model.Shop.Products.ProductReviews prodRevModel;
            JsonArray jsonArray = JsonConvert.Import<JsonArray>(data);
            long orderId = -1;
            foreach (JsonObject jsonObject in jsonArray)
            {
                long pid = Globals.SafeInt(jsonObject["pid"].ToString(), -1);
                orderId = Globals.SafeInt(jsonObject["orderId"].ToString(), -1);
                string contentval = InjectionFilter.Filter(jsonObject["contentval"].ToString());
                string imagesurlPath = Globals.SafeString(jsonObject["imagesurlPath"].ToString(), "");
                string imagesurlName = Globals.SafeString(jsonObject["imagesurlName"].ToString(), "");
                string attribute = InjectionFilter.Filter(jsonObject["attribute"].ToString());
                string sku = InjectionFilter.Filter(jsonObject["sku"].ToString());

                if (pid > 0 && orderId > 0 && !String.IsNullOrWhiteSpace(contentval))
                {
                    prodRevModel = new Model.Shop.Products.ProductReviews();
                    prodRevModel.Attribute = attribute;
                    prodRevModel.CreatedDate = DateTime.Now;
                    prodRevModel.OrderId = orderId;
                    prodRevModel.ProductId = pid;
                    prodRevModel.ReviewText = contentval;
                    prodRevModel.SKU = sku;
                    prodRevModel.Status = 0;
                    prodRevModel.UserEmail = currentUser.Email;
                    prodRevModel.UserId = currentUser.UserID;
                    prodRevModel.UserName = currentUser.UserName;
                    prodRevModel.ParentId = 0;
                    if (!String.IsNullOrWhiteSpace(imagesurlPath) && !String.IsNullOrWhiteSpace(imagesurlName))
                    {
                        //创建文件夹  移动文件
                        string path = string.Format("/Upload/Shop/ProductReviews/{0}/", DateTime.Now.ToString("yyyyMM"));
                        string mapPath = Request.MapPath(path);
                        if (!Directory.Exists(mapPath))
                        {
                            Directory.CreateDirectory(mapPath);
                        }
                        string[] pathArr = imagesurlPath.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        string[] namesArr = imagesurlName.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        if (pathArr.Length != namesArr.Length)
                        {
                            throw new ArgumentOutOfRangeException("路径与文件名长度不匹配！");
                        }
                        for (int i = 0; i < pathArr.Length; i++)
                        {
                            System.IO.File.Move(Request.MapPath(pathArr[i]), mapPath + namesArr[i]);
                        }
                        prodRevModel.ImagesPath = path + "{0}";
                        prodRevModel.ImagesNames = string.Join("|", namesArr);
                    }
                    modelList.Add(prodRevModel);
                }
                else
                {
                    return Content("false");
                }
            }
            if (modelList.Count > 0)
            {
                int pointers;
                int rankScore;
                if (prodRevBll.AddEx(modelList, orderId, out  pointers,out rankScore))
                {
                    return Content(string.Format("{0}|{1}", pointers, rankScore));//评论成功   返回获得的积分
                }
            }
            return Content("false");
        }
        #endregion

        #region 查看订单明细

        /// <summary>
        /// 查看订单明细
        /// </summary>
        public ActionResult OrderInfo(long id = -1, string viewname = "OrderInfo")
        {
            OrderInfo orderModel = _orderManage.GetModelInfo(id);
            //Safe
            if (orderModel == null ||
                orderModel.BuyerID != CurrentUser.UserID
                ) return Redirect(ViewBag.BasePath + "UserCenter/Orders");
            if (orderModel.OrderStatus == (int)EnumHelper.OrderStatus.Cancel)//已取消的订单
            {
                viewname = "OrderCanceled";
            }

            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "查看订单详细信息" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion

            return View(viewname, orderModel);
        }
        public PartialViewResult OrderAction(long OrderId = -1, string viewName = "_ActionList")
        {
            OrderAction actionBll = new OrderAction();
            List<Model.Shop.Order.OrderAction> actionList = actionBll.GetModelList(" OrderId=" + OrderId);
            return PartialView(viewName, actionList);
        }

        #endregion

        #region 左侧导航

        public ActionResult LeftMenu(string viewName = "_LeftMenu")
        {
            YSWL.MALL.BLL.Members.SiteMessage msgBll = new BLL.Members.SiteMessage();
            ViewBag.privatecount = msgBll.GetReceiveMsgNotReadCount(CurrentUser.UserID, -1);//未读私信的条数
            return View(viewName);
        }

        #endregion

        #region 收藏列表
        public ActionResult MyFavor(string viewName = "MyFavor")
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "我的收藏" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View(viewName);
        }

        public PartialViewResult FavorList(int pageIndex = 1, string viewName = "_FavorList")
        {
            StringBuilder strBuilder = new StringBuilder();
            // strBuilder.AppendFormat(" UserId ={0}  and  SaleStatus in ( {1},{2} ) ", CurrentUser.UserID, (int )ProductSaleStatus.InStock, (int )ProductSaleStatus.OnSale);
            strBuilder.AppendFormat(" favo.UserId ={0} and favo.Type= {1} ", CurrentUser.UserID, (int)FavoriteEnums.Product);

            int _pageSize = 10;

            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;
            int toalCount = favoBll.GetRecordCount(" UserId =" + CurrentUser.UserID + " and Type=" + (int)FavoriteEnums.Product);//获取总条数 
            if (toalCount < 1)
            {
                return PartialView(viewName);//NO DATA
            }
            List<YSWL.MALL.ViewModel.Shop.FavoProdModel> favoList = favoBll.GetFavoriteProductListByPage(strBuilder.ToString(), startIndex, endIndex);
            PagedList<YSWL.MALL.ViewModel.Shop.FavoProdModel> lists = new PagedList<YSWL.MALL.ViewModel.Shop.FavoProdModel>(favoList, pageIndex, _pageSize, toalCount);
            if (Request.IsAjaxRequest())
                return PartialView(viewName, lists);
            return PartialView(viewName, lists);
        }

        #endregion

        #region Ajax方法
        /// <summary>
        /// 移除收藏项
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        public ActionResult RemoveFavorItem(FormCollection Fm)
        {
            if (!String.IsNullOrWhiteSpace(Fm["ItemId"]))
            { 
                if (favoBll.Delete(Common.Globals.SafeInt(Fm["ItemId"], 0)))
                    return Content("Ok");
            }
            return Content("No");
        }



        /// <summary>
        /// 加入收藏
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        public ActionResult AjaxAddFav(FormCollection Fm)
        {
            if (!String.IsNullOrWhiteSpace(Fm["ProductId"]))
            {
                int productId = Common.Globals.SafeInt(Fm["ProductId"], 0);
                YSWL.MALL.BLL.Shop.Favorite favBll = new BLL.Shop.Favorite();
                //是否已经收藏
                if (favBll.Exists(productId, CurrentUser.UserID, 1))
                {
                    return Content("Rep");
                }
                YSWL.MALL.Model.Shop.Favorite favMode = new YSWL.MALL.Model.Shop.Favorite();
                favMode.CreatedDate = DateTime.Now;
                favMode.TargetId = productId;
                favMode.Type = 1;
                favMode.UserId = CurrentUser.UserID;
                return favBll.Add(favMode) > 0 ? Content("True") : Content("False");
            }
            return Content("False");
        }
        /// <summary>
        /// 添加咨询
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        public ActionResult AjaxAddConsult(FormCollection Fm)
        {
            if (!String.IsNullOrWhiteSpace(Fm["ProductId"]))
            {
                int productId = Common.Globals.SafeInt(Fm["ProductId"], 0);
                string content = Common.InjectionFilter.SqlFilter(Fm["Content"]);
                YSWL.MALL.BLL.Shop.Products.ProductConsults consultBll = new YSWL.MALL.BLL.Shop.Products.ProductConsults();
                YSWL.MALL.Model.Shop.Products.ProductConsults consultMode = new YSWL.MALL.Model.Shop.Products.ProductConsults();
                consultMode.CreatedDate = DateTime.Now;
                consultMode.TypeId = 0;
                consultMode.Status = 0;
                consultMode.UserId = CurrentUser.UserID;
                consultMode.UserName = CurrentUser.NickName;
                consultMode.UserEmail = CurrentUser.Email;
                consultMode.IsReply = false;
                consultMode.Recomend = 0;
                consultMode.ProductId = productId;
                consultMode.ConsultationText = content;
                return consultBll.Add(consultMode) > 0 ? Content("True") : Content("False");
            }
            return Content("False");
        }
        /// <summary>
        /// 用户评论
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        public ActionResult AjaxAddComment(FormCollection Fm)
        {
            if (!String.IsNullOrWhiteSpace(Fm["ProductId"]))
            {
                int productId = Common.Globals.SafeInt(Fm["ProductId"], 0);
                string productName = Fm["ProductName"];
                string content = Common.InjectionFilter.SqlFilter(Fm["Content"]);
                YSWL.MALL.BLL.Shop.Products.ProductReviews reviewBll = new YSWL.MALL.BLL.Shop.Products.ProductReviews();
                YSWL.MALL.Model.Shop.Products.ProductReviews reviewMode = new YSWL.MALL.Model.Shop.Products.ProductReviews();
                reviewMode.CreatedDate = DateTime.Now;
                reviewMode.Status = 0;
                reviewMode.UserId = CurrentUser.UserID;
                reviewMode.UserName = CurrentUser.NickName;
                reviewMode.UserEmail = CurrentUser.Email;
                reviewMode.ParentId = 0;
                reviewMode.ProductId = productId;
                reviewMode.ReviewText = content;
                bool IsPost = BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_Create_Post");
                return reviewBll.AddEx(reviewMode, productName, IsPost) ? Content("True") : Content("False");
            }
            return Content("False");
        }
         
   
        /// <summary>
        /// 预定
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AjaxAddPre(FormCollection Fm)
        {       
            int productId = Common.Globals.SafeInt(Fm["ProductId"], 0);
            string sku = Fm["SKU"];
            int count = Common.Globals.SafeInt(Fm["Count"], 0); 
            YSWL.MALL.BLL.Shop.PrePro.PreOrder preBll = new BLL.Shop.PrePro.PreOrder();
            YSWL.MALL.Model.Shop.PrePro.PreOrder preModel =null;
            if (count <= 0  || (productId <= 0 &&　String.IsNullOrWhiteSpace(sku)))
            {
                return Content("False");
            }
            Model.Shop.Products.SKUInfo skuInfo = null;
            if (!String.IsNullOrWhiteSpace(sku))
            {
                skuInfo = skuBll.GetModelBySKU(sku);
            }
            else
            {
                YSWL.MALL.ViewModel.Shop.ProductSKUModel prouctsku = skuBll.GetProductSKUInfoByProductId(productId); 
                if (prouctsku == null || prouctsku.ListSKUInfos == null || prouctsku.ListSKUInfos.Count <= 0)
                {
                    return Content("NOSKU");
                }
                skuInfo = prouctsku.ListSKUInfos[0];
            }
            //NOSKU
            if (skuInfo == null)
            {
                return Content("NOSKU");
            }
            Model.Shop.Products.ProductInfo prodModel = prodBll.GetModelByCache(skuInfo.ProductId);
            if (prodModel == null)
            {
                return Content("ProdModelNull");
            }

            #region  检测限购数
            int quantity = 0;//已购数
            int restCount=prodBll.GetRestrictionCount(productId);//限购数
            preModel = preBll.GetModel(CurrentUser.UserID, skuInfo.SKU, 1);
            if (preModel != null && preModel.PreOrderId > 0)
            {
                quantity = preModel.Count;
            }
            if (restCount > 0 && (quantity + count) > restCount)
            {
                return Content("GreaRestCount");
            }
            #endregion 

            if (preModel!=null && preModel.PreOrderId > 0)
            {
                preModel.Count = count;      
                return preBll.Update(preModel.PreOrderId, count, prodModel.DeliveryTip) ? Content("True") : Content("False");
            }
            else
            {
                preModel = new Model.Shop.PrePro.PreOrder();
                preModel.CreatedDate = DateTime.Now;
                preModel.ProductId = productId;  
                preModel.ProductName = prodModel.ProductName;
                preModel.UserId = currentUser.UserID;
                preModel.Count = count;
                preModel.DeliveryTip = prodModel.DeliveryTip;
                preModel.Phone = currentUser.Phone;
                preModel.SKU = skuInfo.SKU;
                preModel.Status = 1;
                preModel.UserName = currentUser.UserName;
                return preBll.Add(preModel) > 0 ? Content("True") : Content("False");
            }
        }

        #endregion

        #region 用户绑定

        public ActionResult TestWeiBo()
        {
            return View();
        }

        public ActionResult UserBind()
        {
            ViewBag.Title = "会员中心—帐号绑定";
            YSWL.MALL.ViewModel.UserCenter.UserBindList model = userBind.GetListEx(CurrentUser.UserID);
            return View(model);
        }

        [HttpPost]
        public void CancelBind(FormCollection collection)
        {
            if (!String.IsNullOrWhiteSpace(collection["BindId"]))
            {
                Response.ContentType = "application/text";
                int bindId = Common.Globals.SafeInt(collection["BindId"], 0);
                if (userBind.Delete(bindId))
                {
                    Response.Write("success");
                }
            }
        }

        #endregion 用户绑定

        #region 我的推荐
        //
        // GET: /Tao/User/
        /// <summary>
        /// 我的邀请
        /// </summary>
        /// <returns></returns>
        public ActionResult MyInvite()
        {
            BLL.CMS.Content contBll = new BLL.CMS.Content();
            ViewBag.Url = "Account/Register/" + Common.DEncrypt.Hex16.Encode(CurrentUser.UserID.ToString());
            BLL.SysManage.WebSiteSet webSiteSet = new BLL.SysManage.WebSiteSet(ApplicationKeyType.Shop);
            ViewBag.WebName = webSiteSet.WebName;

            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "我的盟友" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View();
        }

        /// <summary>
        ///  邀请列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public PartialViewResult InviteList(int pageIndex = 1, string viewName = "_InviteList")
        {
            int _pageSize = 5;

            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;
            int toalCount = inviteBll.GetRecordCount(" InviteUserId=" + CurrentUser.UserID);//获取总条数 
            if (toalCount < 1)
            {
                return PartialView(viewName);//NO DATA
            }
            List<Model.Members.UserInvite> lists = inviteBll.GetListByPage(" InviteUserId=" + CurrentUser.UserID, startIndex, endIndex);
            PagedList<Model.Members.UserInvite> pagelist = new PagedList<Model.Members.UserInvite>(lists, pageIndex, _pageSize, toalCount);
            if (Request.IsAjaxRequest())
                return PartialView(viewName, pagelist);
            return PartialView(viewName, pagelist);
        }
        #endregion

        #region 充值
        /// <summary>
        /// 余额 
        /// </summary>
        public ActionResult Balance(string viewName = "Balance")
        {
            ViewBag.Activity = CurrentUser.Activity ? "有效" : "无效";
            ViewBag.Balance = userEXBll.GetUserBalance(CurrentUser.UserID);
            ViewBag.BalanceDraw = balanDrawBll.GetBalanceDraw(CurrentUser.UserID);
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "账户余额" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View(viewName);
        }
        /// <summary>
        ///  收支列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public PartialViewResult BalanceDetList(int pageIndex = 1, string viewName = "_BalanceDetList")
        {
            int _pageSize = 10;

            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;
            int toalCount = balanDetaBll.GetRecordCount(" UserId =" + CurrentUser.UserID);//获取总条数 
            if (toalCount < 1)
            {
                return PartialView(viewName);//NO DATA
            }
            List<YSWL.MALL.Model.Pay.BalanceDetails> list = balanDetaBll.GetListByPage(" UserId = " + CurrentUser.UserID, startIndex, endIndex);
            PagedList<YSWL.MALL.Model.Pay.BalanceDetails> lists = new PagedList<YSWL.MALL.Model.Pay.BalanceDetails>(list, pageIndex, _pageSize, toalCount);
            if (Request.IsAjaxRequest())
                return PartialView(viewName, lists);
            return PartialView(viewName, lists);
        }
        /// <summary>
        /// 充值明细
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public PartialViewResult RechargeList(int pageIndex = 1, string viewName = "_RechargeList")
        {
            int _pageSize = 10;

            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;
            int toalCount = rechargeBll.GetRecordCount(" UserId =" + CurrentUser.UserID);//获取总条数 
            if (toalCount < 1)
            {
                return PartialView(viewName);//NO DATA
            }
            List<YSWL.MALL.Model.Pay.RechargeRequest> list = rechargeBll.GetRechargeListByPage(" UserId= " + CurrentUser.UserID, startIndex, endIndex);
            PagedList<YSWL.MALL.Model.Pay.RechargeRequest> lists = new PagedList<YSWL.MALL.Model.Pay.RechargeRequest>(list, pageIndex, _pageSize, toalCount);
            if (Request.IsAjaxRequest())
                return PartialView(viewName, lists);
            return PartialView(viewName, lists);
        }
        /// <summary>
        ///  提现列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public PartialViewResult DrawDetList(int pageIndex = 1, string viewName = "_DrawDetList")
        {
            int _pageSize = 10;

            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;
            int toalCount = balanDrawBll.GetRecordCount(" RequestType=1 AND UserId =" + CurrentUser.UserID);//获取总条数 
            if (toalCount < 1)
            {
                return PartialView(viewName);//NO DATA
            }
            List<YSWL.MALL.Model.Pay.BalanceDrawRequest> list = balanDrawBll.GetListByPage(" RequestType=1 AND UserId= " + CurrentUser.UserID, startIndex, endIndex);
            PagedList<YSWL.MALL.Model.Pay.BalanceDrawRequest> lists = new PagedList<YSWL.MALL.Model.Pay.BalanceDrawRequest>(list, pageIndex, _pageSize, toalCount);
            if (Request.IsAjaxRequest())
                return PartialView(viewName, lists);
            return PartialView(viewName, lists);
        }
        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public ActionResult Recharge(string viewName = "Recharge")
        {
            decimal rechargeRatio = BLL.SysManage.ConfigSystem.GetDecimalValueByCache("Shop_RechargeRatio");
            if (rechargeRatio > decimal.MinusOne)
            {
                ViewBag.RechargeRatio = rechargeRatio;
            }
            ViewBag.UserName = CurrentUser.UserName;
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "账户充值" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            List<YSWL.Payment.Model.PaymentModeInfo> list = Payment.BLL.PaymentModeManage.GetPaymentModes(YSWL.Payment.Model.DriveEnum.Web).Where(o => o.AllowRecharge == true).ToList();//筛选出允许在线支付的
            return View(viewName, list);
        }
        /// <summary>
        /// 提交充值
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        public ActionResult AjaxRecharge(FormCollection Fm)
        {
            if (!String.IsNullOrWhiteSpace(Fm["rechargmoney"]) && !String.IsNullOrWhiteSpace(Fm["payid"]))
            {
                int payid = Globals.SafeInt(Fm["payid"], 0);
                decimal rechMoney = Globals.SafeDecimal(Fm["rechargmoney"], decimal.Zero);
                if (payid > 0 && rechMoney > 0)
                {
                    #region 充值比例计算
                    decimal rechargeRadio = BLL.SysManage.ConfigSystem.GetDecimalValueByCache("Shop_RechargeRatio");
                    decimal money = rechMoney;
                    if (rechargeRadio > decimal.MinusOne)
                    {
                        money = Math.Round(rechMoney / rechargeRadio, 2);
                    }
                    #endregion

                    Model.Pay.RechargeRequest rechModel = new Model.Pay.RechargeRequest();
                    Payment.Model.PaymentModeInfo paymodel = Payment.BLL.PaymentModeManage.GetPaymentModeById(payid);
                    if (paymodel == null)
                    {
                        return Content("No");
                    }
                    rechModel.RechargeBlance = money;
                    rechModel.PaymentGateway = paymodel.Gateway;
                    rechModel.PaymentTypeId = payid;
                    rechModel.Status = 0;
                    rechModel.TradeDate = DateTime.Now;
                    rechModel.Tradetype = 1;
                    rechModel.UserId = CurrentUser.UserID;
                    long rechCode = rechargeBll.Add(rechModel);
                    if (rechCode > 0)
                    {
                        return Content(rechCode.ToString());
                    }
                }
            }
            return Content("No");
        }
        /// <summary>
        /// 确认请求 
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public ActionResult RechargeConfirm(int? id, string viewName = "RechargeConfirm")
        {
            if (id.HasValue)
            {
                #region SEO 优化设置
                IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
                ViewBag.Title = pageSetting.Title;
                ViewBag.Keywords = pageSetting.Keywords;
                ViewBag.Description = pageSetting.Description;
                #endregion
                Model.Pay.RechargeRequest rechmodel = rechargeBll.GetModelByCache(id.Value);
                if (rechmodel != null)
                {
                    ViewBag.RechargeId = id;
                    ViewBag.RechargeBlance = rechmodel.RechargeBlance;
                    return View(viewName);
                }
            }
            return Redirect(ViewBag.BasePath + "UserCenter/Recharge");
        }
        [HttpPost]
        public ActionResult DelRecharge(FormCollection fm)
        {
            long id = Globals.SafeLong(fm["Id"], 0);
            if (id <= 0)
            {
                return Content("False");
            }
            Model.Pay.RechargeRequest model = rechargeBll.GetModel(id);
            if (model != null && model.Status == 0)
            {
                if (rechargeBll.Delete(id))
                {
                    return Content("True");
                }
            }
            return Content("False");
        }

        #endregion

        #region 提现
        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public ActionResult Draw(string viewName = "Draw")
        {
            ViewBag.Balance = userEXBll.GetUserBalance(CurrentUser.UserID);
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "申请提现" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View(viewName);
        }

        /// <summary>
        /// 申请提现 ajax请求
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public ActionResult AjaxDraw(FormCollection Fm)
        {
            decimal amount = Globals.SafeDecimal(Fm["Amount"], 0);
            int typeid = Globals.SafeInt(Fm["Type"], -1);
            string bankcard = InjectionFilter.Filter(Fm["BankCard"]);
            if (amount <= 0 || typeid <= 0 || String.IsNullOrWhiteSpace(bankcard))
            {
                return Content("no");
            }
            string trueName = "";
            string bankName = "";
            if (typeid == 1) //帐号类型为银行卡
            {
                trueName = InjectionFilter.Filter(Fm["TrueName"]);
                bankName = InjectionFilter.Filter(Fm["BankName"]);
                if (String.IsNullOrWhiteSpace(trueName) || String.IsNullOrWhiteSpace(bankName))
                {
                    return Content("no");
                }
            }
            if (amount > userEXBll.GetUserBalance(CurrentUser.UserID))//提现金额大于余额
            {
                return Content("low");//余额不足
            }
            Model.Pay.BalanceDrawRequest balanDrawModel = new Model.Pay.BalanceDrawRequest();
            balanDrawModel.Amount = amount;
            balanDrawModel.BankCard = bankcard;
            balanDrawModel.CardTypeID = typeid;
            balanDrawModel.RequestStatus = 1;
            balanDrawModel.RequestTime = DateTime.Now;
            if (typeid == 1)
            {
                balanDrawModel.BankName = bankName;
                balanDrawModel.TrueName = trueName;
            }
            balanDrawModel.UserID = CurrentUser.UserID;
            if (balanDrawBll.AddEx(balanDrawModel))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }


        #endregion

        #region  物流信息
        public PartialViewResult ExpressList(long OrderId = -1, string viewName = "_ExpressList")
        {
            List<YSWL.MALL.ViewModel.Shop.Express> expressList = YSWL.MALL.Web.Components.ExpressHelper.GetExpress(OrderId);
            string apiType = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("OpenAPI_Express_ApiType");
            ViewBag.ApiType = apiType;
            if (apiType == "html")
            {
                ViewBag.ExpressUrl= YSWL.MALL.Web.Components.ExpressHelper.GetHtmlExpress(OrderId);
            }
            return PartialView(viewName, expressList);
        }
        #endregion


        #region 预定列表
        public ActionResult PreOrders(string viewName = "PreOrders")
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "我的预定";// + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View(viewName);
        }
        public PartialViewResult PreList(int pageIndex = 1, string viewName = "_PreList")
        {
            YSWL.MALL.BLL.Shop.PrePro.PreOrder preBll = new BLL.Shop.PrePro.PreOrder();
            int _pageSize = 8;
            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;
            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;

            //获取总条数
            int toalCount = 0;
            List<YSWL.MALL.Model.Shop.PrePro.PreOrder> list = preBll.GetModelList(CurrentUser.UserID, startIndex, endIndex, out toalCount);
            if (toalCount < 1)
            {
                return PartialView(viewName);//NO DATA
            }
            PagedList<YSWL.MALL.Model.Shop.PrePro.PreOrder> lists = new PagedList<YSWL.MALL.Model.Shop.PrePro.PreOrder>(list, pageIndex, _pageSize, toalCount);
            if (Request.IsAjaxRequest())
                return PartialView(viewName, lists);
            return PartialView(viewName, lists);
        }
        #endregion

        #region 推广商品
        public ActionResult CommissionPro(string viewName = "CommissionPro")
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "商品推广" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            //获取商品一级分类
            YSWL.MALL.BLL.Shop.Products.CategoryInfo categoryBll = new YSWL.MALL.BLL.Shop.Products.CategoryInfo();
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> categoryList=  categoryBll.GetCategorysByDepth(1);
            return View(viewName, categoryList);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public PartialViewResult PromoList(int cid=0,string name="",int pageIndex = 1, string viewName = "_PromoList")
        {
            YSWL.MALL.BLL.Shop.Commission.CommissionPro comProBll=new CommissionPro();
            int _pageSize = 10;
            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;
            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;
            int toalCount = comProBll.GetComProCount(cid,name);//获取总条数 
            if (toalCount < 1)
            {
                return PartialView(viewName);//NO DATA
            }
            List<YSWL.MALL.ViewModel.Shop.ProComModel> promoList = comProBll.GetComProByPage(cid, name,startIndex, endIndex);
            string baseCode = "{0}&" + currentUser.UserID;
            foreach (var item in promoList)
            {
                item.PromoCode = YSWL.Common.UrlOper.Base64Encrypt(String.Format(baseCode, item.ProductId));
            }

            PagedList<YSWL.MALL.ViewModel.Shop.ProComModel> lists = new PagedList<YSWL.MALL.ViewModel.Shop.ProComModel>(promoList, pageIndex, _pageSize, toalCount);
            if (Request.IsAjaxRequest())
                return PartialView(viewName, lists);
            return PartialView(viewName, lists);
        }
        #endregion

        #region 我的推广
        public ActionResult MyPromo(int pageIndex = 1,string viewName = "MyPromo")
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "我的推广佣金" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion

          

            int _pageSize = 15;

            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;
            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;
            int toalCount = 0;
            //获取总条数
            toalCount = comdetailBll.GetRecordCount(" UserId=" + CurrentUser.UserID);
            ViewBag.AllFee = comdetailBll.GetUserFees(currentUser.UserID);
            if (toalCount < 1)
            {
                return View(viewName);//NO DATA
            }
            List<YSWL.MALL.Model.Shop.Commission.CommissionDetail> detailList = comdetailBll.GetListByPageEx("UserID=" + CurrentUser.UserID, "TradeDate Desc ", startIndex, endIndex);
            PagedList<YSWL.MALL.Model.Shop.Commission.CommissionDetail> lists = new PagedList<YSWL.MALL.Model.Shop.Commission.CommissionDetail>(detailList, pageIndex, _pageSize, toalCount);

            string baseCode = "{0}&" + currentUser.UserID;
            foreach (var item in lists)
            {
                item.PromoCode = YSWL.Common.UrlOper.Base64Encrypt(String.Format(baseCode, item.ProductId));
            }

            return View(viewName,lists);
        }

        public PartialViewResult PromoLay( string viewName = "_PromoLay")
        {
            return PartialView(viewName);
        }
        #endregion


        #region 店铺收藏
        /// <summary>
        /// 店铺加入收藏
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        public ActionResult AjaxAddStoreFav(FormCollection Fm)
        {
            if (!String.IsNullOrWhiteSpace(Fm["suppId"]))
            {
                int suppId = Common.Globals.SafeInt(Fm["suppId"], 0);
                //是否已经收藏
                if (favoBll.Exists(suppId, currentUser.UserID, (int)YSWL.MALL.Model.Shop.FavoriteEnums.Store))
                {
                    return Content("Rep");
                }
                YSWL.MALL.Model.Shop.Favorite favMode = new YSWL.MALL.Model.Shop.Favorite();
                favMode.CreatedDate = DateTime.Now;
                favMode.TargetId = suppId;
                favMode.Type = (int)YSWL.MALL.Model.Shop.FavoriteEnums.Store;
                favMode.UserId = currentUser.UserID;
                return favoBll.AddEx(favMode) ? Content("True") : Content("False");
            }
            return Content("False");
        }

        #region 列表
        public ActionResult StoreFavor(string viewName = "StoreFavor")
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "我的收藏" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View(viewName);
        }
        public PartialViewResult StoreFavorList(int pageIndex = 1, int pageSize = 10, string viewName = "_StoreFavorList")
        {
            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * pageSize + 1 : 1;
            //计算分页结束索引
            int endIndex = pageIndex * pageSize;
            ViewBag.PageSize = pageSize;
            int toalCount = favoBll.GetStoreRecordCount(CurrentUser.UserID);//获取总条数 
            if (toalCount < 1)
            {
                return PartialView(viewName);//NO DATA
            }
            List<YSWL.MALL.ViewModel.Shop.FavoStoreModel> list = favoBll.GetStoreListByPage(CurrentUser.UserID, startIndex, endIndex);
            PagedList<YSWL.MALL.ViewModel.Shop.FavoStoreModel> pagelist = new PagedList<YSWL.MALL.ViewModel.Shop.FavoStoreModel>(list, pageIndex, pageSize, toalCount);
            return PartialView(viewName, pagelist);
        }
        #endregion


        /// <summary>
        /// 是否已经加入收藏
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        public ActionResult IsAddedFav(FormCollection Fm)
        {
            if (String.IsNullOrWhiteSpace(Fm["Id"]) || String.IsNullOrWhiteSpace(Fm["type"]))
            {
                return Content("False");
            }
            int targetId = Common.Globals.SafeInt(Fm["Id"], 0);
            int type = Common.Globals.SafeInt(Fm["type"], 0);
            return favoBll.Exists(targetId, currentUser.UserID, type) ? Content("True") : Content("False");
        }
        /// <summary>
        /// 根据targetId和用户Id删除收藏项
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        public ActionResult DelFav(FormCollection Fm)
        {
            if (String.IsNullOrWhiteSpace(Fm["targetId"]))
            {
                return Content("False");
            }
            int targetId = Globals.SafeInt(Fm["targetId"], 0);
            int type = Globals.SafeInt(Fm["type"], 0);
            if (targetId <= 0)
            {
                return Content("False");
            }
            return favoBll.Delete(targetId, CurrentUser.UserID, type) ? Content("True") : Content("False");
        }
        #endregion


        #region 会员升级VIP

        /// <summary>
        /// 会员升级VIP
        /// </summary>
        /// <returns></returns>
        public ActionResult UserVip()
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "升级VIP" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion

            //首页用户数据
            YSWL.MALL.Model.Members.UsersExpModel userEXModel = userEXBll.GetUsersModel(CurrentUser.UserID);
            if (userEXModel != null)
            {
                ViewBag.UserInfo = userEXModel;
            }
            ViewBag.Nickname = CurrentUser.NickName;
            ViewBag.userName = CurrentUser.UserName;
            ViewBag.passone = userEXModel.MSN;
            ViewBag.passtwo = userEXModel.MSN;

            int userLastTjrID = inviteBll.GetInvUIdByUsername(CurrentUser.UserName);//向上找一个推荐人为VIP

            ViewBag.tjrName = userBll.GetUserName(userLastTjrID);

            return View("UserVip");
        }

        /// <summary>
        /// 会员升级操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void UserUpVipMsg(FormCollection collection)
        {

            JsonObject json = new JsonObject();
            string Username = Common.InjectionFilter.Filter(collection["Username"]);//升级会员用户名
            string Nicktitle = Common.InjectionFilter.Filter(collection["Nicktitle"]);//会员姓名
            string Passone = Common.InjectionFilter.Filter(collection["Passone"]);//登录密码
            string Passtwo = Common.InjectionFilter.Filter(collection["Passtwo"]);//交易密码
            string UserLeave = Common.InjectionFilter.Filter(collection["UserLeave"]);//会员等级
            string Tjruser = Common.InjectionFilter.Filter(collection["Tjruser"]);//推荐人编号
            string strUserTel= Common.InjectionFilter.Filter(collection["Telphone"]);//手机号
            string strIssup = "NO";
            string strShenghuoguan = Common.InjectionFilter.Filter(collection["Shenghuoguan"]);//手机号

            if (string.IsNullOrWhiteSpace(Nicktitle))
            {
                json.Accumulate("STATUS", "NICKNAMENULL");
            }
            else if (string.IsNullOrWhiteSpace(strUserTel))
            {
                json.Accumulate("STATUS", "strUserTelNULL");
            }
            else if (string.IsNullOrWhiteSpace(Tjruser))
            {
                json.Accumulate("STATUS", "TjruserNULL");
            }
            else
            {
                json.Accumulate("STATUS", "TjruserNULL");
            }
            Response.Write(json.ToString());
        }


        #endregion 会员升级VIP

        #region 检查升级VIP时，各种条件

        
        /// <summary>
        ///检查推荐人用户名是否存在，不存在不运行升级
        /// </summary>
        [HttpPost]
        public void CheckTjrName(FormCollection collection)
        {
            if (!HttpContext.User.Identity.IsAuthenticated || CurrentUser == null)
            {
                RedirectToAction(ViewBag.BasePath + "Account/Login");//去登录
            }
            else
            {
                JsonObject json = new JsonObject();
                string txtTjrUsername = collection["TjrName"];//获取页面的积分数量

                ////从接口获取用户是否存在

                string strTjrUsername = spcom.GetUserTrueName(txtTjrUsername);

                if (strTjrUsername.Trim().Length > 0)
                {
                    json.Accumulate("msg", "" + strTjrUsername + "");
                    json.Accumulate("STATUS", "EXISTS");
                }
                else
                {
                    json.Accumulate("STATUS", "NOTEXISTS");
                }

                Response.Write(json.ToString());
            }
        }

        #endregion 检查升级VIP时，各种条件


        #region  积分转移 检查用户输入的用户名是否存在

        /// <summary>
        ///检查用户输入的用户名是否存在（把用户本身去除）
        /// </summary>
        [HttpPost]
        public void ExistsUserNameJF(FormCollection collection)
        {
            if (!HttpContext.User.Identity.IsAuthenticated || CurrentUser == null)
            {
                RedirectToAction(ViewBag.BasePath + "Account/Login");//去登录
            }
            else
            {
                JsonObject json = new JsonObject();
                string username = collection["NickName"];//获取页面的用户名

                if (CurrentUser.UserName != username)
                {
                    if (!string.IsNullOrWhiteSpace(username))
                    {

                        if (userBll.ExistsUserName(username))//判断输入的用户名是否存在
                        {
                            YSWL.MALL.Model.Members.Users userModel = userBll.GetModelByCache(CurrentUser.UserID);
                            json.Accumulate("msg", "" + userModel.NickName + "");
                            json.Accumulate("STATUS", "EXISTS");
                        }
                        else
                        {
                            json.Accumulate("STATUS", "NOTEXISTS");
                        }
                    }
                    else
                    {
                        json.Accumulate("STATUS", "NOTNULL");
                    }
                }
                else
                {
                    json.Accumulate("STATUS", "NOTEXISTS");
                }
                Response.Write(json.ToString());
            }
        }


        #endregion 检查用户输入的用户名是否存在（把用户本身去除）

        #region 积分互转

        /// <summary>
        /// 积分互转
        /// </summary>
        /// <returns></returns>
        public ActionResult SendPoint()
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "积分互转" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion

            //首页用户数据
            YSWL.MALL.Model.Members.UsersExpModel userEXModel = userEXBll.GetUsersModel(CurrentUser.UserID);
            if (userEXModel != null)
            {
                ViewBag.UserInfo = userEXModel;
            }

            ViewBag.Name = Request.Params["name"];
            return View("SendPoint");
        }

        #endregion 积分互转

        #region 积分转移操作

        /// <summary>
        ///检查用户输入的积分数量是否足够
        /// </summary>
        [HttpPost]
        public void PointToB(FormCollection collection)
        {
            if (!HttpContext.User.Identity.IsAuthenticated || CurrentUser == null)
            {
                RedirectToAction(ViewBag.BasePath + "Account/Login");//去登录
            }
            else
            {
                JsonObject json = new JsonObject();


                string txtTitle = collection["Title"];//获取页面的积分数量
                int txtPint = int.Parse(txtTitle.ToString());

                YSWL.MALL.Model.Members.UsersExpModel userEXModel = userEXBll.GetUsersModel(CurrentUser.UserID);
                int UserPonint = int.Parse(userEXModel.Points.ToString());//用户积分

                if (txtPint <= UserPonint)
                {
                    json.Accumulate("STATUS", "EXISTS");
                }
                else
                {
                    json.Accumulate("STATUS", "NOTEXISTS");
                }
                Response.Write(json.ToString());
            }
        }

        /// <summary>
        /// 积分转移操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void SendPointMsg(FormCollection collection)
        {

            JsonObject json = new JsonObject();
            string username = Common.InjectionFilter.Filter(collection["NickName"]);//转给会员
            string title = Common.InjectionFilter.Filter(collection["Title"]);//积分数量
            if (string.IsNullOrWhiteSpace(username))
            {
                json.Accumulate("STATUS", "NICKNAMENULL");
            }
            else if (string.IsNullOrWhiteSpace(title))
            {
                json.Accumulate("STATUS", "TITLENULL");
            }
            else
            {
                BLL.Members.Users bll = new BLL.Members.Users();
                if (bll.ExistsUserName(username))
                {
                    int ReceiverID = bll.GetUserIdByUserName(username); ////转给对方的用户会员ID

                    string Description = "会员" + CurrentUser.UserName + "转给" + username + "积分：" + title + "";

                    string DescriptionRe = "会员" + username + "接收" + CurrentUser.UserName + "积分：" + title + "";

                    int Score = Common.Globals.SafeInt(title, 0);



                    BLL.Members.PointsDetail pdetail = new PointsDetail();

                    if (pdetail.PointsHuzhuan(98, CurrentUser.UserID, Description, Score, ReceiverID.ToString(), 1) > 0 && pdetail.PointsHuzhuan(99, ReceiverID, DescriptionRe, Score, CurrentUser.UserID.ToString(), 0) > 0)
                    {
                        json.Accumulate("STATUS", "SUCC");
                    }
                    else
                    {
                        json.Accumulate("STATUS", "FAIL");
                    }
                }
                else
                {
                    json.Accumulate("STATUS", "NICKNAMENOTEXISTS");
                }
            }
            Response.Write(json.ToString());
        }

        #endregion 积分转移操作



        #region  提交订单时，判断生活馆是否存在
        /// <summary>
        ///提交订单时，判断生活馆是否存在
        /// </summary>
        [HttpPost]
        public void ExistsWdbhfzr(FormCollection collection)
        {
            if (!HttpContext.User.Identity.IsAuthenticated || CurrentUser == null)
            {
                RedirectToAction(ViewBag.BasePath + "Account/Login");//去登录
            }
            else
            {
                JsonObject json = new JsonObject();
                string Wdbh = collection["ShenghgName"];//获取页面的网点编号


                YSWL.MALL.Model.Members.UsersExpModel usersModel = userEXBll.GetUsersModel(CurrentUser.UserID);
                //网点自提，网点编号不能为空
                Wdbh = InjectionFilter.Filter(Wdbh);

                if (Wdbh.Length > 0)
                {
                    string strwdbhname = spcom.GetIsWdbh(Wdbh).ToString();
                    if (strwdbhname != "不是生活馆")
                    {
                        if (strwdbhname.Length > 0)
                        {
                            json.Accumulate("STATUS", "EXISTS");
                            json.Accumulate("msg", "" + strwdbhname + "");
                        }
                        else
                        {
                            json.Accumulate("STATUS", "NOTEXISTS");
                        }
                    }
                    else
                    {
                        json.Accumulate("STATUS", "NOTshenghg");
                    }
                }
                else
                {
                    json.Accumulate("STATUS", "NOTEXISTS");
                }
                Response.Write(json.ToString());
            }
        }
        #endregion

        
    }
}