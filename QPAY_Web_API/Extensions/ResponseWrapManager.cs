using Microsoft.AspNetCore.Http.Extensions;
using QPAY_Web_API.Models;
using System.Net;

namespace QPay.API.Extensions
{
    public class ResponseWrapManager
    {
        public static APIResponses ResponseWrapper(object? result, HttpContext context, object? exception = null)
        {
           // var requestUrl = context.Request.GetDisplayUrl();
            var data = result;            

            ErrorDetails error = null;            

            var status = result != null;
            var httpStatusCode = (HttpStatusCode)context.Response.StatusCode;

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                error = new ErrorDetails()
                {
                    ErrorCode = context.Response.StatusCode,
                    ErrorMessage = "Unauthorized. Token Validation Has Failed. Request Access Denied",
                   // Description = "Invalid Credentials. Make sure your API invocation call has a header: 'Authorization : Bearer ACCESS_TOKEN'"
                };
            }

            if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
            {
                error = new ErrorDetails()
                {
                    ErrorCode = 800404,
                    ErrorMessage = "Resource not found"
                    
                };
            }

            if (context.Response.StatusCode == (int)HttpStatusCode.MethodNotAllowed)
            {
                error = new ErrorDetails()
                {
                    ErrorCode = 800405,
                    ErrorMessage = "Method Not Allowed"
                   
                };
            }

            if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                error = new ErrorDetails()
                {
                    ErrorCode = 800400,
                    ErrorMessage = "Bad Request"
                   
                };
            }

            if (!status && error == null)
            {
                error = new ErrorDetails()
                {
                    ErrorCode = 800401,
                    ErrorMessage = ""  //"There is no data for given input parameters."

                };
            }

            // NOTE: Add any further customizations if needed here

            var sumessage = status==true ? "Success" : "API Working";

            if(!status)
            {
                var statusCode = 200;
                var response = new APIResponses((HttpStatusCode)statusCode, sumessage, data, null);
                return response;
            }
            else
            {
                if (!status && httpStatusCode==(HttpStatusCode)200)
                    httpStatusCode=(HttpStatusCode)(201);
                var response = new APIResponses(httpStatusCode, sumessage, data, error);
                return response;
            }



        }

    }
}
