using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YSWL.MALL.Web.Admin.Members.UserRank
{
    public partial class AddLimit : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 296; } }
        private YSWL.MALL.BLL.Members.RankLimit LimitBll = new BLL.Members.RankLimit();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropCycleUnit.DataBind();
            }
        }

        public void btnSave_Click(object sender, System.EventArgs e)
        {

            if (LimitBll.Exists(this.tName.Text.Trim()))
            {
                YSWL.Common.MessageBox.ShowSuccessTip(this, "已存在该规则限制名称，请重新填写");
            }
            else
            {
                YSWL.MALL.Model.Members.RankLimit LimitModel = new Model.Members.RankLimit();
                LimitModel.Name = this.tName.Text.Trim();
                if (Common.PageValidate.IsNumber(this.tCycle.Text.Trim()))
                {
                    LimitModel.Cycle = Common.Globals.SafeInt(this.tCycle.Text.Trim(), 0);
                }
                if (Common.PageValidate.IsNumber(this.tMaxTimes.Text.Trim()))
                {
                    LimitModel.MaxTimes = Common.Globals.SafeInt(this.tMaxTimes.Text.Trim(), 0);
                }
                LimitModel.CycleUnit = DropCycleUnit.SelectedValue;
                LimitModel.TargetId = 0;
                LimitModel.TargetType = 0;
                if (LimitBll.Add(LimitModel) > 0)
                {
                    Response.Redirect("RankLimit.aspx");
                }
                else
                {
                    this.lblMsg.ForeColor = Color.Red;
                    this.lblMsg.Text = "新增条件限制出错！";
                }
            }

        }

        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("RankLimit.aspx");
        }
    }
}