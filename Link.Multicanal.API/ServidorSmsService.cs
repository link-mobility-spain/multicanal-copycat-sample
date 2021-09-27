using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Link.Multicanal.API
{
    public class ServidorSmsService
    {

        public event EventHandler<MessageInspectorArgs> OnMessageInspected;

        private readonly Link.Multicanal.API.Multicanal.WebService.ServidorSms.AddServicePortTypeClient _multicanalWebService;
        private readonly ILogger<ServidorSmsService> _logger;

        public ServidorSmsService(ILogger<ServidorSmsService> logger, Link.Multicanal.API.Multicanal.WebService.ServidorSms.AddServicePortTypeClient multicanalWebService) {
            this._logger = logger;
            this._multicanalWebService = multicanalWebService;
            this.Initialize();
        }

        private void Initialize() {

            var mediaType = "text/xml";
            var encoding = "ISO-8859-1";
            var messageVersion = MessageVersion.Soap11;

            // Create a custom text message encoder
            var customTextMessageEncoderFactory =
                    new CustomTextMessageEncoderFactory(mediaType,
                        encoding, messageVersion);

            // Intercept Request
            customTextMessageEncoderFactory.CustomEncoder.OnMessageInspected += (src, e) =>
            {
                if (this.OnMessageInspected != null) OnMessageInspected(this, e);
            };

            // Create a custom Text Message Binding Element
            var customTextMessageBindingElement = new CustomTextMessageBindingElement(customTextMessageEncoderFactory);
            
            // Create a custom binding
            this._multicanalWebService.Endpoint.Binding = new CustomBinding(
                customTextMessageBindingElement,
                new HttpsTransportBindingElement());

            // Set a Message Inspector Behavior to intercept the Response
            this.SetMessageInspectorBehavior();

        }

        private void SetMessageInspectorBehavior() {
            // Create  Message Inspector Behavior
            MessageInspectorBehavior cb = new MessageInspectorBehavior();
            this._multicanalWebService.Endpoint.EndpointBehaviors.Add(cb);

            // Intercept Response via event
            cb.OnMessageInspected += (src, e) =>
            {
                if (this.OnMessageInspected != null) OnMessageInspected(this, e);
            };
        }

        public async Task<Multicanal.WebService.ServidorSms.envioSmsDirectoResponse> CallEnvioSmsDirecto(string host, Multicanal.WebService.ServidorSms.envioSmsDirectoRequest request) {

            Multicanal.WebService.ServidorSms.envioSmsDirectoResponse result = null;
            try
            {

                this._multicanalWebService.Endpoint.Address = new System.ServiceModel.EndpointAddress(host);

                result = await this._multicanalWebService.envioSmsDirectoAsync(request);
            }
            catch (Exception ex) {
                throw new Exception($"Error on call method 'EnvioSmsDirecto', host: '{host}', request: {request}", ex);
            }

            return result;
            
        }
    }
}
