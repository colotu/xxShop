using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSWL.MALL.ViewModel.WeChat
{
    public class AddMenuVm
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// actionId
        /// </summary>
        public int ActionId { get; set; }
        /// <summary>
        /// url地址
        /// </summary>
        public string Url { get; set; }
    }
}
