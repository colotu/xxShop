/**
* List.cs
*
* 功 能： [N/A]
* 类 名： List.cs
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using YSWL.Common;
using System.Drawing;
using YSWL.Accounts.Bus;
using System.Web.UI.HtmlControls;
namespace YSWL.MALL.Web.Admin.Shop.Depot
{
    public partial class List : PageBaseAdmin
    {
        YSWL.MALL.BLL.Shop.DisDepot.Depot bll = new BLL.Shop.DisDepot.Depot();
        YSWL.MALL.BLL.Ms.Regions regionsBLL = new BLL.Ms.Regions();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!IsConnectionOMS && IsOpenMultiDepot)
                {
                    liAdd.Visible = true;
                   // gridView.Columns[9].Visible = true;
                }
          

                gridView.OnBind();
            }
        }
        private bool IsConnectionOMS
        {
            get
            {
                return YSWL.MALL.BLL.Shop.Service.CommonHelper.ConnectionOMS();//是否对接oms
            }
        }
        private bool IsOpenMultiDepot
        {
            get
            {
                return YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();//是否开启分仓
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }
       
        #region gridView
                        
        public void BindData()
        {
            DataSet ds = new DataSet();
            StringBuilder strWhere = new StringBuilder();
            if (txtKeyword.Text.Trim() != "")
            {      
                strWhere.AppendFormat(" Name like '%{0}%'", txtKeyword.Text.Trim());
            }
          

            //页面索引
            int pageIndex = gridView.PageIndex + 1;
            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * gridView.PageSize + 1 : 1;
            //计算分页结束索引
            int endIndex = pageIndex > 1 ? startIndex + gridView.PageSize - 1 : gridView.PageSize;
            gridView.ToalCount = bll.GetRecordCount(strWhere.ToString());
            gridView.DataSetSource = bll.GetListByPage(strWhere.ToString(), " DepotId desc", startIndex, endIndex);
     
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
                 
                //object obj1 = DataBinder.Eval(e.Row.DataItem, "Levels");
                //if ((obj1 != null) && ((obj1.ToString() != "")))
                //{
                //    e.Row.Cells[4].Text = obj1.ToString() == "0" ? "Private" : "Shared";
                //}

                 if (!IsConnectionOMS && IsOpenMultiDepot)
                {
                    HyperLink modifyBut = (HyperLink)e.Row.FindControl("modifyBut");
                    modifyBut.Visible = true;

                    LinkButton linkDel = (LinkButton)e.Row.FindControl("linkDel1");
                    linkDel.Visible = true;
                         
                }
               
            }
        }
        protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //删除分仓商品
            if (e.CommandName == "Del")
            {
                int depotId = Common.Globals.SafeInt(e.CommandArgument.ToString(),0);
                BLL.Shop.DisDepot.Depot depotBll = new BLL.Shop.DisDepot.Depot();
                BLL.Shop.DisDepot.DepotProSKUs depotSkuBll = new BLL.Shop.DisDepot.DepotProSKUs();
                BLL.Shop.DisDepot.DepotRegion depotregionBll = new BLL.Shop.DisDepot.DepotRegion();
                if (depotSkuBll.GetRecordCount(depotId,"") > 0)
                {
                    MessageBox.Show(this, "操作失败，请先删除该仓库下的商品信息！");
                    return;
                }
                if (depotregionBll.GetRecordCount(depotId)>0)
                {
                    MessageBox.Show(this, "操作失败，请先删除该仓库的地区关联信息！");
                    return;
                }
                
                if (depotBll.DeleteEx(depotId))
                {
                    MessageBox.ShowSuccessTip(this, "操作成功！");
                    
                }else{
                    MessageBox.ShowFailTip(this, "操作失败！");
                }
                gridView.OnBind();
            }
        }
        #endregion


        #region 辅助方法
        //获取地区全名
        protected string GetRegionName(object regionid)
        {
            return regionsBLL.GetAddress(Globals.SafeInt(regionid, 0));
        }
        //获取状态
        protected string GetStatus(object Ostatus)
        {
            int  status = Globals.SafeInt(Ostatus, 0);
            if (status == 1)
            {
                return "启用";
            }
            else {
                return "未启用";
            }
            
        }
        #endregion



    }
}
