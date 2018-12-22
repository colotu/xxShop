/**
* List.cs
*
* 功 能： [N/A]
* 类 名： List.cs
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
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;
using YSWL.Web;

namespace  YSWL.Web.Admin.Shop.Sample
{
    public partial class SamplePDF : PageBaseAdmin
    {
        //int Act_ShowInvalid = -1; //查看失效数据行为

        private YSWL.BLL.Shop.Sample.SampleDetail bll = new YSWL.BLL.Shop.Sample.SampleDetail();
        protected override int Act_PageLoad { get { return 95; } } //社区管理_是否显示样本分类页面

        protected new int Act_DelData = 99;    //社区管理_样本分类_删除样本分类
        protected new int Act_UpdateData = 98;    //社区管理_样本分类_编辑样本分类
        protected new int Act_AddData = 97;    //社区管理_样本分类_新增样本分类
        protected new int Act_DeleteList = 96;    //社区管理_样本分类_批量删除样本分类

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_DeleteList)) && GetPermidByActID(Act_DeleteList) != -1)
                {
                    liDel.Visible = false;
                    btnDelete.Visible = false;
                }
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_AddData)) && GetPermidByActID(Act_AddData) != -1)
                {
                    AddLi.Visible = false;
                }
                BindCategoryData();
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


        private void BindCategoryData()
        {
            YSWL.BLL.Shop.Sample.Sample sampleBll = new BLL.Shop.Sample.Sample();
            DataSet ds = sampleBll.GetList("");
            if (!DataSetTools.DataSetIsNull(ds))
            {
                this.ddrSampleList.DataSource = ds;
                this.ddrSampleList.DataTextField = "Tiltle";
                this.ddrSampleList.DataValueField = "SampleId";
                this.ddrSampleList.DataBind();
                this.ddrSampleList.Items.Insert(0, new ListItem("请选择样本", "0"));
            }
            if (SampleId > 0)
            {
                this.ddrSampleList.SelectedValue = SampleId.ToString();
            }
        }



        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }

        #region gridView

        public void BindData()
        {
            #region
            var strWhere = new StringBuilder();
            int SelectId = Globals.SafeInt(ddrSampleList.SelectedValue, 0);
            if (txtKeyword.Text.Trim() != "")
            {
                strWhere.AppendFormat(" Title like '%{0}%'", txtKeyword.Text.Trim());
            }
            if (SelectId > 0)
            {
                if (strWhere.Length > 0)
                {
                    strWhere.Append(" and ");
                }
                strWhere.Append(" SampleId=" + SelectId + " ");
            }
            if (strWhere.Length > 0)
            {
                strWhere.Append(" and ");
            }
            strWhere.Append(" Type=1");

            #endregion gridView

            gridView.DataSetSource = bll.GetList(strWhere.ToString());
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }

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
            int ID = (int)gridView.DataKeys[e.RowIndex].Value;
            if (bll.Delete(ID))
            {
                MessageBox.ShowSuccessTip(this, Resources.Site.TooltipDelOK);
            }
            gridView.OnBind();
        }

        protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
          
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

                    //#warning 代码生成警告：请检查确认Cells的列索引是否正确
                    if (gridView.DataKeys[i].Value != null)
                    {
                        //idlist += gridView.Rows[i].Cells[1].Text + ",";
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

        #endregion


        public string GetStatus(object target)
        {
            //状态 0：草稿 1：已发布
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(target))
            {
                switch (Globals.SafeInt(target.ToString(), -1))
                {
                    case 0:
                        str = "草稿";
                        break;

                    case 1:
                        str = "已发布";
                        break;
                    default:
                        break;
                }
            }
            return str;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string idlist = GetSelIDlist();
            if (idlist.Trim().Length == 0) return;
            if (bll.DeleteList(idlist))
            {
                LogHelp.AddUserLog(CurrentUser.UserName, CurrentUser.UserType, "批量删除样本(id=" + idlist + ")成功", this);
                MessageBox.ShowSuccessTip(this, Resources.Site.TooltipDelOK);
            }
            else
            {
                LogHelp.AddUserLog(CurrentUser.UserName, CurrentUser.UserType, "批量删除样本(id=" + idlist + ")失败", this);
            }
            gridView.OnBind();
        }
        protected void ddrSampleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridView.OnBind();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("SampleList.aspx");

        }
    }
}