using System;
namespace YSWL.MALL.Web.Forms
{
    public partial class Add : PageBaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Master.TabTitle = "信息新增，请详细填写下列信息";
        }
        protected override int Act_PageLoad { get { return 354; } } //客服管理_问卷管理_新增页
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtName.Text))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, Resources.Poll.ErrorFormsNameNull);
                return;
            }
            if (string.IsNullOrWhiteSpace(this.txtDescription.Text))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, Resources.Poll.ErrorFormsExplainNull);
                return;
            }
            string Name = this.txtName.Text;
            string Description = this.txtDescription.Text;

            YSWL.MALL.Model.Poll.Forms model = new YSWL.MALL.Model.Poll.Forms();
            model.Name = Name;
            model.Description = Description;

            YSWL.MALL.BLL.Poll.Forms bll = new YSWL.MALL.BLL.Poll.Forms();
            int id = bll.Add(model);
            if (id > 0)
            {
                Response.Redirect("../Topics/Index.aspx?fid=" + id.ToString());
            }
            else
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "保存失败！");
            }
        }

    }
}
