using System;
using YSWL.Common;
namespace YSWL.MALL.Web.FriendlyLink.FLinks
{
    public partial class AddNew : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 380; } } //设置_友情链接管理_新增页
        protected void Page_Load(object sender, EventArgs e)
        {
           

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string Name = this.txtName.Text;
            string ImgUrl = this.txtImgUrl.Text;
            int ImgWidth = Globals.SafeInt(this.txtImgWidth.Text, 0);
            int ImgHeight = Globals.SafeInt(this.txtImgHeight.Text, 0);
            string LinkUrl = this.txtLinkUrl.Text;
            string LinkDesc = this.txtLinkDesc.Text;
            //int State = int.Parse(this.dropState.SelectedValue);
            int OrderID = int.Parse(this.txtOrderID.Text);
            string ContactPerson = this.txtContactPerson.Text;
            string Email = this.txtEmail.Text;
            string TelPhone = this.txtTelPhone.Text;
            int TypeID = int.Parse(this.dropTypeID.SelectedValue);
            bool IsDisplay = this.radioBtnYes.Checked;

            YSWL.MALL.Model.Settings.FriendlyLink model = new YSWL.MALL.Model.Settings.FriendlyLink();
            model.Name = Name;
            model.ImgUrl = ImgUrl;
            model.ImgWidth = ImgWidth;
            model.ImgHeight = ImgHeight;
            model.LinkUrl = LinkUrl;
            model.LinkDesc = LinkDesc;
            //model.State = State;
            model.OrderID = OrderID;
            model.ContactPerson = ContactPerson;
            model.Email = Email;
            model.TelPhone = TelPhone;
            model.TypeID = TypeID;
            model.IsDisplay = IsDisplay;

            YSWL.MALL.BLL.Settings.FriendlyLink bll = new YSWL.MALL.BLL.Settings.FriendlyLink();
            if (bll.Add(model) > 0)
            {
                YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.TooltipSaveOK, "ListNew.aspx");
            }
            else
            {
                YSWL.Common.MessageBox.ShowFailTip(this, Resources.Site.TooltipSaveError, "ListNew.aspx");
            }
        }


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("listnew.aspx");
        }

       

    
    }
}
