using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Members.UserRank
{
    public partial class AddRank : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 304; } } //用户管理_会员等级管理_新增页
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_AddData)) && GetPermidByActID(Act_AddData) != -1)
            {
                MessageBox.ShowAndBack(this, "您没有权限");
                return;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.btnCancle.Enabled = false;
            this.btnSave.Enabled = false;
            if (string.IsNullOrWhiteSpace(this.txtLevel.Text))
            {
                this.btnCancle.Enabled = true;
                this.btnSave.Enabled = true;
                MessageBox.ShowFailTip(this, "请输入等级级别！");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.txtName.Text))
            {
                this.btnCancle.Enabled = true;
                this.btnSave.Enabled = true;
                MessageBox.ShowFailTip(this, "请输入等级名称！");
                return;
            }
            if (Globals.SafeInt(this.txtLevel.Text, 0) > 20)
            {
                this.btnCancle.Enabled = true;
                this.btnSave.Enabled = true;
                MessageBox.ShowFailTip(this, "请输入0-20之间正确的等级级别！");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.txtMinRange.Text))
            {
                this.btnCancle.Enabled = true;
                this.btnSave.Enabled = true;
                MessageBox.ShowFailTip(this, "请输入等级成长值下限！");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.txtMaxRange.Text))
            {
                this.btnCancle.Enabled = true;
                this.btnSave.Enabled = true;
                MessageBox.ShowFailTip(this, "请输入等级成长值上限！");
                return;
            }

            string Name = this.txtName.Text;
            int level = Common.Globals.SafeInt(this.txtLevel.Text, 0);
            int MinRange = int.Parse(this.txtMinRange.Text);
            int MaxRange = int.Parse(this.txtMaxRange.Text);

            YSWL.MALL.Model.Members.UserRank model = new Model.Members.UserRank();
            model.Name = Name;
            model.RankLevel = level;
            model.ScoreMin= MinRange;
            model.ScoreMax = MaxRange;
            model.IsDefault = false;
            model.Description = this.txtDesc.Text;
        
            model.RankType = 0;

            YSWL.MALL.BLL.Members.UserRank bll = new BLL.Members.UserRank();
            int ID;
            if ((ID = bll.Add(model)) > 0)
            {
                YSWL.Common.MessageBox.ShowSuccessTip(this, "保存成功！", "RankList.aspx");
                LogHelp.AddUserLog(CurrentUser.UserName, CurrentUser.UserType, "新增用户等级(GradeID=" + ID + ")成功", this);
            }
            else
            {
                this.btnCancle.Enabled = true;
                this.btnSave.Enabled = true;
                YSWL.Common.MessageBox.ShowFailTip(this, "系统忙，请稍后再试！");
                LogHelp.AddUserLog(CurrentUser.UserName, CurrentUser.UserType, "新增用户等级失败", this);
            }
        }

        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("RankList.aspx");
        }
    }
}