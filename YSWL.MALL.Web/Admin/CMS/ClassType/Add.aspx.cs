using System;
using YSWL.Common;

namespace YSWL.MALL.Web.CMS.ClassType
{
    public partial class Add : PageBaseAdmin
    {
        private YSWL.MALL.BLL.CMS.ClassType bll = new YSWL.MALL.BLL.CMS.ClassType();
        protected override int Act_PageLoad { get { return 194; } } //CMS_类型管理_新增页面
        
        protected void Page_Load(object sender, EventArgs e)
        {
       
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtClassTypeName.Text))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, Resources.CMS.ClassErrorCLassNameNull);
                return;
            }
            YSWL.MALL.Model.CMS.ClassType model = new YSWL.MALL.Model.CMS.ClassType();
            model.ClassTypeName = this.txtClassTypeName.Text;
            if (bll.Add(model))
            {
                YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.TooltipSaveOK, "List.aspx");
            }
            else
            {
                YSWL.Common.MessageBox.ShowFailTip(this, Resources.Site.TooltipSaveError);
            }
        }

        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("List.aspx");
        }
    }
}