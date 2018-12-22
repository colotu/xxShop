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
    public partial class ActiveCount : PageBaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.txtTo.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.txtFrom.Text = new DateTime(DateTime.Now.Year,DateTime.Now.Month,1).ToString("yyyy-MM-dd");
            }
        }

        #region gridView

        protected void btnReStatistic_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
            gridViewRefer.OnBind();

        }
        #region 客户活跃统计--日期
        public void BindData()
        {
            DateTime startDate = Common.Globals.SafeDateTime(this.txtFrom.Text, DateTime.Now.AddDays(-6)).Date;
            DateTime endDate = Common.Globals.SafeDateTime(this.txtTo.Text, DateTime.Now).Date.AddDays(1).AddSeconds(-1);
            DataSet dayDs = YSWL.MALL.BLL.Shop.Order.OrderManage.ActiveCount(2, startDate, endDate, -1, null);
            
            this.hfCategory.Value = "";
            this.hfDayCount.Value = "";

            List<YSWL.MALL.ViewModel.Order.OrderCount> dayCountList = new List<ViewModel.Order.OrderCount>();

            DataTable dt = dayDs.Tables[0];
            YSWL.MALL.ViewModel.Order.OrderCount orderCount = null;
            //把dataset转换成list
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                orderCount = new YSWL.MALL.ViewModel.Order.OrderCount();
                orderCount.DateStr = dt.Rows[i]["D"].ToString();
                orderCount.Count = Common.Globals.SafeInt(dt.Rows[i]["BuyerCount"], 0);
                dayCountList.Add(orderCount);
            }

            List<YSWL.MALL.ViewModel.Order.OrderCount> countList = new List<ViewModel.Order.OrderCount>();
            int dayCount = (endDate-startDate).Days;
            //填充没有数据的日期
            for (int i = 0; i <= dayCount; i++)
            {
                string dateStr = startDate.AddDays(i).ToString("yyyy/MM/dd");

                YSWL.MALL.ViewModel.Order.OrderCount cancel = dayCountList.FirstOrDefault(c => c.DateStr == dateStr);
                if (cancel == null)
                {
                    countList.Add(new YSWL.MALL.ViewModel.Order.OrderCount() { DateStr = dateStr, Count = 0 });
                    dt.Rows.Add(new object[] { dateStr, 0 });
                }
                else
                {
                    countList.Add(cancel);
                }
            }
            this.hfCategory.Value = String.Join(",", countList.Select(c => c.DateStr));
            this.hfDayCount.Value = String.Join(",", countList.Select(c => c.Count));

            gridView.DataSetSource = dayDs;
        }
        #endregion

        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridView.PageIndex = e.NewPageIndex;
            gridView.OnBind();
        }

        #endregion

        #region 客户活跃统计--来源
        public void ReferBindData()
        {
            DateTime startDate = Common.Globals.SafeDateTime(this.txtFrom.Text, DateTime.Now.AddDays(-6)).Date;
            DateTime endDate = Common.Globals.SafeDateTime(this.txtTo.Text, DateTime.Now).Date.AddDays(1).AddSeconds(-1);
            DataSet referDs = YSWL.MALL.BLL.Shop.Order.OrderManage.ActiveCountbyType(2, startDate, endDate, null);
            
            this.hfReferData.Value = "";
            JsonObject referItemObj = null;
            JsonArray referArry = new JsonArray();

            DataTable dt = referDs.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                int refer = Common.Globals.SafeInt(dt.Rows[i]["ReferType"], -1);
                int count = Common.Globals.SafeInt(dt.Rows[i]["BuyerCount"], 0);

               referItemObj = new JsonObject();
               referItemObj.Accumulate("name", GetReferName(refer));
               referItemObj.Accumulate("y", count);
               referArry.Add(referItemObj);


            }
            this.hfReferData.Value = referArry.ToString();
            gridViewRefer.DataSetSource = referDs;
        }
        protected void gridViewRefer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridViewRefer.PageIndex = e.NewPageIndex;
            gridViewRefer.OnBind();
        }
        #endregion

        protected string GetReferName(int refer)
        {
            //来源类型 1：表示PC下单，2：表示微信下单，3：表示业务员待下单
            switch (refer)
            {
                case 1:
                    return "PC商城";
                case 2:
                    return "微信";
                case 3:
                    return "400客服";
                case 4:
                    return "订货";
                default:
                    return "未知";
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

    }
}