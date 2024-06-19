using Medium_Net8WorkerService_SeriLog;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.File(@"C:\\LOGS\\Net8Worker_Medium\\StartupLog.txt")
    //.CreateLogger();
    .CreateBootstrapLogger();

// var builder = Host.CreateApplicationBuilder(args);

IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseWindowsService()
        .UseSerilog((context, services, configuration) => configuration
            //.WriteTo.Console()
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            )
        .ConfigureServices((services) =>
        {
            services.AddWindowsService(options =>
            {
                options.ServiceName = "SampleService";
            });
            services.AddHostedService<Worker>();
        });

//builder.Services.AddWindowsService(options =>
//{
//    options.ServiceName = "SampleService";
//});

//builder.Services.AddHostedService<Worker>();

//var host = builder.Build();
//host.Run();

try
{
    Log.Information("Starting up the service");
    var host = CreateHostBuilder(args).Build();
    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "There was a problem starting the serivce");
    throw;
}
finally
{
    Log.Information("Service successfully stopped");
    Log.CloseAndFlush();
}