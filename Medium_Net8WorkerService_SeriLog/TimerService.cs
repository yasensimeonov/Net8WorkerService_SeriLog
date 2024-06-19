using System.ServiceProcess;

namespace Medium_Net8WorkerService_SeriLog
{
    public sealed class TimerService(ILogger<TimerService> logger) : IHostedService, IAsyncDisposable
    {
        private readonly Task _completedTask = Task.CompletedTask;
        private int _executionCount = 0;
        private Timer? _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("{Service} is running.", nameof(Medium_Net8WorkerService_SeriLog));
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return _completedTask;
        }

        private void DoWork(object? state)
        {
            int count = Interlocked.Increment(ref _executionCount);

            logger.LogInformation(
                "{Service} is working, execution count: {Count:#,0}",
                nameof(Medium_Net8WorkerService_SeriLog),
                count);
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "{Service} is stopping.", nameof(Medium_Net8WorkerService_SeriLog));

            _timer?.Change(Timeout.Infinite, 0);

            return _completedTask;
        }

        public async ValueTask DisposeAsync()
        {
            if (_timer is IAsyncDisposable timer)
            {
                await timer.DisposeAsync();
            }

            _timer = null;
        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        if (_logger.IsEnabled(LogLevel.Information))
        //        {
        //            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        //        }
        //        await Task.Delay(1000, stoppingToken);
        //    }
        //}    

    }
}
