/**
* Add.cs
*
* 功 能： [N/A]
* 类 名： Add.cs
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01   
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
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
using YSWL.Web;

namespace  YSWL.Web.Admin.Shop.Sample
{
    public partial class SampleModify: PageBaseAdmin
    {
        private  int RestrictPhotoSize = 10240000;
        private string SavePath = "/Upload/Shop/Files/";
        private string BigImageSize = "800X800";
        private string SmallImageSize = "400X400";
        YSWL.BLL.Shop.Sample.Sample bll = new YSWL.BLL.Shop.Sample.Sample();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_AddData)))
                {
                    MessageBox.ShowFailTip(this,"您没有此权限");
                    return;
                }
                BindToData();
               
            }
        }

        public int SampleId
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

        private void BindToData()
        {
            YSWL.Model.Shop.Sample.Sample model=bll.GetModel(SampleId);
            txtTiltle.Text = model.Tiltle;
            txtSequence.Text = model.Sequence.ToString();
            txtUrlRule.Text = model.SeoUrl;
            txtSeoImageAlt.Text = model.SeoImageAlt;
            txtSeoImageTitle.Text = model.SeoImageTitle;
            txtMeta_Description.Text = model.Meta_Description;
            txtMeta_Keywords.Text = model.Meta_KeyWords;
            txtMeta_Title.Text = model.Meta_Title;
            chkStatus.Checked = model.Status == 1 ? true : false;

        }
        protected void btnSave_Click(object sender, EventArgs e)
		{
            YSWL.Model.Shop.Sample.Sample model =bll.GetModel(SampleId);
            string Tiltle = this.txtTiltle.Text.Trim();
            if (Tiltle.Length > 50)
			{
                MessageBox.ShowServerBusyTip(this, "名称不能超过50个字符！");
                return;
			}
            if (!Common.PageValidate.IsNumber(txtSequence.Text.Trim()))
            {
                 MessageBox.ShowServerBusyTip(this, "显示顺序必须是数字，1表示显示在最前面！");
                return;
            }
            #region 图片封面

            if (uploadJPGCover.HasFile)
            {
                string ElecImageFileName = uploadJPGCover.PostedFile.FileName;
                int filelength = uploadJPGCover.PostedFile.ContentLength;
                if (filelength > RestrictPhotoSize)
                {
                    Common.MessageBox.ShowSuccessTip(this, "您上传的图片过大，请上传较小的文件");
                    return;
                }
                string ElecCoverImageUrl = SavePath + ElecImageFileName;
                string NormalElecCoverImageUrl = "";
                string ThumblElecCoverImageUrl = "";
                if (
                    !ImageCut(ElecImageFileName, SavePath, SmallImageSize, BigImageSize, out ThumblElecCoverImageUrl,
                              out NormalElecCoverImageUrl))
                {
                    Common.MessageBox.ShowSuccessTip(this, "出现异常，请重试");
                    return;
                }
                model.ElecCoverImageUrl = ElecCoverImageUrl;
                model.NormalElecCoverImageUrl = NormalElecCoverImageUrl;
                model.ThumblElecCoverImageUrl = ThumblElecCoverImageUrl;
            }

            #endregion
            #region PDF封面
            if (uploadPDFCover.HasFile)
            {
                string PDFImageFileName = uploadPDFCover.PostedFile.FileName;
                int  filelength = uploadPDFCover.PostedFile.ContentLength;
                if (filelength > RestrictPhotoSize)
                {
                    Common.MessageBox.ShowSuccessTip(this, "您上传的图片过大，请上传较小的文件");
                    return;
                }
                string PdfCoverImageUrl = SavePath + PDFImageFileName;
                string NormalPdfCoverImageUrl = "";
                string ThumblPdfCoverImageUrl = "";
                if (
                    !ImageCut(PDFImageFileName, SavePath, SmallImageSize, BigImageSize, out ThumblPdfCoverImageUrl,
                              out NormalPdfCoverImageUrl))
                {
                    Common.MessageBox.ShowSuccessTip(this, "出现异常，请重试");
                    return;
                }
                model.PdfCoverImageUrl = PdfCoverImageUrl;
                model.NormalPdfCoverImageUrl = NormalPdfCoverImageUrl;
                model.ThumbPdfCoverImageUrl = ThumblPdfCoverImageUrl;
            }

            #endregion
            model.Tiltle = Tiltle;
            model.Status = chkStatus.Checked ? 1 : 0;
            model.Sequence = Common.Globals.SafeInt(txtSequence.Text, 0);
            model.SeoImageAlt = txtSeoImageAlt.Text;
            model.SeoImageTitle = txtSeoImageTitle.Text;
            model.SeoUrl = txtUrlRule.Text;
            model.Meta_Description = txtMeta_Description.Text;
            model.Meta_KeyWords = txtMeta_Keywords.Text;
            model.Meta_Title = txtMeta_Title.Text;
            model.CreatedDate = DateTime.Now;
            if (bll.Update(model))
            {
                LogHelp.AddUserLog(CurrentUser.UserName, CurrentUser.UserType, "更新电子样本(id=" +model.SampleId + ")成功", this);
                MessageBox.ShowSuccessTip(this, Resources.Site.TooltipSaveOK, "SampleList.aspx");
                Response.Redirect("SampleList.aspx");
            }
            else
            {
                LogHelp.AddUserLog(CurrentUser.UserName, CurrentUser.UserType, "更新电子样本(id=" + model.SampleId + ")失败", this);
                MessageBox.ShowServerBusyTip(this, Resources.Site.TooltipSaveError);
            }
		}


        /// <summary>
        /// 图片的裁剪
        /// </summary>
        /// <param name="imgname">图片的名字</param>
        /// <param name="uploadpath">存放的位置</param>
        /// <param name="SmallImageSize">小图的大小 长X宽的形式</param>
        /// <param name="BigImageSize">大图的大小 长X宽的形式</param>
        /// <param name="SmallImagePath">out 小图的保存的位置</param>
        /// <param name="BigImagePath">out 大图保存的位置</param>
        /// <returns></returns>
        private bool ImageCut(string imgname, string uploadpath, string SmallImageSize, string BigImageSize,out string SmallImagePath,out string BigImagePath)
        {
            try
            {

                //生成小图
                string SthumbImage = "S_" + imgname;
                string SthumbImagePath = HttpContext.Current.Server.MapPath(uploadpath + SthumbImage);
                int SWindthInt = 400;
                int SHeightInt = 400;
                if (SmallImageSize != null && SmallImageSize.Split('X').Length > 1)
                {
                    string[] Size = SmallImageSize.Split('X');
                    SWindthInt = Common.Globals.SafeInt(Size[0], 400);
                    SHeightInt = Common.Globals.SafeInt(Size[1], 400);

                }
                ImageTools.MakeThumbnail(HttpContext.Current.Server.MapPath(uploadpath + imgname), SthumbImagePath, SWindthInt, SHeightInt, MakeThumbnailMode.W);

                ///生成大图
                string BthumbImage = "B_" + imgname;
                string BthumbImagePath = HttpContext.Current.Server.MapPath(uploadpath + BthumbImage);

                int BWindthInt = 800;
                int BHeightInt = 800;
                if (BigImageSize != null && BigImageSize.Split('X').Length > 1)
                {
                    string[] Size = BigImageSize.Split('X');
                    BWindthInt = Common.Globals.SafeInt(Size[0], 800);
                    BHeightInt = Common.Globals.SafeInt(Size[1], 800);

                }
                ImageTools.MakeThumbnail(HttpContext.Current.Server.MapPath(uploadpath + imgname), BthumbImagePath, BWindthInt, BHeightInt, MakeThumbnailMode.W);
                SmallImagePath = uploadpath + SthumbImage;
                BigImagePath = uploadpath + BthumbImage;
                return true;

            }
            catch (Exception)
            {
                SmallImagePath = "";
                BigImagePath = "";
                return false;
            }
             

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("SampleList.aspx");
        }


    
    }
}
