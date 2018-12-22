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
using System.Web.UI;
namespace YSWL.MALL.Web.Admin.Shop.TrialReports
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
                    int ReportId = (Convert.ToInt32(strid));
                    ShowInfo(ReportId);
                }
            }
        }

        private void ShowInfo(int ReportId)
        {
            YSWL.MALL.BLL.Shop.Trial.TrialReports bll = new YSWL.MALL.BLL.Shop.Trial.TrialReports();
            YSWL.MALL.Model.Shop.Trial.TrialReports model = bll.GetModel(ReportId);
            this.lblReportId.Text = model.ReportId.ToString();
            this.lblTitle.Text = model.Title;
            this.lblLinkUrl.Text = model.LinkUrl;
            this.lblShortDescription.Text = model.ShortDescription;
            this.lblCreatedUserName.Text = model.CreatedUserName;
            this.imgUrl.ImageUrl = model.ImageUrl;

        }



        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }

}
