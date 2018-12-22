using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;
using System.Data;
using System.Text;

namespace YSWL.MALL.Web.Admin.Statistics
{
    public partial class ShopSales : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
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
            DateTime startDate = Common.Globals.SafeDateTime(this.txtFrom.Text, DateTime.Now.AddDays(-6)).Date;
            DateTime endDate = Common.Globals.SafeDateTime(this.txtTo.Text, DateTime.Now).Date.AddDays(1).AddSeconds(-1);
            //Model.Shop.Order.StatisticMode mode = (Model.Shop.Order.StatisticMode)Globals.SafeInt(rdoMode.SelectedValue, 0);
            Model.Shop.Order.StatisticMode mode = Model.Shop.Order.StatisticMode.Day;
            DataSet ds = BLL.Shop.Order.OrderManage.ShopSaleInfo(mode, startDate, endDate);

            BindJson(ds, mode);
            this.gridView.DataSetSource = ds;
        }


        private void BindJson(DataSet ds, Model.Shop.Order.StatisticMode mode)
        {
            this.hfCategory.Value = "";
            this.hfAmount.Value = "";
           

            List<YSWL.MALL.ViewModel.Order.ShopSaleInfo> shopSaleList = new List<YSWL.MALL.ViewModel.Order.ShopSaleInfo>();
            //把dataset转换成list
            YSWL.MALL.ViewModel.Order.ShopSaleInfo model = null;
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                model = new ViewModel.Order.ShopSaleInfo();
                model.Name = dr.Field<string>("Name");
                model.Amount = Common.Globals.SafeDecimal(dr["Amount"], 0);
                model.ShopName = dr.Field<string>("ShopName");
                shopSaleList.Add(model);
            }
            this.hfCategory.Value = String.Join(",", shopSaleList.Select(c => c.Name));
            this.hfAmount.Value = String.Join(",", shopSaleList.Select(c => c.Amount));
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