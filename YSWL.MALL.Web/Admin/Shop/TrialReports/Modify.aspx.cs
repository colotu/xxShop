/**
* Modify.cs
*
* 功 能： N/A
* 类 名： Modify
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01							N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using YSWL.Common;
using YSWL.Accounts.Bus;
namespace YSWL.MALL.Web.Admin.Shop.TrialReports
{
    public partial class Modify : PageBaseAdmin
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.Params["id"] != null && Request.Params["id"].Trim() != "")
                {
                    int ReportId = (Convert.ToInt32(Request.Params["id"]));
                    ShowInfo(ReportId);
                }
            }
        }

        private void ShowInfo(int ReportId)
        {
            YSWL.MALL.BLL.Shop.Trial.TrialReports bll = new YSWL.MALL.BLL.Shop.Trial.TrialReports();
            YSWL.MALL.Model.Shop.Trial.TrialReports model = bll.GetModel(ReportId);
            this.lblReportId.Text = model.ReportId.ToString();
            this.txtTitle.Text = model.Title;
            this.txtLinkUrl.Text = model.LinkUrl;
            this.txtShortDescription.Text = model.ShortDescription;
            this.txtCreatedUserName.Text = model.CreatedUserName;

            this.hfImageUrl.Value = model.ImageUrl;
            this.hfOldImage.Value = model.ImageUrl;
        }

        public void btnSave_Click(object sender, EventArgs e)
        {

            string strErr = "";
            if (this.txtTitle.Text.Trim().Length == 0)
            {
                strErr += "名称不能为空！\\n";
            }
            if (this.txtLinkUrl.Text.Trim().Length == 0)
            {
                strErr += "链接不能为空！\\n";
            }
            if (this.txtShortDescription.Text.Trim().Length == 0)
            {
                strErr += "介绍不能为空！\\n";
            }
            if (this.txtCreatedUserName.Text.Trim().Length == 0)
            {
                strErr += "发布者不能为空！\\n";
            }

            if (strErr != "")
            {
                MessageBox.Show(this, strErr);
                return;
            }
            int ReportId = int.Parse(this.lblReportId.Text);
            string Title = this.txtTitle.Text;
            string LinkUrl = this.txtLinkUrl.Text;
            string ShortDescription = this.txtShortDescription.Text;
            string CreatedUserName = this.txtCreatedUserName.Text;
            string ImageUrl = string.Empty;

            //待上传的图片名称
            string tempFile = string.Format("/Upload/Temp/{0}", DateTime.Now.ToString("yyyyMMdd"));
            string ImageFile = "/Upload/Shop/TrialReports";
            System.Collections.ArrayList imageList = new System.Collections.ArrayList();
            if (!string.IsNullOrWhiteSpace(hfIsModifyImage.Value))
            {
                string imageUrl = string.Format(hfImageUrl.Value, "");
                imageList.Add(imageUrl.Replace(tempFile, ""));
                ImageUrl = imageUrl.Replace(tempFile, ImageFile);
            }
            else
            {
                ImageUrl = this.hfImageUrl.Value;
            }

            YSWL.MALL.Model.Shop.Trial.TrialReports model = new YSWL.MALL.Model.Shop.Trial.TrialReports();
            model.ReportId = ReportId;
            model.Title = Title;
            model.LinkUrl = LinkUrl;
            model.ShortDescription = ShortDescription;
            model.CreatedUserName = CreatedUserName;
            model.ImageUrl = ImageUrl;

            YSWL.MALL.BLL.Shop.Trial.TrialReports bll = new YSWL.MALL.BLL.Shop.Trial.TrialReports();
            if (bll.Update(model))
            {
                if (!string.IsNullOrWhiteSpace(hfIsModifyImage.Value))
                {
                    //将图片从临时文件夹移动到正式的文件夹下
                    Common.FileManage.MoveFile(Server.MapPath(tempFile), Server.MapPath(ImageFile), imageList);
                    if (!string.IsNullOrWhiteSpace(this.hfOldImage.Value))
                    {
                        FileManage.DeleteFile(Server.MapPath(this.hfOldImage.Value));
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(this.hfIsModifyImage.Value))
                    {
                        //删除文件
                        Common.FileManage.DeleteFile(Server.MapPath(this.hfOldImage.Value));
                    }
                }
                YSWL.Common.MessageBox.ShowSuccessTip(this, "保存成功！", "list.aspx");
            }
            YSWL.Common.MessageBox.ShowSuccessTip(this, "保存失败！ 请稍后再试.");

        }


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
