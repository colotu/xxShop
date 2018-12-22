using System;
using System.Web.UI;

namespace YSWL.MALL.Web.Admin.Members.SiteMessages
{
    public partial class Show : PageBaseAdmin
    {        
       
        public string strid="";
        protected override int Act_PageLoad { get { return 303; } }//客服管理_站内信_详细页
        YSWL.MALL.BLL.Members.SiteMessage bll = new BLL.Members.SiteMessage();
        YSWL.Accounts.Bus.UserType UserType = new YSWL.Accounts.Bus.UserType();
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				if (Request.Params["id"] != null && Request.Params["id"].Trim() != "")
				{
					strid = Request.Params["id"];
					int ID=(Convert.ToInt32(strid));
					ShowInfo(ID);
				}
			}
		}
		
	private void ShowInfo(int ID)
	{
        YSWL.MALL.BLL.Members.SiteMessage bll = new YSWL.MALL.BLL.Members.SiteMessage();
        YSWL.MALL.Model.Members.SiteMessage model = bll.GetModel(ID);
		this.lblID.Text=model.ID.ToString();

        this.lblReceiverID.Text = GetUser(model.MsgType,model.ReceiverID);
	
		this.lblContent.Text=model.Content;
		

	}

    protected void btnCancle_Click(object sender, EventArgs e)
    {
        Response.Redirect("list.aspx");
    }
    public string GetUser(object obj, object ReceiverID)
    {
        if (obj != null)
        {

            string TypeDes = UserType.GetDescription(obj.ToString());
            if (TypeDes != null)
            {
                return TypeDes;
            }
        }
        else if (YSWL.Common.PageValidate.IsNumber(ReceiverID.ToString()))
        {
            YSWL.MALL.BLL.Members.Users user = new BLL.Members.Users();
            YSWL.MALL.Model.Members.Users usermodel = user.GetModel(Convert.ToInt32(ReceiverID));
            if (usermodel != null)
            {
                return usermodel.UserName;
            }

        }

        return "";
    }

    }
}
