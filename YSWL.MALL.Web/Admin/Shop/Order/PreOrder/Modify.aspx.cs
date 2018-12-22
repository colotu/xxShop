using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using YSWL.Common;
namespace YSWL.MALL.Web.Shop.Order.PreOrder
{
    public partial class Modify : PageBaseAdmin
    {
        YSWL.MALL.BLL.Shop.PrePro.PreOrder bll = new YSWL.MALL.BLL.Shop.PrePro.PreOrder();
        BLL.Members.Users userBll = new BLL.Members.Users();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ShowInfo();
            }
        }
        private int PreOrderId
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
            YSWL.MALL.Model.Shop.PrePro.PreOrder model = bll.GetModel(PreOrderId);
            if (model != null)
            {
                this.lblPreOrderId.Text = model.PreOrderId.ToString();
                this.lblProductName.Text = model.ProductName;
                this.txtCount.Text = model.Count.ToString();
                this.lblPhone.Text = model.Phone;
                this.lblUserName.Text = model.UserName;
                this.lblCreatedDate.Text = model.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
                this.lblHandleUserId.Text = model.HandleUserId > 0 ? userBll.GetUserName(model.HandleUserId) : "";
                this.lblHandleDate.Text = model.HandleDate.ToString();
                this.lblDeliveryTip.Text = model.DeliveryTip;
                this.dropStatus.SelectedValue = model.Status.ToString();
                //if (model.Status == 2)
                //{
                //    dropStatus.Enabled = false;
                //}
                this.txtRemark.Text = model.Remark;
            }
        }

		public void btnSave_Click(object sender, EventArgs e)
		{
			int Status=Globals.SafeInt(dropStatus.SelectedValue,1);
			string Remark=this.txtRemark.Text;
            int Count =Globals.SafeInt( txtCount.Text,0);
            if (Count <= 0) {
                MessageBox.ShowFailTip(this, "请正确填写数量！");
                return;
            }
            YSWL.MALL.Model.Shop.PrePro.PreOrder model = bll.GetModel(PreOrderId);
            if (model != null) {
                BLL.Shop.Products.ProductInfo prodBll = new BLL.Shop.Products.ProductInfo();
                Model.Shop.Products.ProductInfo prodModel = prodBll.GetModelByCache(model.ProductId);
                if (prodModel == null)
                {
                    MessageBox.ShowFailTip(this, "改商品已不存在！");
                    return;
                }
                if (Count > prodModel.RestrictionCount)
                {
                    MessageBox.ShowFailTip(this, "非常抱歉, 最多只能订购" + prodModel.RestrictionCount + "个，您已超出限购数量！");
                    return;
                }
 
                model.Count = Count;
                model.Status = Status;
                model.Remark = Remark;
                model.HandleDate = DateTime.Now;
                model.HandleUserId = CurrentUser.UserID;
                if (bll.Update(model))
                {
                    MessageBox.ShowSuccessTipScript(this, "保存成功！", "$(parent.document).find('[id$=btnSearch]').click();");
                }
                else
                {
                    YSWL.Common.MessageBox.ShowFailTip(this, "保存失败！");
                }
            }
		}
 
        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
