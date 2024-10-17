using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

/// <summary>
/// The main program class for the ContactManager application.
/// </summary>
public class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static void Main(string[] args)
    {
        // Read environment variables
        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
        var database = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "ContactManager";
        var username = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres";
        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "Admin";
        var connectionString = $"Host={host};Database={database};Username={username};Password={password}";
        
        var builder = WebApplication.CreateBuilder(args);       
        builder.Services.AddControllers();
        builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "ContactManager API",
                Description = "An ASP.NET Core Web API for managing Contact items.",
                Contact = new OpenApiContact
                {
                    Name = "Jeannette Läpple",
                    Email = "jeannette.laepple@gmail.com",
                }
            });

            options.EnableAnnotations(); // Enable annotations

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

        });
        builder.Services.AddMvc().AddJsonOptions(options =>
        {
            //ensure that JSON output ignores null values
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        // Initialize app build here because of a .NET8 problem -> https://stackoverflow.com/questions/78760537/the-service-collection-cannot-be-modified-because-it-is-read-only 
        var app = builder.Build();
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        // Apply migrations at startup
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            try
            {
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Migration failed: {ex.Message}");
            }
        }

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ContactManager V1");
        });
        app.MapControllers();
        app.Run();
    }
}