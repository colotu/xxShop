/**
* Show.cs
*
* 功 能： [N/A]
* 类 名： Show.cs
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
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using YSWL.Common;
namespace YSWL.MALL.Web.Admin.Shop.Depot
{
    public partial class Show : PageBaseAdmin
    {
        YSWL.MALL.BLL.Ms.Regions regionsBLL = new BLL.Ms.Regions();
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
					ShowInfo();
			}
		}
        public int DepotId
        {
            get
            {
                int id = 0;
                string strid = Request.Params["id"];
                if (!string.IsNullOrWhiteSpace(strid) )
                {
                    id = Common.Globals.SafeInt(strid,0);
                }
                return id;
            }
        }
	private void ShowInfo()
	{
        YSWL.MALL.BLL.Shop.DisDepot.Depot bll = new BLL.Shop.DisDepot.Depot();
        YSWL.MALL.Model.Shop.DisDepot.Depot model = bll.GetModel(DepotId);
        if (model != null)
        {
            this.lblName.Text = model.Name;
            this.lblCode.Text = model.Code;
            this.lblRegionId.Text = regionsBLL.GetAddress(model.RegionId);
            this.lblAddress.Text = model.Address;
            this.lblContactName.Text = model.ContactName;
            this.lblPhone.Text = model.Phone;
            this.lblEmail.Text = model.Email;
            string statusStr;
            if (model.Status == 0)
            {
                statusStr = "未启用";
            }
            else
            {
                statusStr = "启用";
            }
            this.lblStatus.Text = statusStr;
            this.lblHelpCode.Text = model.HelpCode;
            this.lblCreatedDate.Text = model.CreatedDate.ToString("yyyy-MM-DD HH:mm:ss");
            this.lblLatitude.Text = model.Latitude.ToString();
            this.lblLongitude.Text = model.Longitude.ToString();
            this.lblType.Text = model.Type.ToString();
            string deattr;
            if (model.DepotAttr == 0)
            {
                deattr = "自己的仓库";
            }
            else
            {
                deattr = "第三方";
            }
            this.lblDepotAttr.Text = deattr;
            this.lblRemark.Text = model.Remark;

        }
    
	}


 
    }
}
