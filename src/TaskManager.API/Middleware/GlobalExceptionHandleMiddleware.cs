using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.API.Middleware
{
    public class GlobalExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandleMiddleware> _logger;
        public GlobalExceptionHandleMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandleMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);

            }
        }
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.Clear();
            context.Response.ContentType = "application/problem+json";
            var problemDetails = CreateProblemDetails(exception);
            context.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;

            var jsonOption = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,

            };
            var json = JsonSerializer.Serialize(problemDetails, jsonOption);
            await context.Response.WriteAsJsonAsync(json);

        }

        private static ProblemDetails CreateProblemDetails(Exception exception)
        {
            return exception switch
            {

                FluentValidation.ValidationException validationEx => new ValidationProblemDetails
                {
                    Title = "Validation Error",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = "One or more validation errors occurred.",
                    Errors = validationEx.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()),
                    Type = "https://httpstatuses.com/400",
                    Instance = Guid.NewGuid().ToString()
                },
                UnauthorizedAccessException => new ProblemDetails
                {
                    Title = "Unauthorized",
                    Status = (int)HttpStatusCode.Unauthorized,
                    Detail = "Authentication is required to access the data",
                    Type = "https://httpstatuses.com/401",
                    Instance = Guid.NewGuid().ToString()
                },
                KeyNotFoundException => new ProblemDetails
                {
                    Title = "Resource not found",
                    Status = (int)HttpStatusCode.NotFound,
                    Detail = "The Request resource not found",
                    Type = "https://httpstatuses.com/404",
                    Instance = Guid.NewGuid().ToString()
                },
                InvalidOperationException => new ProblemDetails
                {
                    Title = "Invalid operation",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = "Invalid operation",
                    Type = "https://httpstatuses.com/400",
                    Instance = Guid.NewGuid().ToString()
                },
                _ => new ProblemDetails
                {
                    Title = "Internal server error",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Detail = "An unexpected error occured. please try again later.",
                    Type = "https://httpstatuses.com/500",
                    Instance = Guid.NewGuid().ToString()
                }
            };
        }
    }
}