using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Web.Mvc;
using YSWL.Accounts.Bus;
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

namespace YSWL.MALL.Web.Areas.Shop.Controllers
{
    public class CommissionController : ShopControllerBaseUser
    {
        private BLL.Shop.ReturnOrder.ReturnOrders retuOrderBll = new BLL.Shop.ReturnOrder.ReturnOrders();
        private BLL.Shop.Commission.CommissionDetail comDetailBll = new BLL.Shop.Commission.CommissionDetail();
        #region 佣金统计 

        #region  
        /// <summary>
        /// 佣金统计商品维度 
        /// </summary>
        /// <param name="r"> 1. 我推广的商品获得的佣金 ; 2. 盟友推广的商品我获得的佣金</param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public ActionResult CommPro(int r=1,string viewName = "CommPro")
        {
            if (r != 1 && r != 2)
            {
                r = 1;
            }
            if (r == 2) {
                viewName = "CommProAllyPro";
            }
            DateTime startDate = DateTime.Now.AddDays(-7);
            DateTime endDate = DateTime.Now.AddDays(1);
            ViewBag.StartDate = startDate.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.AddDays(-1).ToString("yyyy-MM-dd");
            return View(viewName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="ruleLevel">  1. 我推广的商品获得的佣金 ; 2. 盟友推广的商品我获得的佣金</param>
        /// <param name="pageIndex"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public PartialViewResult CommProList(string startDate, string endDate,int ruleLevel=1,  int pageIndex = 1,string viewName = "_CommProList")
        {
            if (ruleLevel!=1 && ruleLevel!= 2){
                ruleLevel = 1;
            }
            int _pageSize =10;

            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;
            decimal totalFee;
           
            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;
            int toalCount;
            List<ViewModel.Shop.CommissionProStat> list = comDetailBll.StatPro(CurrentUser.UserID, ruleLevel, Globals.SafeDateTime(startDate, DateTime.Now), Globals.SafeDateTime(endDate, DateTime.Now).AddDays(1),startIndex,endIndex,out toalCount,out totalFee);
            ViewBag.TotalFee = totalFee;
            if (list == null) {
                return PartialView(viewName, null);
            }


            PagedList<ViewModel.Shop.CommissionProStat> lists = new PagedList<ViewModel.Shop.CommissionProStat>(list, pageIndex, _pageSize, toalCount);
            if (Request.IsAjaxRequest())
                return PartialView(viewName, lists);
            return PartialView(viewName, lists);
        }
        #endregion
 
        #region 盟友排行
        /// <summary>
        /// 盟友排行 
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public ActionResult AllyRanking(string viewName = "AllyRanking")
        {
            DateTime startDate = DateTime.Now.AddDays(-7);
            DateTime endDate = DateTime.Now.AddDays(1);
            ViewBag.StartDate = startDate.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.AddDays(-1).ToString("yyyy-MM-dd");
            return View(viewName);
        }
        /// <summary>
        /// 盟友排行 (按佣金)
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public PartialViewResult AllyRankingList(string startDate, string endDate, int pageIndex = 1, string viewName = "_AllyRankingList")
        { 
            int _pageSize = 10;

            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;
            int toalCount;
            List<ViewModel.Shop.CommissionStat> list = comDetailBll.AllyRanking(CurrentUser.UserID, Globals.SafeDateTime(startDate, DateTime.Now), Globals.SafeDateTime(endDate, DateTime.Now).AddDays(1), startIndex, endIndex, out toalCount);
            if (list == null)
            {
                return PartialView(viewName, null);
            }
            PagedList<ViewModel.Shop.CommissionStat> lists = new PagedList<ViewModel.Shop.CommissionStat>(list, pageIndex, _pageSize, toalCount);
            if (Request.IsAjaxRequest())
                return PartialView(viewName, lists);
            return PartialView(viewName, lists);
        }
        #endregion

        #endregion
    }
}
