using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.Json;
using System.Text;
using YSWL.Common;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using YSWL.MALL.Model.Settings;
using YSWL.MALL.Web.Components;

namespace YSWL.MALL.Web.Handlers
{
    public class ImageReGenHandler : IHttpHandler
    {
        public const string POLL_KEY_STATUS = "STATUS";
        public const string POLL_KEY_DATA = "DATA";

        public const string POLL_STATUS_SUCCESS = "SUCCESS";
        public const string POLL_STATUS_FAILED = "FAILED";
        public const string POLL_STATUS_ERROR = "ERROR";

        public static List<YSWL.MALL.Model.SysManage.TaskQueue> TaskList;
        YSWL.MALL.BLL.SysManage.TaskQueue taskBll = new BLL.SysManage.TaskQueue();
        YSWL.MALL.BLL.Shop.Products.ProductInfo productBll=new ProductInfo();
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            //安全起见, 所有SNS相关Ajax请求为POST模式
            string action = context.Request.Form["Action"];
            context.Response.Clear();
            context.Response.ContentType = "application/json";

            try
            {
                switch (action)
                {
                    case "HttpToGen":
                        HttpToGen(context);
                        break;
                    case "GenerateImage":
                        GenerateImage(context);
                        break;
                    case "DeleteTask":
                        DeleteTask(context);
                        break;
                    case "ContinueTask":
                        ContinueTask(context);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                JsonObject json = new JsonObject();
                json.Put(POLL_KEY_STATUS, POLL_STATUS_SUCCESS);
                json.Put(POLL_KEY_DATA, ex);
                context.Response.Write(json.ToString());
            }
        }

        protected void HttpToGen(HttpContext context)
        {
            //TODO: 清除系统缓存 临时处理 To:涂朝辉 BEN ADD 2012-12-22
            JsonObject json = new JsonObject();
            StringBuilder strWhere = new StringBuilder();
            //strWhere.AppendFormat(" Status=1");
            string startDate = context.Request.Form["From"];
            string endDate = context.Request.Form["To"];
         
            int taskType = Common.Globals.SafeInt(context.Request.Form["TaskType"], 0);
               List<int> list =new List<int>();
            //商品图片任务
            if (taskType == (int) YSWL.MALL.Model.SysManage.EnumHelper.TaskQueueType.ShopImageReGen)
            {
                if (!String.IsNullOrWhiteSpace(startDate) && Common.PageValidate.IsDateTime(startDate))
                {
                    if (!String.IsNullOrWhiteSpace(strWhere.ToString()))
                    {
                        strWhere.Append("and");
                    }
                    strWhere.AppendFormat("  AddedDate >'" + startDate + "' ");
                }
                if (!String.IsNullOrWhiteSpace(endDate) && Common.PageValidate.IsDateTime(endDate))
                {
                    if (!String.IsNullOrWhiteSpace(strWhere.ToString()))
                    {
                        strWhere.Append("and");
                    }
                    strWhere.AppendFormat(" AddedDate <'" + endDate + "' ");
                }
                list = productBll.GetListToReGen(strWhere.ToString());
            }

            #region 循环静态化
            //静态化之前先清除表任务
            taskBll.DeleteTask((int)YSWL.MALL.Model.SysManage.EnumHelper.TaskQueueType.SNSImageReGen);

            TaskList = new List<Model.SysManage.TaskQueue>();
            if (list != null && list.Count > 0)
            {
                YSWL.MALL.Model.SysManage.TaskQueue taskModel = null;
                int i = 1;
                foreach (int item in list)
                {
                    //去重，不要重复的添加任务
                    if (!TaskList.Select(c => c.TaskId).Contains(item))
                    {
                        taskModel = new Model.SysManage.TaskQueue();
                        taskModel.ID = i;
                        taskModel.TaskId = item;
                        taskModel.Status = 0;
                        taskModel.Type = taskType;
                        if (taskBll.Add(taskModel))
                        {
                            TaskList.Add(taskModel);
                            i++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            #endregion
            json.Put(POLL_KEY_STATUS, POLL_STATUS_SUCCESS);
            json.Put(POLL_KEY_DATA, TaskList.Count);
            context.Response.Write(json.ToString());


        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="context"></param>
        protected void GenerateImage(HttpContext context)
        {
            JsonObject json = new JsonObject();
            int TaskId = Globals.SafeInt(context.Request.Form["TaskId"], 0);
            YSWL.MALL.Model.SysManage.TaskQueue item = TaskList.FirstOrDefault(c => c.ID == TaskId);
            int taskType = Common.Globals.SafeInt(context.Request.Form["TaskType"], 0);
            if (item != null)
            {
                if(taskType == (int) YSWL.MALL.Model.SysManage.EnumHelper.TaskQueueType.ShopImageReGen)
                {
                    ReGenProductImage(item.TaskId);
                }
                item.RunDate = DateTime.Now;
                item.Status = 1;
                taskBll.Update(item);

            }
            context.Response.Write(json.ToString());
        }
        /// <summary>
        /// 继续任务
        /// </summary>
        protected void ContinueTask(HttpContext context)
        {
            int taskType = Common.Globals.SafeInt(context.Request.Form["TaskType"], 0);
            TaskList = taskBll.GetContinueTask(taskType);
            JsonObject json = new JsonObject();
            json.Put(POLL_KEY_STATUS, POLL_STATUS_SUCCESS);

            YSWL.MALL.Model.SysManage.TaskQueue item = TaskList.Count == 0 ? null : TaskList.First();
          
            if (item == null)
            {
                taskBll.DeleteTask(taskType);
                json.Put(POLL_KEY_DATA, 0);
                context.Response.Write(json.ToString());
            }
            else
            {

                json.Put(POLL_KEY_DATA, item.ID);
                context.Response.Write(json.ToString());
            }
        }

        //删除任务（不删除未完成任务）
        protected void DeleteTask(HttpContext context)
        {
            int type = Globals.SafeInt(context.Request.Form["TaskType"], 0);
            taskBll.DeleteTask(type);//删除SNS图片生成任务列表
        }

        #region 生成缩略图
        

        public static string GetAbsolutePath(string path)
        {
            return "/" + path.Replace(HttpContext.Current.Request.PhysicalApplicationPath, "").Replace("\\", "/");
        }
        #endregion

        #region 生成商品缩略图
        protected void ReGenProductImage(int productId)
        {
             BLL.Shop.Products.ProductImage manage = new BLL.Shop.Products.ProductImage();
            List<Model.Shop.Products.ProductImage> list = manage.ProductImagesList(productId);
                  List<YSWL.MALL.Model.Ms.ThumbnailSize> thumSizeList =
                    YSWL.MALL.BLL.Ms.ThumbnailSize.GetThumSizeList(YSWL.MALL.Model.Ms.EnumHelper.AreaType.Shop,MvcApplication.ThemeName);
            string ShopThumbBath = "/Upload/Shop/Images/ProductThumbs/";
            if (list.Count > 0)
            {
                ArrayList old_Original_ImageList = new ArrayList();
                int index = 0;
                foreach (var item in list)
                {
                    index++;
                    //原始图片地址
                    string origialStr = item.ImageUrl;
               //是否是旧数据
                bool IsOldData = false;
                    //如果图片地址是云存储或者不存在
                if (origialStr.StartsWith("http://") || !File.Exists(HttpContext.Current.Server.MapPath(origialStr)))
                {
                    continue;
                }
               
                    //原始文件名
                    string fileName = origialStr.Substring(origialStr.LastIndexOf('/') + 1);
                    //原图水印保存地址
                    string origialPath = origialStr.Substring(0, origialStr.LastIndexOf('/') + 1);
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath(origialPath)))
                    {
                        //不存在则自动创建文件夹
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(origialPath));
                    }
                    //缩略图保存地址
                    string thumPath = item.ThumbnailUrl1.Substring(0, item.ThumbnailUrl1.LastIndexOf('/') + 1);
                    if (!item.ThumbnailUrl1.Contains(ShopThumbBath))
                    {
                        thumPath = ShopThumbBath + DateTime.Now.ToString("yyyyMMdd") + "/";
                        IsOldData = true;
                        item.ThumbnailUrl1 = thumPath + "{0}" + fileName;
                    }
                
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath(thumPath)))
                    {
                        //不存在则自动创建文件夹
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(thumPath));
                    }
                    try
                    {
                      bool isAddWater = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_ThumbImage_AddWater");
                    //原图水印保存地址
                    string imagePath = origialPath;
                    if (isAddWater)
                    {
                        imagePath = origialPath + "W_";
                        //生成临时原图水印图
                        FileHelper.MakeWater(HttpContext.Current.Server.MapPath(origialStr), HttpContext.Current.Server.MapPath(imagePath + fileName));
                    }

                    //重新生成缩略图
                    if (thumSizeList != null && thumSizeList.Count > 0)
                    {
                        foreach (var thumSize in thumSizeList)
                        {
                            ImageTools.MakeThumbnail(HttpContext.Current.Server.MapPath(imagePath + fileName), HttpContext.Current.Server.MapPath(String.Format(item.ThumbnailUrl1, thumSize.ThumName)),
                                thumSize.ThumWidth, thumSize.ThumHeight, GetThumMode(thumSize.ThumMode));
                        }
                    }
                    }
                    catch (Exception ex)
                    {
                        LogHelp.AddErrorLog(string.Format("商品：{0}重新生成缩略图时发生异常:{1}", productId, ex.StackTrace), "", "重新生成缩略图时发生异常");
                    }
                    try
                    {
                  
                        if (IsOldData)
                        {
                            //更新到数据库
                            if (index == 1)
                            {
                                Model.Shop.Products.ProductInfo model = new Model.Shop.Products.ProductInfo();
                                model.ProductId = productId;
                                model.ImageUrl = origialStr;
                                model.ThumbnailUrl1 = item.ThumbnailUrl1;
                                BLL.Shop.Products.ProductInfo productManage = new BLL.Shop.Products.ProductInfo();
                                productManage.UpdateThumbnail(model);
                            }
                            else
                            {
                                Model.Shop.Products.ProductImage model = new Model.Shop.Products.ProductImage();
                                model.ProductId = productId;
                                model.ImageUrl = origialStr;
                                model.ThumbnailUrl1 = item.ThumbnailUrl1;
                                BLL.Shop.Products.ProductImage productImageManage = new BLL.Shop.Products.ProductImage();
                                productImageManage.UpdateThumbnail(model);
                            }

                            //删除旧缩略图数据
                            if (old_Original_ImageList.Count > 0)
                            {
                                foreach (string imagePath in old_Original_ImageList)
                                {
                                    FileManage.DeleteFile(HttpContext.Current.Server.MapPath(imagePath));
                                }
                            }
                        }
                      
                    }
                    catch (Exception ex)
                    {
                        LogHelp.AddErrorLog(string.Format("商品：{0}重新生成缩略图更新到数据库时发生异常:{1}", productId, ex.StackTrace), "", "重新生成缩略图时发生异常");
                    }
                }
            }
            }
        #endregion
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
    }
}