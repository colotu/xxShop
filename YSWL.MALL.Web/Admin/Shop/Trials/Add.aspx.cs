/**
* Add.cs
*
* 功 能： N/A
* 类 名： Add
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
    public partial class Add : PageBaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
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
            int CategoryId = -1;
            string TrialName = this.txtTrialName.Text;
            int EnterpriseId = -1;
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
            if (!string.IsNullOrWhiteSpace(this.hfImageUrl.Value))
            {
                string imageUrl = string.Format(this.hfImageUrl.Value, "");

                imageList.Add(imageUrl.Replace(tempFile, ""));

                ImageUrl = imageUrl.Replace(tempFile, ImageFile);
            }

            YSWL.MALL.Model.Shop.Trial.Trials model = new YSWL.MALL.Model.Shop.Trial.Trials();
            model.CategoryId = CategoryId;
            model.TrialName = TrialName;
            model.EnterpriseId = EnterpriseId;
            model.RegionId = -1;
            model.ShortDescription = ShortDescription;
            model.Description = Description;
            model.LinklUrl = LinklUrl;
            model.TrialStatus = TrialStatus;
            model.StartDate = StartDate;
            model.EndDate = EndDate.AddHours(23).AddMinutes(59).AddSeconds(59);
            model.CreatedDate = DateTime.Now;
            model.CreatedUserID = CurrentUser.UserID;
            model.VistiCounts = 0;
            model.TrialCounts = TrialCounts;
            model.DisplaySequence = DisplaySequence;
            model.MarketPrice = MarketPrice;
            model.LowestSalePrice = 0;
            model.Points = 0;
            model.ImageUrl = ImageUrl;
            //model.ThumbnailUrl = ThumbnailUrl;

            YSWL.MALL.BLL.Shop.Trial.Trials bll = new YSWL.MALL.BLL.Shop.Trial.Trials();
            if (bll.Add(model) > 0)
            {
                if (!string.IsNullOrWhiteSpace(this.hfImageUrl.Value))
                {
                    //将图片从临时文件夹移动到正式的文件夹下
                    Common.FileManage.MoveFile(Server.MapPath(tempFile), Server.MapPath(ImageFile), imageList);
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
