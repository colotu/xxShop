/*----------------------------------------------------------------
// Copyright (C) 2012 云商未来 版权所有。
//
// 文件名：ProductImages.cs
// 文件功能描述：
// 
// 创建标识： [Ben]  2012/06/11 20:36:26
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Web;
using YSWL.Common;
using YSWL.MALL.Model.Shop.Products;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.Products;
namespace YSWL.MALL.BLL.Shop.Products
{
    /// <summary>
    /// ProductImage
    /// </summary>
    public partial class ProductImage
    {
        private readonly IProductImage dal = DAShopProducts.CreateProductImage();
        public ProductImage()
        {}
        #region  Method

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long ProductId,int ProductImageId)
        {
            return dal.Exists(ProductId,ProductImageId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int  Add(YSWL.MALL.Model.Shop.Products.ProductImage model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Shop.Products.ProductImage model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ProductImageId)
        {
            
            return dal.Delete(ProductImageId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(long ProductId,int ProductImageId)
        {
            
            return dal.Delete(ProductId,ProductImageId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string ProductImageIdlist )
        {
            return dal.DeleteList(Common.Globals.SafeLongFilter(ProductImageIdlist ,0) );
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Products.ProductImage GetModel(int ProductImageId)
        {
            
            return dal.GetModel(ProductImageId);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Shop.Products.ProductImage GetModelByCache(int ProductImageId)
        {
            
            string CacheKey = "ProductImagesModel-" + ProductImageId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(ProductImageId);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch{}
            }
            return (YSWL.MALL.Model.Shop.Products.ProductImage)objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top,string strWhere,string filedOrder)
        {
            return dal.GetList(Top,strWhere,filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Products.ProductImage> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Products.ProductImage> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Products.ProductImage> modelList = new List<YSWL.MALL.Model.Shop.Products.ProductImage>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Products.ProductImage model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.Model.Shop.Products.ProductImage();
                    if(dt.Rows[n]["ProductImageId"]!=null && dt.Rows[n]["ProductImageId"].ToString()!="")
                    {
                        model.ProductImageId=int.Parse(dt.Rows[n]["ProductImageId"].ToString());
                    }
                    if(dt.Rows[n]["ProductId"]!=null && dt.Rows[n]["ProductId"].ToString()!="")
                    {
                        model.ProductId=long.Parse(dt.Rows[n]["ProductId"].ToString());
                    }
                    if(dt.Rows[n]["ImageUrl"]!=null && dt.Rows[n]["ImageUrl"].ToString()!="")
                    {
                    model.ImageUrl=dt.Rows[n]["ImageUrl"].ToString();
                    }
                    if(dt.Rows[n]["ThumbnailUrl1"]!=null && dt.Rows[n]["ThumbnailUrl1"].ToString()!="")
                    {
                    model.ThumbnailUrl1=dt.Rows[n]["ThumbnailUrl1"].ToString();
                    }
                    if(dt.Rows[n]["ThumbnailUrl2"]!=null && dt.Rows[n]["ThumbnailUrl2"].ToString()!="")
                    {
                    model.ThumbnailUrl2=dt.Rows[n]["ThumbnailUrl2"].ToString();
                    }
                    if(dt.Rows[n]["ThumbnailUrl3"]!=null && dt.Rows[n]["ThumbnailUrl3"].ToString()!="")
                    {
                    model.ThumbnailUrl3=dt.Rows[n]["ThumbnailUrl3"].ToString();
                    }
                    if(dt.Rows[n]["ThumbnailUrl4"]!=null && dt.Rows[n]["ThumbnailUrl4"].ToString()!="")
                    {
                    model.ThumbnailUrl4=dt.Rows[n]["ThumbnailUrl4"].ToString();
                    }
                    if(dt.Rows[n]["ThumbnailUrl5"]!=null && dt.Rows[n]["ThumbnailUrl5"].ToString()!="")
                    {
                    model.ThumbnailUrl5=dt.Rows[n]["ThumbnailUrl5"].ToString();
                    }
                    if(dt.Rows[n]["ThumbnailUrl6"]!=null && dt.Rows[n]["ThumbnailUrl6"].ToString()!="")
                    {
                    model.ThumbnailUrl6=dt.Rows[n]["ThumbnailUrl6"].ToString();
                    }
                    if(dt.Rows[n]["ThumbnailUrl7"]!=null && dt.Rows[n]["ThumbnailUrl7"].ToString()!="")
                    {
                    model.ThumbnailUrl7=dt.Rows[n]["ThumbnailUrl7"].ToString();
                    }
                    if(dt.Rows[n]["ThumbnailUrl8"]!=null && dt.Rows[n]["ThumbnailUrl8"].ToString()!="")
                    {
                    model.ThumbnailUrl8=dt.Rows[n]["ThumbnailUrl8"].ToString();
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
            //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion  Method

        public List<Model.Shop.Products.ProductImage> GetModelList(long productId)
        {
            DataSet ds = dal.GetList(string.Format(" ProductId={0}", productId));
            if (ds != null && ds.Tables.Count > 0)
            {
                return DataTableToList(ds.Tables[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="ImageUrl"></param>
        /// <param name="savePath"></param>
        /// <param name="saveThumbsPath"></param>
        /// <returns></returns>
        public static string MoveImage(string ImageUrl, string savePath, string saveThumbsPath)
        {
            try
            {
                if (BLL.SysManage.ConfigSystem.GetValueByCache("Shop_ImageStoreWay") == "1")
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
                        YSWL.MALL.BLL.Ms.ThumbnailSize.GetThumSizeList(YSWL.MALL.Model.Ms.EnumHelper.AreaType.Shop);

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
                                //为了防止编辑时 未修改的旧图片移动会导致已存在(目标文件与源文件路径路径相同)
                                if (!File.Exists(HttpContext.Current.Server.MapPath(destImage)))
                                {
                                    System.IO.File.Move(HttpContext.Current.Server.MapPath(String.Format(ImageUrl, thumbSize.ThumName)), HttpContext.Current.Server.MapPath(destImage));
                                }                       
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


        public List<Model.Shop.Products.ProductImage> ProductImagesList(long productId)
        {
            DataSet ds = dal.ProductImagesList(productId);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return ProductImageDtToList(ds.Tables[0]);
            }
            else
            {
                return null;
            }
        }

        public List<YSWL.MALL.Model.Shop.Products.ProductImage> ProductImageDtToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Products.ProductImage> modelList = new List<YSWL.MALL.Model.Shop.Products.ProductImage>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Products.ProductImage model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.Model.Shop.Products.ProductImage();
                    if (dt.Rows[n]["ProductId"] != null && dt.Rows[n]["ProductId"].ToString() != "")
                    {
                        model.ProductId = long.Parse(dt.Rows[n]["ProductId"].ToString());
                    }
                    if (dt.Rows[n]["ImageUrl"] != null && dt.Rows[n]["ImageUrl"].ToString() != "")
                    {
                        model.ImageUrl = dt.Rows[n]["ImageUrl"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl1"] != null && dt.Rows[n]["ThumbnailUrl1"].ToString() != "")
                    {
                        model.ThumbnailUrl1 = dt.Rows[n]["ThumbnailUrl1"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl2"] != null && dt.Rows[n]["ThumbnailUrl2"].ToString() != "")
                    {
                        model.ThumbnailUrl2 = dt.Rows[n]["ThumbnailUrl2"].ToString();
                    }
                    if (dt.Rows[n]["ThumbnailUrl3"] != null && dt.Rows[n]["ThumbnailUrl3"].ToString() != "")
                    {
                        model.ThumbnailUrl3 = dt.Rows[n]["ThumbnailUrl3"].ToString();
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// 更新一条数据 
        /// </summary>
        public bool UpdateThumbnail(YSWL.MALL.Model.Shop.Products.ProductImage model)
        {
            return dal.UpdateThumbnail(model);
        }
    }
}

