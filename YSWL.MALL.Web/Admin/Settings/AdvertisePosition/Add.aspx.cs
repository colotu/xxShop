/**
* Add.cs
*
* 功 能： [N/A]
* 类 名： Add.cs
*
* Ver    变更日期                             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012年5月31日 14:38:42   孙鹏   创建
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.AdvertisePosition
{
    public partial class Add : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 366; } } //网站管理_广告管理_新增页
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            YSWL.MALL.Model.Settings.AdvertisePosition model = new YSWL.MALL.Model.Settings.AdvertisePosition();
            if (this.txtAdvPositionName.Text.Trim().Length == 0)
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "广告位名称不能为空！");
                return;
            }
            string strShowType = this.ddlShowType.SelectedValue;
            if (strShowType == "1")
            {
                if (!PageValidate.IsNumber(txtRepeatColumns.Text))
                {
                    YSWL.Common.MessageBox.ShowFailTip(this, "请数如正确的横向平铺时行显示个数！");
                    return;
                }
                else
                {
                    model.RepeatColumns = Globals.SafeInt(this.txtRepeatColumns.Text, 0);
                }
            }
            if (strShowType == "4")
            {
                if (string.IsNullOrWhiteSpace(this.txtAdvHtml.Text))
                {
                    YSWL.Common.MessageBox.ShowFailTip(this, "请数广告位内容！");
                    return;
                }
                else
                {
                    model.AdvHtml = this.txtAdvHtml.Text.Trim();
                }
            }

            model.Width = Globals.SafeInt(this.txtWidth.Text, 0);
            model.Height = Globals.SafeInt(this.txtHeight.Text, 0);

            if (this.chkIsOne.Checked)
            {
                if (!PageValidate.IsNumber(txtTimeInterval.Text))
                {
                    YSWL.Common.MessageBox.ShowFailTip(this, "请输入正确的循环广告时间间隔！");
                    return;
                }
                else
                {
                    model.IsOne = true;
                    model.TimeInterval = Globals.SafeInt(this.txtTimeInterval.Text, 0);
                }
            }
            else
            {
                model.IsOne = false;
            }
            model.ShowType = Globals.SafeInt(strShowType, -1);
            model.AdvPositionName = this.txtAdvPositionName.Text.Trim();
            model.CreatedDate = DateTime.Now;
            model.CreatedUserID = CurrentUser.UserID;

            YSWL.MALL.BLL.Settings.AdvertisePosition bll = new YSWL.MALL.BLL.Settings.AdvertisePosition();
            int result = 0;
            if (strShowType == "4")
            {
                result = bll.Add(model);
                YSWL.MALL.BLL.Settings.Advertisement advbll = new YSWL.MALL.BLL.Settings.Advertisement();

                YSWL.MALL.Model.Settings.Advertisement modelContent = new YSWL.MALL.Model.Settings.Advertisement();
                modelContent.AdvertisementName = "自定义广告代码";
                modelContent.ContentType = 3;
                modelContent.AdvPositionId = result;
                modelContent.CreatedDate = DateTime.Now;
                advbll.Add(modelContent);
            }
            else
            {
                result = bll.Add(model);
            }
            if (result > 0)
            {
                YSWL.Common.MessageBox.ResponseScript(this, "parent.location.href='List.aspx'");
            }
            else
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "网络异常，请稍后再试");
                return;
            }
        }

        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}