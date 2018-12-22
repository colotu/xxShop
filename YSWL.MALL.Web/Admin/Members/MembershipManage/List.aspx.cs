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
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using YSWL.Accounts.Bus;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Members.MembershipManage
{
    public partial class List : PageBaseAdmin
    {
        //int Act_ShowInvalid = -1; //查看失效数据行为
        protected override int Act_PageLoad { get { return 182; } } //用户管理_是否显示会员信息管理页面

        protected int Act_BatActive = 183;
        protected int Act_BatUnActive = 184;

        //private YSWL.MALL.BLL.SNS.GradeConfig bll = new YSWL.MALL.BLL.SNS.GradeConfig();
        // YSWL.MALL.BLL.Members.UserRank rankBll = new BLL.Members.UserRank();
        private YSWL.MALL.BLL.Members.Users user = new BLL.Members.Users();

        YSWL.MALL.BLL.Members.UsersExp userexpbll = new BLL.Members.UsersExp();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_BatActive)) && GetPermidByActID(Act_AddData) != -1)
                {
                    btnActivity.Visible = false;
                }
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_BatUnActive)) && GetPermidByActID(Act_BatUnActive) != -1)
                {
                    btnUnActivity.Visible = false;
                }

            
            }
        }
        #region 是否开启会员等级

        /// <summary>
        /// 是否开启会员等级
        /// </summary>
        protected bool RankScoreIsEnable
        {
            get
            {
                return BLL.SysManage.ConfigSystem.GetBoolValueByCache("RankScoreEnable");
            }
        }

        #endregion
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }

        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    string idlist = GetSelIDlist();
        //    if (idlist.Trim().Length == 0) return;
        //    rankBll.DeleteList(idlist);
        //    YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.TooltipDelOK);
        //    gridView.OnBind();
        //}

        #region 用户批量解冻

        /// <summary>
        /// 用户批量解冻
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnActivity_Click(object sender, EventArgs e)
        {
            string idlist = GetSelIDlist();
            if (idlist.Trim().Length == 0) return;
            if (user.UpdateActiveStatus(idlist, 1))
            {
                YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.TooltipUpdateOK);
                gridView.OnBind();
            }
        }

        #endregion 用户批量解冻

        #region 冻结用户

        /// <summary>
        /// 用户批量冻结
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUnActivity_Click(object sender, EventArgs e)
        {
            string idlist = GetSelIDlist();
            if (idlist.Trim().Length == 0) return;
            if (user.UpdateActiveStatus(idlist, 0))
            {
                YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.TooltipUpdateOK);
                gridView.OnBind();
            }
        }

        #endregion 冻结用户

        #region gridView

        public void BindData()
        {
            #region

            if (RankScoreIsEnable)
            {
                gridView.Columns[5].Visible = true;
            }
            
            #endregion gridView

            DataSet ds = new DataSet();
            StringBuilder strWhere = new StringBuilder();
            if (txtKeyword.Text.Trim() != "")
            {
                strWhere.AppendFormat("( NickName like '%{0}%'  or  UserName  like '%{0}%'  or   Phone like '%{0}%'  )", Common.InjectionFilter.SqlFilter(txtKeyword.Text));
            }

            if (!string.IsNullOrEmpty(txtBeginTime.Text.Trim()))
            {
                if (strWhere.Length > 0)
                {
                    strWhere.Append(" and ");
                }
                strWhere.Append("  User_dateCreate   >='" + Common.InjectionFilter.SqlFilter(txtBeginTime.Text.Trim()) + "' ");
            }
            if (Common.Globals.SafeInt(dropType.SelectedValue, -1) > -1)
            {
                if (strWhere.Length > 0)
                {
                    strWhere.Append(" and ");
                }
                strWhere.Append("  Activity=" + dropType.SelectedValue + " ");
            }

            if (ckboxvip.Checked)
            {
                if (strWhere.Length > 0)
                {
                    strWhere.Append(" and ");
                }
                strWhere.Append("  Accounts_Users.UserID in (select Accounts_UsersExp.UserID from dbo.Accounts_UsersExp where BodilyForm='VIP') ");
            }

            if (!string.IsNullOrEmpty(txtEndTime.Text.Trim()))
            {
                DateTime? endTime = Common.Globals.SafeDateTime(this.txtEndTime.Text, null);
                if (endTime.HasValue)
                {
                    if (strWhere.Length > 0)
                    {
                        strWhere.Append(" and ");
                    }
                    strWhere.Append("  User_dateCreate  <'" + endTime.Value.AddDays(1) + "' ");
                }
            }
            ds = user.GetSearchList("", strWhere.ToString());

            //ds = bll.GetList(strWhere.ToString(), UserPrincipal.PermissionsID, UserPrincipal.PermissionsID.Contains(GetPermidByActID(Act_ShowInvalid)));
            gridView.DataSetSource = ds;
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
                //object obj1 = DataBinder.Eval(e.Row.DataItem, "Levels");
                //if ((obj1 != null) && ((obj1.ToString() != "")))
                //{
                //    e.Row.Cells[4].Text = obj1.ToString() == "0" ? "Private" : "Shared";
                //}
                if (RankScoreIsEnable)
                {
                    HtmlGenericControl hlRankScore = (HtmlGenericControl)e.Row.FindControl("hlRankScore");
                    hlRankScore.Visible = true;
                }

            }
        }

        protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //int ID = (int)gridView.DataKeys[e.RowIndex].Value;
            //if (rankBll.Delete(ID))
            //{
            //    YSWL.Common.MessageBox.ShowSuccessTip(this, "删除成功！");
            //}
            gridView.OnBind();
        }
        protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Status")
            {
                if (e.CommandArgument != null)
                {
                    int Id = 0;
                    string[] Args = e.CommandArgument.ToString().Split(new char[] { ',' });
                    Id = Common.Globals.SafeInt(Args[0], 0);
                    AccountsPrincipal user = new AccountsPrincipal(Id);
                    User currentUser = new YSWL.Accounts.Bus.User(user);
                    bool Status = Common.Globals.SafeBool(Args[1], false);
                    currentUser.Activity = Status ? false : true;
                    currentUser.Update();
                    gridView.OnBind();
                }
            }
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


        protected string GetGravatar(object target)
        {
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(target))
            {
                string savePath = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValue("SNSGravatarPath");
                if (string.IsNullOrEmpty(savePath))
                {
                    savePath = "/Upload/User/Gravatar/";
                }
                int UserId = Common.Globals.SafeInt(target.ToString(), 0);
                return savePath + UserId + ".jpg";
            }
            return str;
        }
        #endregion

        BLL.Ms.Regions RegionBll = new BLL.Ms.Regions();
        public string GetRegion(object o)
        {
            if (o == null || String.IsNullOrWhiteSpace(o.ToString()))
            {
                return "";
            }
            return RegionBll.GetAddress(Globals.SafeInt(o.ToString(), null));
        }


        #region 一键导出
        protected void btnExport_Click(object sender, EventArgs e)
        {
            BindData();
            DataSet dataSet = gridView.DataSetSource as DataSet;
            if (Common.DataSetTools.DataSetIsNull(dataSet))
            {
                MessageBox.ShowServerBusyTip(this, "抱歉, 当前没有可以导出的数据!");
                return;
            }
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("编号", typeof(Int32)));
            dataTable.Columns.Add(new DataColumn("用户名", typeof(string)));
            dataTable.Columns.Add(new DataColumn("昵称", typeof(string)));
            dataTable.Columns.Add(new DataColumn("手机号码", typeof(string)));
            dataTable.Columns.Add(new DataColumn("性别", typeof(string)));
            dataTable.Columns.Add(new DataColumn("邮箱", typeof(string)));
            dataTable.Columns.Add(new DataColumn("注册时间", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("用户状态", typeof(string)));

            DataRow tmpRow;
            string sex;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                tmpRow = dataTable.NewRow();
                tmpRow["编号"] = row.Field<Int32>("UserID");
                tmpRow["用户名"] = row.Field<string>("UserName");
                tmpRow["昵称"] = row.Field<string>("NickName");
                tmpRow["手机号码"] = row.Field<string>("Phone");
                sex = row.Field<string>("Sex");
                if (!String.IsNullOrWhiteSpace(sex))
                {
                    sex = sex.Trim();
                }
                tmpRow["性别"] = sex == "0" ? "女" : "男";
                tmpRow["邮箱"] = row.Field<string>("Email");
                tmpRow["注册时间"] = row.Field<DateTime>("User_dateCreate").ToString("yyyy-MM-dd HH:mm:ss");
                tmpRow["用户状态"] = row.Field<bool>("Activity") ? "已激活" : "已冻结";

                dataTable.Rows.Add(tmpRow);
            }
            DataSetToExcel(dataTable);
        }
        private void DataSetToExcel(DataTable data)
        {
            Response.Clear();
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                string nowDate = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
                NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
                NPOI.HSSF.UserModel.HSSFSheet sheet = (NPOI.HSSF.UserModel.HSSFSheet)workbook.CreateSheet(
                    string.Format("导出用户_{0}",
                    nowDate));
                NPOI.HSSF.UserModel.HSSFRow headerRow = (NPOI.HSSF.UserModel.HSSFRow)sheet.CreateRow(0);
                foreach (DataColumn column in data.Columns)
                {
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                }
                int rowIndex = 1;
                foreach (DataRow row in data.Rows)
                {
                    NPOI.HSSF.UserModel.HSSFRow dataRow = (NPOI.HSSF.UserModel.HSSFRow)sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in data.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                    dataRow = null;
                    rowIndex++;
                }
                //自动调整列宽
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
                workbook.Write(ms);
                headerRow = null;
                sheet = null;
                workbook = null;
                Response.AddHeader("Content-Disposition",
                    string.Format("attachment; filename=ExportUser_{0}.xls",
                    nowDate));
                Response.BinaryWrite(ms.ToArray());
                ms.Close();
                ms.Dispose();
            }
            Response.End();
        }
        #endregion



        protected void btndel_Click(object sender, EventArgs e)
        {
            string idlist = GetSelIDlist();
            if (idlist.Trim().Length == 0) return;
            if (user.DeleteUserListByUserID(idlist))
            {
                YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.TooltipUpdateOK);
                gridView.OnBind();
            }
        }
    }
}