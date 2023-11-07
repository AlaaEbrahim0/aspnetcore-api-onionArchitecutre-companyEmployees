using Contracts;
using LoggerService;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repository;
using Service.Contracts;
using Services;
using Shared.DTOs;

namespace CompanyEmployees.Extensions;

public static class ServiceExtensions
{
	public static void ConfigureCors(this IServiceCollection services)
	{
		services.AddCors(options =>
		{
			options.AddPolicy("CorsPolicy", builder =>
			{
				builder
					.AllowAnyMethod()
					.AllowAnyOrigin()
					.AllowAnyHeader();
			});
		});
	}
	public static void ConfigureIISIntegration(this IServiceCollection services)
	{
		services.Configure<IISOptions>(options =>
		{			

		});
	}

	public static void ConfigureLoggerService(this IServiceCollection services)
	{
		services.AddSingleton<ILoggerManager, LoggerManager>();
	}

	public static void ConfigureRepositoryManager(this IServiceCollection services)
	{
		services.AddScoped<IRepositoryManager, RepositoryManager>();
	}
	public static void ConfigureServiceManager(this IServiceCollection services)
	{
		services.AddScoped<IServiceManager, ServiceManager>();
	}

	public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContextPool<RepositoryContext>(options =>
		{
			options.UseSqlServer(configuration.GetConnectionString("sqlConnection"), b =>
			{
				b.MigrationsAssembly(nameof(Repository));
			});
		});
	}

	public static void ConfigureControllersAndFormatters(this IServiceCollection services)
	{
		services
		.AddControllers(config =>
		{
			// Respect the HTTP [Accept] Header 
			config.RespectBrowserAcceptHeader = true;

			// tells the server that if the client tries to negotiate
			// for a media type the server doesn’t support, 
			// it should return the 406 Not Acceptable status code.
			config.ReturnHttpNotAcceptable = true;

			config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
			
				
		})
		.AddCustomCSVFormatter<CompanyDto>()
		.AddXmlDataContractSerializerFormatters()
		.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
	}

	private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
	{
		return
			new ServiceCollection()
			.AddLogging()
			.AddMvc()
			.AddNewtonsoftJson()
			.Services
			.BuildServiceProvider()
			.GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
			.OfType<NewtonsoftJsonPatchInputFormatter>()
			.First();
	}

}
