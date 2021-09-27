using System;

using Newtonsoft.Json;

using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Link.Multicanal.WorkerService.Service
{
    public class AppService
    {

        private readonly Config.AppConfig _appConfig;
        private readonly ILogger<AppService> _logger;
        private readonly Multicanal.API.ServidorSmsService _servidorSmsService;

        public AppService(ILogger<AppService> logger,
            Config.AppConfig appConfig,
            Multicanal.API.ServidorSmsService servidorSmsService
            )
        {
            // set condig
            this._appConfig = appConfig;

            // set logger
            this._logger = logger;

            // set services
            this._servidorSmsService = servidorSmsService;
        }

        public void Initialize()
        {
            this._logger.LogInformation("AppService Initializing...");

            this._logger.LogDebug($"configuration: \r\n {Newtonsoft.Json.JsonConvert.SerializeObject(this._appConfig, Formatting.Indented)}");

            /*    
            // set other object intilializing below
            // ....
            // ....
            */
        }


        public async Task Start()
        {
            this._logger.LogInformation("AppService Start...");

            try
            {
                await this.RunMulticanalCopycatTest();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);
            }

            this._logger.LogInformation("AppService Finish...");
        }


        private async Task RunMulticanalCopycatTest()
        {

            try
            {
                await this.DoEnvioSmsDirectotest();

            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Error on RunMulticanalCopycatTest()");

            }
        }


        private async Task DoEnvioSmsDirectotest()
        {

            try
            {
                // Activate Message Inspector to get XML Request
                /*
                this._servidorSmsService.OnMessageInspected += (src, e) =>
                {
                    if (e.MessageInspectionType == API.eMessageInspectionType.Request)
                    {
                        this._logger.LogInformation("Message Request");
                        this._logger.LogInformation(e.Message);
                    }
                };
                */

                var request = new API.Multicanal.WebService.ServidorSms.envioSmsDirectoRequest
                {
                    login = this._appConfig.MulticanalAPI.user,
                    password = this._appConfig.MulticanalAPI.password,
                    idCuenta = String.Empty,
                    departamento = String.Empty,
                    referencia = Guid.NewGuid().ToString(),
                    destinatario = "+346000000",
                    remitente = "link.es",
                    charset = String.Empty, // For Unicode SMS set "UCS UTF-8"
                    texto = "Test Multicanal copycat",
                    solicitarDR = "1"
                };

                var result = await this._servidorSmsService.CallEnvioSmsDirecto(this._appConfig.MulticanalAPI.host, request);

                var refWeb = result.refWeb;

                this._logger.LogInformation($"Request success. refWeb: {refWeb}");

                if (result.resultado != "62000")
                    this._logger.LogError($"Error to send envioSmsDirecto: {result.descripcion}");

                this._servidorSmsService.OnMessageInspected -= (src, e) => { };

            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Error call EnvioSmsDirecto");
            }
        }
    }
}
