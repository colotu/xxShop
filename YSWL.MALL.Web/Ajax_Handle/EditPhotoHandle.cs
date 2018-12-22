using System.Data;
/**
* EditPhoto.cs
*
* 功 能： 编辑图片信息
* 类 名： EditPhoto
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/5/24 15:40:36  伍伟    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System.Web;
using YSWL.MALL.Model.CMS;

namespace YSWL.MALL.Web.Ajax_Handle
{
    public class EditPhotoHandle : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string Action = Request.Params["Action"];
            switch (Action)
            {
                case "EditPhotoName":
                    EditPhotoName(Request, Response);
                    break;
                case "EditCover":
                    EditCover(Request, Response);
                    break;
                case "EditAlbumName":
                    EditAlbumName(Request, Response);
                    break;
                default:
                    break;
            }
        }

        public void EditPhotoName(HttpRequest Request, HttpResponse Response)
        {
            if (!string.IsNullOrWhiteSpace(Request.Params["PhotoName"]) && !string.IsNullOrWhiteSpace(Request.Params["PhotoId"]))
            {
                string PhotoName = Request.Params["PhotoName"];
                int PhotoId = int.Parse(Request.Params["PhotoId"]);

                BLL.CMS.Photo photoBLL = new BLL.CMS.Photo();
                Model.CMS.Photo photoModel = photoBLL.GetModel(PhotoId);
                photoModel.PhotoName = PhotoName;
                Response.Write(!photoBLL.Update(photoModel) ? "" : PhotoName);
            }
        }

        public void EditAlbumName(HttpRequest Request, HttpResponse Response)
        {
            if (!string.IsNullOrWhiteSpace(Request.Params["AlbumName"]) && !string.IsNullOrWhiteSpace(Request.Params["AlbumId"]))
            {
                string AlbumName = Request.Params["AlbumName"];
                int AlbumId = int.Parse(Request.Params["AlbumID"]);

                BLL.CMS.PhotoAlbum AlbumBLL = new BLL.CMS.PhotoAlbum();
                PhotoAlbum AlbumModel = AlbumBLL.GetModel(AlbumId);
                AlbumModel.AlbumName = AlbumName;
                Response.Write(!AlbumBLL.Update(AlbumModel) ? "" : AlbumName);
            }
        }

        public void EditCover(HttpRequest Request, HttpResponse Response)
        {
            BLL.CMS.Photo photoBll = new BLL.CMS.Photo();
            BLL.CMS.PhotoAlbum albumBll = new BLL.CMS.PhotoAlbum();
            int photoId = Common.Globals.SafeInt(Request.Params["PhotoId"], 0);
            DataSet dsPhoto = photoBll.GetList("photoid=" + photoId);
            if (dsPhoto == null) return;
            int albumID = Common.Globals.SafeInt(dsPhoto.Tables[0].Rows[0]["AlbumID"],0);
            PhotoAlbum album = albumBll.GetModel(albumID);
            album.CoverPhoto = photoId;

            Response.Write(albumBll.Update(album) ? "Success" : "Fail");
        }
    }
}