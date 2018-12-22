/**
* UploadMarkImgHandlerBase.cs
*
* 功 能： 地图Ajax上传Marker图片基类
* 类 名： UploadMarkImgHandlerBase
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/5/30 21:06:25   Ben    初版
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace YSWL.Map.Handler
{
    public abstract class UploadMarkImgHandlerBase : IHttpHandler, IRequiresSessionState
    {
        protected readonly string UploadFolder = "/" + YSWL.Common.ConfigHelper.GetConfigString("UploadFolder") + "/";
        protected readonly string MarkerImagesPath = "Images/MapMarkers";

        protected static readonly string[] AllowFileExt = ".jpg|.jpeg|.png|.gif|.bmp".Split('|');

        #region IHttpHandler 成员

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            Upload(context);
        }

        #endregion

        protected virtual void Upload(HttpContext context)
        {
            HttpPostedFile file = context.Request.Files["Filedata"];
            string uploadPath = this.UploadFolder + this.MarkerImagesPath + "/";
            string localUploadPath = HttpContext.Current.Server.MapPath(uploadPath);
            string fileFullName = string.Empty;

            if (file == null)
            {
                context.Response.Write("0");
                return;
            }

            if (!Directory.Exists(localUploadPath)) Directory.CreateDirectory(localUploadPath);

            string path = file.FileName;
            if (path.Length < 1)
            {
                context.Response.Write("0");
                return;
            }

            string fileExt = Path.GetExtension(path).ToLower();
            if (!AllowFileExt.Contains(fileExt))
            {
                context.Response.Clear();
                return;
            }

            string filename = Guid.NewGuid() + fileExt;
            fileFullName = localUploadPath + filename;
            file.SaveAs(fileFullName);

            context.Response.Write("1|" + uploadPath + filename);
        }
    }
}
