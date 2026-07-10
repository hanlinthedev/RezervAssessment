using System.Text.Json.Serialization;
using Rezerv.WhatsApp.Infrastructure;
using Rezerv.WhatsApp.Infrastructure.Mongo;
using Rezerv.WhatsApp.Application;
using Rezerv.WhatsApp.Infrastructure.Seed;
public partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
            });
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddApplication();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Frontend", policy =>
            {
                policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var indexInitializer = scope.ServiceProvider
                .GetRequiredService<MongoIndexInitializer>();

            await indexInitializer.InitializeAsync();

            var databaseSeeder = scope.ServiceProvider
                .GetRequiredService<DatabaseSeeder>();

            await databaseSeeder.SeedAsync();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("Frontend");
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = "An unexpected server error occurred."
                };

                await context.Response.WriteAsJsonAsync(response);
            });
        });
        app.UseAuthorization();
        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }
}