using System;
using YSWL.Common;
namespace YSWL.MALL.Web.Admin.Accounts
{
    public partial class MailConfigInfo : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 162; } } //网站管理_是否显示邮件配置页面
        protected new int Act_UpdateData = 163;    //网站管理_邮件配置-编辑邮件信息
        YSWL.MALL.BLL.MailConfig bll = new YSWL.MALL.BLL.MailConfig();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_UpdateData)) && GetPermidByActID(Act_UpdateData) != -1)
                {
                    btnSave.Visible = false;
                }
                BoundData();
            }
        }

        private void BoundData()
        {
            YSWL.MALL.Model.MailConfig model = bll.GetModel();
            if (model != null )
            {
                this.txtSMTPServer.Text = model.SMTPServer;
                this.txtSMTPPort.Text = model.SMTPPort.ToString();
                this.txtMailaddress.Text = model.Mailaddress;
                this.txtPasswordMail.Attributes.Add("value", YSWL.Common.DEncrypt.DESEncrypt.Decrypt(model.Password));
                this.txtUserNameMail.Text = model.Username;
                if (model.SMTPSSL )
                {
                    this.chkSMTPSSL.Checked = true;
                }
                else
                {
                    this.chkSMTPSSL.Checked = false;
                }
                this.HiddenField_ID.Value = model.ID.ToString();
            }
            else
            {
                this.txtSMTPServer.Text = "";
                this.txtSMTPPort.Text = "25";
                this.txtMailaddress.Text ="";
                this.txtUserNameMail.Text ="";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            YSWL.MALL.Model.MailConfig model = bll.GetModel();
            if (model != null )
            {
                model.Mailaddress = txtMailaddress.Text;
                model.Password = YSWL.Common.DEncrypt.DESEncrypt.Encrypt(txtPasswordMail.Text);
                model.SMTPPort = Convert.ToInt32(txtSMTPPort.Text);
                model.SMTPServer = txtSMTPServer.Text;
                model.SMTPSSL = chkSMTPSSL.Checked;
                model.Username = txtUserNameMail.Text;
                bll.Update(model);
                MessageBox.ShowSuccessTip(this, Resources.Site.TooltipSaveOK);
            }
            else
            {
                YSWL.MALL.Model.MailConfig models = new YSWL.MALL.Model.MailConfig();
                models.Mailaddress = txtMailaddress.Text;
                models.Password = YSWL.Common.DEncrypt.DESEncrypt.Encrypt(txtPasswordMail.Text);
                models.SMTPPort = Convert.ToInt32(txtSMTPPort.Text);
                models.SMTPServer = txtSMTPServer.Text;
                models.SMTPSSL = chkSMTPSSL.Checked;
                models.Username = txtUserNameMail.Text;
                if (this.CurrentUser != null)
                {
                    models.UserID = this.CurrentUser.UserID;
                }
                YSWL.MALL.BLL.MailConfig blls = new YSWL.MALL.BLL.MailConfig();
                if (!bll.Exists(models.UserID, models.Mailaddress))
                {
                    bll.Add(models);
                    MessageBox.ShowSuccessTip(this, Resources.Site.TooltipSaveOK);
                }
            }
        }


        public void btnCancle_Click(object sender, EventArgs e)
        {
            BoundData();
        }
    }
}