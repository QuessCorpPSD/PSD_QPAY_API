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
            
            // Storing Context Body Response
            var currentBody = context.Response.Body;

            // Using MemoryStream to hold Controller Response
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            // Passing call to Controller
            //await _next(context);
            Exception exception = null;
            try
            {
                await _next(context);
            }
            catch (AccessViolationException avEx)
            {
                //_logger.LogError($"A new violation exception has been thrown: {avEx}");
                //await HandleExceptionAsync(httpContext, avEx);
                exception = avEx;
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Something went wrong: {ex}");
                //await HandleExceptionAsync(httpContext, ex);
                exception = ex;
            }

            // Resetting Context Body Response
            context.Response.Body = currentBody;

            // Setting Memory Stream Position to Beginning
            memoryStream.Seek(0, SeekOrigin.Begin);

            // Read Memory Stream data to the end
            var readToEnd = new StreamReader(memoryStream).ReadToEnd();

            context.Response.ContentType = "application/json";

            if (exception == null)
            {
                // Deserializing Controller Response to an object                
                //var result = JsonSerializer.Deserialize(readToEnd, typeof(object));

                var result = string.IsNullOrEmpty(readToEnd) ? null : JsonSerializer.Deserialize(readToEnd, typeof(object));

                // Invoking Customizations Method to handle Custom Formatted Response
                var response = ResponseWrapManager.ResponseWrapper(result, context);

                // returing response to caller
                //await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            else
            {
                var response = ResponseWrapManager.ResponseWrapper(null, context, exception.InnerException);

                // returing response to caller
                //await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
