﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Shop.Products
{
    public partial class AddHotKeyword : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 475; } } //Shop_热门关键词管理_编辑页
        private YSWL.MALL.BLL.Shop.Products.HotKeyword hotKeywordBll = new BLL.Shop.Products.HotKeyword();
        private YSWL.MALL.Model.Shop.Products.HotKeyword model = new Model.Shop.Products.HotKeyword();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            //修改
            if (Request.Params["id"] != null && Request.Params["id"].Trim() != "")
            {
                btnAdd.Text = "修改";

                model = hotKeywordBll.GetModel(Convert.ToInt32(Request.Params["id"].ToString()));
                tbKeyWord.Text = model.Keywords;
                BindDropList();
                if (model.CategoryId != -1)
                {
                    this.dropCategories.SelectedValue = model.CategoryId.ToString();
                }
               
            }
            //新增
            else
            {
                BindDropList();
            }
        }

        private void BindDropList()
        {
            //一级分类
            YSWL.MALL.BLL.Shop.Products.CategoryInfo bllc = new BLL.Shop.Products.CategoryInfo();
            DataSet dsc = bllc.GetList(" Depth = 1");
            if (!DataSetTools.DataSetIsNull(dsc))
            {
                this.dropCategories.DataSource = dsc;
                this.dropCategories.DataTextField = "Name";
                this.dropCategories.DataValueField = "CategoryId";
                this.dropCategories.DataBind();
            }
            this.dropCategories.Items.Insert(0, new ListItem("请选择", "0"));
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int cateId = Common.Globals.SafeInt(dropCategories.SelectedValue,0);
            model.Keywords = tbKeyWord.Text.Trim();
            model.CategoryId = cateId;
            if (cateId == 0)
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "请选择分类！");
                return;
            }
            if (Request.Params["id"] != null && Request.Params["id"].Trim() != "")
            {
                model.Id =Convert.ToInt32(Request.Params["id"].ToString());
                if (hotKeywordBll.Update(model))
                {
                    YSWL.Common.MessageBox.ResponseScript(this, "parent.location.href='HotKeyword.aspx'");
                }
                else
                {
                    YSWL.Common.MessageBox.ShowFailTip(this, "修改失败！");
                }
            }
            else
            {
                if (hotKeywordBll.Add(model) > 0)
                {
                    YSWL.Common.MessageBox.ResponseScript(this, "parent.location.href='HotKeyword.aspx'");
                }
                else
                {
                    YSWL.Common.MessageBox.ShowFailTip(this, "保存失败！");
                }
            }
        }
    }
}