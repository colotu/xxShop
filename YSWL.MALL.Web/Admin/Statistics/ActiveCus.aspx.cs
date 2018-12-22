using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Json;
using System.Data;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Statistics
{
    public partial class ActiveCus : PageBaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.txtTo.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.txtFrom.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
            }
        }

        #region gridView

        protected void btnReStatistic_Click(object sender, EventArgs e)
        {
            gridView.OnBind();

        }
        #region 订购门店数统计--日期
        public void BindData()
        {
            DateTime startDate = Common.Globals.SafeDateTime(this.txtFrom.Text, DateTime.Now.AddDays(-6)).Date;
            DateTime endDate = Common.Globals.SafeDateTime(this.txtTo.Text, DateTime.Now).Date.AddDays(1).AddSeconds(-1);
                 Model.Shop.Order.StatisticMode mode = (Model.Shop.Order.StatisticMode)Globals.SafeInt(rdoMode.SelectedValue, 0);
                 DataSet dayDs = YSWL.MALL.BLL.Shop.Order.OrderManage.ActiveCount(-1, startDate, endDate, -1, null, mode);

            this.hfCategory.Value = "";
            this.hfDayCount.Value = "";
            this.hfFreData.Value = "";

            DataTable dt = dayDs.Tables[0];
            List<YSWL.MALL.ViewModel.Order.OrderCount> dayCountList = new List<ViewModel.Order.OrderCount>();
            YSWL.MALL.ViewModel.Order.OrderCount orderCount = null;
            //把dataset转换成list
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                orderCount = new YSWL.MALL.ViewModel.Order.OrderCount();
                orderCount.DateStr = dt.Rows[i]["D"].ToString();
                orderCount.Count = Common.Globals.SafeInt(dt.Rows[i]["BuyerCount"], 0);
                dayCountList.Add(orderCount);
            }

            this.hfCategory.Value = String.Join(",", dayCountList.Select(c => c.DateStr));
            this.hfDayCount.Value = String.Join(",", dayCountList.Select(c => c.Count));

            gridView.DataSetSource = dayDs;
        }
        #endregion

        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridView.PageIndex = e.NewPageIndex;
            gridView.OnBind();
        }



        #endregion

    }
}