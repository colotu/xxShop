using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Accounts.Bus;

namespace YSWL.MALL.Web.Admin.Accounts
{
    public partial class SendSMS : PageBaseAdmin
    {
      //  protected override int Act_PageLoad { get { return 164; } }  

       // protected new int Act_UpdateData = 165;     
        private YSWL.MALL.BLL.Members.Users userBll=new BLL.Members.Users();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_UpdateData)) && GetPermidByActID(Act_UpdateData) != -1)
                {
                    btnNext.Visible = false;
                }
                string SerialNo = BLL.SysManage.ConfigSystem.GetValueByCache("Emay_SMS_SerialNo");
                string Key = BLL.SysManage.ConfigSystem.GetValueByCache("Emay_SMS_Key");
                bool IsOpen=  BLL.SysManage.ConfigSystem.GetBoolValueByCache("Emay_SMS_IsOpen");
                if (!IsOpen)
                {
                    YSWL.Common.MessageBox.ShowFailTip(this, "请先开启短信接口功能", "/Admin/Ms/SMS/Settings.aspx");
                    return;
                }
                if (String.IsNullOrWhiteSpace(SerialNo) || String.IsNullOrWhiteSpace(Key))
                {
                    YSWL.Common.MessageBox.ShowFailTip(this, "请填写短信接口相关信息","/Admin/Ms/SMS/Settings.aspx");
                    return;
                }
            }
        }
        /// <summary>
        /// 发送短信
        /// </summary>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtContent.Text))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, Resources.SysManage.ErrorContentNotNull);
                return;
            }
            if (this.Multi.Checked)
            {
                List<YSWL.MALL.Model.Members.Users> userList = userBll.GetModelList(String.Format("Activity=1 and UserType='{0}'", this.DropUserType.SelectedValue));
                string[] numbers = null;
                //移除手机为空的对象
                userList = userList.Where(c => !String.IsNullOrWhiteSpace(c.Phone)).ToList();
                string content = this.txtContent.Text;
                int page=userList.Count%200>0? (userList.Count/200)+1: (userList.Count/200);
                for (int i = 0; i < page; i++)
                {
                    var selectList = userList.Skip(i*200).Take(200).ToList();
                    if (selectList.Count() > 0)
                    {
                        numbers= new string[selectList.Count()];
                        int j = 0;
                        foreach (var item in selectList)
                        {
                            numbers[j] = item.Phone;
                            j++;
                        }
                        YSWL.MALL.Web.Components.SMSHelper.SendSMS(content, numbers);
                    }
                }
                YSWL.Common.MessageBox.ShowSuccessTip(this, "群发短信完成", "SendSMS.aspx");
            }
            else
            {
                int userId = Common.Globals.SafeInt(txtUserId.Text, 0);
                User user = new YSWL.Accounts.Bus.User(userId);
                string content = this.txtContent.Text;
                string[] numbers = new string[] { user.Phone };
                bool isSuccess = YSWL.MALL.Web.Components.SMSHelper.SendSMS(content, numbers);
                if (isSuccess)
                {
                    YSWL.Common.MessageBox.ShowSuccessTip(this,"发送短信成功", "SendSMS.aspx");
                }
                else
                {
                    YSWL.Common.MessageBox.ShowFailTip(this, "发送短信失败", "SendSMS.aspx");
                }
            }
        }
    }
}