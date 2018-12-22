using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Ms;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Members.Guestbook
{
    public partial class ReplyGuestBook : PageBaseAdmin
    {
        YSWL.MALL.BLL.Ms.EmailTemplet EmailBll=new EmailTemplet();
        protected override int Act_PageLoad { get { return 286; } } //客服管理_客户反馈_回复页
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public int Id
        {
            get
            {
                int id = -1;
                if (!string.IsNullOrWhiteSpace(Request.Params["id"]))
                {
                    id = Globals.SafeInt(Request.Params["id"], -1);
                }
                return id;
            }
        }
        protected void btnSend_Click(object sender, EventArgs e)
        {
            YSWL.MALL.BLL.Members.Guestbook bll=new BLL.Members.Guestbook();
            YSWL.MALL.Model.Members.Guestbook model=bll.GetModel(Id);
            try
            {
             if (model != null)
            {
                model.ReplyDescription = TxtReply.Text;
                model.Status = 1;
                model.HandlerDate = DateTime.Now;
                model.HandlerNickName = CurrentUser.NickName;
                model.HandlerUserID = CurrentUser.UserID;
                if (bll.Update(model))//EmailBll.SendGuestBookEmail(model)&&
                {
                    Common.MessageBox.ShowSuccessTipScript(this, "操作成功", "$(parent.document).find('[id$=btnSearch]').click();");
                    //lblTip.Visible = true;
                }
                else
                {
                    Common.MessageBox.ShowFailTip(this, "操作失败");
                    //Common.MessageBox.ShowFailTip(this, "发送失败，请检查邮件配置");
                    //lblTip.InnerText = "出现异常，请重试";
                    //lblTip.Visible = true;

                }
            }
    
            }
            catch (Exception)
            {
                Common.MessageBox.ShowFailTip(this, "发送失败，请检查邮件配置");
            }
          

        }
    }
}