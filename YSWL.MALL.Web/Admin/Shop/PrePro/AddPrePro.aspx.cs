using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.MALL.BLL.Shop.PrePro;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Shop.PrePro
{
    public partial class AddPrePro : PageBaseAdmin
    {
        private YSWL.MALL.BLL.Shop.Products.ProductInfo productInfoBll = new ProductInfo();
        private YSWL.MALL.BLL.Shop.PrePro.PreProduct preProBll = new PreProduct();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ////商品分类
                //this.ddlCateList.DataSource = categoryInfoBll.GetList("Depth=1");
                //ddlCateList.DataTextField = "Name";
                //ddlCateList.DataValueField = "CategoryId";
                //ddlCateList.DataBind();
                //ddlCateList.Items.Insert(0, new ListItem("请选择", "0"));
                //this.txtSequence.Text = (downBll.MaxSequence() + 1).ToString();

                //加载订购商品
                ddlProduct.DataSource = productInfoBll.GetPreProductList();
                ddlProduct.DataTextField = "ProductName";
                ddlProduct.DataValueField = "ProductId";
                ddlProduct.DataBind();
                if (ProductId > 0)
                {
                    ddlProduct.SelectedValue = ProductId.ToString();
                }

            }
        }

        public long ProductId
        {
            get
            {
                return Globals.SafeLong(Request.Params["id"], 0);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            YSWL.MALL.Model.Shop.PrePro.PreProduct model = new YSWL.MALL.Model.Shop.PrePro.PreProduct();
            long productId = Common.Globals.SafeLong(this.ddlProduct.SelectedValue, 0);
            if (productId == 0)
            {
                Common.MessageBox.ShowFailTip(this, "请选择预订商品！");
                return;
            }
           
            int limitCount = Common.Globals.SafeInt(txtLimitCount.Text, 0);
            decimal price = Common.Globals.SafeDecimal(txtPrice.Text, 0);

            if (String.IsNullOrWhiteSpace(txtPreStartDate.Text) || String.IsNullOrWhiteSpace(txtPreEndDate.Text))
            {
                Common.MessageBox.ShowFailTip(this, "请选择活动预订时间");
                return;
            }

            if (String.IsNullOrWhiteSpace(txtBuyStartDate.Text) || String.IsNullOrWhiteSpace(txtBuyEndDate.Text))
            {
                Common.MessageBox.ShowFailTip(this, "请选择活动预订购买时间");
                return;
            }
            //判重
            if (preProBll.IsExists(productId))
            {
                Common.MessageBox.ShowFailTip(this, "该商品已加入订购活动");
                return;
            }
       
            model.Description = this.txtDesc.Text;
            model.PreStartDate = Common.Globals.SafeDateTime(txtPreStartDate.Text, DateTime.Now);
            model.PreEndDate = Common.Globals.SafeDateTime(txtPreEndDate.Text, DateTime.MaxValue);
            model.BuyStartDate = Common.Globals.SafeDateTime(txtBuyStartDate.Text, DateTime.Now);
            model.BuyEndDate = Common.Globals.SafeDateTime(txtBuyEndDate.Text, DateTime.Now);
            model.PreAmount = Common.Globals.SafeDecimal(price, 0);
            model.ProductId = productId;
            model.Status = chkStatus.Checked ? 1 : 0;
            model.LimitQty = limitCount;
            if (preProBll.Add(model) > 0)
            {
                Common.MessageBox.ShowSuccessTip(this, "操作成功", "PreProList.aspx");
            }
            else
            {
                Common.MessageBox.ShowFailTip(this, "操作失败");
            }
        }

    }
}