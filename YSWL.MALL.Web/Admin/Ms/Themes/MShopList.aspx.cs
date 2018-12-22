using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;
using YSWL.Json;
using YSWL.MALL.Model.SysManage;
using YSWL.MALL.Web;

namespace YSWL.MALL.Web.Admin.Ms.Themes
{
    public partial class MShopList : PageBaseAdmin
    {
        //protected override int Act_PageLoad { get { return 334; } } //设置_模版管理页

       // protected new int Act_DelData = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    this.labelB2CUrl.Text = "http://" + Common.Globals.DomainFullName + MvcApplication.GetCurrentRoutePath(YSWL.Web.AreaRoute.MShop);
                    BindData();

                    if (!string.IsNullOrWhiteSpace(this.Request.Form["Callback"]) && (this.Request.Form["Callback"] == "true"))
                    {
                        this.Controls.Clear();
                        this.DoCallback();
                    }
                }
                catch (Exception ex )
                {
                    Log.LogHelper.AddWarnLog(ex.Message,ex.StackTrace);
                    throw;
                }
             
            }
        }

        #region DataList

        public void BindData()
        {
            //获取该主区域 下的所有模板
            List<YSWL.MALL.Model.Ms.Theme> themeList = YSWL.MALL.Web.Components.FileHelper.GetThemes("MShop");
            //获取当前主模板
            string name = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("MShop_Theme");
            hidCurrentColor.Value = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("MShop_Theme_Color");

           List<YSWL.MALL.Model.Ms.Theme> themeListNew = new List<Model.Ms.Theme>();

            //获取多个颜色的模板列表
            List<string> tList = new List<string>();
            string hasColorThemes = hidHasColorThemes.Value;
            if (!String.IsNullOrWhiteSpace(hasColorThemes))
            {
                tList = hidHasColorThemes.Value.Split(',').ToList();
            }
            YSWL.MALL.Model.Ms.Theme greenItem=null;
            YSWL.MALL.Model.Ms.Theme blueItem=null;
            YSWL.MALL.Model.Ms.Theme orangeItem=null;
            YSWL.MALL.Model.Ms.Theme redItem=null;
            foreach (var item in themeList)
            {
                if (item.Name == name)
                {
                    item.IsCurrent = true;
                }
                if (tList.Contains(item.Name)) {
                    greenItem = new Model.Ms.Theme {
                        ID = item.ID,
                        CreatedDate = item.CreatedDate,
                        Name = item.Name,
                        Author = item.Author,
                        Description = item.Description,
                        PreviewPhotoSrc = item.PreviewPhotoSrc,
                        Remark = "绿色",
                        Color= "green"
                    };
                    if (item.Name == name && hidCurrentColor.Value=="green")
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
                        PreviewPhotoSrc = "/Areas/MShop/Themes/" + item.Name + "/Theme-blue.png",
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
                        PreviewPhotoSrc = "/Areas/MShop/Themes/" + item.Name + "/Theme-orange.png",
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
                        PreviewPhotoSrc = "/Areas/MShop/Themes/" + item.Name + "/Theme-red.png",
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
                    YSWL.MALL.BLL.SysManage.ConfigSystem.Modify("MShop_Theme", cA[0], "微商城模板的名称");
                    if (cA.Length > 1) {
                        YSWL.MALL.BLL.SysManage.ConfigSystem.Modify("MShop_Theme_Color", cA[1], "微商城模板颜色");
                    }
                    Cache.Remove("ConfigSystemHashList");    //清除网站设置的缓存文件
                    MessageBox.ShowSuccessTip(this, "启用成功", "MShopList.aspx");
                }
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            BindData();
        }

        #endregion


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
            string  color = this.Request.Form["color"];
            if (string.IsNullOrWhiteSpace(color))
            {
                json.Put("STATUS", "FAILED");
            } else{
                //写
                YSWL.MALL.BLL.SysManage.ConfigSystem.Modify("MShop_Theme_Color", color, "微商城模板的颜色");
                Cache.Remove("ConfigSystemHashList");    //清除网站设置的缓存文件
                json.Put("STATUS", "SUCCESS");
            }
            return json.ToString();
        }
        #endregion

    }
}