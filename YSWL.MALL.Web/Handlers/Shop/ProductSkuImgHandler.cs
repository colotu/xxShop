using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YSWL.Common;
using YSWL.Json;

namespace YSWL.MALL.Web.Handlers.Shop
{
    public class ProductSkuImgHandler : UploadImageHandlerBase
    {

        protected ProductSkuImgHandler() : base(MakeThumbnailMode.Auto)
        {
        }

        public const string POLL_KEY_DATA = "data";
        public const string POLL_KEY_SUCCESS = "success";

        protected override void SaveAs(string uploadPath, string fileName, HttpPostedFile file)
        {
            file.SaveAs(uploadPath + fileName);
            //对生成水印之后的图片进行缩略
            ImageTools.MakeThumbnail(uploadPath + fileName, uploadPath + "T32X32_" + fileName, 32, 32, MakeThumbnailMode.HW);
            ImageTools.MakeThumbnail(uploadPath + fileName, uploadPath + "T130X130_" + fileName, 130, 130, MakeThumbnailMode.HW);
            ImageTools.MakeThumbnail(uploadPath + fileName, uploadPath + "T300X390_" + fileName, 300, 390, MakeThumbnailMode.Cut);
            ImageTools.MakeThumbnail(uploadPath + fileName, uploadPath + "T350X350_" + fileName, 350, 350, MakeThumbnailMode.HW);
            ImageTools.MakeThumbnail(uploadPath + fileName, uploadPath + "T240X300_" + fileName, 240, 300, MakeThumbnailMode.Cut);//M4 模板使用
        }
        protected override void ProcessSub(HttpContext context, string uploadPath, string fileName)
        {
            try
            {
                JsonObject json = new JsonObject();
                json.Put(POLL_KEY_SUCCESS, true);
                json.Put(POLL_KEY_DATA, uploadPath + "{0}" + fileName);
                context.Response.Write(json.ToString());
            }
            catch (Exception)
            {

            }
        }
        

    }
}
