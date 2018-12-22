using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using YSWL.MALL.BLL.Settings;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.MALL.Model.Shop.Products;
using YSWL.MALL.Web.Components.Setting.Shop;
using YSWL.Web;

namespace YSWL.MALL.Web.Areas.Shop.Controllers
{
    public class PartialController : ShopControllerBase
    {
        private YSWL.MALL.BLL.Shop.Products.ProductInfo productBll = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
        private YSWL.MALL.BLL.Shop.Products.CategoryInfo categoryBll = new YSWL.MALL.BLL.Shop.Products.CategoryInfo();
        private BLL.CMS.ContentClass contentclassBll = new BLL.CMS.ContentClass();
        private BLL.Shop.Supplier.SupplierInfo supplierBll = new BLL.Shop.Supplier.SupplierInfo();
        private BLL.Shop.Activity.ActivityInfo activInfoBll = new BLL.Shop.Activity.ActivityInfo();
        private BLL.Ms.Regions regsBll = new BLL.Ms.Regions();
        #region 网站共通页面
        public PartialViewResult Header(string viewName = "_Header")
        {
            BLL.SysManage.WebSiteSet webSiteSet = new BLL.SysManage.WebSiteSet(Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Logo = webSiteSet.LogoPath;
            ViewBag.WebName = webSiteSet.WebName;
            ViewBag.Domain = webSiteSet.WebSite_Domain;
            return PartialView(viewName);
        }
 
        public PartialViewResult Search(string viewName = "_Search")
        {
            BLL.SysManage.WebSiteSet webSiteSet = new BLL.SysManage.WebSiteSet(Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Logo = webSiteSet.LogoPath;
            ViewBag.WebName = webSiteSet.WebName;
            ViewBag.Domain = webSiteSet.WebSite_Domain;
            return PartialView(viewName);
        }

        #region 网站导航

        public PartialViewResult Navigation(int top=0, string viewName = "_Navigation", string Theme = "M1")
        {
            YSWL.MALL.BLL.Settings.MainMenus meneBll = new BLL.Settings.MainMenus();
            List<YSWL.MALL.Model.Settings.MainMenus> NavList = meneBll.GetMenusByAreaByCacle(YSWL.MALL.Model.Ms.EnumHelper.AreaType.Shop, Theme,top);
            return PartialView(viewName, NavList);
        }

        #endregion 网站导航
        public PartialViewResult Footer(string viewName = "_Footer")
        {
            return PartialView(viewName);
        }
        #endregion

        #region 推荐商品
        public PartialViewResult ProductRec(ProductRecType Type = ProductRecType.Recommend, int Cid = 0, int Top = 5, string ViewName = "_ProductRec")
        {
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> productList = productBll.GetProductRecList(Type, Cid, Top);
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> prolist = new List<Model.Shop.Products.ProductInfo>();
            #region 是否静态化
            string IsStatic = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("ShopProductStatic");
            string basepath = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.Shop);
            if (productList != null)
            {
                foreach (var item in productList)
                {
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

        public PartialViewResult SearchCart(string ViewName = "_SearchCart")
        {
            int userId = currentUser == null ? -1 : currentUser.UserID;
            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(userId);
            YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo = cartHelper.GetShoppingCart();
            ViewBag.CartCount = cartInfo.Quantity;

            BLL.SysManage.WebSiteSet webSiteSet = new BLL.SysManage.WebSiteSet(Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Logo = webSiteSet.LogoPath;
            return PartialView(ViewName);
        }


        #region 广告位
        public PartialViewResult AdDetail(int id, string ViewName = "_IndexAd")
        {
            YSWL.MALL.BLL.Settings.Advertisement bll = new Advertisement();
            List<YSWL.MALL.Model.Settings.Advertisement> list = bll.GetListByAidCache(id);
            ViewBag.AdDetailId = id;
            return PartialView(ViewName, list);
        }

        /// <summary>
        /// 小广告
        /// </summary>
        /// <param name="AdvPositionId"></param>
        /// <returns></returns>
        public PartialViewResult AD(int AdvPositionId, string viewName = "_AD")
        {
            BLL.Settings.Advertisement bllAdvertisement = new BLL.Settings.Advertisement();
            Model.Settings.Advertisement model = bllAdvertisement.GetModelByAdvPositionId(AdvPositionId);
            return PartialView(viewName, model);
        }
        #endregion

        #region 商品分类
        public PartialViewResult CategoryList(int Cid = 0, int Top = 10, string ViewName = "_CategoryList")
        {
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateList = YSWL.MALL.BLL.Shop.Products.CategoryInfo.GetAvailableCateList();
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> categoryInfos = cateList.Where(c => c.ParentCategoryId == Cid).ToList();
            if (Top > 0)
            {
                categoryInfos = categoryInfos.Take(Top).ToList();
            }
            return PartialView(ViewName, categoryInfos);
        }
        public PartialViewResult IndexSecondCategoryList(int Cid = 0, int Top = 10, string ViewName = "_SecondCateAll")
        {
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateList = YSWL.MALL.BLL.Shop.Products.CategoryInfo.GetAvailableCateList();
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> categoryInfos = null;
            if (Cid == 0)
                categoryInfos = cateList.Where(c => c.ParentCategoryId == Cid).ToList();
            else
            {
                var xxx = cateList.FirstOrDefault(c => c.CategoryId == Cid);
                if (xxx != null)
                    categoryInfos = cateList.Where(c => c.Path.StartsWith(xxx.Path + "|")).ToList();
            }
            if (Top > 0)
            {
                categoryInfos = categoryInfos.Take(Top).ToList();
            }
            return PartialView(ViewName, categoryInfos);
        }
        #endregion

        #region 头部登录注册导航
        public PartialViewResult Login(string notLoginView = "_NotLogin", string userLoginView = "_UserLogin")
        {
            //判断用户是否已登录  用来决定页面上显示什么(登录 或  退出)
            if (HttpContext.User.Identity.IsAuthenticated && CurrentUser != null && CurrentUser.UserType != "AA")
            {
                ViewBag.loginnickname = CurrentUser.NickName;
                return PartialView(userLoginView);
            }
            ViewBag.RegisterToggle = BLL.SysManage.ConfigSystem.GetValueByCache("Shop_RegisterToggle");//注册方式  
            return PartialView(notLoginView);
        }
        #endregion

        #region 文章

        public PartialViewResult ContentList(string viewName, int ClassID, int Top)
        {
            BLL.CMS.Content conBll = new BLL.CMS.Content();
            List<Model.CMS.Content> list = conBll.GetModelList(ClassID, Top);
            ViewBag.contentclassName = contentclassBll.GetClassnameById(ClassID);
            return PartialView(viewName, list);
        }

        #endregion

        #region 菜单详细
        public PartialViewResult MenuDetail(int Cid = 0, int Top = 4, string ViewName = "_MenuDetail")
        {
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateList = YSWL.MALL.BLL.Shop.Products.CategoryInfo.GetAvailableCateList();
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> categoryInfos = cateList.Where(c => c.ParentCategoryId == Cid).ToList();
            ViewBag.Cid = Cid;
            int haschildren = 0;//子节点的个数
            categoryInfos.ForEach(x =>
                {
                    if (x.HasChildren == true)
                        haschildren++;
                });
            ViewBag.haschildren = haschildren;           
            return PartialView(ViewName, categoryInfos);
        }
        #endregion

        #region 热门关键字
        public PartialViewResult HotKeyword(int Cid = 0, int Top = 6, string ViewName = "_HotKeyword")
        {
            YSWL.MALL.BLL.Shop.Products.HotKeyword keywordBll = new YSWL.MALL.BLL.Shop.Products.HotKeyword();
            List<YSWL.MALL.Model.Shop.Products.HotKeyword> keywords = keywordBll.GetKeywordsList(Cid, Top);
            ViewBag.Cid = Cid;
            return PartialView(ViewName, keywords);
        }
        #endregion

        #region 百度分享脚本
        public ActionResult BaiduShare()
        {
            ViewBag.BaiduUid = BLL.SysManage.ConfigSystem.GetValueByCache("BaiduShareUserId");
            return View("_BaiduShare");
        }
        #endregion

        #region 商家浮动层
        public PartialViewResult FloatSuppLayer(int suppId=0,string viewName = "_FloatSuppLayer")
        {
            Model.Shop.Supplier.SupplierInfo model= new BLL.Shop.Supplier.SupplierInfo().GetModelByCache(suppId);
            if (model != null && model.Status == 1 && model.StoreStatus==1)
            {
                model.Address = model.RegionId.HasValue
                                    ? new BLL.Ms.Regions().GetAddress(model.RegionId)
                                    : "暂未设置";
                if(!String.IsNullOrWhiteSpace(model.QQ))
                {
                    model.QQArr = model.QQ.Replace(" ", "").Replace("，", ",").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
                return PartialView(viewName, model);
            }
            return PartialView(viewName);
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
            List<Model.Shop.Supplier.SupplierInfo> list = supplierBll.GetList(top,rec);
            return PartialView(viewName,list);
        }
        public ActionResult SuppLogo(int id=0,string size="")
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

        #region 侧边菜单
        public PartialViewResult Menu(string ViewName = "_Menu")
        {
            return PartialView(ViewName);
        }
        #endregion

        public PartialViewResult LoginLayer(string viewName = "_LoginLayer") 
        {
            ViewBag.RegisterToggle = BLL.SysManage.ConfigSystem.GetValueByCache("Shop_RegisterToggle");//注册方式   
            return PartialView(viewName);
        }

        #region 促销活动
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="suppId"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public PartialViewResult ActivityList(long pId = 0, int suppId = 0, string viewName = "_ActivityList")
        {
            return PartialView(viewName, activInfoBll.GetActivityList(pId, suppId));
        }
        #endregion

        #region 判断是否含有禁用词
        [ValidateInput(false)]
        public ActionResult ContainsDisWords()
        {
            string desc = Request.Params["Desc"];
            return YSWL.MALL.BLL.Settings.FilterWords.ContainsDisWords(desc) ? Content("True") : Content("False");
        }
        #endregion
 
        //初始化regionId
        [HttpPost]
        public ActionResult InitializeRegionId()
        {
            int regionId = GetRegionId;
            return Content("True");
        }

        #region 选择地区
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public PartialViewResult SelectRegion(string viewName = "_SelectRegion")
        {
            int regionId;
            if (IsMultiDepot)
            {
                //开启了分仓
                regionId = GetRegionId;
            }
            else
            {
                regionId = Common.Globals.SafeInt(Common.Cookies.getKeyCookie("groupbuy_regionId"), 0);
            }
            if (regionId <= 0)
            {
                //先清空值  避免出现  id与名称不对应的情况
                Common.Cookies.setKeyCookie("deliveryareas_lastRegionName", "", 1440);
                regionId = Common.Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("Shop_GroupBuy_DefaultRegion"), 651);//默认北京
                Common.Cookies.setKeyCookie("deliveryareas_regionname", regsBll.GetRegionFullName(regionId), 1440);
                YSWL.MALL.Model.Ms.Regions regionsModel = regsBll.GetModelByCache(regionId);
                if (regionsModel != null)
                {
                    Common.Cookies.setKeyCookie("deliveryareas_lastRegionName", regionsModel.RegionName, 1440);
                }
            }
            ViewBag.IsMultiDepot = IsMultiDepot;
            ViewBag.RegionId = regionId;
            return PartialView(viewName);
        }
        #endregion
    }
}
