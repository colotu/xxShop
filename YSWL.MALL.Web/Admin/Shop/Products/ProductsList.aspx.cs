using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Json;
using YSWL.Common;
using System.Web.UI.HtmlControls;
using YSWL.Web;

namespace YSWL.MALL.Web.Admin.Shop.Products
{
    public partial class ProductsList : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 482; } } //Shop_商品管理_列表页
        protected new int Act_UpdateData = 483;    //Shop_商品管理_编辑数据
        protected new int Act_DelData = 484;    //Shop_商品管理_删除数据
        private BLL.Shop.Products.ProductInfo bll = new BLL.Shop.Products.ProductInfo();
        private BLL.Shop.Products.ProductCategories productCategory = new BLL.Shop.Products.ProductCategories();
        private BLL.Shop.Products.CategoryInfo manage = new BLL.Shop.Products.CategoryInfo();
        private YSWL.MALL.BLL.Shop.Supplier.SupplierInfo supplierBll = new BLL.Shop.Supplier.SupplierInfo();
        private YSWL.MALL.BLL.Shop.Favorite favorBll = new BLL.Shop.Favorite();

        private bool isHasShop=YSWL.Components.MvcApplication.HasArea(AreaRoute.Shop);
        public string strTitle = string.Empty;
        private bool IsConnectionOMS
        {
            get {
                return YSWL.MALL.BLL.Shop.Service.CommonHelper.ConnectionOMS(); //是否对接oms
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
                if (IsConnectionOMS)
                {
                    gridView.Columns[7].Visible = false;
                    gridView.Columns[5].Visible = false;
                }

                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_DelData)) && GetPermidByActID(Act_DelData) != -1)
                {
                    liDel.Visible = false;
                    btnDelete.Visible = false;
                }

           
                BindCategories();
            }
        }


        private void DoCallback()
        {
            string action = this.Request.Form["Action"];
            this.Response.Clear();
            this.Response.ContentType = "application/json";
            string writeText = string.Empty;

            switch (action)
            {
               
                case "GetDate":
                    writeText = GetDate();
                    break;

            }
            this.Response.Write(writeText);
            this.Response.End();
        }

        #region 首页数据展示

        private string GetDate()
        {
            JsonObject json = new JsonObject();
            json.Put("STATUS", "SUCCESS");
            json.Put("ALL", bll.GetProductCount(-1));
            json.Put("SALE", bll.GetProductCount(1));
            json.Put("UNSALE", bll.GetProductCount(0));
            json.Put("FAVOR", favorBll.FavProductCount());
            return json.ToString();
        }
        #endregion
        protected int SaleStatus
        {
            get
            {
                int status = -1;
                if (!string.IsNullOrWhiteSpace(Request.Params["SaleStatus"]))
                {
                    status = Common.Globals.SafeInt(Request.Params["SaleStatus"], -1);
                }
                return status;
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

            switch (SaleStatus)
            {
                    //出售中
                case  (int)YSWL.MALL.Model.Shop.Products.ProductSaleStatus.OnSale:
                    strTitle = "您可以对出售中的商品进行编辑、删除和下架以及查询库存低于警戒库存的商品操作";
                    break;
                    //未审核
                case (int)YSWL.MALL.Model.Shop.Products.ProductSaleStatus.UnCheck:
                    btnCheck.Visible = true;
                    strTitle = "您可以对未审核中的商品进行编辑、删除和审核操作";
                    this.btnInverseApprove.Visible = false;
                    btnInverseApprove2.Visible = false;
                    break;
                case (int)YSWL.MALL.Model.Shop.Products.ProductSaleStatus.InStock:
                       strTitle = "您可以对仓库中的商品进行删除和上架功能";
                this.btnInverseApprove.Text = "批量上架";
                btnInverseApprove2.Text = "批量上架";
                    break;
            }

            Model.Shop.Products.ProductInfo model = new Model.Shop.Products.ProductInfo();
            model.SaleStatus = SaleStatus;
            if (!string.IsNullOrWhiteSpace(txtKeyword.Text.TrimEnd()))
            {
                model.ProductName = InjectionFilter.SqlFilter(this.txtKeyword.Text);
            }
            if (!string.IsNullOrWhiteSpace(drpProductCategory.SelectedValue))
            {
                model.CategoryId = Globals.SafeInt(this.drpProductCategory.SelectedValue,0);
            }
            if (!string.IsNullOrWhiteSpace(this.txtProductNum.Text))
            {
                model.ProductCode = InjectionFilter.SqlFilter(this.txtProductNum.Text);
            }
            //int salesType = Globals.SafeInt(dropSalesType.SelectedValue, 0);
            //if (salesType > 0)
            //{
            //    model.SalesType = salesType;
            //}
            //else {
            //    model.SalesType = 0;
            //}
            //chkRest.Checked
            //gridView.DataSetSource = bll.GetProductInfo(model,chkAlert.Checked,false);
            gridView.DataSetSource = bll.GetProductInfo(model,false,false);
           
        }

        private void BindCategories()
        {
            YSWL.MALL.BLL.Shop.Products.CategoryInfo bll = new BLL.Shop.Products.CategoryInfo();
            DataSet ds = bll.GetList("  Depth = 1   ");

            if (!DataSetTools.DataSetIsNull(ds))
            {
                this.drpProductCategory.DataSource = ds;
                this.drpProductCategory.DataTextField = "Name";
                this.drpProductCategory.DataValueField = "CategoryId";
                this.drpProductCategory.DataBind();
            }
            this.drpProductCategory.Items.Insert(0, new ListItem("请选择", string.Empty));
        }
 

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(GetSelIDlist()))
            {
                string idlist = GetSelIDlist();
                if (idlist.Trim().Length == 0) return;
                bll.UpdateList(idlist, YSWL.MALL.Model.Shop.Products.ProductSaleStatus.Deleted);
                supplierBll.UpdateProductCountList(idlist);
                YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.TooltipDelOK);
            }
            gridView.OnBind();
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
                Literal litProductCate = (Literal)e.Row.FindControl("litProductCate");
                object productId = DataBinder.Eval(e.Row.DataItem, "ProductId");
                if (productId != null)
                {
                    litProductCate.Text = ProductCategories(Common.Globals.SafeLong(productId.ToString(), 0));
                }

                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_UpdateData)) && GetPermidByActID(Act_UpdateData) != -1)
                {
                    HtmlGenericControl productModify = (HtmlGenericControl)e.Row.FindControl("productModify");
                    productModify.Visible = false;
                }
  
            }
        }
 
        private string GetSelIDlist()
        {
            string idlist = "";
            bool BxsChkd = false;
            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                CheckBox ChkBxItem = (CheckBox)gridView.Rows[i].FindControl(gridView.CheckBoxID);
                if (ChkBxItem != null && ChkBxItem.Checked)
                {
                    BxsChkd = true;
                    if (gridView.DataKeys[i].Value != null)
                    {
                        idlist += gridView.DataKeys[i].Value.ToString() + ",";
                    }
                }
            }
            if (BxsChkd)
            {
                idlist = idlist.Substring(0, idlist.LastIndexOf(","));
            }
            return idlist;
        }

        #region 批量上 下架

        /// <summary>
        /// 批量上 下架
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInverseApprove_Click(object sender, EventArgs e)
        {
            string idlist = GetSelIDlist();
            if (idlist.Trim().Length == 0) return;
            YSWL.MALL.Model.Shop.Products.ProductSaleStatus status;
            if (SaleStatus == (int)YSWL.MALL.Model.Shop.Products.ProductSaleStatus.OnSale)
                status = YSWL.MALL.Model.Shop.Products.ProductSaleStatus.InStock;
            else
                status = YSWL.MALL.Model.Shop.Products.ProductSaleStatus.OnSale;
            bll.UpdateList(idlist, status);
            supplierBll.UpdateProductCountList(idlist);
            YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.TooltipUpdateOK);
            gridView.OnBind();
        }
        

        #endregion 批量上 下架

        /// <summary>
        /// 批量审核操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCheck_Click(object sender, EventArgs e)
        {
            string idlist = GetSelIDlist();
            if (idlist.Trim().Length == 0) return;
            bll.UpdateList(idlist, YSWL.MALL.Model.Shop.Products.ProductSaleStatus.OnSale);
            supplierBll.UpdateProductCountList(idlist);
            YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.TooltipUpdateOK);
            gridView.OnBind();
        }

        protected void btnFresh_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }

        /// <summary>
        /// 获取商品所在分类信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private string ProductCategories(long productId)
        {
            List<Model.Shop.Products.ProductCategories> list = productCategory.GetModelList(productId);
            StringBuilder strName = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                foreach (Model.Shop.Products.ProductCategories productCategoriese in list)
                {
                    strName.Append(manage.GetFullNameByCache(productCategoriese.CategoryId));
                    strName.Append("</br>");
                }
            }
            return strName.ToString();
        }

        protected long StockNum(object obj)
        {
            if (obj != null)
            {
                if (!string.IsNullOrWhiteSpace(obj.ToString()))
                {
                    long productId = Common.Globals.SafeLong(obj.ToString(), 0);
                    return bll.StockNum(productId);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
 
    }
}