using System.Net;
using System.Text.Json;

namespace QPay.API.Extensions
{
    public class WrapperResponse
    {
        /// <summary>
        /// Request Delegate field to invoke HTTP Context
        /// </summary>
        private readonly RequestDelegate _next;
        public WrapperResponse(RequestDelegate next) => _next = next;
        public async Task Invoke(HttpContext context)
        {
            var originalBody = context.Response.Body;

            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            Exception exception = null;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Move stream to start
            memoryStream.Seek(0, SeekOrigin.Begin);

            var contentType = context.Response.ContentType;

            // 🔥 If response is NOT JSON — skip wrapper
            if (!string.IsNullOrEmpty(contentType) &&
                (contentType.Contains("application/pdf")
                 || contentType.Contains("application/octet-stream")
                 || contentType.Contains("application/zip")
                 || contentType.Contains("image/")
                 || contentType.Contains("text/csv")))
            {
                // Write raw bytes back
                context.Response.Body = originalBody;
                await memoryStream.CopyToAsync(originalBody);
                return;
            }

            // --- JSON WRAPPER LOGIC BEGINS HERE ---

            context.Response.Body = originalBody;

            string bodyText = new StreamReader(memoryStream).ReadToEnd();

            context.Response.ContentType = "application/json";

            if (exception == null)
            {
                object? result = null;

                if (!string.IsNullOrWhiteSpace(bodyText))
                {
                    try
                    {
                        result = JsonSerializer.Deserialize(bodyText, typeof(object));
                    }
                    catch
                    {
                        // If failed to deserialize → do NOT wrap
                        await context.Response.WriteAsync(bodyText);
                        return;
                    }
                }

                var response = ResponseWrapManager.ResponseWrapper(result, context);
                if (context.Response.StatusCode != 204)
                {
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }

            }
            else
            {
                var response = ResponseWrapManager.ResponseWrapper(exception, context, exception.InnerException);
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }

    }
}

