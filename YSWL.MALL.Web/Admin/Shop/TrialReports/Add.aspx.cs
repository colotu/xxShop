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
namespace YSWL.MALL.Web.Admin.Shop.TrialReports
{
    public partial class Add : PageBaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
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
            string Title = this.txtTitle.Text;
            string LinkUrl = this.txtLinkUrl.Text;
            string ShortDescription = this.txtShortDescription.Text;
            string CreatedUserName = this.txtCreatedUserName.Text;
            string ImageUrl = string.Empty;

            //待上传的图片名称
            string tempFile = string.Format("/Upload/Temp/{0}", DateTime.Now.ToString("yyyyMMdd"));
            string ImageFile = "/Upload/Shop/TrialReports";
            System.Collections.ArrayList imageList = new System.Collections.ArrayList();
            if (!string.IsNullOrWhiteSpace(this.hfImageUrl.Value))
            {
                string imageUrl = string.Format(this.hfImageUrl.Value, "");

                imageList.Add(imageUrl.Replace(tempFile, ""));

                ImageUrl = imageUrl.Replace(tempFile, ImageFile);
            }

            YSWL.MALL.Model.Shop.Trial.TrialReports model = new YSWL.MALL.Model.Shop.Trial.TrialReports();
            model.Title = Title;
            model.LinkUrl = LinkUrl;
            model.ShortDescription = ShortDescription;
            model.CreatedUserName = CreatedUserName;
            model.ImageUrl = ImageUrl;

            YSWL.MALL.BLL.Shop.Trial.TrialReports bll = new YSWL.MALL.BLL.Shop.Trial.TrialReports();
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
