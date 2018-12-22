using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace YSWL.Common.CustomMessage
{
    /// <summary>
    /// 服务端自定义消息处理
    /// </summary>
    public class ServiceMessageInspector : IDispatchMessageInspector
    {
        #region messageHandle

        /// <summary>
        /// 接收到客户端消息请求后执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <param name="instanceContext"></param>
        /// <returns></returns>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            try
            {
                System.ServiceModel.Web.WebOperationContext context = System.ServiceModel.Web.WebOperationContext.Current;
                string enterpriseStr = context.IncomingRequest.Headers.Get("YSWL_Auto_Enterprise");
                if (string.IsNullOrEmpty(enterpriseStr))
                {
                    enterpriseStr = request.Headers.GetHeader<string>("YSWL_Auto_Enterprise", "YSWL");
                }
                long enterpriseId = Common.DEncrypt.DEncrypt.ConvertToNumber(enterpriseStr);
                Common.CallContextHelper.SetValue("YSWL_SAAS_EnterpriseID", enterpriseId.ToString());
            }
            catch (Exception ex)
            {
                //记录日志
            }
            return null;
        }

        /// <summary>
        /// 服务端响应消息时执行
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="correlationState"></param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            //服务端响应消息时执行
        }

        #endregion

    }
}
