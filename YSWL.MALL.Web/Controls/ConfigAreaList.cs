using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace YSWL.MALL.Web.Controls
{
    public  class ConfigAreaList : DropDownList
    {
        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                base.OnLoad(e);
                this.DataSource = YSWL.MALL.BLL.SysManage.ConfigArea.GetAllArea();
                this.DataTextField = "AreaName";
                this.DataValueField = "AreaName";
                this.DataBind();
                if (VisibleAll)
                {
                    this.Items.Insert(0,new ListItem("全部",""));
                }
            }
        }
        #region 属性
        public int AreaInt
        {
            get { return YSWL.MALL.BLL.SysManage.ConfigArea.GetAreaInt(this.SelectedValue); }
        }
        public string AreaName
        {
            set
            {
                this.SelectedValue = value.ToString();
            }
            get { return this.SelectedValue; }
        }

        public bool VisibleAll
        {
            set; 
            get; 
        }

        #endregion
    }
}