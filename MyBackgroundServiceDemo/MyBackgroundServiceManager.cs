namespace MyBackgroundServiceDemo;
public class MyBackgroundServiceManager : BackgroundService
{
    private readonly ILogger<MyBackgroundServiceManager> _logger;
    private readonly IServiceProvider _myScopedService;
    public MyBackgroundServiceManager(IServiceProvider myScopedService,
        ILogger<MyBackgroundServiceManager> logger)
    {
        _logger = logger;
        _myScopedService = myScopedService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _myScopedService.CreateScope())
            {
                _logger.LogInformation("ExecuteAsync Started {dateTime}", DateTime.Now);
                 var scopedService = scope.ServiceProvider.GetRequiredService<IScopedService>();
                scopedService.Write();
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }

    public override Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("StopAsync Started {dateTime}", DateTime.Now);
        return base.StopAsync(stoppingToken);
    }
}
