using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using YSWL.Log;

namespace YSWL.Common.CustomMessage
{
    /// <summary>
    /// 自定义客户端消息拦截
    /// </summary>
    public class ClientMessageInspector : IClientMessageInspector, IEndpointBehavior
    {

        #region 客户端消息处理，发送前后执行方法

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            //收到消息后执行
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {           
            request.Headers.Add(System.ServiceModel.Channels.MessageHeader.CreateHeader("YSWL_Auto_Enterprise", "YSWL", Common.CallContextHelper.GetWCFTag()));
            return null;
        }

        #endregion

        #region 将消息处理添加到httpruntime中

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            //添加绑定参数
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(this);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            //注册服务端
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            //可以做验证
        }

        #endregion


    }
}
