using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Link.Multicanal.API
{
    public class ServidorService
    {

        public event EventHandler<MessageInspectorArgs> OnMessageInspected;

        private readonly Link.Multicanal.API.Multicanal.WebService.Servidor.AddServicePortTypeClient _multicanalWebService;
        private readonly ILogger<ServidorService> _logger;

        public ServidorService(ILogger<ServidorService> logger, Link.Multicanal.API.Multicanal.WebService.Servidor.AddServicePortTypeClient multicanalWebService) {
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

        public async Task<Response.EnvioSmsResponse> CallEnvioSms(string host, Request.EnvioSmsRequest request) {

            Response.EnvioSmsResponse result = null;
            try
            {

                this._multicanalWebService.Endpoint.Address = new System.ServiceModel.EndpointAddress(host);

                var _result = await this._multicanalWebService.envioSmsAsync(request.login, request.password, request.tipoProgramacion, request.dia, request.mes, request.ano, request.hora, request.minuto, request.departamento, request.referencia, request.asunto, request.RemitenteSms, request.texto, request.destino);

                result = Response.EnvioSmsResponse.BuildResponse(_result);

            }
            catch (Exception ex) {
                throw new Exception($"Error on call method 'EnvioSms', host: '{host}', request: {request}", ex);
            }

            return result;
            
        }

        public async Task<Multicanal.WebService.Servidor.envioSms02Response> CallEnvioSms02(string host, Multicanal.WebService.Servidor.envioSms02Request request)
        {

            Multicanal.WebService.Servidor.envioSms02Response result = null;
            try
            {

                this._multicanalWebService.Endpoint.Address = new System.ServiceModel.EndpointAddress(host);

                result = await this._multicanalWebService.envioSms02Async(request);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error on call method 'EnvioSms02', host: '{host}', request: {request}", ex);
            }

            return result;

        }
    }
}
