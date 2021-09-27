using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Link.Multicanal.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly Service.AppService _appService;

        public Worker(ILogger<Worker> logger, Service.AppService appService)
        {
            _logger = logger;
            this._appService = appService;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            this._appService.Initialize();

            await this._appService.Start();
        }
    }
}
