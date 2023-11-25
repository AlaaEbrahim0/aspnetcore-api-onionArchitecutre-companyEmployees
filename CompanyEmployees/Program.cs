using CompanyEmployees.Presentation.ActionFilters;
using Contracts;
using NLog;
using Services.DataShaping;
using Shared.DTOs;

namespace CompanyEmployees;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.

		builder.Services
			.ConfigureCors()
			.ConfigureIISIntegration()
			.ConfigureLoggerService()
			.ConfigureRepositoryManager()
			.ConfigureServiceManager()
			.AddAutoMapper(typeof(Program))
			.ConfigureSqlContext(builder.Configuration)
			.ConfigureControllersAndFormatters()
			.AddScoped<ValidationFilterAttribute>()
			.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();

		builder.Services.Configure<ApiBehaviorOptions>(options =>
		{
			options.SuppressModelStateInvalidFilter = true;
		});
		

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
		
		// will help us in deployment
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
