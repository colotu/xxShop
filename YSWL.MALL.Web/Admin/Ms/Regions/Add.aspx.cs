/**
* Add.cs
*
* 功 能： [N/A]
* 类 名： Add.cs
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
using YSWL.Common;
using System.Data;
using System.Web.UI.WebControls;
namespace YSWL.MALL.Web.Ms.Regions  
{
   
    public partial class Add : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 326; } } //省市区域管理_新增页
        protected void Page_Load(object sender, EventArgs e)
        {
            Regions1.ProvinceVisible = true;
            Regions1.CityVisible = true;
            Regions1.AreaVisible = false;
            Regions1.VisibleAll = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.btnSave.Enabled = false;
            this.btnCancle.Enabled = false;
            if (string.IsNullOrEmpty(this.txtRegionName.Text.Trim()))
            {
                this.btnSave.Enabled = true;
                this.btnCancle.Enabled = true;
                MessageBox.ShowFailTip(this, "地区名称不能为空！");
                return;
            }
            YSWL.MALL.Model.Ms.Regions model = new YSWL.MALL.Model.Ms.Regions();
            model.AreaId = null;//区域ID 华东 华南
            //新增省份
            if (0 == this.Regions1.Province_iID && 0 == this.Regions1.City_iID && 0 == this.Regions1.Area_iID)
            {
                model.ParentId = null;
                model.Path = "0,";
                model.Depth = 1;
            }
            //新增市区
            if (0 != this.Regions1.Province_iID && 0 == this.Regions1.City_iID && 0 == this.Regions1.Area_iID)
            {
                model.ParentId = this.Regions1.Province_iID;
                model.Path = "0," + this.Regions1.Province_iID;
                model.Depth = 2;
            }
            //新增县城
            if (0 != this.Regions1.Province_iID && 0 != this.Regions1.City_iID && 0 == this.Regions1.Area_iID)
            {
                model.ParentId = this.Regions1.City_iID;
                model.Path = "0," + this.Regions1.Province_iID + "," + this.Regions1.City_iID;
                model.Depth = 3;
            }
            //新增乡镇
            if (0 != this.Regions1.Province_iID && 0 != this.Regions1.City_iID && 0 != this.Regions1.Area_iID)
            {
                YSWL.Common.MessageBox.ShowSuccessTip(this, "暂时支持新增到三级区域");
                return;
            }

            YSWL.MALL.BLL.Ms.Regions bll = new YSWL.MALL.BLL.Ms.Regions();

            model.RegionId = bll.GetMaxId();

            model.RegionName = this.txtRegionName.Text;

            model.Spell = null;

            model.SpellShort = this.txtSpellShort.Text;

            model.DisplaySequence = Globals.SafeInt(this.txtDisplaySequence.Text, 1);

            if (0 < bll.Add(model))
            {
                this.btnSave.Enabled = false;
                this.btnCancle.Enabled = false;
                YSWL.Common.MessageBox.ShowSuccessTip(this, "保存成功！", "add.aspx");
            }
            else
            {
                this.btnSave.Enabled = true;
                this.btnCancle.Enabled = true;
                MessageBox.ShowSuccessTip(this, "新增失败！");
            }
        }

        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
