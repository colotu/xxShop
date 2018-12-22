/**
* ImageTools.cs
*
* 功 能： 缩略图功能
* 类 名： ImageTools
*
* Ver变更日期负责人：  变更内容
* ───────────────────────────────────
* V0.01  2012/05/22 16:28:49  蒋海滨初版
* V0.02  2012/06/08 17:50:00  Ben缩略图使用枚举参数 和 原方法已过时的处理
* V0.03  2012/08/14 20:15:07  Ben更正默认缩略图为png, 解决透明区域缩略后为黑色问题
* V0.04  2012/08/15 16:18:51  Ben新增ImageFormat imageFormat重载参数, 可自定义缩略图的文件格式.
* V0.05  2013/01/23 17:00:00  Ben新增生成缩略图同时生成水印合并方法
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace YSWL.Common
{
    /// <summary>
    /// 图片工具类
    /// </summary>
    public class ImageTools
    {
        //TODO: 图片生成类应加入图片无损压缩代码 BEN ADD 2013-03-18

        #region 生成缩略图

        #region 已过时
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>
        [Obsolete]
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            MakeThumbnailMode makeThumbnailMode = MakeThumbnailMode.None;
            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）
                    makeThumbnailMode = MakeThumbnailMode.HW;
                    break;
                case "W"://指定宽，高按比例
                    makeThumbnailMode = MakeThumbnailMode.W;
                    break;
                case "H"://指定高，宽按比例
                    makeThumbnailMode = MakeThumbnailMode.H;
                    break;
                case "Cut"://指定高宽裁减（不变形） 
                    makeThumbnailMode = MakeThumbnailMode.Cut;
                    break;
                default:
                    break;
            }
            MakeThumbnail(originalImagePath, thumbnailPath, width, height, makeThumbnailMode);
        }
        #endregion

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>
        /// <param name="interpolationMode">指定在缩放或旋转图像时使用的算法</param>
        /// <param name="smoothingMode">指定是否将平滑处理（抗锯齿）应用于直线、曲线和已填充区域的边缘</param>
        /// <remarks>缩略图默认png格式</remarks>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height,
                                         MakeThumbnailMode mode,
                                         InterpolationMode interpolationMode = InterpolationMode.High,
                                         SmoothingMode smoothingMode = SmoothingMode.HighQuality)
        {

            string fileExtemsion = System.IO.Path.GetExtension(originalImagePath).ToLower();
            ImageFormat imgF = ImageFormat.Png;
            switch (fileExtemsion) { 
                case ".gif":
                    imgF = ImageFormat.Gif;
                    break;
                case ".jpg":
                    imgF = ImageFormat.Jpeg;
                    break;
                case ".bmp":
                    imgF = ImageFormat.Bmp;
                    break;
                case ".png":
                    imgF = ImageFormat.Png;
                    break;
            }
            MakeThumbnail(originalImagePath, thumbnailPath, width, height, mode,
                imgF,
                interpolationMode, smoothingMode);
        }


        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>
        /// <param name="imageFormat">图片保存格式</param>
        /// <param name="interpolationMode">指定在缩放或旋转图像时使用的算法</param>
        /// <param name="smoothingMode">指定是否将平滑处理（抗锯齿）应用于直线、曲线和已填充区域的边缘</param>
        /// <remarks>当原图尺寸小于指定缩略尺寸时, 不进行缩略, 直接输出原图, 并且不更改尺寸.</remarks>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height,
                                         MakeThumbnailMode mode, System.Drawing.Imaging.ImageFormat imageFormat,
                                         InterpolationMode interpolationMode = InterpolationMode.High,
                                         SmoothingMode smoothingMode = SmoothingMode.HighQuality)
        {
            using (Image originalImage = Image.FromFile(originalImagePath))
            {
                Graphics graphics;
                Bitmap bitmap = GetThumbnail(originalImage, width, height, mode, out graphics,
                    interpolationMode, smoothingMode);
                try
                {
                    //以xxx格式保存缩略图
                    bitmap.Save(thumbnailPath, imageFormat);
                }
                finally
                {
                    bitmap.Dispose();
                    graphics.Dispose();
                }
            }
        }

        /// <summary>
        /// 获取缩略图对象
        /// </summary>
        /// <param name="originalImage">源图对象</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>
        /// <param name="graphics">输出绘图对象, 可继续追加水印</param>
        /// <param name="interpolationMode">指定在缩放或旋转图像时使用的算法</param>
        /// <param name="smoothingMode">指定是否将平滑处理（抗锯齿）应用于直线、曲线和已填充区域的边缘</param>
        /// <remarks>当原图尺寸小于指定缩略尺寸时, 不进行缩略, 直接输出原图, 并且不更改尺寸.</remarks>
        public static Bitmap GetThumbnail(Image originalImage, int width, int height,
                MakeThumbnailMode mode, out Graphics graphics,
            //DONE: 由外部设置缩略图的清晰度, 拟减少文件大小 BEN ADD 20130123 22:41
            //TODO: 参数太多, 用对象封装, 拟减少调用的痛苦程度 BEN ADD 20130124 16:40
                InterpolationMode interpolationMode = InterpolationMode.High,
                SmoothingMode smoothingMode = SmoothingMode.HighQuality)
        {
            int towidth = width;
            int toheight = height;
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            //TODO: Auto目前按照容器宽度进行等比例判断, 这是错误的, 应该根据原图的宽高比进行判断
            if (mode == MakeThumbnailMode.Auto)
            {
                if (towidth > toheight)
                {
                    mode = MakeThumbnailMode.W;
                }
                else
                {
                    mode = MakeThumbnailMode.H;
                }
            }
            //当原图尺寸小于指定缩略尺寸时, 不进行缩略, 直接输出原图, 并且不更改尺寸.
            if (ow < towidth && oh < toheight)
            {
                towidth = ow;
                toheight = oh;
            }
            else
            {
                switch (mode)
                {
                    case MakeThumbnailMode.HW: //指定高宽缩放（可能变形）
                        break;
                    case MakeThumbnailMode.W: //指定宽，高按比例
                        toheight = originalImage.Height * width / originalImage.Width;
                        break;
                    case MakeThumbnailMode.H: //指定高，宽按比例
                        towidth = originalImage.Width * height / originalImage.Height;
                        break;
                    case MakeThumbnailMode.Cut: //指定高宽裁减（不变形）
                        if ((double)originalImage.Width / (double)originalImage.Height >
                        (double)towidth / (double)toheight)
                        {
                            oh = originalImage.Height;
                            ow = originalImage.Height * towidth / toheight;
                            y = 0;
                            x = (originalImage.Width - ow) / 2;
                        }
                        else
                        {
                            ow = originalImage.Width;
                            oh = originalImage.Width * height / towidth;
                            x = 0;
                            y = (originalImage.Height - oh) / 2;
                        }
                        break;
                    default:
                        break;
                }
            }
            //新建一个bmp图片
            Bitmap bitmap = new System.Drawing.Bitmap(towidth, toheight);
            bitmap.MakeTransparent(Color.Transparent);
            //新建一个画板
            graphics = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法
            graphics.InterpolationMode = interpolationMode;
            //设置高质量,低速度呈现平滑程度
            graphics.SmoothingMode = smoothingMode;
            //清空画布并以透明背景色填充
            graphics.Clear(Color.Transparent);
            //在指定位置并且按指定大小绘制原图片的指定部分
            graphics.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
            new Rectangle(x, y, ow, oh),
            GraphicsUnit.Pixel);
            return bitmap;
        }

        #region 备用方案
        ///// <summary>创建规定大小的图像/// 源图像只能是JPG格式  
        ///// </summary>  
        ///// <param name="oPath">源图像绝对路径</param>  
        ///// <param name="tPath">生成图像绝对路径</param>  
        ///// <param name="width">生成图像的宽度</param>  
        ///// <param name="height">生成图像的高度</param>  
        //public void CreateImage(string oPath, string tPath, int width, int height)
        //{
        //Bitmap originalBmp = new Bitmap(oPath);
        //// 源图像在新图像中的位置  
        //int left, top;


        //if (originalBmp.Width <= width && originalBmp.Height <= height)
        //{
        //// 原图像的宽度和高度都小于生成的图片大小  
        //left = (int)Math.Round((decimal)(width - originalBmp.Width) / 2);
        //top = (int)Math.Round((decimal)(height - originalBmp.Height) / 2);


        //// 最终生成的图像  
        //Bitmap bmpOut = new Bitmap(width, height);
        //using (Graphics graphics = Graphics.FromImage(bmpOut))
        //{
        //// 设置高质量插值法  
        //graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //// 清空画布并以白色背景色填充  
        //graphics.Clear(Color.White);
        //// 把源图画到新的画布上  
        //graphics.DrawImage(originalBmp, left, top, originalBmp.Width, originalBmp.Height);
        //}
        //bmpOut.Save(tPath);
        //bmpOut.Dispose();
        //originalBmp.Dispose();
        //return;
        //}


        //// 新图片的宽度和高度，如400*200的图像，想要生成160*120的图且不变形，  
        //// 那么生成的图像应该是160*80，然后再把160*80的图像画到160*120的画布上  
        //int newWidth, newHeight;
        //if (width * originalBmp.Height < height * originalBmp.Width)
        //{
        //newWidth = width;
        //newHeight = (int)Math.Round((decimal)originalBmp.Height * width / originalBmp.Width);
        //// 缩放成宽度跟预定义的宽度相同的，即left=0，计算top  
        //left = 0;
        //top = (int)Math.Round((decimal)(height - newHeight) / 2);
        //}
        //else
        //{
        //newWidth = (int)Math.Round((decimal)originalBmp.Width * height / originalBmp.Height);
        //newHeight = height;
        //// 缩放成高度跟预定义的高度相同的，即top=0，计算left  
        //left = (int)Math.Round((decimal)(width - newWidth) / 2);
        //top = 0;
        //}


        //// 生成按比例缩放的图，如：160*80的图  
        //Bitmap bmpOut2 = new Bitmap(newWidth, newHeight);
        //using (Graphics graphics = Graphics.FromImage(bmpOut2))
        //{
        //graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //graphics.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
        //graphics.DrawImage(originalBmp, 0, 0, newWidth, newHeight);
        //}
        //// 再把该图画到预先定义的宽高的画布上，如160*120  
        //Bitmap lastbmp = new Bitmap(width, height);
        //using (Graphics graphics = Graphics.FromImage(lastbmp))
        //{
        //// 设置高质量插值法  
        //graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //// 清空画布并以白色背景色填充  
        //graphics.Clear(Color.White);
        //// 把源图画到新的画布上  
        //graphics.DrawImage(bmpOut2, left, top);
        //}
        //lastbmp.Save(tPath);
        //lastbmp.Dispose();
        //originalBmp.Dispose();
        //}

        ///// <summary>
        ///// Creates a thumbnail from an existing image. Sets the biggest dimension of the
        ///// thumbnail to either desiredWidth or Height and scales the other dimension down
        ///// to preserve the aspect ratio
        ///// </summary>
        ///// <param name="imageStream">stream to create thumbnail for</param>
        ///// <param name="desiredWidth">maximum desired width of thumbnail</param>
        ///// <param name="desiredHeight">maximum desired height of thumbnail</param>
        ///// <returns>Bitmap thumbnail</returns>
        //public Bitmap CreateThumbnail(Bitmap originalBmp, int desiredWidth, int desiredHeight)
        //{
        //// If the image is smaller than a thumbnail just return it
        //if (originalBmp.Width <= desiredWidth && originalBmp.Height <= desiredHeight)
        //{
        //return originalBmp;
        //}

        //int newWidth, newHeight;

        //// scale down the smaller dimension
        //if (desiredWidth * originalBmp.Height < desiredHeight * originalBmp.Width)
        //{
        //newWidth = desiredWidth;
        //newHeight = (int)Math.Round((decimal)originalBmp.Height * desiredWidth / originalBmp.Width);
        //}
        //else
        //{
        //newHeight = desiredHeight;
        //newWidth = (int)Math.Round((decimal)originalBmp.Width * desiredHeight / originalBmp.Height);
        //}

        //// This code creates cleaner (though bigger) thumbnails and properly
        //// and handles GIF files better by generating a white background for
        //// transparent images (as opposed to black)
        //// This is preferred to calling Bitmap.GetThumbnailImage()
        //Bitmap bmpOut = new Bitmap(newWidth, newHeight);

        //using (Graphics graphics = Graphics.FromImage(bmpOut))
        //{
        //graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //graphics.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
        //graphics.DrawImage(originalBmp, 0, 0, newWidth, newHeight);
        //}

        //return bmpOut;
        //} 
        #endregion
        #endregion

        #region 生成缩略图同时生成水印

        /// <summary>
        /// 生成文字水印缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">水印缩略图保存路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>
        /// <param name="watermarkText">水印文字</param>
        /// <param name="watermarkPosition">水印位置</param>
        /// <param name="fontFamily">水印字体样式</param>
        /// <param name="fontSize">水印字体大小</param>
        /// <param name="fontColor">水印字体颜色</param>
        /// <param name="interpolationMode">指定在缩放或旋转图像时使用的算法</param>
        /// <param name="smoothingMode">指定是否将平滑处理（抗锯齿）应用于直线、曲线和已填充区域的边缘</param>
        /// <remarks>缩略图默认png格式</remarks>
        public static void MakeTextWaterThumbnail(string originalImagePath, string thumbnailPath, int width, int height,
                                                  MakeThumbnailMode mode, string watermarkText,
                                                  string watermarkPosition = "WM_CENTER", string fontFamily = "arial",
                                                  int fontSize = 14, string fontColor = "#FFFFFF",
                                                  InterpolationMode interpolationMode = InterpolationMode.High,
                                                  SmoothingMode smoothingMode = SmoothingMode.HighQuality)
        {
            MakeTextWaterThumbnail(originalImagePath, thumbnailPath, width, height, mode,
                ImageFormat.Png, watermarkText, watermarkPosition, fontFamily, fontSize, fontColor,
                interpolationMode, smoothingMode);
        }

        /// <summary>
        /// 生成文字水印缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">水印缩略图保存路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>
        /// <param name="imageFormat">图片保存格式</param>
        /// <param name="watermarkText">水印文字</param>
        /// <param name="watermarkPosition">水印位置</param>
        /// <param name="fontFamily">水印字体样式</param>
        /// <param name="fontSize">水印字体大小</param>
        /// <param name="fontColor">水印字体颜色</param>
        /// <param name="interpolationMode">指定在缩放或旋转图像时使用的算法</param>
        /// <param name="smoothingMode">指定是否将平滑处理（抗锯齿）应用于直线、曲线和已填充区域的边缘</param>
        /// <remarks>缩略图默认png格式</remarks>
        public static void MakeTextWaterThumbnail(string originalImagePath, string thumbnailPath, int width, int height,
                                    MakeThumbnailMode mode, ImageFormat imageFormat, string watermarkText,
                                    string watermarkPosition = "WM_CENTER", string fontFamily = "arial",
                                    int fontSize = 14, string fontColor = "#FFFFFF",
                                    InterpolationMode interpolationMode = InterpolationMode.High,
                                    SmoothingMode smoothingMode = SmoothingMode.HighQuality)
        {
            //using (Image originalImage = Image.FromFile(originalImagePath))
            //{
            
                //Bitmap bitmap = GetThumbnail(originalImage, width, height, mode, out picture,
                //                             interpolationMode, smoothingMode);

                //Font crFont = new Font(fontFamily, fontSize);
                //SizeF crSize = picture.MeasureString(watermarkText, crFont);

                //float xpos = 0;
                //float ypos = 0;
                //switch (watermarkPosition)
                //{
                //    case "WM_TOP_LEFT":
                //        xpos = (crSize.Width / 2);
                //        ypos = 8;
                //        break;
                //    case "WM_TOP_RIGHT":
                //        xpos = ((float)bitmap.Width * (float)1.00) - (crSize.Width / 2);
                //        ypos = 8;
                //        break;
                //    case "WM_BOTTOM_RIGHT":
                //        xpos = ((float)bitmap.Width * (float)1.00) - (crSize.Width / 2);
                //        ypos = ((float)bitmap.Height * (float)1.00) - (crSize.Height);
                //        break;
                //    case "WM_BOTTOM_LEFT":
                //        xpos = (crSize.Width / 2);
                //        ypos = ((float)bitmap.Height * (float)1.00) - crSize.Height;
                //        break;
                //    case "WM_CENTER":
                //        xpos = ((float)bitmap.Width * (float).50);
                //        ypos = ((float)bitmap.Height * (float).50) - (crSize.Height / 2);
                //        break;
                //}
                //StringFormat strFormat = new StringFormat();
                //strFormat.Alignment = StringAlignment.Center;
                //SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));
                //picture.DrawString(watermarkText, crFont, semiTransBrush2, xpos + 1, ypos + 1, strFormat);

                //SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, ColorTranslator.FromHtml(fontColor)));
                //picture.DrawString(watermarkText, crFont, semiTransBrush, xpos, ypos, strFormat);

                //try
                //{
                //    //以xxx格式保存缩略+水印图
                //    bitmap.Save(thumbnailPath, imageFormat);
                //}
                //finally
                //{
                //    semiTransBrush2.Dispose();
                //    semiTransBrush.Dispose();
                //    bitmap.Dispose();
                //    picture.Dispose();
                //}
                string tempPath="/Upload/Temp/"+new Guid()+".jpg";
                addWatermarkText(originalImagePath, HttpContext.Current.Server.MapPath(tempPath), watermarkText, watermarkPosition, fontFamily, fontSize, fontColor, interpolationMode, smoothingMode);
                MakeThumbnail(HttpContext.Current.Server.MapPath(tempPath), thumbnailPath, width, height, mode);
            //}
        }

        /// <summary>
        /// 生成图片水印缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">水印缩略图保存路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>
        /// <param name="waterMarkPicPath">水印图片</param>
        /// <param name="watermarkPosition">水印位置</param>
        /// <param name="transparentValue">水印透明度</param>
        /// <param name="interpolationMode">指定在缩放或旋转图像时使用的算法</param>
        /// <param name="smoothingMode">指定是否将平滑处理（抗锯齿）应用于直线、曲线和已填充区域的边缘</param>
        /// <remarks>缩略图默认png格式</remarks>
        public static void MakeImageWaterThumbnail(string originalImagePath, string thumbnailPath, int width, int height,
                                                   MakeThumbnailMode mode, string waterMarkPicPath,
                                                   string watermarkPosition = "WM_CENTER", int transparentValue = 30,
                                                   InterpolationMode interpolationMode = InterpolationMode.High,
                                                   SmoothingMode smoothingMode = SmoothingMode.HighQuality)
        {
            MakeImageWaterThumbnail(originalImagePath, thumbnailPath, width, height, mode, ImageFormat.Png,
                waterMarkPicPath, watermarkPosition, transparentValue,
                                             interpolationMode, smoothingMode);
        }

        /// <summary>
        /// 生成图片水印缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">水印缩略图保存路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>
        /// <param name="imageFormat">图片保存格式</param>
        /// <param name="waterMarkPicPath">水印图片</param>
        /// <param name="watermarkPosition">水印位置</param>
        /// <param name="transparentValue">水印透明度</param>
        /// <param name="interpolationMode">指定在缩放或旋转图像时使用的算法</param>
        /// <param name="smoothingMode">指定是否将平滑处理（抗锯齿）应用于直线、曲线和已填充区域的边缘</param>
        /// <remarks>缩略图默认png格式</remarks>
        public static void MakeImageWaterThumbnail(string originalImagePath, string thumbnailPath, int width, int height,
                                                  MakeThumbnailMode mode, ImageFormat imageFormat, string waterMarkPicPath,
                                                  string watermarkPosition = "WM_CENTER", int transparentValue = 30,
                                                  InterpolationMode interpolationMode = InterpolationMode.High,
                                                  SmoothingMode smoothingMode = SmoothingMode.HighQuality)
        {
            using (Image originalImage = Image.FromFile(originalImagePath))
            {
                Graphics picture;
                Bitmap bitmap = GetThumbnail(originalImage, width, height, mode, out picture,
                                             interpolationMode, smoothingMode);

                Image watermark = new Bitmap(waterMarkPicPath);
                ImageAttributes imageAttributes = new ImageAttributes();
                ColorMap colorMap = new ColorMap();
                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                ColorMap[] remapTable = { colorMap };
                imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
                float tranValue = (float)transparentValue / 100;
                float[][] colorMatrixElements = {
                                                 new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                              new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                 new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                               new float[] {0.0f,  0.0f,  0.0f,  tranValue, 0.0f},//透明度默认值为0.3
                                               new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                            };

                ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                int xpos = 0;
                int ypos = 0;
                int watermarkWidth = 0;
                int watermarkHeight = 0;
                double bl = 1d;
                //计算水印图片的比率
                //取背景的1/4宽度来比较
                if ((bitmap.Width > watermark.Width * 4) && (bitmap.Height > watermark.Height * 4))
                {
                    bl = 1;
                }
                else if ((bitmap.Width > watermark.Width * 4) && (bitmap.Height < watermark.Height * 4))
                {
                    bl = Convert.ToDouble(bitmap.Height / 4) / Convert.ToDouble(watermark.Height);
                }
                else

                    if ((bitmap.Width < watermark.Width * 4) && (bitmap.Height > watermark.Height * 4))
                    {
                        bl = Convert.ToDouble(bitmap.Width / 4) / Convert.ToDouble(watermark.Width);
                    }
                    else
                    {
                        if ((bitmap.Width * watermark.Height) > (bitmap.Height * watermark.Width))
                        {
                            bl = Convert.ToDouble(bitmap.Height / 4) / Convert.ToDouble(watermark.Height);
                        }
                        else
                        {
                            bl = Convert.ToDouble(bitmap.Width / 4) / Convert.ToDouble(watermark.Width);
                        }
                    }

                watermarkWidth = Convert.ToInt32(watermark.Width * bl);
                watermarkHeight = Convert.ToInt32(watermark.Height * bl);
                switch (watermarkPosition)
                {
                    case "WM_TOP_LEFT":
                        xpos = 10;
                        ypos = 10;
                        break;
                    case "WM_TOP_RIGHT":
                        xpos = bitmap.Width - watermarkWidth - 10;
                        ypos = 10;
                        break;
                    case "WM_BOTTOM_RIGHT":
                        xpos = bitmap.Width - watermarkWidth - 10;
                        ypos = bitmap.Height - watermarkHeight - 10;
                        break;
                    case "WM_BOTTOM_LEFT":
                        xpos = 10;
                        ypos = bitmap.Height - watermarkHeight - 10;
                        break;
                    case "WM_CENTER":
                        xpos = bitmap.Width / 2 - (watermarkWidth / 2);
                        ypos = bitmap.Height / 2 - (watermarkHeight / 2);
                        break;
                }
                picture.DrawImage(watermark, new Rectangle(xpos, ypos, watermarkWidth, watermarkHeight), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);

                try
                {
                    //以xxx格式保存缩略+水印图
                    bitmap.Save(thumbnailPath, imageFormat);
                }
                finally
                {
                    watermark.Dispose();
                    imageAttributes.Dispose();
                    bitmap.Dispose();
                    picture.Dispose();
                }
            }
        }
        #endregion

        #region 仅生成水印
        /// <summary>
        /// 文字水印
        /// </summary>
        /// <param name="picture">添加水印的图片</param>
        /// <param name="_watermarkText">水印文字</param>
        /// <param name="_watermarkPosition">水印位置</param>
        /// <param name=" image.Width">宽度</param>
        /// <param name="image.Height">高度</param>
        /// <param name="interpolationMode">指定在缩放或旋转图像时使用的算法</param>
        /// <param name="smoothingMode">指定是否将平滑处理（抗锯齿）应用于直线、曲线和已填充区域的边缘</param>
        /// <param name="pixelFormat">指定图像中每个像素的颜色数据的格式</param>
        public static void addWatermarkText(string oldpath, string newpath, string _watermarkText,
            string _watermarkPosition = "WM_CENTER", string fontStyle = "arial", int fontSize = 14,
            string color = "#FFFFFF",
            InterpolationMode interpolationMode = InterpolationMode.High,
            SmoothingMode smoothingMode = SmoothingMode.HighQuality,
            PixelFormat pixelFormat = PixelFormat.Format24bppRgb,
            int alpha = 156)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(oldpath);
            Bitmap b = new Bitmap(image.Width, image.Height, pixelFormat);
            Graphics picture = Graphics.FromImage(b);
            picture.Clear(Color.White);
            picture.SmoothingMode = smoothingMode;
            picture.InterpolationMode = interpolationMode;
            //picture.DrawImage(image, 0, 0, image.Width, image.Height);
            picture.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0,
                image.Width, image.Height, GraphicsUnit.Pixel);

            Font crFont = null;
            SizeF crSize = new SizeF();
            crFont = new Font(fontStyle, fontSize);
            crSize = picture.MeasureString(_watermarkText, crFont);

            float xpos = 0;
            float ypos = 0;
            switch (_watermarkPosition)
            {
                case "WM_TOP_LEFT":
                    xpos = (crSize.Width / 2);
                    ypos = 8;
                    break;
                case "WM_TOP_RIGHT":
                    xpos = ((float)image.Width * (float)1.00) - (crSize.Width / 2);
                    ypos = 8;
                    break;
                case "WM_BOTTOM_RIGHT":
                    //xpos = ((float)image.Width * (float)1.00) - (crSize.Width / 2);
                    //ypos = ((float)image.Height * (float)1.00) - (crSize.Height);
                    ypos = image.Height - crSize.Height;
                    xpos = image.Width - crSize.Width;
                    break;
                case "WM_BOTTOM_LEFT":
                    xpos = (crSize.Width / 2);
                    ypos = ((float)image.Height * (float)1.00) - crSize.Height;
                    break;
                case "WM_CENTER":
                    xpos = ((float)image.Width * (float).50);
                    ypos = ((float)image.Height * (float).50) - (crSize.Height / 2);
                    break;
            }
            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;
            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(alpha, 0, 0, 0));
            picture.DrawString(_watermarkText, crFont, semiTransBrush2, xpos + 1, ypos + 1, StrFormat);

            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(alpha, ColorTranslator.FromHtml(color)));
            picture.DrawString(_watermarkText, crFont, semiTransBrush, xpos, ypos, StrFormat);
            semiTransBrush2.Dispose();
            semiTransBrush.Dispose();

            b.Save(newpath, System.Drawing.Imaging.ImageFormat.Jpeg);
            b.Dispose();
            image.Dispose();
        }

        /// <summary>
        /// 图片水印
        /// </summary>
        /// <param name="picture">添加水印的图片</param>
        /// <param name="WaterMarkPicPath">水印图片</param>
        /// <param name="_watermarkPosition">水印位置</param>
        /// <param name=" image.Width">宽度</param>
        /// <param name="image.Height">高度</param>
        /// <param name="interpolationMode">指定在缩放或旋转图像时使用的算法</param>
        /// <param name="smoothingMode">指定是否将平滑处理（抗锯齿）应用于直线、曲线和已填充区域的边缘</param>
        /// <param name="pixelFormat">指定图像中每个像素的颜色数据的格式</param>
        public static void addWatermarkImage(string oldpath, string newpath, string WaterMarkPicPath,
            string _watermarkPosition = "WM_CENTER", int transparentValue = 30,
            InterpolationMode interpolationMode = InterpolationMode.High,
            SmoothingMode smoothingMode = SmoothingMode.HighQuality,
            PixelFormat pixelFormat = PixelFormat.Format24bppRgb)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(oldpath);
            Bitmap b = new Bitmap(image.Width, image.Height, pixelFormat);
            Graphics picture = Graphics.FromImage(b);
            picture.Clear(Color.White);
            picture.SmoothingMode = smoothingMode;
            picture.InterpolationMode = interpolationMode;
            picture.DrawImage(image, 0, 0, image.Width, image.Height);

            Image watermark = new Bitmap(WaterMarkPicPath);
            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };
            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
            float tranValue = (float)transparentValue / 100;
            float[][] colorMatrixElements = {
                                                 new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                              new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                 new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                               new float[] {0.0f,  0.0f,  0.0f,  tranValue, 0.0f},//透明度默认值为0.3
                                               new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                            };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            int xpos = 0;
            int ypos = 0;
            int WatermarkWidth = 0;
            int WatermarkHeight = 0;
            double bl = 1d;
            //计算水印图片的比率
            //取背景的1/4宽度来比较
            if ((image.Width > watermark.Width * 4) && (image.Height > watermark.Height * 4))
            {
                bl = 1;
            }
            else if ((image.Width > watermark.Width * 4) && (image.Height < watermark.Height * 4))
            {
                bl = Convert.ToDouble(image.Height / 4) / Convert.ToDouble(watermark.Height);
            }
            else

                if ((image.Width < watermark.Width * 4) && (image.Height > watermark.Height * 4))
                {
                    bl = Convert.ToDouble(image.Width / 4) / Convert.ToDouble(watermark.Width);
                }
                else
                {
                    if ((image.Width * watermark.Height) > (image.Height * watermark.Width))
                    {
                        bl = Convert.ToDouble(image.Height / 4) / Convert.ToDouble(watermark.Height);
                    }
                    else
                    {
                        bl = Convert.ToDouble(image.Width / 4) / Convert.ToDouble(watermark.Width);
                    }
                }

            WatermarkWidth = Convert.ToInt32(watermark.Width * bl);
            WatermarkHeight = Convert.ToInt32(watermark.Height * bl);
            switch (_watermarkPosition)
            {
                case "WM_TOP_LEFT":
                    xpos = 10;
                    ypos = 10;
                    break;
                case "WM_TOP_RIGHT":
                    xpos = image.Width - WatermarkWidth - 10;
                    ypos = 10;
                    break;
                case "WM_BOTTOM_RIGHT":
                    xpos = image.Width - WatermarkWidth - 10;
                    ypos = image.Height - WatermarkHeight - 10;
                    break;
                case "WM_BOTTOM_LEFT":
                    xpos = 10;
                    ypos = image.Height - WatermarkHeight - 10;
                    break;
                case "WM_CENTER":
                    xpos = image.Width / 2 - (WatermarkWidth / 2);
                    ypos = image.Height / 2 - (WatermarkHeight / 2);
                    break;
            }
            picture.DrawImage(watermark, new Rectangle(xpos, ypos, WatermarkWidth, WatermarkHeight), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);
            watermark.Dispose();
            imageAttributes.Dispose();

            b.Save(newpath, System.Drawing.Imaging.ImageFormat.Jpeg);
            b.Dispose();
            image.Dispose();
        }

        #endregion
    }

    /// <summary>
    /// 生成缩略图模式
    /// </summary>
    public enum MakeThumbnailMode
    {
        None,
        /// <summary>
        /// 自动缩放
        /// </summary>
        Auto,
        /// <summary>
        /// 指定宽，高按比例
        /// </summary>
        W,
        /// <summary>
        /// 指定高，宽按比例
        /// </summary>
        H,
        /// <summary>
        /// 指定高宽缩放（可能变形）
        /// </summary>
        HW,
        /// <summary>
        /// 指定高宽裁减（不变形）
        /// </summary>
        Cut
    }
}
