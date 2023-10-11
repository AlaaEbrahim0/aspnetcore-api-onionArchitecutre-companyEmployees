using System.Net;
using CompanyEmployees.CustomFormatters;
using Contracts;
using Entities.ErrorModel;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace CompanyEmployees.Extensions;

public static class ExceptionMiddlewareExtensions
{
	public static IMvcBuilder AddCustomCSVFormatter<T>(this IMvcBuilder builder)
	{
		builder.AddMvcOptions(config =>
		{
			config.OutputFormatters.Add(new CsvOutputFormatter<T>());
		});

		return builder;
	}

	public static void ConfigureExceptionHandler(this WebApplication app, 
		ILoggerManager logger)
	{
		app.UseExceptionHandler(error =>
		{
			error.Run(async context =>
			{
				context.Response.ContentType = "application/json";

				var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
				if (contextFeature != null)
				{
					context.Response.StatusCode = contextFeature.Error
					switch
					{
						NotFoundException => StatusCodes.Status404NotFound,
						BadRequestException => StatusCodes.Status400BadRequest,
						_ => StatusCodes.Status500InternalServerError
					};
					logger.LogError($"Something went wrong: {contextFeature.Error}");

					await context.Response.WriteAsync(new ErrorDetails
					{
						StatusCode = context.Response.StatusCode,
						Message = contextFeature.Error.Message
					}.ToString());

				};
			});
		});
	}
}
