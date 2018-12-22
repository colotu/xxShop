using System;
using System.Web.UI;
using YSWL.Accounts.Bus;

namespace YSWL.MALL.Web.Admin.Members.MembershipManage
{
    public partial class Show : PageBaseAdmin
    {
        public string strid = "";

        private YSWL.MALL.BLL.Members.SiteMessage bll = new BLL.Members.SiteMessage();
        private YSWL.Accounts.Bus.UserType UserType = new YSWL.Accounts.Bus.UserType();
        protected override int Act_PageLoad { get { return 287; } } //用户管理_会员信息管理_详细页
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.Params["id"] != null && Request.Params["id"].Trim() != "")
                {
                    strid = Request.Params["id"];
                    int ID = (Convert.ToInt32(strid));
                    ShowInfo(ID);
                }
            }
        }

        private void ShowInfo(int ID)
        {
            AccountsPrincipal user = new AccountsPrincipal(ID);
            User manage = new YSWL.Accounts.Bus.User(user);

            BLL.Members.UsersExp expBll = new BLL.Members.UsersExp();
            Model.Members.UsersExpModel model = expBll.GetUsersExpModel(ID);
            if (manage != null && model != null)
            {
                this.lblUserName.Text = manage.UserName;
                this.lblTrueName.Text = manage.TrueName;
                this.lblPhone.Text = manage.Phone;
                this.lblNickName.Text = manage.NickName;
                this.lblEmail.Text = manage.Email;

                this.lblSex.Text = (!string.IsNullOrWhiteSpace(manage.Sex) && manage.Sex.Trim() == "0") ? "女" : "男";
                this.lblActivity.Text = manage.Activity ? "正常使用" : "已经冻结";
                this.lblCreTime.Text = manage.User_dateCreate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (model != null)
            {
                YSWL.MALL.BLL.Ms.Regions RegionBll = new BLL.Ms.Regions();
                this.imageGra.ImageUrl = string.Format("/Upload/User/Gravatar/{0}.jpg", model.UserID);
                this.lblAddress.Text = RegionBll.GetAddress(model.RegionId);
                this.lblPoints.Text = model.Points.ToString();
                this.lblBalance.Text = model.Balance.HasValue ? model.Balance.Value.ToString("F") : "0.00";
                this.lblLoginDate.Text = model.LastLoginTime.ToString("yyyy-MM-dd HH:mm:ss");
                lblSingature.Text = model.Singature;
                lblRankScore.Text = model.RankScore.ToString();
            }
        }
 
    }
}