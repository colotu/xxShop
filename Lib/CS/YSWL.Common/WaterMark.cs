using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Web.UI;
using System.Web;

namespace YSWL.Common
{
    /// <summary>
    /// 图片水印
    /// </summary>
    public class WaterMark
    {
        #region 仅生成水印
        /// <summary>
        /// 文字水印
        /// </summary>
        /// <param name="picture">添加水印的图片</param>
        /// <param name="_watermarkText">水印文字</param>
        /// <param name="_watermarkPosition">水印位置</param>
        /// <param name=" image.Width">宽度</param>
        /// <param name="image.Height">高度</param>
        public static bool addWatermarkText(string oldpath, string newpath, string _watermarkText, string _watermarkPosition = "WM_CENTER", string fontStyle = "arial", int fontSize = 14, string color = "#FFFFFF")
        {
            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(oldpath));
                Bitmap b = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb);
                Graphics picture = Graphics.FromImage(b);
                picture.Clear(Color.White);
                picture.SmoothingMode = SmoothingMode.HighQuality;
                picture.InterpolationMode = InterpolationMode.High;
                picture.DrawImage(image, 0, 0, image.Width, image.Height);

                Font crFont = null;
                SizeF crSize = new SizeF();
                crFont = new Font(fontStyle, fontSize);
                crSize = picture.MeasureString(_watermarkText, crFont);
                //for (int i = 0; i < 7; i++)
                //{
                //    crFont = new Font("arial", sizes[i]);
                //    crSize = picture.MeasureString(_watermarkText, crFont);
                //    if ((ushort)crSize.Width < (ushort) image.Width)
                //        break;
                //}
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
                        xpos = ((float)image.Width * (float)1.00) - (crSize.Width / 2);
                        ypos = ((float)image.Height * (float)1.00) - (crSize.Height);
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
                SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));
                picture.DrawString(_watermarkText, crFont, semiTransBrush2, xpos + 1, ypos + 1, StrFormat);

                //if (color.Equals(Color.Empty))
                //{
                //    color = Color.White;
                //}
                SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, ColorTranslator.FromHtml(color)));
                picture.DrawString(_watermarkText, crFont, semiTransBrush, xpos, ypos, StrFormat);
                semiTransBrush2.Dispose();
                semiTransBrush.Dispose();

                b.Save(HttpContext.Current.Server.MapPath(newpath));
                b.Dispose();
                image.Dispose();
                return true;
            }
            catch
            {
                //if (File.Exists(HttpContext.Current.Server.MapPath(oldpath)))
                //{
                //    File.Delete(oldpath);
                //}
            }
            finally
            {
                //if (File.Exists(HttpContext.Current.Server.MapPath(oldpath)))
                //{
                //    File.Delete(oldpath);
                //}
            }
            return false;
        }
        /// <summary>
        /// 图片水印
        /// </summary>
        /// <param name="picture">添加水印的图片</param>
        /// <param name="WaterMarkPicPath">水印图片</param>
        /// <param name="_watermarkPosition">水印位置</param>
        /// <param name=" image.Width">宽度</param>
        /// <param name="image.Height">高度</param>
        public static bool addWatermarkImage(string oldpath, string newpath, string WaterMarkPicPath, string _watermarkPosition = "WM_CENTER", int transparentValue = 30)
        {
            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(oldpath));
                Bitmap b = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb);
                Graphics picture = Graphics.FromImage(b);
                picture.Clear(Color.White);
                picture.SmoothingMode = SmoothingMode.HighQuality;
                picture.InterpolationMode = InterpolationMode.High;
                picture.DrawImage(image, 0, 0, image.Width, image.Height);

                Image watermark = new Bitmap(HttpContext.Current.Server.MapPath(WaterMarkPicPath));
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

                b.Save(HttpContext.Current.Server.MapPath(newpath));
                b.Dispose();
                image.Dispose();
                return true;
            }
            catch
            {
                //if (File.Exists(HttpContext.Current.Server.MapPath(oldpath)))
                //{
                //    File.Delete(oldpath);
                //}
            }
            finally
            {
                //if (File.Exists(HttpContext.Current.Server.MapPath(oldpath)))
                //{
                //    File.Delete(oldpath);
                //}
            }
            return false;
        }
        
        #endregion
    }
}
