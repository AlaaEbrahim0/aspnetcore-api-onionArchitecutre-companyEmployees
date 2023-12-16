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
			.ConfigureLoggerService()
			.ConfigureIISIntegration()
			.ConfigureRepositoryManager()
			.ConfigureServiceManager()
			.ConfigureCaching()
			.ConfigureJwtOptions(builder.Configuration)
            .ConfigureIdentity()
			.ConfigureAuthentication(builder.Configuration)
			.ConfigureAuthorization()
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

		app.UseHttpsRedirection();

		app.UseStaticFiles();
		
		// will help us in deployment
		app.UseForwardedHeaders(new ForwardedHeadersOptions
		{
			ForwardedHeaders = ForwardedHeaders.All
		});

		// Using the cors configuration in the cors middleware
		app.UseCors("CorsPolicy");

		app.UseResponseCaching();

		app.UseAuthentication();

		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}
