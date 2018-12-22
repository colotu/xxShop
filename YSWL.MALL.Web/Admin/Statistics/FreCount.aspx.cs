using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Statistics
{
    public partial class FreCount :PageBaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.txtTo.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.txtFrom.Text = new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy-MM-dd");
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
            DataSet dayDs = YSWL.MALL.BLL.Shop.Order.OrderManage.ActiveCount(-1, startDate, endDate, -1, null, Model.Shop.Order.StatisticMode.Month);

            this.hfCategory.Value = "";
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

            DataSet freDs = YSWL.MALL.BLL.Shop.Order.OrderManage.StatOrderCountPrice(Model.Shop.Order.StatisticMode.Month, startDate, endDate);

            List<YSWL.MALL.ViewModel.Order.OrderFreCount> orderList = new List<YSWL.MALL.ViewModel.Order.OrderFreCount>();
            //把dataset转换成list
            YSWL.MALL.ViewModel.Order.OrderFreCount model = null;
            DataTable freDt = freDs.Tables[0];
            freDt.Columns.Add(new DataColumn("DateFormat", typeof(string)));
            freDt.Columns.Add(new DataColumn("FreCount", typeof(string)));
            foreach (DataRow dr in freDt.Rows)
            {
                if (dr["GeneratedDate"] == null)
                {
                    continue;
                }
                model = new ViewModel.Order.OrderFreCount();
                model.DateStr = Convert.ToDateTime(dr["GeneratedDate"].ToString()).ToString("yyyy/MM");
                dr["DateFormat"] = model.DateStr;
                var orderCountModel = dayCountList.FirstOrDefault(c => c.DateStr == model.DateStr);
                int cusCount = orderCountModel == null ? 0 : orderCountModel.Count;//活跃人数
                model.Count = cusCount;
                int oCount = Common.Globals.SafeInt(dr["OrderCount"], 0);//订单数
                model.FreCount = (cusCount == 0 || oCount == 0) ? 0.00 : Math.Round(oCount * 1.0 / cusCount, 2);//频次=订单数量/下订单的人数
                dr["FreCount"] = model.FreCount.ToString("F");
                orderList.Add(model);
            }


            this.hfCategory.Value = String.Join(",", orderList.Select(c => c.DateStr));
            this.hfFreData.Value = String.Join(",", orderList.Select(c => c.FreCount));

            gridView.DataSetSource = freDs;
        }
        #endregion

        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridView.PageIndex = e.NewPageIndex;
            gridView.OnBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion

    }
}