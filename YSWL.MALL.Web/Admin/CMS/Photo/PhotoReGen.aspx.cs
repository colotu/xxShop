using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.Model.SysManage;
using YSWL.MALL.Web.Components.Setting.CMS;

namespace YSWL.MALL.Web.Admin.CMS.Photo
{
    public partial class PhotoReGen :PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 242; } } //CMS_图片管理_缩略图生成页
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

            }
        }


    }
}