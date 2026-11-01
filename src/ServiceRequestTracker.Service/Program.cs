using MassTransit;
using Microsoft.EntityFrameworkCore;
using ServiceRequestTracker.Persistence;
using ServiceRequestTracker.Service.Consumers;
using ServiceRequestTracker.Service.Repositories;

namespace ServiceRequestTracker.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true,
                    reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddDbContext<ServiceRequestTrackerDbContext>(dbContextOptionsBuilder =>
            {
                var connectionString = configuration.GetSection("ConnectionStrings:DBConnection").Get<string>();
                dbContextOptionsBuilder.UseNpgsql(connectionString);
            });

            builder.Services.AddMassTransit(x =>
            {
                // Register the consumers
                x.AddConsumer<CreateServiceRequestConsumer>();
                x.AddConsumer<UpdateServiceRequestConsumer>();

                // Configure the Transactional Outbox & Inbox State
                x.AddEntityFrameworkOutbox<ServiceRequestTrackerDbContext>(o =>
                {
                    // Enables the Inbox pattern to detect duplicate messages
                    o.UsePostgres();
                });

                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitMqUri = configuration.GetSection("ConnectionStrings:RabbitMqUri").Get<string>();
                    cfg.Host(new Uri(rabbitMqUri));
                    cfg.ConfigureEndpoints(context);
                });
            });

            builder.Services.AddScoped<IServiceRequestsRepository, ServiceRequestsRepository>();

            var host = builder.Build();
            host.Run();
        }
    }
}
