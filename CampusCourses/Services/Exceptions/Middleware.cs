using System.Net;
using System.Text.Json;
using CampusCourses.Data.Entities;

namespace CampusCourses.Services.Exceptions
{
    public class Middleware
    {
        private readonly RequestDelegate _requestDelegate;

        public Middleware(RequestDelegate next)
        {
            _requestDelegate = next;
        }

        public async Task InvokeAsync(HttpContext db)
        {
            try
            {
                await _requestDelegate(db);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(db, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            int statusCode;
            string message;

            Console.WriteLine($"Exception caught: {exception.GetType().Name} - {exception.Message}");
            Console.WriteLine(exception.StackTrace);

            switch (exception)
            {
                case BadRequestException badRequestException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = !string.IsNullOrEmpty(badRequestException.Message) ? badRequestException.Message : "Плохой запрос";
                    break;

                case UnauthorizedException unauthorizedException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    message = !string.IsNullOrEmpty(unauthorizedException.Message) ? unauthorizedException.Message : "Данный пользователь не авторизован";
                    break;

                case InternalServerErrorException internalServerErrorException:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    message = !string.IsNullOrEmpty(internalServerErrorException.Message) ? internalServerErrorException.Message : "Внутренняя ошибка сервера";
                    break;

                case ForbiddenException forbiddenException:
                    statusCode = (int)HttpStatusCode.Forbidden;
                    message = !string.IsNullOrEmpty(forbiddenException.Message) ? forbiddenException.Message : "Запрещенный";
                    break;

                case NotFoundException notFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    message = !string.IsNullOrEmpty(notFoundException.Message) ? notFoundException.Message : "Не найдено";
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    message = "Внутренняя ошибка сервера";
                    break;
            }

            response.StatusCode = statusCode;

            var error = new Error
            {
                StatusCode = statusCode,
                Message = !string.IsNullOrEmpty(message) ? message : "Внутренняя ошибка сервера"
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(error, options);

            await response.WriteAsync(json);
        }
    }
}
