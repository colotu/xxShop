/**
* Modify.cs
*
* 功 能： [N/A]
* 类 名： Modify.cs
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.AD.Advertisement
{
    public partial class Modify : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 372; } } //网站管理_广告内容管理_编辑页
        public int AdvertisementId
        {
            get
            {
                int id = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["id"]))
                {
                    id = Globals.SafeInt(Request.Params["id"], 0);
                }
                return id;
            }
        }

        public int AdPositionID
        {
            get
            {
                int id = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["Adid"]))
                {
                    id = Globals.SafeInt(Request.Params["Adid"], 0);
                }
                return id;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                AltInfo();
                ShowInfo(AdvertisementId);
            }
        }

        private void AltInfo()
        {
            YSWL.MALL.BLL.Settings.AdvertisePosition bllPosition = new BLL.Settings.AdvertisePosition();
            YSWL.MALL.Model.Settings.AdvertisePosition modelPosition = bllPosition.GetModel(AdPositionID);
            if (modelPosition != null)
            {
                this.Literal2.Text = modelPosition.AdvPositionName + "】广告位修改广告内容";
            }
        }

        private void ShowInfo(int AdvertisementId)
        {
            YSWL.MALL.BLL.Settings.Advertisement bll = new YSWL.MALL.BLL.Settings.Advertisement();
            YSWL.MALL.Model.Settings.Advertisement model = bll.GetModel(AdvertisementId);
            this.txtAdvertisementName.Text = model.AdvertisementName;
            switch (model.ContentType.Value)
            {
                case 0:
                    this.rbTextContent.Checked = true;
                    this.imgShow.Visible = false;
                    this.FlashPlay.Visible = false;
                    break;

                case 1:
                    this.rbImgContent.Checked = true;
                    this.hfFileUrl.Value = model.FileUrl;
                    this.imgShow.Visible = true;
                    this.FlashPlay.Visible = false;
                    break;

                case 2:
                    this.rbFlashContent.Checked = true;
                    this.hfSwfUrl.Value = model.FileUrl;
                    this.imgShow.Visible = false;
                    this.FlashPlay.Visible = true;
                    this.litVideo.Text = "<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,28,0\" width=\"200\" height=\"170\"  ></a></li><param name=\"wmode\" value=\"opaque\" /><param name=\"quality\" value=\"high\" /><param name=\"movie\" value=\"" + model.FileUrl + "\" /><embed src=\"" + model.FileUrl + "\" allowfullscreen=\"true\" quality=\"high\" width=\"200\" height=\"170\"\" align=\"middle\" wmode=\"transparent\" allowscriptaccess=\"always\" type=\"application/x-shockwave-flash\"></embed></object></td></tr>";
                    break;

                case 3:
                    this.rbCodeContent.Checked = true;
                    this.imgShow.Visible = false;
                    this.FlashPlay.Visible = false;
                    break;
                default:
                    break;
            }
            this.txtAdvHtml.Text = model.AdvHtml;
            this.txtAlternateText.Text = model.AlternateText;
            this.txtCPMPrice.Text = model.CPMPrice.Value.ToString("0.00");
            this.txtDayMaxIP.Text = model.DayMaxIP.ToString();
            this.txtDayMaxPV.Text = model.DayMaxPV.ToString();
            this.txtEndDate.Text = model.EndDate.HasValue ? model.EndDate.Value.ToString("yyyy-MM-dd") : "";
            this.imgAd.ImageUrl = model.FileUrl;
            BLL.Ms.Enterprise bllEn = new BLL.Ms.Enterprise();
            Model.Ms.Enterprise modelEn = bllEn.GetModel(model.EnterpriseID.Value);
            if (modelEn != null)
            {
                this.txtEnterpriseID.Text = modelEn.Name;
            }
            else
            {
                this.txtEnterpriseID.Text = "";
            }
            this.txtImpressions.Text = model.Impressions.Value.ToString();
            this.txtNavigateUrl.Text = model.NavigateUrl;
            this.txtStartDate.Text = model.StartDate.HasValue ? model.StartDate.Value.ToString("yyyy-MM-dd") : "";
            if (model.AutoStop.Value.Equals(0))
            {
                this.rbNoStup.Checked = true;
            }
            else if (model.AutoStop.Value.Equals(1))
            {
                this.rbAutoStop.Checked = true;
            }
            else
            {
                this.rbNoLimit.Checked = true;
            }
            switch (model.State.Value)
            {
                case 0:
                    this.rbStatusN.Checked = true;
                    break;

                case 1:
                    this.rbStatusY.Checked = true;
                    break;

                case -1:
                    this.rbStop.Checked = true;
                    break;
                default:
                    break;
            }
        }

        public void btnSave_Click(object sender, EventArgs e)
        {
            YSWL.MALL.BLL.Settings.Advertisement bll = new YSWL.MALL.BLL.Settings.Advertisement();
            YSWL.MALL.Model.Settings.Advertisement model = bll.GetModel(AdvertisementId);

            model.AdvertisementName = this.txtAdvertisementName.Text;
            string strAdType = string.Empty;
            if (this.rbTextContent.Checked)
            {
                strAdType = "0";
            }
            else if (this.rbImgContent.Checked)
            {
                strAdType = "1";
            }
            else if (rbFlashContent.Checked)
            {
                strAdType = "2";
            }
            else
            {
                strAdType = "3";
            }

            //if (strAdType.Equals("0"))
            //{
            //    if (string.IsNullOrWhiteSpace(this.txtAlternateText.Text))
            //    {
            //        YSWL.Common.MessageBox.ShowFailTip(this, "广告语不能为空！");
            //        return;
            //    }
            //}

            //待上传的图片名称
            string tempFile = string.Format("/Upload/Temp/{0}", DateTime.Now.ToString("yyyyMMdd"));
            string ImageFile = string.Format("/Upload/AD/{0}", AdPositionID);
            ArrayList imageList = new ArrayList();
            if (strAdType.Equals("1"))
            {
                if (string.IsNullOrWhiteSpace(this.hfFileUrl.Value))
                {
                    YSWL.Common.MessageBox.ShowFailTip(this, "请选择要上传的图片！");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(HiddenField_ISModifyImage.Value))
                {
                    string imageUrl = string.Format(hfFileUrl.Value, "");
                    imageList.Add(imageUrl.Replace(tempFile, ""));
                    model.FileUrl = imageUrl.Replace(tempFile, ImageFile);
                }
                else
                {
                    model.FileUrl = this.hfFileUrl.Value;
                }
            }

            //if (strAdType.Equals("0")||strAdType.Equals("1"))
            //{
            //    if (!string.IsNullOrWhiteSpace(this.txtNavigateUrl.Text))
            //    {
            //        YSWL.Common.MessageBox.ResponseScript(this, "clickautohide(5, \"连接地址不能为空！\", 2000);");
            //        return;
            //    }
            //}

            if (strAdType.Equals("2"))
            {
                //上传Flash
                model.FileUrl = this.hfSwfUrl.Value;
            }
            if (strAdType.Equals("3"))
            {
                if (string.IsNullOrWhiteSpace(this.txtAdvHtml.Text))
                {
                    YSWL.Common.MessageBox.ShowFailTip(this, "广告HTML代码不能为空！");
                    return;
                }
                else
                {
                    model.AdvHtml = this.txtAdvHtml.Text;
                }
            }
            model.ContentType = int.Parse(strAdType);
            model.AlternateText = this.txtAlternateText.Text;
            model.NavigateUrl = this.txtNavigateUrl.Text;

            if (!PageValidate.IsNumber(this.txtImpressions.Text))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "显示频率格式不正确！");
                return;
            }
            model.Impressions = int.Parse(this.txtImpressions.Text);
            if (this.rbStatusY.Checked)
            {
                model.State = 1;
            }
            else if (this.rbStatusN.Checked)
            {
                model.State = 0;
            }
            else
            {
                model.State = -1;
            }
            if (!string.IsNullOrWhiteSpace(this.txtStartDate.Text))
            {
                if (!PageValidate.IsDateTime(this.txtStartDate.Text))
                {
                    YSWL.Common.MessageBox.ShowFailTip(this, "请输入正确的开始时间！");
                    return;
                }
                else
                {
                    model.StartDate = DateTime.Parse(this.txtStartDate.Text);
                }
            }
            if (!string.IsNullOrWhiteSpace(this.txtEndDate.Text))
            {
                if (!PageValidate.IsDateTime(this.txtEndDate.Text))
                {
                    YSWL.Common.MessageBox.ShowFailTip(this, "请输入正确的结束时间！");
                    return;
                }
                else
                {
                    model.EndDate = DateTime.Parse(this.txtEndDate.Text);
                }
            }
            if (!PageValidate.IsNumber(txtDayMaxPV.Text))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "最大PV格式不正确！");
                return;
            }
            model.DayMaxPV = int.Parse(this.txtDayMaxPV.Text);
            if (!PageValidate.IsNumber(txtDayMaxIP.Text))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "最大IP格式不正确！");
                return;
            }
            model.DayMaxIP = int.Parse(this.txtDayMaxIP.Text);

            if (string.IsNullOrWhiteSpace(this.txtCPMPrice.Text))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "请输入正确的价格！");
                return;
            }
            decimal CPMPrice = 0M;
            if (!decimal.TryParse(this.txtCPMPrice.Text, out CPMPrice))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "价格格式不正确！");
                return;
            }

            model.CPMPrice = CPMPrice;
            if (this.rbAutoStop.Checked)
            {
                model.AutoStop = 1;
            }
            else if (this.rbNoStup.Checked)
            {
                model.AutoStop = 0;
            }
            else
            {
                model.AutoStop = -1;
            }

            if (!string.IsNullOrWhiteSpace(this.txtEnterpriseID.Text))
            {
                string enterpriseName = this.txtEnterpriseID.Text;
                BLL.Ms.Enterprise enteBLL = new BLL.Ms.Enterprise();
                if (!string.IsNullOrWhiteSpace(enterpriseName))
                {
                    List<Model.Ms.Enterprise> list = enteBLL.GetModelByEnterpriseName(enterpriseName);
                    if (list.Count > 0)
                    { model.EnterpriseID = list[0].EnterpriseID; }
                    else
                    {
                        YSWL.Common.MessageBox.ShowFailTip(this, "没有找到相应商户，请重新输入！");
                        return;
                    }
                }
                else
                {
                    model.EnterpriseID = -1;
                }
            }
            else
            {
                model.EnterpriseID = -1;
            }

            if (bll.Update(model))
            {
                string url = string.Format("SingleList.aspx?id={0}", AdPositionID);
                this.btnCancle.Enabled = false;
                this.btnSave.Enabled = false;

                if (!string.IsNullOrWhiteSpace(HiddenField_ISModifyImage.Value))
                {
                    //将图片从临时文件夹移动到正式的文件夹下
                    Common.FileManage.MoveFile(Server.MapPath(tempFile), Server.MapPath(ImageFile), imageList);
                }
                //这部分没有办法清除某一部分缓存，只能清除全部缓存
                IDictionaryEnumerator de = Cache.GetEnumerator();
                ArrayList list = new ArrayList();
                while (de.MoveNext())
                {
                    list.Add(de.Key.ToString());
                }
                foreach (string key in list)
                {
                    Cache.Remove(key);
                }
                YSWL.Common.MessageBox.ShowSuccessTip(this, "保存成功", url);
            }
            else
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "网络异常，请稍后再试！");
                return;
            }
        }

        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}