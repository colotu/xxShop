using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.MALL.BLL.Shop.PromoteSales;

namespace YSWL.MALL.Web.Admin.Shop.PromoteSales
{
    public partial class AddGroupBuy : PageBaseAdmin
    {
        private YSWL.MALL.BLL.Shop.Products.CategoryInfo categoryInfoBll = new CategoryInfo();
        private YSWL.MALL.BLL.Shop.Products.ProductInfo productInfoBll = new ProductInfo();
        private YSWL.MALL.BLL.Shop.PromoteSales.GroupBuy buyBll = new GroupBuy();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //商品分类
                this.ddlCateList.DataSource = categoryInfoBll.GetList("Depth=1");
                ddlCateList.DataTextField = "Name";
                ddlCateList.DataValueField = "CategoryId";
                ddlCateList.DataBind();
                ddlCateList.Items.Insert(0, new ListItem("请选择", "0"));
                this.txtSequence.Text = (buyBll.MaxSequence() + 1).ToString();

            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            YSWL.MALL.Model.Shop.PromoteSales.GroupBuy buyModel = new Model.Shop.PromoteSales.GroupBuy();
            long productId = Common.Globals.SafeLong(this.ddlProduct.SelectedValue, 0);
            int groupCount=Common.Globals.SafeInt(this.txtGroupCount.Text, 0);
            int maxCount=Common.Globals.SafeInt(this.txtMaxCount.Text, 0);
       
            if (productId == 0)
            {
                Common.MessageBox.ShowFailTip(this, "请选择团购商品！");
                return;
            }
            decimal price = Common.Globals.SafeDecimal(txtPrice.Text, -1);
            if (price == -1)
            {
                Common.MessageBox.ShowFailTip(this, "请填写团购价格");
                return;
            }
            if (String.IsNullOrWhiteSpace(txtStartDate.Text))
            {
                Common.MessageBox.ShowFailTip(this, "请选择活动开始时间");
                return;
            }
            if (String.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                Common.MessageBox.ShowFailTip(this, "请选择活动结束时间");
                return;
            }
            if (String.IsNullOrWhiteSpace(txtLimitCount.Text))
            {
                Common.MessageBox.ShowFailTip(this, "请设置单次限购数量");
                return;
            }
            int limitCount = YSWL.Common.Globals.SafeInt(txtLimitCount.Text, 1);
            if (limitCount > maxCount)
            {
                Common.MessageBox.ShowFailTip(this, "单次购买数量须小于限购总数量");
                return; 
            }
            if (maxCount < groupCount)
            {
                Common.MessageBox.ShowFailTip(this, "限购总数量必须大于团购满足数量");
                return;
            }
            //判重
            if (buyBll.IsExists(productId))
            {
                Common.MessageBox.ShowFailTip(this, "该商品已加入团购活动");
                return;
            }
            int? selectedRegionId = Common.Globals.SafeInt(ajaxRegion.SelectedValue, -1);
            if (selectedRegionId == -1)
            {
                Common.MessageBox.ShowFailTip(this, "请选择团购地区");
                return;
            }
            if (ddlCateList.SelectedValue == "0")
            {
                Common.MessageBox.ShowFailTip(this, "请选择商品分类");
                return;
            }
            string categoryId = ddlCateList.SelectedValue;
            string path = ddlCateList2.SelectedValue;
            BLL.Shop.Products.ProductInfo bllProductInfo=new ProductInfo();
            YSWL.MALL.Model.Shop.Products.ProductInfo productModel = bllProductInfo.GetModelByCache(productId);
            buyModel.Description = this.txtDesc.Text;
            buyModel.EndDate = Common.Globals.SafeDateTime(txtEndDate.Text, DateTime.Now);
            buyModel.Price = Common.Globals.SafeDecimal(price, 0);
            buyModel.ProductId = productId;
            buyModel.Sequence = Common.Globals.SafeInt(txtSequence.Text, 0);
            buyModel.Status = chkStatus.Checked ? 1 : 0;
            buyModel.FinePrice = Common.Globals.SafeDecimal(this.txtFinePrice.Text, 0);
            buyModel.RegionId = selectedRegionId.Value;
            buyModel.GroupCount = groupCount;
            buyModel.MaxCount = maxCount;
            buyModel.StartDate = Common.Globals.SafeDateTime(txtStartDate.Text, DateTime.MinValue);
            buyModel.CategoryId = Common.Globals.SafeInt(categoryId, 0);
            buyModel.LimitQty = limitCount;
            if (path == "0")
            {
                buyModel.CategoryPath = categoryId;
            }
            else
            {
                buyModel.CategoryPath = categoryId+"|"+path;
            }
            if (null != productModel)
            {
                buyModel.ProductName = productModel.ProductName;
                buyModel.ProductCategory = ddlCateList.SelectedItem.Text;
                buyModel.GroupBuyImage = productModel.ThumbnailUrl1;
            }
            if (buyBll.Add(buyModel) > 0)
            {
                Common.MessageBox.ShowSuccessTip(this, "操作成功", "GroupBuyList.aspx");
            }
            else
            {
                Common.MessageBox.ShowFailTip(this, "操作失败");
            }
        }

        protected void ddlCateList_Changed(object sender, EventArgs e)
        {
            int categoryId = Common.Globals.SafeInt(ddlCateList.SelectedValue, 0);
            if (categoryId == 0)
            {
                ddlCateList2.Visible = false;
                return;
            }

            //绑定二级分类
            this.ddlCateList2.DataSource = categoryInfoBll.GetList("ParentCategoryId=" + categoryId);
            ddlCateList2.DataTextField = "Name";
            ddlCateList2.DataValueField = "CategoryId";
            ddlCateList2.DataBind();
            ddlCateList2.Items.Insert(0, new ListItem("请选择", "0"));
            ddlCateList2.Visible = true;

            ddlProduct.DataSource = productInfoBll.GetProductsByCid(categoryId);
            ddlProduct.DataTextField = "ProductName";
            ddlProduct.DataValueField = "ProductId";
            ddlProduct.DataBind();
        }

        protected void ddlCateList2_Changed(object sender, EventArgs e)
        {
            int categoryId = Common.Globals.SafeInt(ddlCateList2.SelectedValue, 0);
            if (categoryId == 0)
            {
                categoryId = Common.Globals.SafeInt(ddlCateList.SelectedValue, 0);
            }

            ddlProduct.DataSource = productInfoBll.GetProductsByCid(categoryId);
            ddlProduct.DataTextField = "ProductName";
            ddlProduct.DataValueField = "ProductId";
            ddlProduct.DataBind();
        }
    }
}