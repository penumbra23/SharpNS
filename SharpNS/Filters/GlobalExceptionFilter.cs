using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
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
                    err.Type = ApiException.GetErrorType(apiEx.StatusCode);
                    break;
                case DbUpdateException updateEx:
                    switch((updateEx.InnerException as SqliteException).SqliteErrorCode)
                    {
                        case 19:
                            code = 409;
                            err.Type = ApiException.GetErrorType(409);
                            err.Message = "Record with DNS domain already exists.";
                            break;
                        default:
                            code = 500;
                            err.Type = "DatabaseError";
                            err.Message = "Internal database error.";
                            break;
                    }
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
