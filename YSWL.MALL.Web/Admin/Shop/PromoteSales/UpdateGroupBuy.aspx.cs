using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.MALL.BLL.Shop.PromoteSales;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Shop.PromoteSales
{
    public partial class UpdateGroupBuy : System.Web.UI.Page
    {
        private YSWL.MALL.BLL.Shop.Products.ProductInfo productInfoBll = new ProductInfo();
        private YSWL.MALL.BLL.Shop.PromoteSales.GroupBuy buyBll = new GroupBuy();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowInfo();
            }
        }

        public int BuyId
        {
            get
            {
                int id = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["id"]))
                {
                    id = Globals.SafeInt(Request.Params["id"], 0);
                }
                return id;
            }
        }

        private void ShowInfo()
        {
            YSWL.MALL.Model.Shop.PromoteSales.GroupBuy buyModel = buyBll.GetModel(BuyId);
            if (buyModel != null)
            {
                this.txtDesc.Text = buyModel.Description;
                txtPrice.Text = buyModel.Price.ToString("F");
                txtEndDate.Text = buyModel.EndDate.ToString("yyyy-MM-dd");
                lblProductName.Text = productInfoBll.GetProductName(buyModel.ProductId);
                chkStatus.Checked = buyModel.Status == 1;
                txtStartDate.Text = buyModel.StartDate.ToString("yyyy-MM-dd");
                txtFinePrice.Text = buyModel.FinePrice.ToString("F");
                txtLimitCount.Text = buyModel.LimitQty.ToString();
                txtGroupCount.Text = buyModel.GroupCount.ToString();
                txtMaxCount.Text = buyModel.MaxCount.ToString();
                ajaxRegion.Area_iID = buyModel.RegionId;
                ajaxRegion.SelectedValue = buyModel.RegionId.ToString();
                    //this.ajaxRegion.Area_iID = info.RegionId.Value;
                    //this.ajaxRegion.SelectedValue = info.RegionId.Value.ToString();
               
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            YSWL.MALL.Model.Shop.PromoteSales.GroupBuy buyModel = buyBll.GetModel(BuyId);
            decimal price = Common.Globals.SafeDecimal(txtPrice.Text, -1);
            int groupCount = Common.Globals.SafeInt(this.txtGroupCount.Text, 0);
            int maxCount = Common.Globals.SafeInt(this.txtMaxCount.Text, 0);
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
            if (maxCount < groupCount)
            {
                Common.MessageBox.ShowFailTip(this, "限购总数量必须大于团购满足数量");
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

            int? selectedRegionId = Common.Globals.SafeInt(ajaxRegion.SelectedValue, -1);
            if (selectedRegionId == -1)
            {
                Common.MessageBox.ShowFailTip(this, "请选择团购地区");
                return;
            }
            YSWL.MALL.Model.Shop.Products.ProductInfo pro = productInfoBll.GetModelByCache(buyModel.ProductId);
            buyModel.Description = this.txtDesc.Text;
            buyModel.EndDate = Common.Globals.SafeDateTime(txtEndDate.Text, DateTime.Now);
            buyModel.Price = Common.Globals.SafeDecimal(price, 0);
            buyModel.Status = chkStatus.Checked ? 1 : 0;
            buyModel.FinePrice = Common.Globals.SafeDecimal(this.txtFinePrice.Text, 0);
            buyModel.GroupCount = groupCount;
            buyModel.MaxCount = maxCount;
            buyModel.LimitQty = limitCount;
            buyModel.RegionId = selectedRegionId.Value;
            if (null != pro)
            {
                buyModel.ProductName = pro.ProductName;
                buyModel.GroupBuyImage = pro.ThumbnailUrl1;
            }
           
            buyModel.StartDate = Common.Globals.SafeDateTime(txtStartDate.Text, DateTime.MinValue);
            if (buyBll.Update(buyModel))
            {
                Common.MessageBox.ShowSuccessTipScript(this, "操作成功", "window.parent.location.reload();");
            }
            else
            {
                Common.MessageBox.ShowFailTip(this, "操作失败");
            }
        }

    }
}