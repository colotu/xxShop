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
namespace YSWL.MALL.Web.Shop.Order.PreOrder
{
    public partial class Show : Page
    {        
        		public string strid=""; 
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				if (Request.Params["id"] != null && Request.Params["id"].Trim() != "")
				{
					strid = Request.Params["id"];
					int PreOrderId=(Convert.ToInt32(strid));
					ShowInfo(PreOrderId);
				}
			}
		}
		
	private void ShowInfo(int PreOrderId)
	{
		YSWL.MALL.BLL.Shop.PrePro.PreOrder bll=new YSWL.MALL.BLL.Shop.PrePro.PreOrder();
		YSWL.MALL.Model.Shop.PrePro.PreOrder model=bll.GetModel(PreOrderId);
		this.lblPreOrderId.Text=model.PreOrderId.ToString();
		this.lblProductId.Text=model.ProductId.ToString();
		this.lblProductName.Text=model.ProductName;
		this.lblCount.Text=model.Count.ToString();
		this.lblSKU.Text=model.SKU;
		this.lblPhone.Text=model.Phone;
		this.lblUserId.Text=model.UserId.ToString();
		this.lblUserName.Text=model.UserName;
		this.lblCreatedDate.Text=model.CreatedDate.ToString();
		this.lblHandleUserId.Text=model.HandleUserId.ToString();
		this.lblHandleDate.Text=model.HandleDate.ToString();
        this.lblDeliveryDate.Text = model.DeliveryTip;
		this.lblStatus.Text=model.Status.ToString();
		this.lblRemark.Text=model.Remark;

	}

    public void btnCancle_Click(object sender, EventArgs e)
    {
        Response.Redirect("list.aspx");
    }
    }
}
