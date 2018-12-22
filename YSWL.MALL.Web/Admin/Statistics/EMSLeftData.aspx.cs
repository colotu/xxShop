using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Shop.Supplier;

namespace YSWL.MALL.Web.Admin.Statistics
{
    public partial class EMSLeftData : PageBaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public int RegionId
        {
            get
            {
                int regionId = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["regionId"]))
                {
                    regionId = Common.Globals.SafeInt(Request.Params["regionId"], 0);
                }
                return regionId;
            }
        }

        public string Keyword
        {
            get
            {
                string keyword = "";
                if (!string.IsNullOrWhiteSpace(Request.Params["keyword"]))
                {
                    keyword = Request.Params["keyword"];
                }
                return keyword;
            
            }
        }
        public void BindData()
        {
            //DataSet ds = new DataSet();
            YSWL.MALL.BLL.Shop.Supplier.SupplierInfo supplierBll=new SupplierInfo();
            DataSet ds = supplierBll.GetSpDataSet(RegionId, Keyword);
            if (null != ds && ds.Tables[0].Rows.Count < 1)
            {
                this.nodata.Visible = true;
            }
            this.gridView.DataSetSource = ds;
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

        protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            gridView.OnBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}