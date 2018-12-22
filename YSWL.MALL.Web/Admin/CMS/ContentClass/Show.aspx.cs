using System;
using System.Web.UI;
using YSWL.Common;
namespace YSWL.MALL.Web.CMS.ContentClass
{
    public partial class Show : PageBaseAdmin
    {
        YSWL.MALL.BLL.CMS.ClassType bllClassType = new YSWL.MALL.BLL.CMS.ClassType();
        YSWL.MALL.BLL.CMS.ContentClass bllContentClass = new YSWL.MALL.BLL.CMS.ContentClass();
        public string strid = "";
        protected override int Act_PageLoad { get { return 223; } }//CMS_栏目管理_详细页
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (ClassID > 0)
                {
                    ShowInfo();
                }
                else
                {
                    MessageBox.ShowServerBusyTip(this, Resources.CMS.ContentErrorNoContent, "List.aspx");
                }
            }
        }

        public int ClassID
        {
            get
            {
                int id = 0;
                string strid = Request.Params["id"];
                if (!string.IsNullOrWhiteSpace(strid) && PageValidate.IsNumber(strid))
                {
                    id = int.Parse(strid);
                }
                return id;
            }
        }

        private void ShowInfo()
        {
            YSWL.MALL.Model.CMS.ContentClass modelContentClass = bllContentClass.GetModel(ClassID);
            if (null != modelContentClass)
            {
                this.lblClassID.Text = modelContentClass.ClassID.ToString();
                this.lblClassName.Text = modelContentClass.ClassName;
                this.lblClassIndex.Text = modelContentClass.ClassIndex;
                this.lblOrders.Text = modelContentClass.Sequence.ToString();
                if (modelContentClass.ParentId.HasValue)
                {
                    this.lblParentId.Text = modelContentClass.ParentId.ToString();
                }
                this.lblState.Text = modelContentClass.State.ToString();
                this.chkAllowSubclass.Checked = modelContentClass.AllowSubclass;
                this.chkAllowAddContent.Checked= modelContentClass.AllowAddContent ;
                this.imgUrl.ImageUrl = modelContentClass.ImageUrl;
                this.lblDescription.Text = modelContentClass.Description;
                this.lblKeywords.Text = modelContentClass.Keywords;

                //YSWL.MALL.Model.CMS.ClassType modelClassType = bllClassType.GetModel(modelContentClass.ClassTypeID);
                //if (null != modelClassType)
                //{
                //    this.lblClassTypeID.Text = modelClassType.ClassTypeName;
                //}

                this.lblClassModel.Text = modelContentClass.ClassModel.ToString();
                this.lblPageModelName.Text = modelContentClass.PageModelName;
                this.lblCreatedDate.Text = modelContentClass.CreatedDate.ToString();
                this.lblCreatedUserID.Text = modelContentClass.CreatedUserID.ToString();
                this.lblRemark.Text = modelContentClass.Remark;
            }
        }

        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
