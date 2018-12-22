using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.CMS;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.Common;
using YSWL.WeChat.BLL;

namespace YSWL.MALL.Web.Admin.WeChat.Command
{
    public partial class CommandList : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 628; } } //移动营销_微信导航菜单管理_列表页
        protected new int Act_AddData = 629;    //移动营销_微信导航菜单管理_新增数据
        protected new int Act_UpdateData = 630;    //移动营销_微信导航菜单管理_编辑数据
        protected new int Act_DelData = 631;    //移动营销_微信导航菜单管理_删除数据
        private   YSWL.WeChat.BLL.Core.Command commandBll=new YSWL.WeChat.BLL.Core.Command();
        private YSWL.WeChat.BLL.Core.Action actionBll = new YSWL.WeChat.BLL.Core.Action();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_AddData)) && GetPermidByActID(Act_AddData) != -1)
                {
                    liAdd.Visible = false;
                }
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_DelData)) && GetPermidByActID(Act_DelData) != -1)
                {
                    btnDelete.Visible = false;
                }
             
            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }


        #region gridView


        public void BindData()
        {
            if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_UpdateData)) && GetPermidByActID(Act_UpdateData) != -1)
            {
                gridView.Columns[8].Visible = false;
            }
            StringBuilder strWhere = new StringBuilder();
            string Status = this.dropStatus.SelectedValue;
            string keyWord = this.txtKeyword.Text;
            if (!string.IsNullOrWhiteSpace(Status))
            {
                strWhere.AppendFormat(" Status={0}", Status);
            }
            if (!String.IsNullOrWhiteSpace(keyWord))
            {
                if (!String.IsNullOrWhiteSpace(strWhere.ToString()))
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" Name like '%{0}%'", Common.InjectionFilter.SqlFilter(keyWord));
            }
            gridView.DataSetSource = commandBll.GetList(-1, strWhere.ToString(), " Sequence");
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
            if (commandBll.Delete((int)gridView.DataKeys[e.RowIndex].Value))
            {
                gridView.OnBind();
                MessageBox.ShowSuccessTip(this, Resources.Site.TooltipDelOK);
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

        #region 获取对应系统指令名称

        protected string GetAction(object target)
        {
            string value = "";
              if (!StringPlus.IsNullOrEmpty(target))
              {
                 int actionId = Common.Globals.SafeInt(target.ToString(), 0);
               List<YSWL.WeChat.Model.Core.Action> actionList =  YSWL.WeChat.BLL.Core.Action.GetAllAction();
                  YSWL.WeChat.Model.Core.Action actionModel = actionList.FirstOrDefault(c => c.ActionId == actionId);
                  if (actionModel != null)
                  {
                      value = actionModel.Name;
                  }
              }
              return value;
        }

        #endregion 

        protected string GetTarget(object target_obj,object actionId_obj)
        {
            string value = "";
            if (!StringPlus.IsNullOrEmpty(target_obj) && !StringPlus.IsNullOrEmpty(actionId_obj))
            {
                int targetId = Common.Globals.SafeInt(target_obj.ToString(), 0);
                int actionId = Common.Globals.SafeInt(actionId_obj.ToString(), 0);
                switch (actionId)
                {
                    //文章栏目
                    case 1:
                        YSWL.MALL.BLL.CMS.ContentClass classBll = new ContentClass();
                      YSWL.MALL.Model.CMS.ContentClass classModel =
                            classBll.GetModel(targetId);
                        value = classModel == null ? "" :"文章栏目：【"+classModel.ClassName+"】";
                        break;
                    //商品分类
                    case 2:
                        YSWL.MALL.BLL.Shop.Products.CategoryInfo cateBll = new CategoryInfo();
                        YSWL.MALL.Model.Shop.Products.CategoryInfo CategoryInfo =
                            cateBll.GetModel(targetId);
                        value = CategoryInfo == null ? "" : "商品分类：【" + CategoryInfo.Name + "】";
                        break;
                    default:
                        value = "";
                        break;
                }
            }
            return value;
        }

        protected string GetParseType(object target)
        {
            string value = "";
            if (!StringPlus.IsNullOrEmpty(target))
            {
                int typeId = Common.Globals.SafeInt(target.ToString(), 0);
                switch (typeId)
                {
                    case 0:
                        value = "长度";
                        break;
                    case 1:
                         value = "特殊字符";
                        break;
                    default:
                        value = "长度";
                        break;
                }
            }
            return value;
        }

        protected string GetStatus(object target)
        {
            string value = "";
            if (!StringPlus.IsNullOrEmpty(target))
            {
                int status = Common.Globals.SafeInt(target.ToString(), 0);
                switch (status)
                {
                    case 0:
                        value = "不可用";
                        break;
                    case 1:
                        value = "可用";
                        break;
                    default:
                        value = "不可用";
                        break;
                }
            }
            return value;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string idlist = GetSelIDlist();
            if (idlist.Trim().Length == 0)
                return;
            if (commandBll.DeleteList(idlist))
            {
                YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.TooltipDelOK, "CommandList.aspx");
            }
            else
            {
                YSWL.Common.MessageBox.ShowFailTip(this, Resources.Site.TooltipDelError);
            }
            gridView.OnBind();
        }
        

    }
}