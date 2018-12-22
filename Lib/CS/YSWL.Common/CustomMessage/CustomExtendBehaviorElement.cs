using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;

namespace YSWL.Common.CustomMessage
{
    /// <summary>
    /// 客户端自定义元素扩展（通过配置启用客户端消息处理）
    /// </summary>
    public class CustomExtendBehaviorElement : BehaviorExtensionElement
    {
        /// <summary>
        /// 获取客户端消息对象类型
        /// </summary>
        public override Type BehaviorType
        {
            get
            {
                return typeof(ClientMessageInspector);
            }
        }

        /// <summary>
        /// 初始化客户端消息对象
        /// </summary>
        /// <returns></returns>
        protected override object CreateBehavior()
        {
            return new ClientMessageInspector();
        }
    }
}
