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

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{
    public class CommissionController : MShopControllerBaseUser
    { 
        private BLL.Shop.Commission.CommissionDetail comDetailBll = new BLL.Shop.Commission.CommissionDetail();
        private BLL.Members.UserInvite inviteBll = new BLL.Members.UserInvite();
        #region 佣金统计 

        #region 佣金首页
        public ActionResult Index(string viewName = "Index")
        {
            YSWL.MALL.ViewModel.Shop.CommissionModel model = new ViewModel.Shop.CommissionModel();
            model.OrderCount= comDetailBll.GetOrderCount(CurrentUser.UserID, out model.ProductCount);//订单数
            model.AllyCount= inviteBll.GetRecordCount(" InviteUserId=" + CurrentUser.UserID);//盟友数
            model.AllFee = comDetailBll.GetUserFees(currentUser.UserID); //佣金           
            return View(viewName,model);
        }
        #endregion

            #region  佣金统计商品维度
            /// <summary>
            /// 佣金统计商品维度 
            /// </summary>
            /// <param name="sd"> startDate  开始时间</param>
            /// <param name="ed"> endDate 结束时间 </param>
            /// <param name="rl"> ruleLevel 1. 我推广的商品获得的佣金 ; 2. 盟友推广的商品我获得的佣金</param>
            /// <param name="pageIndex"></param>
            /// <param name="viewName"></param>
            /// <returns></returns>
        public ActionResult CommPro(string sd, string ed, int rl = 1, int pageIndex = 1, int pageSize = 10, string viewName = "CommPro", string ajaxViewName = "_CommProList")
        {
            if (rl != 1 && rl != 2)
            {
                rl = 1;
            }
            if (rl == 2) {
                viewName = "CommProAllyPro";
            }
            DateTime startDate = Globals.SafeDateTime(sd, DateTime.Now.AddDays(-7));
            DateTime endDate = Globals.SafeDateTime(ed, DateTime.Now).AddDays(1);
            ViewBag.StartDate = startDate.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.AddDays(-1).ToString("yyyy-MM-dd");

            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = pageIndex * pageSize;
            int toalCount;
            decimal totalFee;
            List<ViewModel.Shop.CommissionProStat> list = comDetailBll.StatPro(CurrentUser.UserID, rl, startDate, endDate, startIndex, endIndex, out toalCount, out totalFee);
            ViewBag.TotalFee = totalFee;
            ViewBag.PageSize = pageSize;
            #region DataParam
            ViewBag.DataParam = String.Format("{0}sd:'{1}',ed:'{2}',rl:{3}{4}", "{", startDate.ToString("yyyy-MM-dd"), endDate.AddDays(-1).ToString("yyyy-MM-dd"), rl, "}");
            #endregion
            if (list == null)
            {
                //检测Ajax请求, 进行无刷新分页
                if (Request.IsAjaxRequest())
                    return PartialView(ajaxViewName,null);
                return View(viewName, null);
            }
            PagedList<ViewModel.Shop.CommissionProStat> lists = new PagedList<ViewModel.Shop.CommissionProStat>(list, pageIndex, pageSize, toalCount);
            //检测Ajax请求, 进行无刷新分页
            if (Request.IsAjaxRequest())
                return PartialView(ajaxViewName,lists);
            return View(viewName, lists);
        }
        #endregion

        #region 盟友排行
        /// <summary>
        /// 盟友排行  (按佣金)
        /// </summary>
        /// <param name="sd"> startDate  开始时间</param>
        /// <param name="ed"> endDate 结束时间 </param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public ActionResult AllyRanking(string sd, string ed, int pageIndex = 1,int pageSize=10, string viewName = "AllyRanking", string ajaxViewName = "_AllyRankingList")
        {
            DateTime startDate = Globals.SafeDateTime(sd, DateTime.Now.AddDays(-7));
            DateTime endDate = Globals.SafeDateTime(ed, DateTime.Now).AddDays(1);
            ViewBag.StartDate = startDate.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.AddDays(-1).ToString("yyyy-MM-dd");

            ViewBag.PageSize = pageSize;
            #region DataParam
            ViewBag.DataParam = String.Format("{0}sd:'{1}',ed:'{2}'{3}", "{", startDate.ToString("yyyy-MM-dd"), endDate.AddDays(-1).ToString("yyyy-MM-dd"), "}");
            #endregion
           
            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = pageIndex * pageSize;
            int toalCount;
            List<ViewModel.Shop.CommissionStat> list = comDetailBll.AllyRanking(CurrentUser.UserID, startDate, endDate, startIndex, endIndex, out toalCount);
            if (list == null)
            { 
                //检测Ajax请求, 进行无刷新分页
                if (Request.IsAjaxRequest())
                    return PartialView(ajaxViewName,null);
                return View(viewName, null);
            }
            PagedList<ViewModel.Shop.CommissionStat> lists = new PagedList<ViewModel.Shop.CommissionStat>(list, pageIndex, pageSize, toalCount);
            //检测Ajax请求, 进行无刷新分页
            if (Request.IsAjaxRequest())
                return PartialView(ajaxViewName,lists);
            return View(viewName, lists);
        }
        #endregion

        #endregion
    }
}
