using System;
using System.Collections.Generic;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace Link.Multicanal.API
{
    //

    /// Class to perform custome message inspection as behaviour.
    /// 
    public class MessageInspectorBehavior : IClientMessageInspector, IEndpointBehavior
    {

        // Acts as the event to notify subscribers of message inspection.
        public event EventHandler<MessageInspectorArgs> OnMessageInspected;

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            if (OnMessageInspected != null)
            {
                // Notify the subscribers of the inpected message.
                OnMessageInspected(this, new MessageInspectorArgs { Message = reply.ToString(), MessageInspectionType = eMessageInspectionType.Response });
            }
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            foreach (var item in request.Headers) {
                Console.WriteLine("");
            }
            return null;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            // Do nothing.
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            // Add the message inspector to the as part of the service behaviour.
            clientRuntime.ClientMessageInspectors.Add(this);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            // Do nothing.
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            // Do nothing.
        }
    }
}
