using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YSWL.MALL.BLL.Settings;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.MALL.Model.Shop.Products;
using YSWL.MALL.Model.SysManage;
using YSWL.MALL.Web.Components.Setting.Shop;
using CategoryInfo = YSWL.MALL.Model.Shop.Products.CategoryInfo;

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{
    public partial class PartialController : MShopControllerBase
    {
        //
        // GET: /Mobile/Partial/
        private BLL.Shop.Supplier.SupplierInfo supplierBll = new BLL.Shop.Supplier.SupplierInfo();
        private BLL.Shop.Activity.ActivityInfo activInfoBll = new BLL.Shop.Activity.ActivityInfo();
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Footer(string viewName = "_Footer")
        {

            if (currentUser!=null)
            {
                ViewBag.usernickname = currentUser.NickName;//用户已登录
            }
            //是否开启微信自动登录
            ViewBag.IsAutoLogin = Common.Globals.SafeBool(WeChat.BLL.Core.Config.GetValueByCache("WeChat_AutoLogin", -1, "AA"), false);
            ViewBag.IsHideMenu = Common.Globals.SafeBool(WeChat.BLL.Core.Config.GetValueByCache("WeChat_HideMenu", -1, "AA"), false);
            return PartialView(viewName);
        }

        public PartialViewResult Header(string viewName = "_Header")
        {
            ViewBag.Name = BLL.SysManage.ConfigSystem.GetValueByCache("Opertors_Name", ApplicationKeyType.Shop);
            ViewBag.MShopName = BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_MShop_Name");
            ViewBag.MShopLogo = BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_MShop_Logo");
            return PartialView(viewName);
        }
        #region 广告位
        public PartialViewResult AdDetail(int id, string ViewName = "_IndexAd")
        {
            YSWL.MALL.BLL.Settings.Advertisement bll = new Advertisement();
            List<YSWL.MALL.Model.Settings.Advertisement> list = bll.GetListByAidCache(id);
            return PartialView(ViewName, list);
        }

        /// <summary>
        /// 小广告
        /// </summary>
        /// <param name="AdvPositionId"></param>
        /// <returns></returns>
        public PartialViewResult AD(int AdvPositionId, string viewName = "_AD")
        {
            BLL.Settings.Advertisement bllAdvertisement = new Advertisement();
            Model.Settings.Advertisement model = bllAdvertisement.GetModelByAdvPositionId(AdvPositionId);
            return PartialView(viewName, model);
        }
        #endregion

        #region 菜单导航
        public PartialViewResult Navigation(string viewName = "_Navigation")
        {
            return PartialView(viewName);
        }
        #endregion 

        #region 商家信息
        public PartialViewResult StoreLayer(int suppId = 0, string viewName = "_StoreLayer")
        {
            if (suppId <=0) {
                return PartialView(viewName);
            }
            BLL.Shop.Supplier.SupplierInfo bllSupp = new BLL.Shop.Supplier.SupplierInfo();
            Model.Shop.Supplier.SupplierInfo model = bllSupp.GetModelByCache(suppId);
            if (model != null && model.Status == 1 && model.StoreStatus == 1)
            {
                model.Address = model.RegionId.HasValue
                                    ? new BLL.Ms.Regions().GetAddress(model.RegionId)
                                    : "暂未设置";
                return PartialView(viewName, model);
            }
            return PartialView(viewName);
        }

        #endregion

        #region 推荐商品
        public PartialViewResult ProductRec(ProductRecType Type = ProductRecType.Recommend, int Cid = 0, int Top = 5, string ViewName = "_ProductRec")
        {
            YSWL.MALL.BLL.Shop.Products.ProductInfo productBll = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> productList = productBll.GetProductRecList(Type, Cid, Top);
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> prolist = new List<Model.Shop.Products.ProductInfo>();
            #region 是否静态化
            string IsStatic = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("ShopProductStatic");
            string basepath = YSWL.Components.MvcApplication.GetCurrentRoutePath(YSWL.Web.AreaRoute.Shop);
            if (productList != null)
            {
                foreach (var item in productList)
                {
                    //zhou20181212修改
                    item.LowestSalePrice = decimal.Parse((item.LowestSalePrice - item.Gwjf).ToString());

                    if (IsStatic == "1")
                    {
                        item.SeoUrl = PageSetting.GetProStaticUrl(Convert.ToInt32(item.ProductId.ToString())).Replace("//", "/");
                    }
                    else if (IsStatic == "2")
                    {
                        item.SeoUrl = basepath + "Product-" + item.ProductId + ".html";
                    }
                    else
                    {
                        item.SeoUrl = basepath + "Product/Detail/" + item.ProductId;
                    }
                    prolist.Add(item);
                }
                return PartialView(ViewName, prolist);
            }
            else
            {
                return PartialView(ViewName, productList);
            }
            #endregion
        }
        #endregion

        public PartialViewResult CategoryList(int Cid = 0, int Top = 10, string ViewName = "_CategoryList")
        {
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateList = YSWL.MALL.BLL.Shop.Products.CategoryInfo.GetAvailableCateList();
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> parentList = cateList.Where(c => c.Depth == 1).OrderBy(c=>c.DisplaySequence).ToList();
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> categoryInfos=new List<CategoryInfo>();//= cateList.Where(c => c.ParentCategoryId == Cid).ToList();
            if (parentList.Count > 0)
            {
                List<YSWL.MALL.Model.Shop.Products.CategoryInfo> sonList;
                foreach (var item in parentList)
                {
                    sonList = cateList.Where(c => c.ParentCategoryId == item.CategoryId).OrderBy(c=>c.DisplaySequence).ToList();
                    categoryInfos.Add(item);
                    categoryInfos.AddRange(sonList);
                }
            }
            return PartialView(ViewName, categoryInfos);
        }

        #region  商品分类
        public PartialViewResult CateList(int parentId = 0, string viewName = "_CateList")
        {
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateList = YSWL.MALL.BLL.Shop.Products.CategoryInfo.GetAvailableCateList();
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> categoryInfos = cateList.Where(c => c.ParentCategoryId == parentId).OrderBy(c => c.DisplaySequence).ToList();
            YSWL.MALL.Model.Shop.Products.CategoryInfo parentInfo =
                cateList.FirstOrDefault(c => c.CategoryId == parentId);
            ViewBag.ParentId = -1;
            if (parentInfo != null)
            {
                ViewBag.CurrentName = parentInfo.Name;
                ViewBag.ParentId = parentInfo.ParentCategoryId;
            }
            return PartialView(viewName, categoryInfos);
        }
        #endregion 

        /// <summary>
        /// 获取推荐的店铺
        /// </summary>
        /// <param name="top"></param>
        /// <param name="rec"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public PartialViewResult RecStore(int top = 5, int rec = 1, string viewName = "_RecStore")
        {
            List<Model.Shop.Supplier.SupplierInfo> list = supplierBll.GetList(top, rec);
            return PartialView(viewName, list);
        }
        public ActionResult SuppLogo(int id = 0, string size = "")
        {
            //过滤size 值
            if (size.Contains("/") || size.Contains(".."))
            {
                return null;
            }
            string pathName = string.Format("/Upload/Supplier/Logo/{0}_{1}", id, size);
            if (System.IO.File.Exists(Server.MapPath(pathName)))
            {
                return File(pathName, "application/x-img");
            }
            return null;
        }

        /// <summary>
        /// 获取商家名称 (目前只有订餐模版使用)
        /// </summary>
        /// <returns></returns>
        public static string GetName()
        {
           return   BLL.SysManage.ConfigSystem.GetValueByCache("Opertors_Name", ApplicationKeyType.Shop);
        }

        #region 促销活动
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public PartialViewResult ActivityList(long pId = 0, int suppId = 0, string viewName = "_ActivityList")
        {
            return PartialView(viewName, activInfoBll.GetActivityList(pId, suppId));
        }
        #endregion

    }
}
