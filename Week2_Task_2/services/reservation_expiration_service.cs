namespace Week2_Task_2.services
{
    public class reservation_expiration_service : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<reservation_expiration_service> logger;

        public reservation_expiration_service(IServiceScopeFactory scopeFactory,ILogger<reservation_expiration_service> logger)
        {
            this.scopeFactory = scopeFactory;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = scopeFactory.CreateScope();

                var service =
                    scope.ServiceProvider.GetRequiredService<iservices_reservation>();

                var expired = await service.ExpireReservations();

                if (expired > 0)
                {
                    logger.LogInformation( "{count} reservations expired",expired);
                }

                await Task.Delay(TimeSpan.FromMinutes(1),stoppingToken);
            }
        }
    }
}