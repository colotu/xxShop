using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;
using YSWL.OAuth.Json;
using System.Data;

namespace YSWL.MALL.Web.Admin.Statistics
{
    public partial class UserRegCount : PageBaseAdmin
    {
        private YSWL.MALL.BLL.Members.Users userBll = new YSWL.MALL.BLL.Members.Users();

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

        public void BindData()
        {
            DateTime startDate = Common.Globals.SafeDateTime(this.txtFrom.Text, DateTime.Now.AddDays(-6));
            DateTime endDate = Common.Globals.SafeDateTime(this.txtTo.Text, DateTime.Now).AddDays(1);
            Model.Shop.Order.StatisticMode mode = (Model.Shop.Order.StatisticMode)Globals.SafeInt(rdoMode.SelectedValue, 0);
            DataSet dayDs = userBll.GetDayRegCount(startDate, endDate, mode);
            this.lblDayCount.Text = userBll.GetRecordCount(" UserType='UU'  and User_dateCreate>='" + DateTime.Now.ToString("yyyy-MM-dd") + "'").ToString();
            this.lblTotalCount.Text = userBll.GetRecordCount("  UserType='UU'  and User_dateCreate<='" + DateTime.Now.ToString() + "'").ToString();
            List<YSWL.MALL.ViewModel.Member.UserCount> DayCountList = new List<YSWL.MALL.ViewModel.Member.UserCount>();
            List<YSWL.MALL.ViewModel.Member.UserCount> CountList = new List<ViewModel.Member.UserCount>();
            this.hfCategory.Value = "";
            this.hfDayCount.Value = "";
            this.hfTotalCount.Value = "";

            //把dataset转换成list
            YSWL.MALL.ViewModel.Member.UserCount userCount = null;
            DataTable dt = dayDs.Tables[0];
            dt.Columns.Add(new DataColumn("TotalUserCount", typeof(int)));
            int totalCount = userBll.GetRecordCount(" UserType='UU'  and User_dateCreate<'" + startDate.ToString("yyyy-MM-dd") + "'");
            int totalCountTemp = totalCount;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                userCount = new YSWL.MALL.ViewModel.Member.UserCount();
                userCount.DateStr = dt.Rows[i]["D"].ToString();
                userCount.DayCount = Common.Globals.SafeInt(dt.Rows[i]["UserCount"], 0);
                totalCountTemp = totalCountTemp + userCount.DayCount;
                dt.Rows[i]["TotalUserCount"] = totalCountTemp;
                userCount.TotalCount = totalCountTemp;
                DayCountList.Add(userCount);
            }

            //YSWL.MALL.ViewModel.Member.UserCount count = null;


            //int days = (endDate - startDate).Days;
            ////填充没有数据的日期
            //for (int i = 0; i <= days; i++)
            //{
            //    string dateStr = startDate.AddDays(i).ToString("yyyy/MM/dd");
            //    count = new ViewModel.Member.UserCount();
            //    count.DateStr = dateStr;
            //    var dayCountModel = DayCountList.FirstOrDefault(c => c.DateStr == dateStr);
            //    count.DayCount = dayCountModel == null ? 0 : dayCountModel.DayCount;
            //    totalCount = totalCount + count.DayCount;
            //    count.TotalCount = totalCount;
            //    CountList.Add(count);

            //    if (dayCountModel == null)
            //    {
            //        dt.Rows.Add(new object[] { dateStr, 0, totalCount });
            //    }
            //}
            this.hfCategory.Value = String.Join(",", DayCountList.Select(c => c.DateStr));
            this.hfDayCount.Value = String.Join(",", DayCountList.Select(c => c.DayCount));
            this.hfTotalCount.Value = String.Join(",", DayCountList.Select(c => c.TotalCount));


            gridView.DataSetSource = dayDs;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridView.PageIndex = e.NewPageIndex;
            gridView.OnBind();
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Attributes.Add("style", "background:#FFF");
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex % 2 == 0)
                {
                    e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#F4F4F4");
                }
                else
                {
                    e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#FFFFFF");
                }

            }
        }
        #endregion

    }
}