using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace YSWL.Common.CustomMessage
{
    /// <summary>
    /// 自定义服务端的行为特性（服务行为、终结点行为、契约行为）
    /// </summary>
    public class CustomExtendBehaviorAttribute : Attribute, IServiceBehavior, IEndpointBehavior, IContractBehavior
    {

        #region EndpointBehavior

        /// <summary>
        /// 设置客户端消息处理对象
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="clientRuntime"></param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new ClientMessageInspector());
        }

        /// <summary>
        /// 设置服务端消息处理对象
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="endpointDispatcher"></param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new ServiceMessageInspector());
        }

        /// <summary>
        /// 终结点行为验证
        /// </summary>
        /// <param name="endpoint"></param>
        public void Validate(ServiceEndpoint endpoint)
        {

        }

        #endregion

        #region ServiceBehavior

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {

        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {

        }

        /// <summary>
        /// 设置服务端消息处理对象
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher endpoint in channelDispatcher.Endpoints)
                {
                    endpoint.DispatchRuntime.MessageInspectors.Add(new ServiceMessageInspector());
                }
            }

        }

        /// <summary>
        /// 服务行为验证
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            return;
        }

        #endregion

        #region ContractBehavior

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {

        }

        /// <summary>
        /// 添加客户端消息处理对象
        /// </summary>
        /// <param name="contractDescription"></param>
        /// <param name="endpoint"></param>
        /// <param name="clientRuntime"></param>
        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new ClientMessageInspector());
        }

        /// <summary>
        /// 添加服务端消息处理对象
        /// </summary>
        /// <param name="contractDescription"></param>
        /// <param name="endpoint"></param>
        /// <param name="dispatchRuntime"></param>
        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.MessageInspectors.Add(new ServiceMessageInspector());
        }

        /// <summary>
        /// 契约行为验证
        /// </summary>
        /// <param name="contractDescription"></param>
        /// <param name="endpoint"></param>
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {

        }

        #endregion

    }
}
