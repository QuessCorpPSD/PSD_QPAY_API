using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace QPay.API.Extensions
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class SkipWrapperAttribute : Attribute { }
    public class ResponseWrapperFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var skipWrapping = context.ActionDescriptor.EndpointMetadata
                .Any(meta => meta is SkipWrapperAttribute);

            if (skipWrapping) return;

            if (context.Result is ObjectResult objectResult)
            {
                var wrapped = new
                {
                    status = "success",
                    data = objectResult.Value
                };
                context.Result = new ObjectResult(wrapped)
                {
                    StatusCode = objectResult.StatusCode
                };
            }
        }

        public void OnActionExecuting(ActionExecutingContext context) { }
    }

}
