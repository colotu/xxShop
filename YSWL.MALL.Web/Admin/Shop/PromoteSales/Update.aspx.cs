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
    public partial class Update : PageBaseAdmin
    {
        private YSWL.MALL.BLL.Shop.Products.ProductInfo productInfoBll = new ProductInfo();
        private YSWL.MALL.BLL.Shop.PromoteSales.CountDown downBll = new CountDown();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowInfo();
            }
        }

        public int DownId
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
           YSWL.MALL.Model.Shop.PromoteSales.CountDown  downModel=downBll.GetModel(DownId);
            if (downModel != null)
            {
                this.txtDesc.Text = downModel.Description;
                txtPrice.Text = downModel.Price.ToString("F");
                txtEndDate.Text = downModel.EndDate.ToString("yyyy-MM-dd HH:mm:ss");
                lblProductName.Text = productInfoBll.GetProductName(downModel.ProductId);
                txtLimitCount.Text = downModel.LimitQty.ToString();
                chkStatus.Checked = downModel.Status == 1;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            YSWL.MALL.Model.Shop.PromoteSales.CountDown downModel = downBll.GetModel(DownId);
          
            decimal price = Common.Globals.SafeDecimal(txtPrice.Text, -1);
            if (price == -1)
            {
                Common.MessageBox.ShowFailTip(this, "请填写商品价格");
                return;
            }
            if (String.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                Common.MessageBox.ShowFailTip(this, "请选择活动结束时间");
                return;
            }
            if (String.IsNullOrWhiteSpace(txtLimitCount.Text))
            {
                Common.MessageBox.ShowFailTip(this, "请设置限购数量");
                return;
            }
            int limitCount = Common.Globals.SafeInt(txtLimitCount.Text, 1);
            //YSWL.MALL.Model.Shop.Products.ProductInfo productInfo = productInfoBll.GetModelByCache(downModel.ProductId);
            //if (null != productInfo)
            //{
            //    if (productInfo.StockNum < limitCount)
            //    {
            //        Common.MessageBox.ShowFailTip(this, "限购数量不得大于商品库存！");
            //        return;
            //    }
            //}
            downModel.Description = this.txtDesc.Text;
            downModel.EndDate = Common.Globals.SafeDateTime(txtEndDate.Text, DateTime.Now);
            downModel.Price = Common.Globals.SafeDecimal(price, 0);
            downModel.Status = chkStatus.Checked ? 1 : 0;
            downModel.LimitQty = limitCount;
            if (downBll.Update(downModel))
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