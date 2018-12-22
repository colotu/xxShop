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
using YSWL.Json;
using System.Web.UI.HtmlControls;
namespace YSWL.MALL.Web.Admin.Shop.Depot
{
    public partial class ProductList : PageBaseAdmin
    {
   
        YSWL.MALL.BLL.Shop.DisDepot.Depot depotBLL = new BLL.Shop.DisDepot.Depot();
        YSWL.MALL.BLL.Shop.DisDepot.DepotProSKUs dskuBLL = new BLL.Shop.DisDepot.DepotProSKUs();
        YSWL.MALL.BLL.Ms.Regions regionsBLL = new BLL.Ms.Regions();
        private YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll = new BLL.Shop.Products.SKUInfo();
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
                return YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot(); //是否开启分仓
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.Request.Form["Callback"]) && (this.Request.Form["Callback"] == "true"))
            {
                this.Controls.Clear();
                this.DoCallback();
            }
            if (!Page.IsPostBack)
            {
                BindDepot();
                if (!IsConnectionOMS && IsOpenMultiDepot)
                {
                    liAdd.Visible = true;
                    //btnDelete.Visible = true;
                   //gridView.Columns[9].Visible = true;
                }
          
           
            }
        }
        
        protected void btnSearch_Click(object sender, EventArgs e)
        {
           gridView.OnBind();
        }
       
        #region gridView
                        
        public void BindData()
        {
            if (!Page.IsPostBack)
            {
                return;
            }
            int depotId = Globals.SafeInt(ddlDepot.SelectedValue, 0);
            if (depotId <= 0)
            {
                MessageBox.ShowFailTip(this, "请选择仓库");
                return;
            }
            StringBuilder strWhere = new StringBuilder();
            string pname = Common.InjectionFilter.SqlFilter(txtKeyword.Text.Trim());
            if (!String.IsNullOrWhiteSpace(pname)) {
                if (strWhere.Length > 0) {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" ProductName like '%{0}%' ", pname);
            }
            gridView.DataSetSource = dskuBLL.GetList(0,depotId, strWhere.ToString(), "");
            gridView.DataBind();
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
                if (!IsConnectionOMS && IsOpenMultiDepot)
                {
                    System.Web.UI.WebControls.Image imgStockbtn = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgStock");
                    imgStockbtn.Visible = true;
                }
                //object obj1 = DataBinder.Eval(e.Row.DataItem, "Levels");
                //if ((obj1 != null) && ((obj1.ToString() != "")))
                //{
                //    e.Row.Cells[4].Text = obj1.ToString() == "0" ? "Private" : "Shared";
                //}
               
            }
 

        }
        protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
             //删除分仓商品
            if (e.CommandName == "Del")
            {
                int depotId = Globals.SafeInt(ddlDepot.SelectedValue, 0);
                if (depotId <= 0)
                {
                    MessageBox.ShowFailTip(this, "请选择仓库");
                    return;
                }
                string sku = e.CommandArgument.ToString();      
                if (dskuBLL.DeleteProduct(depotId, sku))
                {
                    MessageBox.ShowSuccessTip(this, "操作成功！");
                }
                else {
                    MessageBox.ShowFailTip(this, "操作失败！");
                }
                gridView.OnBind();
            }
            if (e.CommandName == "Status")
            {
                if (e.CommandArgument != null)
                {
                       int depotId = Globals.SafeInt(ddlDepot.SelectedValue, 0);
                    
                    string[] Args = e.CommandArgument.ToString().Split(new char[] { ',' });
                   string sku = Args[0];
                   int status = Common.Globals.SafeInt(Args[1], 0);
                    bool isup = !Common.Globals.SafeBool(Args[2], false);
                    dskuBLL.UpdateSkuStatus(depotId, sku, isup, status);
                    //清空缓存
                    Common.DataCache.ClearAll();
                    gridView.OnBind();
                }
            }

        }
 
 
        #endregion


        #region 辅助方法
        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected string GetSKUStr(object target)
        {
            string sku = target.ToString();
            List<YSWL.MALL.Model.Shop.Products.SKUItem> itemList = skuBll.GetSKUItemsBySku(sku);
            if (itemList == null || itemList.Count == 0)
            {
                return "";
            }
            string str = "";
            foreach (var item in itemList)
            {
                str += item.AttributeName + "：" + item.AV_ValueStr + "  ";
            }
            return string.Format("规格({0})", str);
        }


        //获取状态
        protected string GetSaleStatus(object objStatus,object objUpselling)
        {
            //0:下架(仓库中)  1:上架 2:已删除 
            int status = Globals.SafeInt(objStatus, 0);
            bool upselling = Globals.SafeBool(objUpselling, false);
            switch (status)
            {
                case 0:
                    return "下架";
                case 1:
                    if (upselling) {
                        return "上架";
                    }else{
                        return "下架";
                    }
                case 2:
                    return "已删除";
                default:
                    break;
            }
            return "";
        }
          //获取销售类型
        protected string GetSalesType(object objStatus)
        {
            int status = Globals.SafeInt(objStatus, 0);
            switch (status)
            {
                case 1:
                    return "正常";
                case 3:
                    return "赠品";
                default:
                    break;
            }
            return "";
        }
        


        #region 仓库
        /// <summary>
        /// 仓库
        /// </summary>
        private void BindDepot()
        {
            DataSet ds = depotBLL.GetList("  Status = 1 ");
            if (!DataSetTools.DataSetIsNull(ds))
            {
                this.ddlDepot.DataSource = ds;
                this.ddlDepot.DataTextField = "Name";
                this.ddlDepot.DataValueField = "DepotId";
                this.ddlDepot.DataBind();
            }
            this.ddlDepot.Items.Insert(0, new ListItem("请选择", "-1"));
        }
        #endregion

        protected void ddlDepot_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridView.PageIndex = 0;
            gridView.OnBind();
        }
 
        #endregion

        #region AjaxCallback

        private void DoCallback()
        {
            string action = this.Request.Form["Action"];
            this.Response.Clear();
            this.Response.ContentType = "application/json";
            string writeText = string.Empty;

            switch (action)
            {
                case "UpdateStock":
                    writeText = UpdateStock();
                    break;
            }
            this.Response.Write(writeText);
            this.Response.End();
        }

        private string UpdateStock()
        {
            JsonObject json = new JsonObject();
            string sku = Common.InjectionFilter.SqlFilter(this.Request.Form["sku"]);
            int stock = Common.Globals.SafeInt(Request.Form["stock"], 0);
            int depotId = Globals.SafeInt(Request.Form["did"], 0);
            if (depotId <= 0 || String.IsNullOrWhiteSpace(sku))
            {
                json.Put("STATUS", "FAILED");
                return json.ToString();
            } 

            if (dskuBLL.UpdateStockNum(depotId, sku, stock))
            {
                json.Put("STATUS", "SUCCESS");
            }
            else
            {
                json.Put("STATUS", "FAILED");
            }
            return json.ToString();
        }

        #endregion AjaxCallback

    }
}
