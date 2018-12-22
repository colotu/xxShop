using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using YSWL.MALL.Model.Shop.Products;
using YSWL.MALL.ViewModel.Shop;
using YSWL.MALL.Web.Components.Setting.Shop;
using YSWL.Web;

namespace YSWL.MALL.Web.Areas.Shop.Controllers
{
    public  class RecommendController : ShopControllerBase
    {
        //
        // GET: /Recommend/
        YSWL.MALL.BLL.Shop.Products.ProductInfo productBll = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">rec:推荐, hot:热卖,cheap:特价,new:最新,irec:首页推荐</param>
        /// <param name="Cid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="vName">viewName</param>
        /// <param name="ajaxVName">ajaxViewName</param>
        /// <returns></returns>
        public ActionResult Index(string type="rec", int cid = 0, int? pageIndex = 1, int pageSize = 30,string vName = "Index", string ajaxVName = "_ProductList")
        {
            ProductRecType Type;
            switch (type) {
                case "rec":
                    Type= ProductRecType.Recommend;
                    break;
                case "hot":
                    Type = ProductRecType.Hot;
                    break;
                case "cheap":
                    Type = ProductRecType.Cheap;
                    break;
                case "new":
                    Type = ProductRecType.Latest;
                    break;
                case "irec":
                    Type = ProductRecType.IndexRec;
                    break;
                default:
                    Type = ProductRecType.Recommend;
                    break;
            }
            pageIndex = pageIndex.HasValue && pageIndex.Value > 1 ? pageIndex.Value : 1;
            //计算分页起始索引
            int startIndex = pageIndex.Value > 1 ? (pageIndex.Value - 1) * pageSize + 1 : 1;
            //计算分页结束索引
            int endIndex = pageIndex.Value > 1 ? startIndex + pageSize - 1 : pageSize;
            int toalCount = productBll.GetProductRecCount(Type, cid);
            //ViewBag.PageSize = pageSize;
            //#region DataParam
            //ViewBag.DataParam = String.Format("{0}type:'{1}',cid:'{2}'{3}", "{", type, cid, "}");
            //#endregion
            ProductListModel model = new ProductListModel();
            model.CurrentCid = cid;

            //获取总条数
            if (toalCount < 1) return View(vName, model); //NO DATA

            List<YSWL.MALL.Model.Shop.Products.ProductInfo> list = productBll.GetProductRecListByPage(Type,cid, "", startIndex, endIndex);
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> prolist = new List<Model.Shop.Products.ProductInfo>();
            #region 是否静态化
            string IsStatic = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("ShopProductStatic");
            string basepath = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.Shop);
            if (list != null)
            {
                foreach (var item in list)
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
            }
            #endregion

            //分页获取数据
            model.ProductPagedList = new PagedList<Model.Shop.Products.ProductInfo>(prolist, pageIndex ?? 1, pageSize, toalCount);

            //检测Ajax请求, 进行无刷新分页
            if (Request.IsAjaxRequest())
                return PartialView(ajaxVName, model);

            return View(vName, model);
        }
    }
}

