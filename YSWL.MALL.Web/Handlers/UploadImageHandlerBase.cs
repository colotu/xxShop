/**
* UploadImageHandlerBase.cs
*
* 功 能： 上传图片Handler基类
* 类 名： UploadImageHandlerBase
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/6/4 17:06:00    Ben     初版
* V0.02  2012/10/16 18:45:00  Ben     上传图片基类再次抽离出上传文件基类
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System.Drawing;
using System.Web;
using YSWL.Common;
using System.Collections.Generic;
using YSWL.MALL.Model.Ms;
using YSWL.MALL.Model.SysManage;
using YSWL.MALL.Web.Components;

namespace YSWL.MALL.Web.Handlers
{
    public abstract class UploadImageHandlerBase : UploadHandlerBase
    {
        protected readonly MakeThumbnailMode ThumbnailMode;

        protected override string[] AllowFileExt
        {
            get
            {
                return ".jpg|.jpeg|.gif|.png|.bmp".Split('|');
            }
        }

        public UploadImageHandlerBase(MakeThumbnailMode mode = MakeThumbnailMode.None, bool isLocalSave = true,ApplicationKeyType applicationKeyType = ApplicationKeyType.None)
            : base(isLocalSave,applicationKeyType)
        {
            ThumbnailMode = mode == MakeThumbnailMode.None ? MakeThumbnailMode.W : mode;
        }

        #region 子类实现
        /// <summary>
        /// 获取常规缩略图尺寸
        /// </summary>
        protected virtual List<YSWL.MALL.Model.Ms.ThumbnailSize> GetThumSizeList()
        {
            //TODO: 缩略图采用被动方式生成 BEN ADD 2012-11-24
            return new List<ThumbnailSize>();
        }

        /// <summary>
        /// 临时保存原文件并生成缩略图
        /// </summary>
        protected override void SaveAs(string uploadPath, string fileName, HttpPostedFile file)
        {
            //保存临时原图
            file.SaveAs(uploadPath + fileName);
            //生成临时缩略图
            MakeThumbnailList(uploadPath, fileName, GetThumSizeList());
        }

        #endregion

        #region 生成缩略图

        /// <summary>
        /// 生成缩略图
        /// </summary>
        protected virtual void MakeThumbnailList(string uploadPath, string fileName, List<ThumbnailSize> thumSizeList)
        {
            //TODO: 预生成缩略图方法仅考虑最基本尺寸, 更多尺寸请重写此方法 BEN ADD 2012-11-24
        
            bool isAddWater = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_ThumbImage_AddWater");
            //原图水印保存地址
            string imagePath = uploadPath;
            if (isAddWater)
            {
                imagePath = uploadPath + "W_";
                //生成临时原图水印图
                FileHelper.MakeWater(uploadPath + fileName, imagePath + fileName);
            }
            if(thumSizeList!=null&&thumSizeList.Count>0)
            {
                foreach (var thumbnailSize in thumSizeList)
                {
                    ImageTools.MakeThumbnail(imagePath + fileName, uploadPath + thumbnailSize.ThumName + fileName, thumbnailSize.ThumWidth, thumbnailSize.ThumHeight, GetThumMode(thumbnailSize.ThumMode));
                }
            }
        }
        protected virtual void MakeThumbnail(string uploadPath, string fileName, string thumName, int thumWidth, int thumHeight, MakeThumbnailMode mode)
        {
            //TODO: 预生成缩略图方法仅考虑最基本尺寸, 更多尺寸请重写此方法 BEN ADD 2012-11-24
            ImageTools.MakeThumbnail(uploadPath + fileName, uploadPath + thumName + fileName, thumWidth, thumHeight, mode);
        }
        /// <summary>
        /// 获取裁剪模式
        /// </summary>
        /// <param name="ThumMode"></param>
        /// <returns></returns>
        protected MakeThumbnailMode GetThumMode(int ThumMode)
        {
            MakeThumbnailMode mode = MakeThumbnailMode.None;
            switch (ThumMode)
            {
                case 0:
                    mode = MakeThumbnailMode.Auto;
                    break;
                case 1:
                    mode = MakeThumbnailMode.Cut;
                    break;
                case 2:
                    mode = MakeThumbnailMode.H;
                    break;
                case 3:
                    mode = MakeThumbnailMode.HW;
                    break;
                case 4:
                    mode = MakeThumbnailMode.W;
                    break;
                default:
                    mode = MakeThumbnailMode.Auto;
                    break;
            }
            return mode;
        }

        #endregion
    }
}