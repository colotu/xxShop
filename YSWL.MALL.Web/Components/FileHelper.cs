using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;
using YSWL.MALL.Model.Ms;
using YSWL.ZipLib;
using YSWL.Common;
using YSWL.MALL.Model.Settings;
using YSWL.MALL.Model.SysManage;
using YSWL.ZipLib.Zip;
using EnumHelper = YSWL.MALL.Model.Ms.EnumHelper;

namespace YSWL.MALL.Web.Components
{
    public class FileHelper
    {
        private static BLL.SysManage.WebSiteSet WebSiteSetShop = new BLL.SysManage.WebSiteSet(ApplicationKeyType.Shop);

        private static Size GetThumbImageSize()
        {
            int thumbImgWidth = Globals.SafeInt(WebSiteSetShop.Shop_ThumbImageWidth, 0);
            int thumbImgHeight = Globals.SafeInt(WebSiteSetShop.Shop_ThumbImageHeight, 0);
            return YSWL.Common.StringPlus.SplitToSize(
                BLL.SysManage.ConfigSystem.GetValueByCache(SettingConstant.PRODUCT_NORMAL_SIZE_KEY),
                '|', thumbImgWidth == 0 ? SettingConstant.ProductThumbSize.Width : thumbImgWidth, thumbImgHeight == 0 ? SettingConstant.ProductThumbSize.Height : thumbImgHeight);
        }

        private static Size GetNormalImageSize()
        {
            int normalImgWidth = Globals.SafeInt(WebSiteSetShop.Shop_NormalImageWidth, 0);
            int normalImgHeight = Globals.SafeInt(WebSiteSetShop.Shop_NormalImageHeight, 0);
            return YSWL.Common.StringPlus.SplitToSize(
                BLL.SysManage.ConfigSystem.GetValueByCache(SettingConstant.PRODUCT_NORMAL_SIZE_KEY),
                '|', normalImgWidth == 0 ? SettingConstant.ProductNormalSize.Width : normalImgWidth, normalImgHeight == 0 ? SettingConstant.ProductNormalSize.Height : normalImgHeight);
        }

        /// <summary>
        /// 获取缩略图路径
        /// </summary>
        /// <param name="imageurl"></param>
        /// <param name="thumbName"></param>
        /// <returns></returns>
        public static string GeThumbImage(string imageurl, string thumbName)
        {
            if (string.IsNullOrWhiteSpace(imageurl) || string.IsNullOrWhiteSpace(thumbName))
                return string.Empty;

            //先排除淘宝图片
            if (imageurl.Contains("taobaocdn.com"))
            {
                return imageurl;
            }
            //云存储图片
            if (imageurl.StartsWith("http://"))
            {
                return imageurl + YSWL.MALL.BLL.Ms.ThumbnailSize.GetCloudName(thumbName);
            }
            string thumbUrl = String.Format(imageurl, "T_");

            if (File.Exists(HttpContext.Current.Server.MapPath(thumbUrl)))
            {
                return thumbUrl;
            }
            return String.Format(imageurl, thumbName);
        }

        /// <summary>
        /// 删除物理物件
        /// </summary>
        /// <param name="FileUrls"></param>
        /// <param name="Error"></param>
        /// <returns></returns>
        public static bool DeleteFile(List<string> FileUrls, ref string Error)
        {
            try
            {
                if (FileUrls != null && FileUrls.Count > 0)
                {
                    foreach (string FileUrl in FileUrls)
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath(FileUrl)))
                        {
                            File.Delete(HttpContext.Current.Server.MapPath(FileUrl));
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Error = e.Message;
                return false;
            }
        }

        public static bool DeleteFile(List<YSWL.MALL.Model.Ms.ThumbnailSize> thumbnailSizes, string path,
                                      ref string Error)
        {
            try
            {
                if (thumbnailSizes != null && thumbnailSizes.Count > 0)
                {
                    foreach (var thumb in thumbnailSizes)
                    {
                        string pathUrl = String.Format(path, thumb.ThumName);
                        if (File.Exists(HttpContext.Current.Server.MapPath(pathUrl)))
                        {
                            File.Delete(HttpContext.Current.Server.MapPath(pathUrl));
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Error = e.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除物理文件，（包括又拍云上面的文件删除）
        /// </summary>
        /// <param name="areaType"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DeleteFile(YSWL.MALL.Model.Ms.EnumHelper.AreaType areaType, string path)
        {
            try
            {
                ApplicationKeyType applicationKeyType = ApplicationKeyType.SNS;
                switch (areaType)
                {
                    case EnumHelper.AreaType.CMS:
                        applicationKeyType = ApplicationKeyType.CMS;
                        break;
                    case EnumHelper.AreaType.SNS:
                        applicationKeyType = ApplicationKeyType.SNS;
                        break;
                    case EnumHelper.AreaType.Shop:
                        applicationKeyType = ApplicationKeyType.Shop;
                        break;
                    default:
                        applicationKeyType = ApplicationKeyType.SNS;
                        break;
                }
                if (path.Contains("http://"))
                {
                    return YSWL.MALL.BLL.SysManage.UpYunManager.DeleteImage(path, applicationKeyType);
                }
                List<YSWL.MALL.Model.Ms.ThumbnailSize> thumbnailSizes =
             YSWL.MALL.BLL.Ms.ThumbnailSize.GetThumSizeList(areaType);
                if (thumbnailSizes != null && thumbnailSizes.Count > 0)
                {
                    foreach (var thumb in thumbnailSizes)
                    {
                        string pathUrl = String.Format(path, thumb.ThumName);
                        if (File.Exists(HttpContext.Current.Server.MapPath(pathUrl)))
                        {
                            File.Delete(HttpContext.Current.Server.MapPath(pathUrl));
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                YSWL.MALL.Model.SysManage.ErrorLog logModel = new Model.SysManage.ErrorLog();
                logModel.Loginfo = "删除图片【" + path + "】失败";
                logModel.OPTime = DateTime.Now;
                logModel.StackTrace = e.Message;
                logModel.Url = "";
                YSWL.MALL.BLL.SysManage.ErrorLog.Add(logModel);
                return false;
            }
        }


        public static string GetNewFileName(string OldFileName)
        {
            if (!string.IsNullOrEmpty(OldFileName) && OldFileName.Contains("."))
            {
                return CreateIDCode() + "." + OldFileName.Substring(OldFileName.LastIndexOf(".") + 1);
            }
            return "";
        }

        public static bool FileRemove(string OldPath, string NewPath, ref string RefNewPath)
        {
            if (string.IsNullOrEmpty(OldPath) || string.IsNullOrEmpty(NewPath))
            {
                return true;
            }
            try
            {
                string FileName = Path.GetFileName(OldPath);
                string AllOldPath = HttpContext.Current.Server.MapPath(OldPath);
                string AllNewPath = HttpContext.Current.Server.MapPath(NewPath);
                if (System.IO.File.Exists(AllOldPath))
                {
                    RefNewPath = NewPath + FileName;
                    if (System.IO.File.Exists(AllNewPath + FileName))
                    {
                        System.IO.File.Delete(AllNewPath + FileName);
                    }
                    System.IO.File.Move(AllOldPath, AllNewPath + FileName);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 得到一个随意的时间戳
        /// </summary>
        /// <returns></returns>

        public static string CreateIDCode()
        {
            DateTime Time1 = DateTime.Now.ToUniversalTime();
            DateTime Time2 = Convert.ToDateTime("1970-01-01");
            TimeSpan span = Time1 - Time2;   //span就是两个日期之间的差额
            string t = span.TotalMilliseconds.ToString("0");
            return t;
        }

        /// <summary>
        /// 图片的裁剪
        /// </summary>
        /// <param name="imgname">图片的名字</param>
        /// <param name="uploadpath">存放的位置</param>
        /// <param name="SmallImageSize">小图的大小 长X宽的形式</param>
        /// <param name="BigImageSize">大图的大小 长X宽的形式</param>
        /// <param name="SmallImagePath">out 小图的保存的位置</param>
        /// <param name="BigImagePath">out 大图保存的位置</param>
        /// <returns></returns>
        public static bool ImageCutMethod(string imgname, string uploadpath, string SmallImageSize, string BigImageSize, ref string SmallImagePath, ref string BigImagePath)
        {
            try
            {
                //生成小图
                string SthumbImage = "S_" + imgname;
                string SthumbImagePath = HttpContext.Current.Server.MapPath(SmallImagePath + SthumbImage);
                int SWindthInt = 400;
                int SHeightInt = 400;
                if (SmallImageSize != null && SmallImageSize.Split('X').Length > 1)
                {
                    string[] Size = SmallImageSize.Split('X');
                    SWindthInt = Common.Globals.SafeInt(Size[0], 400);
                    SHeightInt = Common.Globals.SafeInt(Size[1], 400);
                }
                MakeWaterThumbnail(HttpContext.Current.Server.MapPath(uploadpath + imgname), SthumbImagePath, SWindthInt, SHeightInt, MakeThumbnailMode.Auto);

                ///生成大图
                string BthumbImage = "B_" + imgname;
                string BthumbImagePath = HttpContext.Current.Server.MapPath(BigImagePath + BthumbImage);

                int BWindthInt = 800;
                int BHeightInt = 800;
                if (BigImageSize != null && BigImageSize.Split('X').Length > 1)
                {
                    string[] Size = BigImageSize.Split('X');
                    BWindthInt = Common.Globals.SafeInt(Size[0], 800);
                    BHeightInt = Common.Globals.SafeInt(Size[1], 800);
                }
                MakeWaterThumbnail(HttpContext.Current.Server.MapPath(uploadpath + imgname), BthumbImagePath, BWindthInt, BHeightInt, MakeThumbnailMode.Auto);
                SmallImagePath = uploadpath + SthumbImage;
                BigImagePath = uploadpath + BthumbImage;
                return true;
            }
            catch (Exception)
            {
                SmallImagePath = "";
                BigImagePath = "";
                return false;
            }
        }

        public static int StartIndex(int PageSize, int PageIndex)
        {
            return PageSize * (PageIndex - 1) + 1;
        }

        public static int EndIndex(int PageSize, int PageIndex)
        {
            return PageSize * PageIndex;
        }

        public static bool UnpackFiles(string zipFile, string directory)
        {
            try
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                var zis = new ZipInputStream(File.OpenRead(zipFile));
                ZipEntry theEntry = null;
                while ((theEntry = zis.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    if (directoryName != string.Empty)
                        Directory.CreateDirectory(directory + directoryName);

                    if (fileName != string.Empty)
                    {
                        FileStream streamWriter = File.Create(Path.Combine(directory, theEntry.Name));
                        int size = 2048;
                        var data = new byte[size];
                        while (true)
                        {
                            size = zis.Read(data, 0, data.Length);
                            if (size > 0)
                                streamWriter.Write(data, 0, size);
                            else
                                break;
                        }

                        streamWriter.Close();
                    }
                }

                zis.Close();
                return true;
            }
            catch (Exception)
            {
                return false;

                throw;
            }
        }

        #region  生成水印及缩略图
        /// <summary>
        /// 生成水印及缩略图
        /// </summary>
        /// <param name="oldpath"></param>
        /// <param name="newpath"></param>
        /// <param name="width"></param>
        /// <param name="Height"></param>
        /// <param name="mode"></param>
        public static void MakeWaterThumbnail(string oldpath, string newpath, int width, int Height, MakeThumbnailMode mode)
        {
            string waterMarkType = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("System_waterMarkType");
            int type = Common.Globals.SafeInt(waterMarkType, 0);
            if (type == 0)
            {
                MakeTextWaterThumbnail(oldpath, newpath, width, Height, mode);
            }
            else
            {
                MakeImageWaterThumbnail(oldpath, newpath, width, Height, mode);
            }
        }

        /// <summary>
        /// 文字水印+缩略图
        /// </summary>
        /// <param name="oldpath"></param>
        /// <param name="newpath"></param>
        /// <param name="_watermarkText"></param>
        public static void MakeTextWaterThumbnail(string oldpath, string newpath, int width, int Height, MakeThumbnailMode mode)
        {
            string waterMarkContent = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("System_waterMarkContent");
            if (String.IsNullOrWhiteSpace(waterMarkContent))
            {
                waterMarkContent = "YSWL";
            }
            string waterMarkFont = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("System_waterMarkFont");
            if (String.IsNullOrWhiteSpace(waterMarkFont))
            {
                waterMarkFont = "arial";
            }
            string waterMarkFontSize = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("System_waterMarkFontSize");
            if (String.IsNullOrWhiteSpace(waterMarkFontSize))
            {
                waterMarkFontSize = "14";
            }
            string waterMarkPosition = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("System_waterMarkPosition");
            if (String.IsNullOrWhiteSpace(waterMarkPosition))
            {
                waterMarkPosition = "WM_CENTER";
            }
            string waterMarkFontColor = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("System_waterMarkFontColor");
            if (String.IsNullOrWhiteSpace(waterMarkFontColor))
            {
                waterMarkFontColor = "#FFFFFF";
            }
            try
            {
                YSWL.Common.ImageTools.MakeTextWaterThumbnail(oldpath, newpath, width, Height, mode, System.Drawing.Imaging.ImageFormat.Png, waterMarkContent, waterMarkPosition, waterMarkFont, Common.Globals.SafeInt(waterMarkFontSize, 14), waterMarkFontColor);

                //  , string _watermarkPosition = "WM_CENTER", string fontStyle = "arial", int fontSize = 14, string color = "#FFFFFF"
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 生成水印
        /// </summary>
        /// <param name="oldpath"></param>
        /// <param name="newpath"></param>
        public static void MakeWater(string oldpath, string newpath)
        {
            string waterMarkType = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("System_waterMarkType");
            int type = Common.Globals.SafeInt(waterMarkType, 0);
            if (type == 0)
            {
                MakeTextWater(oldpath, newpath);
            }
            else
            {
                MakeImageWater(oldpath, newpath);
            }
        }

        /// <summary>
        /// 文字水印
        /// </summary>
        /// <param name="oldpath"></param>
        /// <param name="newpath"></param>
        /// <param name="_watermarkText"></param>
        public static void MakeTextWater(string oldpath, string newpath)
        {
            string waterMarkContent = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("System_waterMarkContent");
            if (String.IsNullOrWhiteSpace(waterMarkContent))
            {
                waterMarkContent = "YSWL ";
            }
            string waterMarkFont = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("System_waterMarkFont");
            if (String.IsNullOrWhiteSpace(waterMarkFont))
            {
                waterMarkFont = "arial";
            }
            string waterMarkFontSize = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("System_waterMarkFontSize");
            if (String.IsNullOrWhiteSpace(waterMarkFontSize))
            {
                waterMarkFontSize = "14";
            }
            string waterMarkPosition = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("System_waterMarkPosition");
            if (String.IsNullOrWhiteSpace(waterMarkPosition))
            {
                waterMarkPosition = "WM_CENTER";
            }
            string waterMarkFontColor = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("System_waterMarkFontColor");
            if (String.IsNullOrWhiteSpace(waterMarkFontColor))
            {
                waterMarkFontColor = "#FFFFFF";
            }
            try
            {
                YSWL.Common.ImageTools.addWatermarkText(oldpath, newpath, waterMarkContent, waterMarkPosition, waterMarkFont, Common.Globals.SafeInt(waterMarkFontSize, 14), waterMarkFontColor);

                //  , string _watermarkPosition = "WM_CENTER", string fontStyle = "arial", int fontSize = 14, string color = "#FFFFFF"
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 图片水印+缩略图
        /// </summary>
        /// <param name="oldpath"></param>
        /// <param name="newpath"></param>
        public static void MakeImageWaterThumbnail(string oldpath, string newpath, int width, int Height, MakeThumbnailMode mode)
        {
            string ImageMarkPosition = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("System_waterMarkPosition");
            if (String.IsNullOrWhiteSpace(ImageMarkPosition))
            {
                ImageMarkPosition = "WM_CENTER";
            }
            string waterMarkTransparent = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("System_waterMarkTransparent");
            if (String.IsNullOrEmpty(waterMarkTransparent))
            {
                waterMarkTransparent = "30";
            }
            string waterMarkPhotoUrl = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValue("System_waterMarkPhotoUrl");
            if (String.IsNullOrEmpty(waterMarkPhotoUrl))
            {
                waterMarkPhotoUrl = "/Upload/WebSiteLogo/sitelogo.png";
            }
            try
            {
                YSWL.Common.ImageTools.MakeImageWaterThumbnail(oldpath, newpath, width, Height, mode, System.Drawing.Imaging.ImageFormat.Png, HttpContext.Current.Server.MapPath(waterMarkPhotoUrl), ImageMarkPosition, Common.Globals.SafeInt(waterMarkTransparent, 30));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 图片水印
        /// </summary>
        /// <param name="oldpath"></param>
        /// <param name="newpath"></param>
        public static void MakeImageWater(string oldpath, string newpath)
        {
            string ImageMarkPosition = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("System_waterMarkPosition");
            if (String.IsNullOrWhiteSpace(ImageMarkPosition))
            {
                ImageMarkPosition = "WM_CENTER";
            }
            string waterMarkTransparent = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("System_waterMarkTransparent");
            if (String.IsNullOrEmpty(waterMarkTransparent))
            {
                waterMarkTransparent = "30";
            }
            string waterMarkPhotoUrl = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValue("System_waterMarkPhotoUrl");
            if (String.IsNullOrEmpty(waterMarkPhotoUrl) || !File.Exists(HttpContext.Current.Server.MapPath(waterMarkPhotoUrl)))
            {
                waterMarkPhotoUrl = "/Upload/WebSiteLogo/sitelogo.png";
            }
            if (File.Exists(HttpContext.Current.Server.MapPath(waterMarkPhotoUrl)))
            {
                try
                {
                    YSWL.Common.ImageTools.addWatermarkImage(oldpath, newpath, HttpContext.Current.Server.MapPath(waterMarkPhotoUrl), ImageMarkPosition, Common.Globals.SafeInt(waterMarkTransparent, 30));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                File.Copy(oldpath, newpath, true);
            }
        }

        #endregion

        #region  时间格式
        /// <summary>
        /// 时间格式转换
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateToString(DateTime dt)
        {
            BLL.SysManage.WebSiteSet WebSiteSet = new BLL.SysManage.WebSiteSet(ApplicationKeyType.System);
            string date = String.IsNullOrWhiteSpace(WebSiteSet.Date_Format) ? "yyyy-MM-dd" : WebSiteSet.Date_Format;
            string time = String.IsNullOrWhiteSpace(WebSiteSet.Time_Format) ? "HH:mm:ss" : WebSiteSet.Time_Format;
            return dt.ToString(date + " " + time);
        }
        /// <summary>
        /// 重载
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateToString(object dt)
        {
            DateTime date = Common.Globals.SafeDateTime(dt.ToString(), DateTime.Now);
            return DateToString(date);
        }
        #endregion


        #region 根据模板区域获取模板名称

        public static List<DirectoryInfo> GetThemeList(string AreaName)
        {
            string AreaBath = "/Areas/{0}/Themes";
            List<DirectoryInfo> themeList = new List<DirectoryInfo>();
            if (Directory.Exists(HttpContext.Current.Server.MapPath(String.Format(AreaBath, AreaName))))
            {
                var dirs = Directory.GetDirectories(HttpContext.Current.Server.MapPath(String.Format(AreaBath, AreaName)));
                if (dirs != null && dirs.Length > 0)
                {
                    foreach (var dir in dirs)
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                        themeList.Add(directoryInfo);
                    }
                }
            }
            return themeList;
        }

        #endregion

        #region  根据主区域获取模板
        public static List<YSWL.MALL.Model.Ms.Theme> GetThemes(string AreaName)
        {
            string AreaBath = "/Areas/{0}/Themes";

            List<YSWL.MALL.Model.Ms.Theme> themeList = new List<YSWL.MALL.Model.Ms.Theme>();
            if (Directory.Exists(HttpContext.Current.Server.MapPath(String.Format(AreaBath, AreaName))))
            {
                var dirs = Directory.GetDirectories(HttpContext.Current.Server.MapPath(String.Format(AreaBath, AreaName)));
                if (dirs != null && dirs.Length > 0)
                {
                    foreach (var dir in dirs)
                    {
                        YSWL.MALL.Model.Ms.Theme model = new Theme();
                        DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                        model.Name = directoryInfo.Name;
                        string filePath = String.Format(AreaBath, AreaName) + "/" + directoryInfo.Name;
                        model = GetThemeModel(filePath, model);
                        themeList.Add(model);
                    }
                }
            }

            return themeList;
        }


        public static YSWL.MALL.Model.Ms.Theme GetThemeModel(string FilePath, YSWL.MALL.Model.Ms.Theme model)
        {
            string ThemeText = "/Theme.xml";
            string ThemePhoto = "/Theme.png";
            string TextInfoPath = FilePath + ThemeText;
            if (File.Exists(HttpContext.Current.Server.MapPath(TextInfoPath)))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(HttpContext.Current.Server.MapPath(TextInfoPath));
                XmlElement rootElement = doc.DocumentElement;
                if (rootElement == null)
                {
                    return model;
                }
                model.Author = rootElement.SelectSingleNode("Author").InnerText;
                model.Description = rootElement.SelectSingleNode("Description").InnerText;
                model.Language = rootElement.SelectSingleNode("Language").InnerText;
            }
            string savePhoto = FilePath + ThemePhoto;
            model.PreviewPhotoSrc = savePhoto;
            return model;
        }

        #endregion

        public static string MoveImage(string ImageUrl, string savePath, string saveThumbsPath, YSWL.MALL.Model.Ms.EnumHelper.AreaType areaType)
        {
            try
            {
                var saveWay = "";
                if (areaType == YSWL.MALL.Model.Ms.EnumHelper.AreaType.SNS)
                {
                    saveWay = BLL.SysManage.ConfigSystem.GetValueByCache("SNS_ImageStoreWay");
                }
                if (areaType == YSWL.MALL.Model.Ms.EnumHelper.AreaType.CMS)
                {
                    saveWay = BLL.SysManage.ConfigSystem.GetValueByCache("CMS_ImageStoreWay");
                }
                if (saveWay == "1")
                {
                    return ImageUrl + "|" + ImageUrl;
                }
                if (!string.IsNullOrEmpty(ImageUrl))
                {

                    if (!Directory.Exists(HttpContext.Current.Server.MapPath(savePath)))
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(savePath));

                    if (!Directory.Exists(HttpContext.Current.Server.MapPath(saveThumbsPath)))
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(saveThumbsPath));

                    List<YSWL.MALL.Model.Ms.ThumbnailSize> ThumbSizeList =
                        YSWL.MALL.BLL.Ms.ThumbnailSize.GetThumSizeList(areaType);

                    string imgname = ImageUrl.Substring(ImageUrl.LastIndexOf("/") + 1);
                    string destImage = "";
                    string originalUrl = "";
                    string thumbUrl = saveThumbsPath + imgname;
                    //首先移动原图片

                    if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(String.Format(ImageUrl, ""))))
                    {
                        originalUrl = String.Format(savePath + imgname, "");
                        System.IO.File.Move(HttpContext.Current.Server.MapPath(String.Format(ImageUrl, "")), HttpContext.Current.Server.MapPath(originalUrl));

                    }
                    if (ThumbSizeList != null && ThumbSizeList.Count > 0)
                    {
                        foreach (var thumbSize in ThumbSizeList)
                        {
                            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(String.Format(ImageUrl, thumbSize.ThumName))))
                            {
                                destImage = String.Format(thumbUrl, thumbSize.ThumName);
                                System.IO.File.Move(HttpContext.Current.Server.MapPath(String.Format(ImageUrl, thumbSize.ThumName)), HttpContext.Current.Server.MapPath(destImage));

                            }
                        }
                    }
                    return originalUrl + "|" + thumbUrl;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return "";
        }

        #region 生成升级包
        /// <summary>
        /// 生成升级包
        /// </summary>
        /// <param name="oldFolder">旧文件夹</param>
        /// <param name="newFolder">新文件夹</param>
        /// <param name="targetFolder">目标文件夹</param>
        public static void GeneratePackage(string oldFolder, string newFolder, string targetFolder)
        {
            DirectoryInfo newdir = new DirectoryInfo(newFolder);
            FindDire(newdir, oldFolder, newFolder, targetFolder);
        }
        /// <summary>
        /// 递归查找所有目录及文件
        /// </summary>
        /// <param name="dir"></param>
        private static void FindDire(DirectoryInfo newdir, string oldFolder, string newFolder, string targetFolder)
        {
            //遍历一个目录下的全部目录
            foreach (DirectoryInfo dChild in newdir.GetDirectories("*"))
            {
                FindDire(dChild, oldFolder, newFolder, targetFolder);
            }
            FindFile(newdir, oldFolder, newFolder, targetFolder);
        }
        /// <summary>
        /// 查找所有文件
        /// </summary>
        /// <param name="dir"></param>
        private static void FindFile(DirectoryInfo dir, string oldFolder, string newFolder, string targetFolder)
        {
            string oldFile;
            string newFile;
            string targetFile;
            string targetPath;
            //遍历一个目录下的全部文件
            foreach (FileInfo dChild in dir.GetFiles("*"))
            {
                oldFile = dChild.FullName.Replace(newFolder, oldFolder);
                newFile = dChild.FullName;
                if (!File.Exists(oldFile) || !CompareFile(oldFile, newFile))
                {
                    targetPath = dChild.DirectoryName.Replace(newFolder, targetFolder);
                    targetFile = dChild.FullName.Replace(newFolder, targetFolder);
                    if (!Directory.Exists(targetPath))
                    {
                        Directory.CreateDirectory(targetPath);
                    }
                    File.Copy(newFile, targetFile, true);
                }
            }
        }
        /// <summary>
        /// 比较两个文件是否完全相等   
        /// </summary>
//TODO: 这样对比文件是否相等会有问题，这个是比较大小的，如果文件更改后，但是文件的大小并没有变，就会识别为没有改变。
        private static bool CompareFile(string p_1, string p_2)
        {
            //计算第一个文件的哈希值
            var hash = System.Security.Cryptography.HashAlgorithm.Create();
            var stream_1 = new System.IO.FileStream(p_1, System.IO.FileMode.Open);
            byte[] hashByte_1 = hash.ComputeHash(stream_1);
            stream_1.Close();
            //计算第二个文件的哈希值
            var stream_2 = new System.IO.FileStream(p_2, System.IO.FileMode.Open);
            byte[] hashByte_2 = hash.ComputeHash(stream_2);
            stream_2.Close();

            //比较两个哈希值 
            return BitConverter.ToString(hashByte_1) == BitConverter.ToString(hashByte_2) ? true : false;
        }
        #endregion

        #region 生成分享推广二维码
        /// <summary>
        /// 生成分享推广二维码
        /// </summary>
        /// <param name="nickName">昵称</param>
        /// <param name="advertise">推广宣传语</param>
        /// <param name="advPoint">推广宣传语坐标</param>
        public static void GetQRCode(string nickName, string advertise, Point advPoint,string imagePath,string QRUrl,string savaPath)
        {
            //基本参数
            PixelFormat pixelFormat = PixelFormat.Format24bppRgb;   //指定图像中每个像素的颜色数据的格式
            int width = 430;    //图像宽
            int higth = 740;   //图像高
            Bitmap bit = new Bitmap(width, higth, pixelFormat);
            Graphics picture = Graphics.FromImage(bit);
            picture.Clear(Color.White);     //以白色背景色填充
            picture.SmoothingMode = SmoothingMode.AntiAlias;


            //头像
            try
            {
                Avatar(149, 30, 132, 132, Image.FromFile(imagePath), picture);
            }
            catch(Exception ex)
            {
                LogHelp.AddErrorLog(ex.Message, ex.StackTrace);
            }


            //昵称
            Font font1 = new Font("微软雅黑", 20);  //字体 字体大小单位为磅不是px!
            SolidBrush brush1 = new SolidBrush(Color.Black); //格式刷
            nickName = "我是:" + nickName;
            int x=0;
            

            if (nickName.Length > 10)
            {
                nickName = nickName.Substring(0, 10) + "\n" + nickName.Substring(10);
                x = 155;
            }
            else
            {
                x = (430 - (nickName.Length - 1) * 27) / 2;

            }

            Point nickPoint = new Point(x, 180);
            try
            {
                
                Text(nickPoint, font1, brush1, nickName, picture);
                
            }
            catch (Exception ex)
            {
                LogHelp.AddErrorLog(ex.Message, ex.StackTrace);
            }

            

            //绘制二维码

            int x1 = 0;     //二维码横轴坐标
            int y1 = 280;     //二维码纵轴坐标
            int width1 = 430;  //二维码的宽
            int hight1 = 430;  //二维码的高 
            try
            {
                Qrcode(x1, y1, width1, hight1, picture, QRUrl);
            }
            catch (Exception ex)
            {
                LogHelp.AddErrorLog(ex.Message, ex.StackTrace);
            }

            //推广文字
            Font font2 = new Font("微软雅黑", 13);  //字体
            SolidBrush brush2 = new SolidBrush(Color.FromArgb(113, 113, 113)); //格式刷
            try
            {
                Text(advPoint, font2, brush2, advertise, picture);
            }
            catch (Exception ex)
            {
                LogHelp.AddErrorLog(ex.Message, ex.StackTrace);
            }

            


            //绘制说明文字
            Font font3 = new Font("微软雅黑", 12);  //字体
            SolidBrush brush3 = new SolidBrush(Color.FromArgb(113, 113, 113)); //格式刷
            try
            {
                Text(new Point(124, 700), font3, brush3, "长按此图识别图中二维码", picture);
            }
            catch (Exception ex)
            {
                LogHelp.AddErrorLog(ex.Message, ex.StackTrace);
            }

            //保存及释放资源
            try
            {              
                picture.Dispose(); 
                Bitmap bit2 = new Bitmap(width, higth, pixelFormat);
                Graphics draw = Graphics.FromImage(bit2);
                draw.DrawImage(bit, 0, 0);            
                bit.Dispose();
                bit2.Save(savaPath);
                bit2.Dispose();
            }
            catch (Exception ex)
            {
                LogHelp.AddErrorLog(ex.Message, ex.StackTrace);
            }
        }

        public static void GetQRCode(string imagePath, string QRUrl, string savaPath,string nickName)
        {

            int adv_x = 50;
            
            string advertise = BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_MShop_QRAd");

            if (advertise.Length > 22 && advertise.Length < 44)
            {
                advertise = advertise.Substring(0, 22) + "\n" + advertise.Substring(22);
                
            }
            else
            {
                adv_x = (430 - (advertise.Length * 17)) / 2;
            }

            Point advPoint = new Point(adv_x, 250);

            GetQRCode(nickName, advertise, advPoint, imagePath, QRUrl, savaPath);

        }


        /// <summary>
        /// 绘制二维码部分
        /// </summary>
        /// <param name="xpos">距画布左顶点横轴坐标</param>
        /// <param name="ypos">距画布左顶点纵轴坐标</param>
        /// <param name="width">绘制图像的宽度</param>
        /// <param name="hight">绘制图像的高度</param>
        /// <param name="picture">画布</param>
        /// <param name="QRUrl">二维码的地址(远程)</param>
        private static void Qrcode(int xpos, int ypos, int width, int hight, Graphics picture, string QRUrl)
        {
            Image img = null;

            System.Net.WebRequest webreq = System.Net.WebRequest.Create(QRUrl);
            System.Net.WebResponse webres = webreq.GetResponse();
            using (System.IO.Stream stream = webres.GetResponseStream())
            {
                img = Image.FromStream(stream);
            }
            picture.DrawImage(img, xpos, ypos, width, hight);
        }


        /// <summary>
        /// 绘制文字部分
        /// </summary>
        /// <param name="point">距离画布左顶点的坐标对</param>
        /// <param name="font">字体</param>
        /// <param name="brush">格式刷</param>
        /// <param name="text">文字内容</param>
        /// <param name="picture">画布</param>
        private static void Text(Point point, Font font, SolidBrush brush, string text, Graphics picture)
        {
            picture.DrawString(text, font, brush, point);
        }

        /// <summary>
        /// 绘制头像部分
        /// </summary>
        /// <param name="xpos">距画布左顶点横轴坐标</param>
        /// <param name="ypos">距画布左顶点纵轴坐标</param>
        /// <param name="width">绘制图像的宽度</param>
        /// <param name="hight">绘制图像的高度</param>
        /// <param name="img">待绘制图像</param>
        /// <param name="picture">画布</param>
        private static void Avatar(int xpos, int ypos, int width, int hight, Image img, Graphics picture)
        {
            picture.DrawImage(img, xpos, ypos, width, hight);
        }





        #endregion


        #region  获取Logo图片

        public static string GetLogoUrl(long enterpriseId)
        {
 
            #region  远程请求Logo
            string basePath = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("YSWL_SAAS_Url");
            basePath = String.IsNullOrWhiteSpace(basePath) ? "http://saas.ys56.com/" : basePath;
            string logoUrl = basePath + "/Upload/Logo/" + enterpriseId + "/logo_" + enterpriseId + ".jpg";

            HttpWebRequest req = null;
            HttpWebResponse res = null;
            try
            {
                req = (HttpWebRequest)WebRequest.Create(logoUrl);
                res = (HttpWebResponse)req.GetResponse();
                if (res != null && res.StatusCode == HttpStatusCode.OK)
                {
                    return logoUrl;
                }
            }
            catch
            {
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
                if (req != null)
                {
                    req.Abort();
                }
            }
            return "";

            #endregion
        }
        #endregion 

    }
}