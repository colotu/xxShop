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
using YSWL.BLL.Shop.Sample;
using YSWL.Common;
using YSWL.Web;

namespace  YSWL.Web.Admin.Shop.Sample
{
    public partial class SampleImage : PageBaseAdmin
    {
        private YSWL.BLL.Shop.Sample.SampleDetail bll=new SampleDetail();
        new protected int Act_DelData = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
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
                this.ddrSampleList.Items.Insert(0,new ListItem("请选择样本","0"));
            }
            if (SampleId > 0)
            {
                this.ddrSampleList.SelectedValue = SampleId.ToString();
            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }

        #region gridView

        public void BindData()
        {

            int SelectId = Common.Globals.SafeInt(ddrSampleList.SelectedValue, 0);
            StringBuilder strWhere = new StringBuilder();
      
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
            strWhere.Append(" Type=0");
            this.AspNetPager1.RecordCount = bll.GetRecordCount(strWhere.ToString());
            DataListPhoto.DataSource = bll.GetListByPage(strWhere.ToString(),  "", this.AspNetPager1.StartRecordIndex, this.AspNetPager1.EndRecordIndex);
            DataListPhoto.DataBind();
        }

  

        protected void DataListPhoto_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                if (e.CommandArgument != null)
                {
                    int id = Globals.SafeInt(e.CommandArgument.ToString(), 0);
                    DataSet ds = bll.GetList("SampleId=" + id + "");
                    if (bll.Delete(id))
                    {
                        if (ds != null)
                        {
                            //物理删除文件
                            PhysicalFileInfo(ds.Tables[0]);
                        }
                        LogHelp.AddUserLog(CurrentUser.UserName, CurrentUser.UserType, "删除图片(PhotoID=" + id + ")成功", this);
                        MessageBox.ShowSuccessTip(this, Resources.Site.TooltipDelOK);
                    }
                    else
                    {
                        MessageBox.ShowFailTip(this, Resources.Site.TooltipDelError);
                        return;
                    }
                }
                BindData();
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private string GetSelIDlist()
        {
            string idlist = "";
            bool BxsChkd = false;
            for (int i = 0; i < DataListPhoto.Items.Count; i++)
            {
                CheckBox ChkBxItem = (CheckBox)DataListPhoto.Items[i].FindControl("ckPhoto");
                HiddenField hfPhotoId = (HiddenField)DataListPhoto.Items[i].FindControl("hfPhotoId");
                if (ChkBxItem != null && ChkBxItem.Checked)
                {
                    BxsChkd = true;
                    if (hfPhotoId.Value != null)
                    {
                        idlist += hfPhotoId.Value + ",";
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


        #region 按钮事件

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string idlist = GetSelIDlist();
            if (idlist.Trim().Length == 0) return;
            DataSet ds = bll.GetList("SampleId in (" + idlist + ")");
            if (bll.DeleteList(idlist))
            {
                if (ds != null)
                {
                    //物理删除文件
                    PhysicalFileInfo(ds.Tables[0]);
                }
                LogHelp.AddUserLog(CurrentUser.UserName, CurrentUser.UserType, "批量删除图片(PhotoIDs=" + idlist + ")成功", this);
                MessageBox.ShowSuccessTip(this, Resources.Site.TooltipDelOK);
            }
            else
            {
                MessageBox.ShowFailTip(this, Resources.Site.TooltipDelError);
                return;
            }
            BindData();
        }

        #region 物理删除文件

        private void PhysicalFileInfo(DataTable dt)
        {
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                for (int n = 0; n < rowsCount; n++)
                {
                    if (dt.Rows[n]["ImageUrl"] != null && dt.Rows[n]["ImageUrl"].ToString() != "")
                    {
                        DeletePhysicalFile(dt.Rows[n]["TargetImageURL"].ToString());
                    }
                    if (dt.Rows[n]["ThumbImageUrl"] != null && dt.Rows[n]["ThumbImageUrl"].ToString() != "")
                    {
                        DeletePhysicalFile(dt.Rows[n]["ThumbImageUrl"].ToString());
                    }
                    if (dt.Rows[n]["NormalImageUrl"] != null && dt.Rows[n]["NormalImageUrl"].ToString() != "")
                    {
                        DeletePhysicalFile(dt.Rows[n]["NormalImageUrl"].ToString());
                    }
                }
            }
        }

        /// <summary>
        /// 删除物理文件
        /// </summary>
        private void DeletePhysicalFile(string path)
        {
            YSWL.Web.Components.FileHelper.DeleteFile(YSWL.Model.Ms.EnumHelper.AreaType.Shop, path);
        }

        #endregion

        protected void DataListPhoto_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton linkBtn = (LinkButton)e.Item.FindControl("lbtnDel");
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_DelData)) && GetPermidByActID(Act_DelData) != -1)
                {
                    linkBtn.Visible = false;
                }
            }
        }
        #endregion

        protected void ddrSampleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void goback_Click(object sender, EventArgs e)
        {

            Response.Redirect("SampleList.aspx");
        }
    }
}