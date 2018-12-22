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
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Shop.Trials
{
    public partial class Modify : PageBaseAdmin
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.Params["id"] != null && Request.Params["id"].Trim() != "")
                {
                    int TrialId = (Convert.ToInt32(Request.Params["id"]));
                    ShowInfo(TrialId);
                }
            }
        }

        private void ShowInfo(int TrialId)
        {
            YSWL.MALL.BLL.Shop.Trial.Trials bll = new YSWL.MALL.BLL.Shop.Trial.Trials();
            YSWL.MALL.Model.Shop.Trial.Trials model = bll.GetModel(TrialId);
            this.lblTrialId.Text = model.TrialId.ToString();
            this.txtTrialName.Text = model.TrialName;
            this.txtShortDescription.Text = model.ShortDescription;
            this.txtDescription.Text = model.Description;
            this.txtLinklUrl.Text = model.LinklUrl;
            this.dropTrialStatus.SelectedValue = model.TrialStatus.ToString();
            this.txtStartDate.Text = model.StartDate.ToString("yyyy-MM-dd");
            this.txtEndDate.Text = model.EndDate.ToString("yyyy-MM-dd");
            this.txtTrialCounts.Text = model.TrialCounts.ToString();
            this.txtDisplaySequence.Text = model.DisplaySequence.ToString();
            this.txtMarketPrice.Text = model.MarketPrice.ToString();
            this.hfImageUrl.Value = model.ImageUrl;
            this.hfOldImage.Value = model.ImageUrl;
        }

        public void btnSave_Click(object sender, EventArgs e)
        {

            string strErr = "";
            if (this.txtTrialName.Text.Trim().Length == 0)
            {
                strErr += "名称不能为空！\\n";
            }
            if (this.txtShortDescription.Text.Trim().Length == 0)
            {
                strErr += "介绍不能为空！\\n";
            }
            if (this.txtDescription.Text.Trim().Length == 0)
            {
                strErr += "描述不能为空！\\n";
            }
            if (this.txtLinklUrl.Text.Trim().Length == 0)
            {
                strErr += "试用链接不能为空！\\n";
            }
            if (!PageValidate.IsDateTime(txtStartDate.Text))
            {
                strErr += "开始时间格式错误！\\n";
            }
            if (!PageValidate.IsDateTime(txtEndDate.Text))
            {
                strErr += "结束时间格式错误！\\n";
            }
            if (!PageValidate.IsNumber(txtTrialCounts.Text))
            {
                strErr += "试用总数格式错误！\\n";
            }
            if (!PageValidate.IsNumber(txtDisplaySequence.Text))
            {
                strErr += "显示顺序格式错误！\\n";
            }
            if (!PageValidate.IsDecimal(txtMarketPrice.Text))
            {
                strErr += "市场价格式错误！\\n";
            }

            if (strErr != "")
            {
                MessageBox.Show(this, strErr);
                return;
            }
            int TrialId = int.Parse(this.lblTrialId.Text);
            string TrialName = this.txtTrialName.Text;
            string ShortDescription = this.txtShortDescription.Text;
            string Description = this.txtDescription.Text;
            string LinklUrl = this.txtLinklUrl.Text;
            int TrialStatus = Globals.SafeInt(dropTrialStatus.SelectedValue, -1);
            DateTime StartDate = DateTime.Parse(this.txtStartDate.Text);
            DateTime EndDate = DateTime.Parse(this.txtEndDate.Text);
            int TrialCounts = int.Parse(this.txtTrialCounts.Text);
            int DisplaySequence = int.Parse(this.txtDisplaySequence.Text);
            decimal MarketPrice = decimal.Parse(this.txtMarketPrice.Text);
            string ImageUrl = string.Empty;

            //待上传的图片名称
            string tempFile = string.Format("/Upload/Temp/{0}", DateTime.Now.ToString("yyyyMMdd"));
            string ImageFile = "/Upload/Shop/Trials";
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

            YSWL.MALL.BLL.Shop.Trial.Trials bll = new YSWL.MALL.BLL.Shop.Trial.Trials();
            YSWL.MALL.Model.Shop.Trial.Trials model = bll.GetModel(TrialId);
            model.TrialId = TrialId;
            model.TrialName = TrialName;
            model.ShortDescription = ShortDescription;
            model.Description = Description;
            model.LinklUrl = LinklUrl;
            model.TrialStatus = TrialStatus;
            model.StartDate = StartDate;
            model.EndDate = EndDate.AddHours(23).AddMinutes(59).AddSeconds(59);
            model.TrialCounts = TrialCounts;
            model.DisplaySequence = DisplaySequence;
            model.MarketPrice = MarketPrice;
            model.ImageUrl = ImageUrl;

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
