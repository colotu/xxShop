using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.Common;
using YSWL.DBUtility;

namespace YSWL.MALL.BLL.Shop.Service
{
    public class PMSServiceHelper
    {

        //private static bool IsSyncPMS = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_ConnectionPMS");
        ///// <summary>
        ///// 同步所有数据
        ///// </summary>
        //public static void SyncAllData()
        //{
        //    if (!IsSyncPMS)
        //    {
        //        return;
        //    }
        //    try
        //    {
        //        using (PMS.ServiceClient client = new PMS.ServiceClient())
        //        {
        //            client.SyncAllData();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        private static DataCacheCore dataCache = new DataCacheCore(new CacheOption
        {
            CacheType = SAASInfo.GetSystemBoolValue("RedisCacheUse") ? CacheType.Redis : CacheType.IIS,
            ReadWriteHosts = SAASInfo.GetSystemValue("RedisCacheReadWriteHosts"),
            ReadOnlyHosts = SAASInfo.GetSystemValue("RedisCacheReadOnlyHosts"),
            DefaultDb = 2
        });

        #region PMS同步商品数据接口
        /// <summary>
        /// 同步商品信息
        /// </summary>
        /// <param name="prpInfos"></param>
        /// <returns></returns>
        public static void SyncProductInfo(List<YSWL.MALL.Model.Shop.Products.ProductInfo> prpInfos)
        {
            YSWL.MALL.BLL.Shop.Products.ProductInfo productInfoBll = new ProductInfo();
            foreach (var item in prpInfos)
            {
                if (productInfoBll.Exists(item.ProductId))
                {
                    YSWL.MALL.BLL.Shop.Products.ProductManage.ModifyPMSProduct(item);
                }
                else
                {
                    YSWL.MALL.BLL.Shop.Products.ProductManage.AddPMSProduct(item);
                }
                //修改分仓库存信息
                YSWL.MALL.BLL.Shop.Products.ProductManage.SubUpdateList(item);
            }
        }
        public static void SyncProductInfoOne(YSWL.MALL.Model.Shop.Products.ProductInfo productInfo)
        {
            YSWL.MALL.BLL.Shop.Products.ProductManage.AddProductOne(productInfo);
            //修改分仓库存信息
            YSWL.MALL.BLL.Shop.Products.ProductManage.SubUpdateList(productInfo);
        }
        /// <summary>
        /// 同步分类信息
        /// </summary>
        /// <param name="cateInfos"></param>
        public static void SyncCategory(List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateInfos)
        {
            YSWL.MALL.BLL.Shop.Products.CategoryInfo cateBll = new CategoryInfo();
            try
            {
                //先重置表
                cateBll.ResetTable();
                foreach (var item in cateInfos)
                {
                    cateBll.CreatePMSCategoryServeice(item);
                }
                //清除分类缓存
                dataCache.DeleteCache("GetAllCateList-CateList");
                dataCache.ClearAll();
            }
            catch (Exception ex)
            {
                Log.LogHelper.AddTextLog("同步分类信息失败",ex.Message+"------"+ex.StackTrace);
            }
        }
        /// <summary>
        /// 同步品牌数据
        /// </summary>
        /// <param name="brandInfos"></param>
        public static void SyncBrands(List<YSWL.MALL.Model.Shop.Products.BrandInfo> brandInfos)
        {
            YSWL.MALL.BLL.Shop.Products.BrandInfo brandBll = new BrandInfo();
            try
            {
                //先重置表
                brandBll.ResetTable();
                foreach (var item in brandInfos)
                {
                    brandBll.CreateBrandsAndTypes(item);
                }
            }
            catch (Exception ex)
            {
                Log.LogHelper.AddTextLog("同步分类信息失败", ex.Message + "------" + ex.StackTrace);
            }
            //foreach (var item in brandInfos)
            //{
            //    if (brandBll.Exists(item.BrandId))
            //    {
            //        brandBll.UpdatePMSBrands(item);
            //    }
            //    else
            //    {
            //        brandBll.CreateBrandsAndTypes(item, Model.Shop.Products.DataProviderAction.Create);
            //    }
            //}
        }

        /// <summary>
        /// 同步商品类型数据
        /// </summary>
        /// <param name="productTypes"></param>
        public static void SyncProductType(List<YSWL.MALL.Model.Shop.Products.ProductType> productTypes)
        {
            YSWL.MALL.BLL.Shop.Products.ProductType typeBll = new YSWL.MALL.BLL.Shop.Products.ProductType();
            try
            {
                //先重置表
                typeBll.ResetTable();
                foreach (var item in productTypes)
                {
                    typeBll.CreatedProductType(item);
                }
            }
            catch (Exception ex)
            {
                Log.LogHelper.AddTextLog("同步类型信息失败", ex.Message + "------" + ex.StackTrace);
            }
           

            //int typeId = 0;
            //foreach (var item in productTypes)
            //{
            //    if (typeBll.Exists(item.TypeId))
            //    {
            //        typeBll.ProductTypeManage(item, Model.Shop.Products.DataProviderAction.Update,out typeId);
            //    }
            //    else
            //    {
            //        typeBll.ProductTypeManage(item, Model.Shop.Products.DataProviderAction.Create, out typeId);
            //    }
            //}
        }

        /// <summary>
        /// 同步属性表数据
        /// </summary>
        /// <param name="attributeInfos"></param>
        public static void SyncAttribute(List<YSWL.MALL.Model.Shop.Products.AttributeInfo> attributeInfos)
        {
            YSWL.MALL.BLL.Shop.Products.AttributeInfo attributeBll = new YSWL.MALL.BLL.Shop.Products.AttributeInfo();

            try
            {
                //先重置表
                attributeBll.ResetTable();
                foreach (var item in attributeInfos)
                {
                    attributeBll.CreatedAttribute(item);
                }

            }
            catch (Exception ex)
            {
                Log.LogHelper.AddTextLog("同步属性信息失败", ex.Message + "------" + ex.StackTrace);
            }

            //int typeId = 0;
            ////先重置表
            //attributeBll.ResetTable();
            //foreach (var item in attributeInfos)
            //{
            //    if (attributeBll.Exists(item.AttributeId))
            //    {
            //        attributeBll.AttributePMSManage(item, Model.Shop.Products.DataProviderAction.Update);
            //    }
            //    else
            //    {
            //        attributeBll.AttributePMSManage(item, Model.Shop.Products.DataProviderAction.Create);
            //    }
            //}
        }

        #endregion 

    }
}

