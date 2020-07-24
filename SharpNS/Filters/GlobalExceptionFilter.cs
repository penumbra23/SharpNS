using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpNS.Exceptions;
using SharpNS.Models.API;

namespace SharpNS.Filters
{
    public class GlobalExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
                return;

            int code = 500;
            Error err = new Error()
            {
                Message = "Internal server error.",
                Type = "ServerError"
            };

            switch(context.Exception)
            {
                case ApiException apiEx:
                    code = apiEx.StatusCode;
                    err.Message = apiEx.Message;
                    err.Type = apiEx.ErrorType;
                    break;
            }

            context.Result = new JsonResult(err)
            {
                StatusCode = code,
            };
            context.ExceptionHandled = true;
        }

    }
}
