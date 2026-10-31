using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using ServiceRequestTracker.API.Repositories;
using ServiceRequestTracker.API.Services;
using ServiceRequestTracker.Persistence;
using System.Text.Json.Serialization;

namespace ServiceRequestTracker.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true,
                    reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Add services to the container.
            await ConfigureServices(builder, configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseRequestTimeouts();
                app.UseDeveloperExceptionPage();

                app.UseHsts();
                app.UseHttpsRedirection();

                using (var scope = app.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ServiceRequestTrackerDbContext>();
                    await dbContext.Database.MigrateAsync();
                }
            }

            app.UseRouting();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllers();

            await app.RunAsync();
        }

        private static async Task ConfigureServices(WebApplicationBuilder builder, IConfiguration configuration)
        {
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddRequestTimeouts(options =>
                {
                    options.DefaultPolicy = new RequestTimeoutPolicy
                    {
                        Timeout = TimeSpan.FromSeconds(60),
                        TimeoutStatusCode = 504
                    };
                });
            }

            builder.Services.AddControllers()
                .AddJsonOptions(opts => { opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.MapType<DateOnly>(() => new OpenApiSchema
                {
                    Type = JsonSchemaType.String,
                    Format = "date"
                });
                // Enables support for nullable object properties
                options.UseAllOfToExtendReferenceSchemas();
                // Enable detection of non-nullable reference types to set Nullable flag accordingly on schema properties
                options.SupportNonNullableReferenceTypes();

                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Service Request Tracker service", Version = "v1" });
            });

            builder.Services.AddDbContext<ServiceRequestTrackerDbContext>(dbContextOptionsBuilder =>
            {
                var connectionString = configuration.GetSection("ConnectionStrings:DefaultConnection").Get<string>();
                dbContextOptionsBuilder.UseNpgsql(connectionString);
            });

            builder.Services.AddScoped<IServiceRequestsRepository, ServiceRequestsRepository>();
            builder.Services.AddScoped<IServiceRequestsService, ServiceRequestsService>();
        }
    }
}
