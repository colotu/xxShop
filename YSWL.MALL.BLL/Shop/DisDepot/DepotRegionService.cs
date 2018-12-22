using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using YSWL.MALL.BLL.Shop.Service;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.DisDepot;

namespace YSWL.MALL.BLL.Shop.DisDepot
{
    public class DepotRegionService
    {
        private static readonly IDepotRegion service = DAShopDisDepot.CreateDepotRegion();
        /// <summary>
        /// 同步仓库区域
        /// </summary>
        /// <param name="depotRegion"></param>
        /// <returns></returns>
        public static bool SyncDepotRegion(YSWL.MALL.Model.Shop.DisDepot.DepotRegion depotRegion)
        {
            //清空远程缓存
            //CommonHelper.ClearCache();
            Common.DataCache.ClearAll();
            return service.SyncDepotRegion(depotRegion);
        }
        /// <summary>
        /// 同步删除地区区域
        /// </summary>
        /// <param name="depotId"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public static bool DeleteDepotRegion(int depotId,int regionId)
        {
            //CommonHelper.ClearCache();
            Common.DataCache.ClearAll();
            return service.Delete(depotId, regionId);
        }
        
        /// <summary>
        /// 同步仓库
        /// </summary>
        /// <param name="depotModel"></param>
        /// <returns></returns>
        public static bool SyncDepot(YSWL.MALL.Model.Shop.DisDepot.Depot depotModel)
        {
            //CommonHelper.ClearCache();
            Common.DataCache.ClearAll();
            YSWL.MALL.BLL.Shop.DisDepot.Depot depotBll = new YSWL.MALL.BLL.Shop.DisDepot.Depot();
            if (depotBll.Exists(depotModel.DepotId))
            {
                return depotBll.Update(depotModel);
            }
            else
            {
                return depotBll.Add(depotModel)>0;
            }
        }

        /// <summary>
        /// 同步默认仓库
        /// </summary>
        /// <param name="depotId"></param>
        /// <returns></returns>
        public static void SyncDefaultDepot(int depotId)
        {
            //CommonHelper.ClearCache();
            Common.DataCache.ClearAll();
            BLL.SysManage.ConfigSystem.Modify("OMS_DefaultDepot", depotId.ToString(), "开启的默认仓库ID");
        }

        /// <summary>
        /// 设置默认仓库
        /// </summary>
        /// <param name="depotId"></param>
        /// <returns></returns>
        public static void SetDefaultDepot(int depotId)
        {
            BLL.SysManage.ConfigSystem.Modify("OMS_DefaultDepot", depotId.ToString(), "开启的默认仓库ID");

          
            ////同步至OMS设置
            //YSWL.MALL.BLL.Shop.Service.OMSServiceHelper.DefaultHelper.SyncDefaultDepot(depotId);
        }
    }
}
