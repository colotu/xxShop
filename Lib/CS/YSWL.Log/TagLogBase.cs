using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.Log
{
    /// <summary>
    /// 日志实体必须继承该基类
    /// </summary>
    public class TagLogBase
    {
        /// <summary>
        /// 这个系统自动赋值，不填写
        /// </summary>
        public object LogType { get; set; }

        /// <summary>
        /// 该字段是为了支持工厂模式设计，可以为空
        /// </summary>
        public string TargetObj { get; set; }

        /// <summary>
        /// 该字段是为了标识具体日志写入对象适配器，为必填
        /// </summary>
        public string TargetAdapterAndClientObj { get; set; }
    }
}
