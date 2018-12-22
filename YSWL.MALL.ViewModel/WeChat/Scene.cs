using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace YSWL.MALL.ViewModel.WeChat
{
    /// <summary>
    /// 渠道统计
    /// </summary>
    public class SecneStat
    {
        /// <summary>
        /// 场景ID
        /// </summary>
        public int SceneId;
        /// <summary>
        /// 场景名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 数量
        /// </summary>
        public int Count;
    }
}
