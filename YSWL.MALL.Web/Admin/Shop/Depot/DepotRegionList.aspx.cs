using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Shop.DisDepot;
using System.Data;
using YSWL.Common;
using System.Text;

namespace YSWL.MALL.Web.Admin.Shop.Depot
{
    public partial class DepotRegionList : PageBaseAdmin
    {
        private YSWL.MALL.BLL.Shop.DisDepot.Depot depotBll = new BLL.Shop.DisDepot.Depot();
        private YSWL.MALL.BLL.Shop.DisDepot.DepotRegion depotRegionBll = new DepotRegion();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool IsConnectionOMS = YSWL.MALL.BLL.Shop.Service.CommonHelper.ConnectionOMS(); ;//是否开启分仓
               bool IsOpenMultiDepot = YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();//是否对接oms
               if (!IsConnectionOMS && IsOpenMultiDepot) {
                   liAdd.Visible = true;
                   //btnDelete.Visible = true;
                   gridView.Columns[3].Visible = true;
               }
               BindDepot();
            }
        }

        #region 仓库
        /// <summary>
        /// 仓库
        /// </summary>
        private void BindDepot()
        {
            DataSet ds = depotBll.GetList("  Status = 1 ");
            if (!DataSetTools.DataSetIsNull(ds))
            {
                this.ddlDepot.DataSource = ds;
                this.ddlDepot.DataTextField = "Name";
                this.ddlDepot.DataValueField = "DepotId";
                this.ddlDepot.DataBind();
            }
            this.ddlDepot.Items.Insert(0, new ListItem("全部", "-1"));
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }

        #region gridView

        public void BindData()
        {
            StringBuilder strWhere = new StringBuilder();
            int depotId = Globals.SafeInt(ddlDepot.SelectedValue, 0);
            if (depotId > 0)
            {
                strWhere.AppendFormat(" depotId = {0} ",depotId);
            }

            //页面索引
            int pageIndex = gridView.PageIndex + 1;
            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * gridView.PageSize + 1 : 1;
            //计算分页结束索引
            int endIndex = pageIndex > 1 ? startIndex + gridView.PageSize - 1 : gridView.PageSize;
            gridView.ToalCount = depotRegionBll.GetRecordCount(strWhere.ToString());
            gridView.DataSetSource = depotRegionBll.GetListByPage(strWhere.ToString(), "", startIndex, endIndex);
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

        protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int depotId = Common.Globals.SafeInt(e.Keys[0], 0);
            int RegionId = Common.Globals.SafeInt(e.Keys[1], 0);
            if (depotRegionBll.Delete(depotId, RegionId))
            {
                depotRegionBll.ClearCache(depotId, RegionId);
                Common.MessageBox.ShowSuccessTip(this, "操作成功！");
            }
            else
            {
                Common.MessageBox.ShowFailTip(this, "操作失败！");
            }
            gridView.OnBind();
        }
  
        #endregion

        protected string GetDepotName(object target)
        {
            int depotId = Common.Globals.SafeInt(target, 0);
            YSWL.MALL.Model.Shop.DisDepot.Depot infoModel = depotBll.GetModel(depotId);
            return infoModel == null ? "" : infoModel.Name;
        }

        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    string idlist = GetSelIDlist();
        //    if (idlist.Trim().Length == 0)
        //        return;
        //    gridView.OnBind();
        //}
        //private string GetSelIDlist()
        //{
        //    string idlist = "";
        //    bool BxsChkd = false;
        //    for (int i = 0; i < gridView.Rows.Count; i++)
        //    {
        //        CheckBox ChkBxItem = (CheckBox)gridView.Rows[i].FindControl(gridView.CheckBoxID);
        //        if (ChkBxItem != null && ChkBxItem.Checked)
        //        {
        //            BxsChkd = true;
        //            if (gridView.DataKeys[i].Value != null)
        //            {
        //                idlist += gridView.DataKeys[i].Value.ToString() + ",";
        //            }
        //        }
        //    }
        //    if (BxsChkd)
        //    {
        //        idlist = idlist.Substring(0, idlist.LastIndexOf(","));
        //    }
        //    return idlist;
        //}
    }

}