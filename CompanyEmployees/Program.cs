	using Contracts;
using LoggerService;
using NLog;
using Service.Contracts;
using Services;

namespace CompanyEmployees;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.

		builder.Services.ConfigureCors();
		builder.Services.ConfigureIISIntegration();

		builder.Services
			.AddControllers()
			.AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);

		builder.Services.ConfigureLoggerService();
		builder.Services.ConfigureRepositoryManager();
		builder.Services.ConfigureServiceManager();
		builder.Services.ConfigureSqlContext(builder.Configuration);
		
		LogManager.Setup(builder =>
			builder.LoadConfigurationFromFile($@"{Directory.GetCurrentDirectory()}\nlog.config"));

		var app = builder.Build();

		var logger = app.Services.GetRequiredService<ILoggerManager>();
		app.ConfigureExceptionHandler(logger);

		if (app.Environment.IsProduction()) 
			app.UseHsts();

		// Configure the HTTP request pipeline.
		app.UseHttpsRedirection();

		// enables using static files for the request
		// If we don’t set a path to the static files directory
		// It will use a wwwroot folder in our project by default.
		app.UseStaticFiles();


		// We will help us in deployment
		app.UseForwardedHeaders(new ForwardedHeadersOptions
		{
			ForwardedHeaders = ForwardedHeaders.All
		});

		// Using the cors configuration in the cors middleware
		app.UseCors("CorsPolicy");

		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}
