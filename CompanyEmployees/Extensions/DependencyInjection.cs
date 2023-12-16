using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Contracts;
using Entities.Models;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Service.Contracts;
using Services;
using Shared.DTOs;
using Shared.Options;

namespace CompanyEmployees.Extensions;

public static class DependencyInjection
{
	public static IServiceCollection ConfigureCors(this IServiceCollection services)
	{
		services.AddCors(options =>
		{
			options.AddPolicy("CorsPolicy", builder =>
			{
				builder
					.AllowAnyMethod()
					.AllowAnyOrigin()
					.AllowAnyHeader()
					.WithExposedHeaders("X-Pagination");
			});
		});
		return services;

	}

	public static IServiceCollection ConfigureIISIntegration(this IServiceCollection services)
	{
		services.Configure<IISOptions>(options =>
		{			

		});
		return services;
	}

	public static IServiceCollection ConfigureLoggerService(this IServiceCollection services)
	{
		services.AddSingleton<ILoggerManager, LoggerManager>();
		return services;
	}

    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
		services.AddAuthentication(config =>
		{
			config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
			.AddJwtBearer(config =>
			{
				config.TokenValidationParameters = new()
				{
					ValidIssuer = configuration["JwtSettings:Issuer"],
					ValidAudience = configuration["JwtSettings:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"])),
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true
				};
			});

		services.AddTransient<IAuthenticationService, AuthenticationService>();
		return services;
    }

    public static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                JwtBearerDefaults.AuthenticationScheme);

            defaultAuthorizationPolicyBuilder =
                defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();

            options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
        });
        return services;
    }

    public static IServiceCollection ConfigureRepositoryManager(this IServiceCollection services)
	{
		services.AddScoped<IRepositoryManager, RepositoryManager>();
		return services;
	}

	public static IServiceCollection ConfigureServiceManager(this IServiceCollection services)
	{
		services.AddScoped<IServiceManager, ServiceManager>();
		return services;
	}

    public static IServiceCollection ConfigureCaching(this IServiceCollection services)
    {
		services.AddResponseCaching();
        return services;
    }

    public static IServiceCollection ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContextPool<RepositoryContext>(options =>
		{
			options.UseSqlServer(configuration.GetConnectionString("sqlConnection"), b =>
			{
				b.MigrationsAssembly(nameof(Repository));
			});
		});
		return services;
	}

    public static IServiceCollection ConfigureJwtOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("JwtSettings"));
        return services;
    }

    public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
		services
			.AddIdentity<AppUser, IdentityRole>()
			.AddEntityFrameworkStores<RepositoryContext>();

        return services;
    }

    public static IServiceCollection ConfigureControllersAndFormatters(this IServiceCollection services)
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

			config.CacheProfiles.Add("120SecondsDuration", new CacheProfile() { Duration = 120 });
			
				
		})
		.AddCustomCSVFormatter<ExpandoObject>()
		.AddCustomCSVFormatter<BaseDto>()
		.AddXmlDataContractSerializerFormatters()
		.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

		return services;
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
