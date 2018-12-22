using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YSWL.MALL.ViewModel.WeChat
{
    public class MenuListVm
    {
        public DateTime CreateDate { get; set; }
        public bool HasChildren { get; set; }
        public int MenuId { get; set; }
        public string MenuKey { get; set; }
        public string MenuUrl { get; set; }
        public string Name { get; set; }
        public string OpenId { get; set; }
        public int ParentId { get; set; }
        public string Remark { get; set; }
        public int Sequence { get; set; }
        public int Status { get; set; }
        public string Type { get; set; }
        public List<MenuListVm> Children { get; set; } 
    }
}
