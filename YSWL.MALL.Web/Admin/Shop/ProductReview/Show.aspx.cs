/**
* Show.cs
*
* 功 能： [N/A]
* 类 名： Show.cs
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
using System.Web.UI;

namespace YSWL.MALL.Web.Admin.Shop.ProductReview
{
    public partial class Show : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 468; } } //Shop_商品评论管理_详细页
        public string strid = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (ReviewId > 0)
                {
                    ShowInfo(ReviewId);
                }
            }
        }

        public int ReviewId
        {
            get
            {
                int reviewid = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["id"]))
                {
                    reviewid = Common.Globals.SafeInt(Request.Params["id"], 0);
                }
                return reviewid;
            }
        }

        private void ShowInfo(int ReviewId)
        {
            YSWL.MALL.BLL.Shop.Products.ProductReviews bll = new YSWL.MALL.BLL.Shop.Products.ProductReviews();
            YSWL.MALL.Model.Shop.Products.ProductReviews model = bll.GetModel(ReviewId);
            if (null != model)
            {
                this.lblReviewId.Text = model.ReviewId.ToString();
                this.lblProductId.Text = new BLL.Shop.Products.ProductInfo().GetProductName(model.ProductId);
                this.lblUserId.Text = model.UserId.ToString();
                this.lblReviewText.Text = Common.Globals.HtmlDecode(model.ReviewText);
                this.lblUserName.Text = model.UserName;
                this.lblUserEmail.Text = model.UserEmail;
                this.lblCreatedDate.Text = model.CreatedDate.ToString();
                this.lblParentId.Text = model.ParentId.ToString();
                this.hidImagesNames.Value=model.ImagesNames;
               this.hidImagesPath.Value= model.ImagesPath;
                if (model.Status == 0)
                {
                    this.lblState.Text = "未审核";
                }
                else if (model.Status == 1)
                {
                    this.lblState.Text = "已审核";
                }
                else
                {
                    this.lblState.Text = "审核失败";
                }
                this.lblScore.Text = new BLL.Shop.Products.ScoreDetails().GetScore(ReviewId).ToString();
            }
        }
        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}