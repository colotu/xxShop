using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using YSWL.MALL.BLL.Shop.Order;
using YSWL.Common;
using YSWL.Components.Setting;
using YSWL.MALL.Model.Shop;
using YSWL.MALL.Model.Shop.Order;
using YSWL.MALL.Web.Components.Setting.Shop;
using YSWL.Json;
using YSWL.Json.Conversion;

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{ 
    public  partial class UserCenterController : MShopControllerBaseUser
{
        YSWL.MALL.BLL.Shop.Favorite favBll = new BLL.Shop.Favorite();
        private readonly Orders _orderManage = new Orders();
        BLL.Shop.Products.ProductReviews prodRevBll = new BLL.Shop.Products.ProductReviews();
        public ActionResult Setting()
        {
            ViewBag.tel = BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_MShop_Phone");
            return View();
        }

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
            return favBll.Exists(targetId, currentUser.UserID, type) ? Content("True") : Content("False");
        }

        /// <summary>
        /// 根据商品Id和用户Id删除收藏你
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        public ActionResult DelFav(FormCollection Fm)
        {
            if (String.IsNullOrWhiteSpace(Fm["pId"]))
            {
                return Content("False");
            }
            int targetId = Globals.SafeInt(Fm["pId"], 0);
            int type = Globals.SafeInt(Fm["type"], 0);
            if (targetId <=0) {
                return Content("False");
            }
            return favBll.Delete(targetId, CurrentUser.UserID, type)? Content("True") : Content("False");
        }
        /// <summary>
        /// 返回订单状态
        /// </summary>
        /// <param name="paymentGateway"></param>
        /// <param name="orderStatus"></param>
        /// <param name="paymentStatus"></param>
        /// <param name="shippingStatus"></param>
        /// <returns></returns>
        public static string GetOrderTypeEx(string paymentGateway, int orderStatus, int paymentStatus, int shippingStatus)
        {
            string str = "";
            YSWL.MALL.BLL.Shop.Order.Orders orderBll = new Orders();
            EnumHelper.OrderMainStatus orderType = orderBll.GetOrderType(paymentGateway,
                                    orderStatus,
                                    paymentStatus,
                                    shippingStatus);
            switch (orderType)
            {
                //  订单组合状态 1 等待付款   | 2 等待处理 | 3 取消订单 | 4 订单锁定 | 5 等待付款确认 | 6 正在处理 |7 配货中  |8 已发货 |9  已完成
                case EnumHelper.OrderMainStatus.Paying:
                    str = "待付款";
                    break;
                case EnumHelper.OrderMainStatus.PreHandle:
                    str = "待发货";
                    break;
                case EnumHelper.OrderMainStatus.Cancel:
                    str = "已取消";
                    break;
                case EnumHelper.OrderMainStatus.Locking:
                    str = "待付款";
                    break;
                case EnumHelper.OrderMainStatus.PreConfirm:
                    str = "待发货";
                    break;
                case EnumHelper.OrderMainStatus.Handling:
                    str = "待发货";
                    break;
                case EnumHelper.OrderMainStatus.Shipping:
                    str = "待发货";
                    break;
                case EnumHelper.OrderMainStatus.Shiped:
                    str = "已发货";
                    break;
                case EnumHelper.OrderMainStatus.Complete:
                    str = "已完成";
                    break;
                default:
                    str = "未知状态";
                    break;
            }
            return str;
        }


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
            ViewBag.SetDetfaultAddress = false;//设为默认地址
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
            }else{
                //设为默认地址
                ViewBag.SetDetfaultAddress= !addressManage.ExistsDefaultAddress(CurrentUser.UserID);
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
                if (baseModel == null || baseModel.UserId != currentUser.UserID)
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
                    if (model.IsDefault)
                    {
                        addressManage.SetDefaultShipAddress(currentUser.UserID, model.ShippingId);
                    }
                    return Content("OK");
                }
                return Content("NO");
            }
            //Add
            model.UserId = CurrentUser.UserID;
            model.ShippingId = addressManage.Add(model);
            if (model.ShippingId > 0)
            {
                if (model.IsDefault)
                {
                    addressManage.SetDefaultShipAddress(currentUser.UserID, model.ShippingId);
                }
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

        #region 收藏
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
                if (favBll.Exists(suppId, currentUser.UserID, (int)YSWL.MALL.Model.Shop.FavoriteEnums.Store))
                {
                    return Content("Rep");
                }
                YSWL.MALL.Model.Shop.Favorite favMode = new YSWL.MALL.Model.Shop.Favorite();
                favMode.CreatedDate = DateTime.Now;
                favMode.TargetId = suppId;
                favMode.Type = (int)YSWL.MALL.Model.Shop.FavoriteEnums.Store;
                favMode.UserId = currentUser.UserID;
                return favBll.AddEx(favMode) ? Content("True") : Content("False");
            }
            return Content("False");
        }
        #endregion 

        public ActionResult MyCouponEx(int p = 1)
        {
            int status = Common.Globals.SafeInt(Request.Params["s"], 1);

            int _pageSize = 15;

            //计算分页起始索引
            int startIndex = p > 1 ? (p - 1) * _pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = p * _pageSize;
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
            PagedList<YSWL.MALL.Model.Shop.Coupon.CouponInfo> lists = new PagedList<YSWL.MALL.Model.Shop.Coupon.CouponInfo>(infoList, p, _pageSize, toalCount);
            return View(lists);
        }

        public PartialViewResult CouponList(int pageIndex = 1, int pageSize = 8, int state = 1, string viewName = "_CouponList")
        {
            #region RouteDataParam

            ViewBag.DataParam = String.Format("{0}state:'{1}'{2}", "{", state, "}");
            #endregion

            ViewBag.PageSize = pageSize;

            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = pageIndex * pageSize;
            int toalCount = 0;


            switch (state)
            {
                case  1:
                    toalCount = infoBll.GetRecordCount(String.Format(" UserID={0} and Status={1} and EndDate > '{2}'", currentUser.UserID, state, System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
                    break;
                case   4:
                    
                    //获取总条数
                    toalCount = infoBll.GetRecordCount(String.Format(" UserID={0} and Status={1} and EndDate < '{2}'", currentUser.UserID, 1, System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
                    break;
                default:
                    toalCount = infoBll.GetRecordCount(String.Format(" UserID={0} and Status={1}", currentUser.UserID, state));
                    break;
            }

           
            if (toalCount < 1)
            {
                return PartialView(viewName);//NO DATA
            }

            List<YSWL.MALL.Model.Shop.Coupon.CouponInfo> infoList;

            switch (state)
            {
                case 1:
                    infoList =
                    infoBll.GetListByPageEX(
                        String.Format(" UserID={0} and Status={1} and EndDate > '{2}'", currentUser.UserID, state,
                            System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
                        " GenerateTime desc", startIndex, endIndex);
                    break;
                case 4:
                    
                    infoList = infoBll.GetListByPageEX(String.Format(" UserID={0} and Status={1} and EndDate < '{2}'", currentUser.UserID, 1, System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
                        " GenerateTime desc", startIndex, endIndex);
                    break;
                default:
                    infoList = infoBll.GetListByPageEX(String.Format(" UserID={0} and Status={1}", currentUser.UserID, state), " GenerateTime desc", startIndex, endIndex);
                    break;
            }

            
            YSWL.MALL.BLL.Shop.Coupon.CouponClass classBll = new YSWL.MALL.BLL.Shop.Coupon.CouponClass();
            foreach (var Info in infoList)
            {
                YSWL.MALL.Model.Shop.Coupon.CouponClass classModel = classBll.GetModelByCache(Info.ClassId);
                Info.ClassName = classModel == null ? "" : classModel.Name;
            }
            PagedList<YSWL.MALL.Model.Shop.Coupon.CouponInfo> lists = new PagedList<YSWL.MALL.Model.Shop.Coupon.CouponInfo>(infoList, pageIndex, pageSize, toalCount);


            if (Request.IsAjaxRequest())
                return PartialView(viewName, lists);
            return PartialView(viewName, lists);
        }

        public PartialViewResult PointsDetailList(int pageIndex = 1, int pageSize = 8,string viewName = "_PointsDetai" )
        {
          

            ViewBag.PageSize = pageSize;
            
            //首页用户数据
            YSWL.MALL.Model.Members.UsersExpModel userEXModel = userEXBll.GetUsersModel(CurrentUser.UserID);
            if (userEXModel != null)
            {
                ViewBag.UserInfo = userEXModel.Points.HasValue ? userEXModel.Points : 0;
                ViewBag.NickName = userEXModel.NickName;
            }
            

            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = pageIndex * pageSize;
            int toalCount = 0;

            //获取总条数
            toalCount = detailBll.GetRecordCount(" UserID=" + CurrentUser.UserID);
            if (toalCount < 1)
            {
                return PartialView(viewName);//NO DATA
            }
            List<YSWL.MALL.Model.Members.PointsDetail> detailList = detailBll.GetListByPageEX("UserID=" + CurrentUser.UserID, "", startIndex, endIndex);
            if (detailList != null && detailList.Count > 0)
            {
                foreach (var item in detailList)
                {
                    item.RuleName = GetRuleName(item.RuleId);
                }
            }
            PagedList<YSWL.MALL.Model.Members.PointsDetail> lists = new PagedList<YSWL.MALL.Model.Members.PointsDetail>(detailList, pageIndex, pageSize, toalCount);

            if (Request.IsAjaxRequest())
                return PartialView(viewName, lists);
            return PartialView(viewName, lists);

        }


        /// <summary>
        /// 系统信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SysInfo()
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "系统消息" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion

            return View();
        }

        public PartialViewResult SysInfoList(int pageIndex = 1, int pageSize = 10, string viewName = "_PointsDetai")
        {

            ViewBag.PageSize = pageSize;
            PagedList<YSWL.MALL.Model.Members.SiteMessage> list = bllSM.GetAllSystemMsgListByMvcPage(CurrentUser.UserID, -1, CurrentUser.UserType, pageSize, pageIndex);
            foreach (YSWL.MALL.Model.Members.SiteMessage item in list)
            {
                if (item.ReceiverIsRead == false)
                    bllSM.SetSystemMsgStateToAlreadyRead(item.ID, CurrentUser.UserID, CurrentUser.UserType);
            }

            return PartialView(viewName, list);
        }

        public ActionResult SysInfoDetail(int Id)
        {
            Model.Members.SiteMessage model = bllSM.GetModel(Id);
            bllSM.SetReceiveMsgAlreadyRead(Id);
            return View(model);

        }


        public ActionResult Preview(long Id=-1,string viewName="Preview")
        {
            OrderInfo orderModel = _orderManage.GetModelInfo(Id);
            if (orderModel == null ||
                orderModel.BuyerID != CurrentUser.UserID || orderModel.IsReviews || orderModel.OrderStatus != (int)EnumHelper.OrderStatus.Complete
                )
                return Redirect(ViewBag.BasePath + "UserCenter/Orders");

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
                if (prodRevBll.AddEx(modelList, orderId, out pointers, out rankScore))
                {
                    return Content(string.Format("{0}|{1}", pointers, rankScore));//评论成功   返回获得的积分
                }
            }
            return Content("false");
        }


        public ActionResult MyPreview()
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "我的评论" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            ViewBag.UserId = currentUser.UserID;
            #endregion
            return View();
            

        }


        public PartialViewResult MyPeviewList(int pageIndex = 1, int pageSize = 10, string viewName = "_MyPreviewList")
        {
            int startIndex = (pageIndex-1)*pageSize+1;
            int endindex = pageIndex*pageSize;
            int total = prodRevBll.GetRecord(currentUser.UserID);
            ViewBag.PageSize = pageSize;
            List<YSWL.MALL.Model.Shop.Products.Reviews> li = prodRevBll.GetListExByPage(currentUser.UserID, startIndex, endindex);
        
            PagedList<YSWL.MALL.Model.Shop.Products.Reviews> list = new PagedList<YSWL.MALL.Model.Shop.Products.Reviews>(li, pageIndex, pageSize, total);
            return PartialView(viewName, list);
        }






        public PartialViewResult FavorStoreList(int pageIndex = 1, int pageSize = 30, string viewName = "_FavorStoreList")
        {

            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * pageSize + 1 : 1;
            //计算分页结束索引
            int endIndex = pageIndex * pageSize;         
            ViewBag.PageSize = pageSize;
            List<YSWL.MALL.ViewModel.Shop.FavoStoreModel> list = favBll.GetStoreListByPage(CurrentUser.UserID, startIndex, endIndex);
            if (Request.IsAjaxRequest())
                return PartialView(viewName, list);
            return PartialView(viewName, list);
        }


    }
}