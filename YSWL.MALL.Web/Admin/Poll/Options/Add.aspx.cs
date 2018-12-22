using System;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;
namespace YSWL.MALL.Web.Admin.Options
{
    public partial class Add : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 360; } } //客服管理_选项管理_列表页
        protected new int Act_AddData = 361;    //客服管理_选项管理_新增数据
        protected new int Act_DelData = 362;    //客服管理_选项管理_删除数据

        YSWL.MALL.BLL.Poll.Topics blltop = new YSWL.MALL.BLL.Poll.Topics();
        YSWL.MALL.BLL.Poll.Options blloption = new YSWL.MALL.BLL.Poll.Options();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_AddData)) && GetPermidByActID(Act_AddData) != -1)
                {
                    btnAdd.Visible = false;
                }
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_DelData)) && GetPermidByActID(Act_DelData) != -1)
                {
                    liDel.Visible = false;
                    btnDelete.Visible = false;
                }


             

                string tid = Request.Params["tid"];
                if (!string.IsNullOrWhiteSpace(tid) && PageValidate.IsNumber(tid))
                {
                    YSWL.MALL.Model.Poll.Topics model = blltop.GetModel(Convert.ToInt32(tid));
                    if (null != model)
                    {
                        lblTopicID.Text = model.ID.ToString();
                        lblTitle.Text = model.Title;
                    }
                    Session["strWhereOptions"] = " TopicID= " + lblTopicID.Text;
                }

                gridView.OnBind();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtName.Text))
            {
                MessageBox.ShowFailTip(this, Resources.Poll.ErrorOptionsNotNull);
                return;
            }
            string Name = this.txtName.Text;
            int TopicID = int.Parse(this.lblTopicID.Text);
            int isChecked = chkisChecked.Checked?1:0;            
            YSWL.MALL.Model.Poll.Options model = new YSWL.MALL.Model.Poll.Options();
            model.Name = Name;
            model.TopicID = TopicID;
            model.isChecked = isChecked;
            model.SubmitNum = 0;
            if (blloption.Exists(TopicID, Name))
            {
                MessageBox.ShowFailTip(this,Resources.Poll.ErrorOptionsExists);
                return;
            }
            blloption.Add(model);
            Session["strWhereOptions"] = " TopicID= " + lblTopicID.Text;
            gridView.OnBind();
        }


        #region BindData
        public void BindData()
        {
            #region 权限检查
            //if (!Context.User.Identity.IsAuthenticated)
            //{
            //    return;
            //}
            if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_DelData)) && GetPermidByActID(Act_DelData) != -1)
            {
                gridView.Columns[4].Visible = false;
            }

            #endregion
            
            string strWhere = "";
            if (Session["strWhereOptions"] != null && Session["strWhereOptions"].ToString() != "")
            {
                strWhere += Session["strWhereOptions"].ToString();
            }
            gridView.DataSetSource = blloption.GetList(strWhere);
        }
        #endregion

        #region gridView事件

        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridView.PageIndex = e.NewPageIndex;
            gridView.OnBind();
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Attributes.Add("style", "background:#FFF");
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex % 2 == 0)
                {
                    e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#F4F4F4");
                }
                else
                {
                    e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#FFFFFF");
                }
            }
        }

        protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string ID = gridView.DataKeys[e.RowIndex].Value.ToString();
            blloption.Delete(Convert.ToInt32(ID));
            gridView.OnBind();
        }
        #endregion

        protected void btnBack_Click(object sender, EventArgs e)
        {
            string tid = lblTopicID.Text.Trim();
            if (PageValidate.IsNumber(tid))
            {
                YSWL.MALL.Model.Poll.Topics model = blltop.GetModel(Convert.ToInt32(tid));
                if (null != model)
                {
                    if (model.FormID.HasValue)
                    {
                        Response.Redirect("../Topics/index.aspx?fid=" + model.FormID);
                    }
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string idlist = GetSelIDlist();
            if (idlist.Trim().Length == 0) return;
            if (blloption.DeleteList(idlist))
            {
                YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.TooltipDelOK);
            }
            else
            {
                YSWL.Common.MessageBox.ShowFailTip(this, Resources.Site.TooltipDelError);
            }
            gridView.OnBind();
        }

        private string GetSelIDlist()
        {
            string idlist = "";
            bool BxsChkd = false;
            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                CheckBox ChkBxItem = (CheckBox)gridView.Rows[i].FindControl(gridView.CheckBoxID);
                if (ChkBxItem != null && ChkBxItem.Checked)
                {
                    BxsChkd = true;
                    if (gridView.DataKeys[i].Value != null)
                    {
                        idlist += gridView.DataKeys[i].Value.ToString() + ",";
                    }
                }
            }
            if (BxsChkd)
            {
                idlist = idlist.Substring(0, idlist.LastIndexOf(","));
            }
            return idlist;
        }
    }
}
