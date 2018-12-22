using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YSWL.MALL.Web.Admin.Members.UserRank
{
    public partial class UpdateLimit : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 297; } }
        private YSWL.MALL.BLL.Members.RankLimit LimitBll = new BLL.Members.RankLimit();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if ((Request.Params["limitId"] != null) && (Request.Params["limitId"].ToString() != ""))
                {
                    int limitId = Common.Globals.SafeInt(Request.Params["limitId"], 0);
                    YSWL.MALL.Model.Members.RankLimit LimitModel = LimitBll.GetModel(limitId);
                    if (LimitModel == null)
                    {
                        Response.Redirect("RankLimit.aspx");
                    }
                    tName.Text = LimitModel.Name;
                    tCycle.Text = LimitModel.Cycle.ToString();
                    tMaxTimes.Text = LimitModel.MaxTimes.ToString();
                    DropCycleUnit.SelectedValue = LimitModel.CycleUnit.ToString();

                }
            }
        }

        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, System.EventArgs e)
        {

            if ((Request.Params["limitId"] != null) && (Request.Params["limitId"].ToString() != ""))
            {
                int limitId = Common.Globals.SafeInt(Request.Params["limitId"], 0);
                YSWL.MALL.Model.Members.RankLimit LimitModel = LimitBll.GetModel(limitId);
                if (LimitBll.Exists(this.tName.Text.Trim(), LimitModel.LimitID))
                {
                    YSWL.Common.MessageBox.ShowSuccessTip(this, "已存在该规则限制名称，请重新填写");
                }
                else
                {
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

                    if (LimitBll.Update(LimitModel))
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

        }

        /// <summary>
        /// 取消操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("RankLimit.aspx");
        }

    }
}