using TheGentlemanLibrary.Application.Models.Orders.Interfaces;

namespace Workers
{
    public class OrderBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<OrderBackgroundService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Timed Hosted Service running.");

            await DoWorkAsync(stoppingToken);

            using PeriodicTimer timer = new(TimeSpan.FromSeconds(30));

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await DoWorkAsync(stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Timed Hosted Service is stopping.");
            }
        }

        private async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("hello");
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IOrderService scopedProcessingService = scope.ServiceProvider.GetRequiredService<IOrderService>();

            await scopedProcessingService.RemoveInactiveOrders(stoppingToken);
        }
    }
}
