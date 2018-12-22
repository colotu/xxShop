using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using YSWL.Common;
using YSWL.Components.Setting;

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{
    public partial class StoreController
    {
        BLL.Members.Users userbll = new BLL.Members.Users();
        BLL.Shop.Supplier.SupplierAD supplierADBLL = new BLL.Shop.Supplier.SupplierAD();
        //
        // GET: /Mobile/Recommend/
        YSWL.MALL.BLL.Shop.Products.ProductInfo productBll = new YSWL.MALL.BLL.Shop.Products.ProductInfo();

        #region 广告位
        public PartialViewResult AdList(int id,int suppId, string viewName = "_AdList")
        {
            List<YSWL.MALL.Model.Shop.Supplier.SupplierAD> list = supplierADBLL.GetListByAidCache(id, suppId);
            return PartialView(viewName, list);
        }

        /// <summary>
        /// 小广告
        /// </summary>
        /// <param name="advPositionId"></param>
        /// <param name="suppId"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public PartialViewResult AD(int advPositionId,int suppId, string viewName = "_AD")
        {
            YSWL.MALL.Model.Shop.Supplier.SupplierAD model = supplierADBLL.GetModelByAdvPositionId(advPositionId, suppId);
            return PartialView(viewName, model);
        }
        #endregion


        #region
       
        /// <summary>
        /// 获取推荐商品列表
        /// </summary>
        /// <param name="suppId"></param>
        /// <param name="type"></param>
        /// <param name="cid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="vName"></param>
        /// <param name="ajaxVName"></param>
        /// <returns></returns>
        public ActionResult RecProdList(int suppId=0, int type=0, int cid = 0, int? pageIndex = 1, int pageSize = 30, string vName = "RecProdList", string ajaxVName = "_RecProdListMore")
        {
            pageIndex = pageIndex.HasValue && pageIndex.Value > 1 ? pageIndex.Value : 1;
            //计算分页起始索引
            int startIndex = pageIndex.Value > 1 ? (pageIndex.Value - 1) * pageSize + 1 : 1;
            //计算分页结束索引
            int endIndex = pageIndex.Value > 1 ? startIndex + pageSize - 1 : pageSize;
            ViewBag.PageSize = pageSize;

            #region DataParam
            ViewBag.DataParam =String.Format("{0}type:'{1}',cid:'{2}',suppid:'{3}'{4}", "{", type, cid, suppId, "}");
            #endregion

            List<YSWL.MALL.Model.Shop.Products.ProductInfo> productList = productBll.GetSuppRecListByPage(type, suppId, startIndex, endIndex);
            //检测Ajax请求, 进行无刷新分页
            if (Request.IsAjaxRequest())
                return PartialView(ajaxVName, productList);

            return View(vName, productList);
        }
        #endregion




        #region 商家商品分类
        /// <summary>
        /// 根据商家id和分类id 获取对应的子分类数据 (若要读取一级分类 pcid的值为0即可)
        /// </summary>
        /// <param name="pcid"></param>
        /// <param name="suppId"></param>
        /// <param name="Top"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public ActionResult CategoryList(long suppId = 0, int pcid = 0, int top = -1, string viewName = "CategoryList")
        {
            List<YSWL.MALL.Model.Shop.Supplier.SupplierCategories> cateList = YSWL.MALL.BLL.Shop.Supplier.SupplierCategories.GetAllCateList(suppId);
            List<YSWL.MALL.Model.Shop.Supplier.SupplierCategories> categoryInfos = null;
            if (top > 0)
            {
                categoryInfos = cateList.Where(c => c.ParentCategoryId == pcid).Take(top).ToList();
            }
            else
            {
                categoryInfos = cateList.Where(c => c.ParentCategoryId == pcid).ToList();
            }
            return View(viewName, categoryInfos);
        }
        #endregion

        #region ListV2
        public ActionResult ListV2(int suppId = 0, int cid = 0, string mod = "hot", string ky = "",
            int p = 1, int pageSize = 30,string vName = "List", string ajaxVName = "_ProdListMore")
        {
            ViewBag.PageSize = pageSize;
            Model.Shop.Supplier.SupplierInfo suppinfoModel = suppinfoBll.GetModelByCache(suppId);
            if (suppinfoModel != null && suppinfoModel.Status == 1 && suppinfoModel.StoreStatus == 1)//
            {
                #region DataParam
                ViewBag.DataParam = String.Format("{0}suppId:'{1}',cid:'{2}',mod:'{3}',ky:'{3}'{4}", "{", suppId, cid, mod, "}");
                #endregion

               // int _pageSize = Common.Globals.SafeInt(YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("MSHOP_Store_PageSize"), 16);

                //计算分页起始索引
                int startIndex = p > 1 ? (p - 1) * pageSize + 1 : 1;

                //计算分页结束索引
                int endIndex = p * pageSize;

                List<Model.Shop.Products.ProductInfo> list = productManage.GetSuppProductsListEx(cid, suppId, ky, "", mod, startIndex, endIndex);

                //检测Ajax请求, 进行无刷新分页
                if (Request.IsAjaxRequest())
                    return PartialView(ajaxVName, list);
                return View(vName, list);
            }
            return RedirectToAction("Index", "Home", new { area = "MShop" });
        }
        #endregion


        #region 申请商家
        [HttpGet]
        [YSWL.Components.Filters.TokenAuthorize]
        public ActionResult Apply(string viewName = "Apply")
        {
            Model.Shop.Supplier.SupplierInfo model = suppinfoBll.GetModel(Globals.SafeInt(CurrentUser.DepartmentID, -1));
            if (model != null && model.Status == 0) //正在审核
            {
                return RedirectToAction("ApplySuccess", "Store", new { area = "MShop" });
            }

            //else if (model != null || CurrentUser.UserType != "UU") //已审核/非普通用户 重定向到商家后台进行登录
            //    return Redirect("/SP/Account/Login");

            return View(viewName);
        }

        [HttpPost]
        public ActionResult ApplySubmit(string shopName, string companyName, string artiPerson, string telPhone)
        {
            if (CurrentUser == null) {
                return Content("NOLOGIN");
            }
            if (CurrentUser.UserType != "UU")
            {
                return Content("ERROR");
            }
            if (String.IsNullOrWhiteSpace(shopName) || String.IsNullOrWhiteSpace(companyName)
                || String.IsNullOrWhiteSpace(artiPerson) || String.IsNullOrWhiteSpace(telPhone)) {
                return Content("ERROR");
            }

            if (suppinfoBll.Exists(companyName) || suppinfoBll.ExistsShopName(shopName)) {
                return Content("ERROR");
            }
            if (userbll.ExistsUserVIP(CurrentUser.UserID.ToString()).ToUpper() == "VIP")
            {
                Model.Shop.Supplier.SupplierInfo model = new Model.Shop.Supplier.SupplierInfo();
                model.ShopName = shopName;
                model.Name = companyName;
                model.ArtiPerson = artiPerson;
                model.TelPhone = telPhone;

                model.UserId = CurrentUser.UserID;
                model.UserName = CurrentUser.UserName;
                model.CreatedUserId = CurrentUser.UserID;
                model.CreatedDate = DateTime.Now;

                model.RegisteredCapital = 0;

                model.SupplierId = suppinfoBll.Add(model);
                if (model.SupplierId < 1) return Content("NO");



                //升级当前用户为商家
                CurrentUser.UserType = "SP";
                CurrentUser.DepartmentID = model.SupplierId.ToString();
                CurrentUser.Update();
                //商家角色
                int defaultEnteRoleId = BLL.SysManage.ConfigSystem.GetIntValueByCache("DefaultSuppRoleID");
                if (defaultEnteRoleId > 0)
                {
                    CurrentUser.AddToRole(defaultEnteRoleId);
                }
                return Content("OK");
            }
            else
            {
                return Content("noVIP");
            }
        }

        [HttpPost]
        public bool ExistsSupplierName(string name)
        {
            return suppinfoBll.Exists(name);
        }
        [HttpPost]
        public bool ExistsShopName(string name)
        {
            return suppinfoBll.ExistsShopName(name);
        }

        [HttpGet]
        [YSWL.Components.Filters.TokenAuthorize]
        public ActionResult ApplySuccess(string viewName = "ApplySuccess")
        {
            Model.Shop.Supplier.SupplierInfo model = suppinfoBll.GetModel(Globals.SafeInt(CurrentUser.DepartmentID, -1));
            if (model == null)
            {
               return Redirect(ViewBag.BasePath + "Store/Apply"); 
            }
            if (model.StoreStatus == 1)//已经审核
            {
                return Redirect(ViewBag.BasePath + "u");
            }
            return View(viewName, model);
        }
        #endregion
    }
}