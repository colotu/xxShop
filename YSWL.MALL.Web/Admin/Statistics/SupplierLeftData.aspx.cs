using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YSWL.MALL.Web.Admin.Statistics
{
    public partial class SupplierLeftData : PageBaseAdmin
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

        protected int SupplierId
        {
            get
            {
                int supplierId = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["sid"]))
                {
                    supplierId = Common.Globals.SafeInt(Request.Params["sid"], 0);
                }
                return supplierId;
            }
        }
        public void BindData()
        {
            //DataSet ds = new DataSet();
            YSWL.MALL.BLL.Members.Users userBll = new BLL.Members.Users();
            DataSet ds = userBll.GetUserPositionDs(RegionId, SupplierId);
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