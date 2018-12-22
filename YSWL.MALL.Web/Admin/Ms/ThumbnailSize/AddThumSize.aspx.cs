using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Ms.ThumbnailSize
{
    public partial class AddThumSize : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 339; } } //设置_缩略图尺寸管理_新增页
        private YSWL.MALL.BLL.Ms.ThumbnailSize thumBll=new BLL.Ms.ThumbnailSize();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
             
            }
        }

        public void btnSave_Click(object sender, System.EventArgs e)
        {

            if (thumBll.Exists(this.tName.Text.Trim()))//,ddlType.AreaInt,
            {
                YSWL.Common.MessageBox.ShowSuccessTip(this, "已存在该缩略图尺寸名称，请重新填写");
            }
            else
            {
              YSWL.MALL.Model.Ms.ThumbnailSize model=new Model.Ms.ThumbnailSize();
                model.Type = ddlType.AreaInt;
                model.ThumName = this.tName.Text.Trim();
                model.ThumWidth = Common.Globals.SafeInt(this.tWidth.Text, 1);
                model.ThumHeight = Common.Globals.SafeInt(this.tHeight.Text, 1);
                model.Remark = this.tDesc.Text.Trim();
                if (thumBll.Add(model))
                {
                    Response.Redirect("ThumSizeList.aspx");
                }
                else
                {
                    MessageBox.ShowFailTip(this, "新增失败！请重试。");
                }
            }
        }

        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("ThumSizeList.aspx");
        }

    }
}
