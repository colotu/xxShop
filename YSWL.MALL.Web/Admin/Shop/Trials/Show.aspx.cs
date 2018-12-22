/**
* Show.cs
*
* 功 能： N/A
* 类 名： Show
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01						   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;

namespace YSWL.MALL.Web.Admin.Shop.Trials
{
    public partial class Show : PageBaseAdmin
    {
        public string strid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.Params["id"] != null && Request.Params["id"].Trim() != "")
                {
                    strid = Request.Params["id"];
                    int TrialId = (Convert.ToInt32(strid));
                    ShowInfo(TrialId);
                }
            }
        }

        private void ShowInfo(int TrialId)
        {
            YSWL.MALL.BLL.Shop.Trial.Trials bll = new YSWL.MALL.BLL.Shop.Trial.Trials();
            YSWL.MALL.Model.Shop.Trial.Trials model = bll.GetModel(TrialId);
            this.lblTrialId.Text = model.TrialId.ToString();
            this.lblTrialName.Text = model.TrialName;
            this.lblShortDescription.Text = model.ShortDescription;
            this.lblDescription.Text = model.Description;
            this.lblLinklUrl.Text = model.LinklUrl;
            this.lblTrialStatus.Text = GetTrialStatus(model.TrialStatus);
            this.lblStartDate.Text = model.StartDate.ToString("yyyy-MM-dd");
            this.lblEndDate.Text = model.EndDate.ToString("yyyy-MM-dd");
            this.lblCreatedDate.Text = model.CreatedDate.ToString();
            this.lblTrialCounts.Text = model.TrialCounts.ToString();
            this.lblDisplaySequence.Text = model.DisplaySequence.ToString();
            this.lblMarketPrice.Text = model.MarketPrice.ToString();
            this.imgUrl.ImageUrl = model.ImageUrl;

        }

        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }


        public string GetTrialStatus(object obj)
        {
            if (obj == null) return string.Empty;
            int trialStatus = Common.Globals.SafeInt(obj.ToString(), -1);
            if (trialStatus < 0) return string.Empty;
            switch (trialStatus)
            {
                //0:即将进行 1:进行中(立即申请) 2:已结束
                case 0:
                    return "即将进行";
                case 1:
                    return "进行中";
                case 2:
                    return "已结束";
            }
            return string.Empty;
        }
    }

}
