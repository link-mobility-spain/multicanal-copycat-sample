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
        private readonly Multicanal.API.ServidorService _servidorService;

        public AppService(ILogger<AppService> logger,
            Config.AppConfig appConfig,
            Multicanal.API.ServidorSmsService servidorSmsService,
            Multicanal.API.ServidorService servidorService
            )
        {
            // set condig
            this._appConfig = appConfig;

            // set logger
            this._logger = logger;

            // set services
            this._servidorSmsService = servidorSmsService;
            this._servidorService = servidorService;
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
                await this.DoEnvioSmsTest();
                await this.DoEnvioSms02Test();

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
                    destinatario = "+346000000000",
                    remitente = "link.es",
                    charset = String.Empty, // For Unicode SMS set "UCS UTF-8"
                    texto = $"Test Multicanal copycat - EnvioSmsDirecto {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}",
                    solicitarDR = "1"
                };

                var result = await this._servidorSmsService.CallEnvioSmsDirecto(this._appConfig.MulticanalAPI.host, request);

                if (result.resultado != "62000")
                    this._logger.LogError($"Error to send envioSmsDirecto: {result.descripcion}");
                else
                    this._logger.LogInformation($"Request success. refWeb: {result.refWeb}");

                this._servidorSmsService.OnMessageInspected -= (src, e) => { };

            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error call EnvioSmsDirecto");
                this._logger.LogError(ex, ex.Message);
            }
        }

        private async Task DoEnvioSmsTest()
        {

            try
            {
                // Activate Message Inspector to get XML Request
                /*
                this._servidorService.OnMessageInspected += (src, e) =>
                {
                    if (e.MessageInspectionType == API.eMessageInspectionType.Request)
                    {
                        this._logger.LogInformation("Message Request");
                        this._logger.LogInformation(e.Message);
                    }
                };
                */

                var request = new Link.Multicanal.API.Request.EnvioSmsRequest
                {
                    login = this._appConfig.MulticanalAPI.user,
                    password = this._appConfig.MulticanalAPI.password,
                    departamento = String.Empty,
                    referencia = Guid.NewGuid().ToString(),
                    tipoProgramacion = String.Empty,
                    ano = String.Empty,
                    mes = String.Empty,
                    dia = String.Empty,
                    hora = String.Empty,
                    minuto = String.Empty,
                    asunto = String.Empty,
                    destino = "+346000000000",
                    RemitenteSms = "link.es",
                    texto = $"Test Multicanal copycat - EnvioSms {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}"
                };

                var result = await this._servidorService.CallEnvioSms(this._appConfig.MulticanalAPI.host, request);
                
                if (result.resultado != "50000")
                    this._logger.LogError($"Error to send envioSms: {result.descripcion}");
                else
                    this._logger.LogInformation($"Request success. refWeb: {result.refWeb}");

                this._servidorService.OnMessageInspected -= (src, e) => { };

            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error call EnvioSms");
                this._logger.LogError(ex, ex.Message);
            }
        }

        private async Task DoEnvioSms02Test()
        {

            try
            {
                // Activate Message Inspector to get XML Request
                /*
                this._servidorService.OnMessageInspected += (src, e) =>
                {
                    if (e.MessageInspectionType == API.eMessageInspectionType.Request)
                    {
                        this._logger.LogInformation("Message Request");
                        this._logger.LogInformation(e.Message);
                    }
                };
                */

                var request = new API.Multicanal.WebService.Servidor.envioSms02Request
                {
                    login = this._appConfig.MulticanalAPI.user,
                    password = this._appConfig.MulticanalAPI.password,
                    departamento = String.Empty,
                    referencia = Guid.NewGuid().ToString(),
                    tipoProgramacion = String.Empty,
                    ano = String.Empty,
                    mes = String.Empty,
                    dia = String.Empty,
                    hora = String.Empty,
                    minuto = String.Empty,
                    asunto = String.Empty,
                    destino = "+346000000000",
                    RemitenteSms = "link.es",
                    texto = $"Test Multicanal copycat - EnvioSms02 {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}",
                    expiracion = String.Empty
                };

                var result = await this._servidorService.CallEnvioSms02(this._appConfig.MulticanalAPI.host, request);
                

                if (result.resultado != "50000")
                    this._logger.LogError($"Error to send envioSms02: {result.descripcion}");
                else
                    this._logger.LogInformation($"Request success. refWeb: {result.refWeb}");

                this._servidorService.OnMessageInspected -= (src, e) => { };

            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error call EnvioSms02");
                this._logger.LogError(ex, ex.Message);
            }
        }
    }
}
