using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Shop.ProductQA
{
    public partial class ShowDetail : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 463; } } //Shop_商品疑问管理_详细页

        YSWL.MALL.BLL.Shop.Products.ProductQA QAbll = new BLL.Shop.Products.ProductQA();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                YSWL.MALL.Model.Shop.Products.ProductQA QAModel = QAbll.GetModel(QAId);
                if (QAModel == null)
                {
                    Response.Redirect("ProductQAList.aspx");
                }
                this.lbQuestion.Text = YSWL.Common.Globals.HtmlDecode(QAModel.Question);
                this.lbUserName.Text = QAModel.UserName;
                this.lbCreatedDate.Text = QAModel.CreatedDate.ToString();
                this.lbReply.Text =YSWL.Common.Globals.HtmlDecode( QAModel.ReplyContent);
                this.lbReplyDate.Text = QAModel.ReplyDate.ToString();
                this.lbReplyName.Text = QAModel.ReplyUserName;
                this.lbState.Text = GetState(QAModel.State);
            }
        }

        public int QAId
        {
            get
            {
                int qaid = 0;
                if (Request.Params["qaid"] != null && PageValidate.IsNumber(Request.Params["qaid"]))
                {
                    qaid = int.Parse(Request.Params["qaid"]);
                }
                else
                {
                    Response.Redirect("ProductQAList.aspx");
                }
                return qaid;
            }
        }
        public string GetState(int state)
        {
            switch (state)
            {
                case 0:
                    return "未审核";
                case 1:
                    return "审核通过";
                case 2:
                    return "未审核通过";
                default:
                    return "";
            }
        }
        public void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProductQAList.aspx");
        }
    }
}
