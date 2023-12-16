using System.Net;
using System.Net.Mime;
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

	public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
	{
		app.UseExceptionHandler(options =>
		{
			options.Run(async context =>
			{
				context.Response.ContentType = "application/json";

				var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
				if (exceptionHandlerFeature is not null)
				{
					context.Response.StatusCode = exceptionHandlerFeature.Error
					switch
					{
						NotFoundException => StatusCodes.Status404NotFound,
						BadRequestException => StatusCodes.Status400BadRequest,
						_ => StatusCodes.Status500InternalServerError
					};

					logger.LogError($"Something went wrong: {exceptionHandlerFeature.Error}");

					await context.Response.WriteAsync(new ErrorDetails
					{
						StatusCode = context.Response.StatusCode,
						Message = exceptionHandlerFeature.Error.Message
					}
					.ToString());
				}
			});
		});
	}
}
