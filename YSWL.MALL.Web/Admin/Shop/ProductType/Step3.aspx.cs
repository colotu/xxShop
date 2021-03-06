﻿/**
* List.cs
*
* 功 能： [N/A]
* 类 名： List.cs
*
* Ver        变更日期                           负责人          变更内容
* ───────────────────────────────────
* V0.01   2012年6月13日 20:46:27    Rock            创建
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
using System.Web.UI.WebControls;
namespace YSWL.MALL.Web.Admin.Shop.ProductType
{
    public partial class Step3 : PageBaseAdmin
    {
        //int Act_ShowInvalid = -1; //查看失效数据行为
        protected override int Act_PageLoad { get { return 493; } } //Shop_商品类型管理_新增页

        YSWL.MALL.BLL.Shop.Products.AttributeInfo bll = new YSWL.MALL.BLL.Shop.Products.AttributeInfo();
        BLL.Shop.Products.ProductType ptypeBll = new BLL.Shop.Products.ProductType();
        YSWL.MALL.BLL.Shop.Products.AttributeValue bllValue = new YSWL.MALL.BLL.Shop.Products.AttributeValue();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (ProductTypeId == 0)
                {
                    YSWL.Common.MessageBox.ShowFailTip(this, "参数错误，正在返回商品类型列表页...", "list.aspx");
                    return;
                }

               if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_DeleteList)) && GetPermidByActID(Act_DeleteList)!=-1)
                {
                    btnDelete.Visible = false;
                }


                gridView.OnBind();
            }
        }

        private int ProductTypeId
        {
            get
            {
                int producrTypeId = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["tid"]))
                {
                    producrTypeId =YSWL.Common.Globals.SafeInt(Request.Params["tid"], 0);
                }
                return producrTypeId;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string idlist = GetSelIDlist();
            if (idlist.Trim().Length == 0) return;
            bll.DeleteList(idlist);
            YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.TooltipDelOK);
            gridView.OnBind();
        }

        #region gridView

        public void BindData()
        {
            #region
            //if (!Context.User.Identity.IsAuthenticated)
            //{
            //    return;
            //}
            //AccountsPrincipal user = new AccountsPrincipal(Context.User.Identity.Name);
            //if (user.HasPermissionID(PermId_Modify))
            //{
            //    gridView.Columns[6].Visible = true;
            //}
            //if (user.HasPermissionID(PermId_Delete))
            //{
            //    gridView.Columns[7].Visible = true;
            //}
            #endregion

            DataSet ds = new DataSet();
            ds = bll.GetList(ProductTypeId,Model.Shop.Products.SearchType.Specification);
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
                Literal litAttValue = (Literal)e.Row.FindControl("litAttValue");

                object obj1 = DataBinder.Eval(e.Row.DataItem, "UseAttributeImage");

                long AttributeId = (long)this.gridView.DataKeys[e.Row.RowIndex].Value;
                System.Collections.Generic.List<YSWL.MALL.Model.Shop.Products.AttributeValue> list = bllValue.GetModelList(AttributeId);
                if ((bool)obj1)
                {
                    StringBuilder strValue = new StringBuilder();
                    foreach (YSWL.MALL.Model.Shop.Products.AttributeValue item in list)
                    {
                        strValue.Append("<span class='SKUValue' id='span" + item.ValueId + "'><span class='span1'><a id='aValue" + item.ValueId + "'  class='a_none' href='#'><img src=\"" + item.ImageUrl + "\" width=\"23\" height=\"20\" alt=\"" + item.ValueStr + "\" /></a></span><span class='span2'  title='删除'><a href=\"javascript:deleteAttributeValue(this,'" + item.ValueId + "');\">删除</a></span> </span>");
                    }
                    litAttValue.Text = strValue.ToString();
                }
                else
                {
                    StringBuilder strValue = new StringBuilder();
                    foreach (YSWL.MALL.Model.Shop.Products.AttributeValue item in list)
                    {
                        strValue.Append("<span class='SKUValue' id='span" + item.ValueId + "'><span class='span1'><a id='aValue" + item.ValueId + "'>" + item.ValueStr + "</a></span><span class='span2' title='删除'><a href=\"javascript:deleteAttributeValue(this,'" + item.ValueId + "');\">删除</a></span> </span>");
                    }
                    litAttValue.Text = strValue.ToString();
                }
                //object obj1 = DataBinder.Eval(e.Row.DataItem, "Levels");
                //if ((obj1 != null) && ((obj1.ToString() != "")))
                //{
                //    e.Row.Cells[4].Text = obj1.ToString() == "0" ? "Private" : "Shared";
                //}

            }
        }

        protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            long ID = (long)gridView.DataKeys[e.RowIndex].Value;
            ptypeBll.DeleteManage(null, ID, null);
            gridView.OnBind();
        }

        protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
            long AttributeId = (long)this.gridView.DataKeys[rowIndex].Value;
            if (e.CommandName == "Fall")
            {
                ptypeBll.SwapSeqManage(ProductTypeId, AttributeId, null, Model.Shop.Products.SwapSequenceIndex.Down,true);
            }
            if (e.CommandName == "Rise")
            {
                ptypeBll.SwapSeqManage(ProductTypeId, AttributeId, null, Model.Shop.Products.SwapSequenceIndex.Up,true);
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Response.Redirect("List.aspx");
        }
    }
}
