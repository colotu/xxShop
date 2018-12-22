/**
* List.cs
*
* 功 能： [N/A]
* 类 名： List.cs
*
* Ver    变更日期      负责人  变更内容
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
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using YSWL.Common;
using YSWL.Json;
using YSWL.MALL.Web;
using YSWL.MALL.Web.Components;
using YSWL.Web;

namespace YSWL.MALL.Web.Admin.Ms.Themes
{
    public partial class List : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 334; } } //设置_模版管理页

        protected new int Act_DelData = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
                if (!string.IsNullOrWhiteSpace(this.Request.Form["Callback"]) && (this.Request.Form["Callback"] == "true"))
                {
                    this.Controls.Clear();
                    this.DoCallback();
                }
            }
        }

        #region DataList

        public void BindData()
        {
            //获取该主区域 下的所有模板
            List<YSWL.MALL.Model.Ms.Theme> themeList = YSWL.MALL.Web.Components.FileHelper.GetThemes("Shop");
            //获取当前主模板
            string name = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_Theme");
            hidCurrentColor.Value = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_Theme_Color");

            //获取该主区域 下的所有模板
            List<YSWL.MALL.Model.Ms.Theme> themeListNew = new List<Model.Ms.Theme>();
            //获取当前主模板
            //获取多个颜色的模板列表
            List<string> tList = new List<string>();
            string hasColorThemes = hidHasColorThemes.Value;
            if (!String.IsNullOrWhiteSpace(hasColorThemes))
            {
                tList = hidHasColorThemes.Value.Split(',').ToList();
            }
            YSWL.MALL.Model.Ms.Theme greenItem = null;
            YSWL.MALL.Model.Ms.Theme blueItem = null;
            YSWL.MALL.Model.Ms.Theme orangeItem = null;
            YSWL.MALL.Model.Ms.Theme redItem = null;
            foreach (var item in themeList)
            {
                if (item.Name == name)
                {
                    item.IsCurrent = true;
                }
                if (tList.Contains(item.Name))
                {
                    greenItem = new Model.Ms.Theme
                    {
                        ID = item.ID,
                        CreatedDate = item.CreatedDate,
                        Name = item.Name,
                        Author = item.Author,
                        Description = item.Description,
                        PreviewPhotoSrc = item.PreviewPhotoSrc,
                        Remark = "绿色",
                        Color = "green"
                    };
                    if (item.Name == name && hidCurrentColor.Value == "green")
                    {
                        greenItem.IsCurrent = true;
                    }
                    themeListNew.Add(greenItem);

                    blueItem = new Model.Ms.Theme
                    {
                        ID = item.ID,
                        CreatedDate = item.CreatedDate,
                        Name = item.Name,
                        Author = item.Author,
                        Description = item.Description,
                        PreviewPhotoSrc = "/Areas/Shop/Themes/" + item.Name + "/Theme-blue.png",
                        Remark = "蓝色",
                        Color = "blue"
                    };
                    if (item.Name == name && hidCurrentColor.Value == "blue")
                    {
                        blueItem.IsCurrent = true;
                    }
                    themeListNew.Add(blueItem);

                    orangeItem = new Model.Ms.Theme
                    {
                        ID = item.ID,
                        CreatedDate = item.CreatedDate,
                        Name = item.Name,
                        Author = item.Author,
                        Description = item.Description,
                        PreviewPhotoSrc = "/Areas/Shop/Themes/" + item.Name + "/Theme-orange.png",
                        Remark = "橙色",
                        Color = "orange"
                    };
                    if (item.Name == name && hidCurrentColor.Value == "orange")
                    {
                        orangeItem.IsCurrent = true;
                    }
                    themeListNew.Add(orangeItem);


                    redItem = new Model.Ms.Theme
                    {
                        ID = item.ID,
                        CreatedDate = item.CreatedDate,
                        Name = item.Name,
                        Author = item.Author,
                        Description = item.Description,
                        PreviewPhotoSrc = "/Areas/Shop/Themes/" + item.Name + "/Theme-red.png",
                        Remark = "红色",
                        Color = "red"
                    };
                    if (item.Name == name && hidCurrentColor.Value == "red")
                    {
                        redItem.IsCurrent = true;
                    }
                    themeListNew.Add(redItem);

                }
                else
                {
                    themeListNew.Add(item);
                }
            }
            DataListPhoto.DataSource = themeListNew;
            DataListPhoto.DataBind();
        }

        protected void DataListPhoto_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "start")
            {
                if (e.CommandArgument != null)
                {
                    string[] cA = e.CommandArgument.ToString().Split(',');

                    //写
                    YSWL.MALL.BLL.SysManage.ConfigSystem.Modify("Shop_Theme", cA[0], "商城模板的名称");
                    if (cA.Length > 1)
                    {
                        YSWL.MALL.BLL.SysManage.ConfigSystem.Modify("Shop_Theme_Color", cA[1], "商城模板颜色");
                    }
                    Cache.Remove("ConfigSystemHashList");    //清除网站设置的缓存文件
                   // MessageBox.ShowSuccessTip(this, "启用成功", "List.aspx");
                    MessageBox.ShowSuccessTip(this, "模版切换成功, 请重新生成网站缩略图, 即将为您跳转..", "/Admin/Shop/Products/ImageReGen.aspx");
                }
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            BindData();
        }


        #region Ajax方法
        private void DoCallback()
        {
            string action = this.Request.Form["Action"];
            this.Response.Clear();
            this.Response.ContentType = "application/json";
            string writeText = string.Empty;

            switch (action)
            {
                case "UpdateColor":
                    writeText = UpdateColor();
                    break;
            }
            this.Response.Write(writeText);
            this.Response.End();
        }

        private string UpdateColor()
        {
            JsonObject json = new JsonObject();
            string color = this.Request.Form["color"];
            if (string.IsNullOrWhiteSpace(color))
            {
                json.Put("STATUS", "FAILED");
            }
            else
            {
                //写
                YSWL.MALL.BLL.SysManage.ConfigSystem.Modify("Shop_Theme_Color", color, "商城模板的颜色");
                Cache.Remove("ConfigSystemHashList");    //清除网站设置的缓存文件
                json.Put("STATUS", "SUCCESS");
            }
            return json.ToString();
        }
        #endregion
        #endregion

    }
}