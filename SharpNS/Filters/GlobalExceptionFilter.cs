using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SharpNS.Exceptions;
using SharpNS.Models.API;
using System.Collections.Generic;
using System.Linq;

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
                    if (apiEx.StatusCode == 422)
                    {
                        // Aggregate error messages in case of multiple validation errors
                        err.Reasons = new Dictionary<string, string>();
                        foreach(var key in context.ModelState.Keys)
                        {
                            err.Reasons.Add(key, 
                                context.ModelState.GetValueOrDefault(key)?.Errors
                                .Select(e => e.ErrorMessage)
                                .Aggregate((a, b) => a + "; " + b));
                        }
                    }
                    break;
                case DbUpdateException updateEx:
                    switch((updateEx.InnerException as SqliteException).SqliteErrorCode)
                    {
                        case 19:
                            int detailErr = (updateEx.InnerException as SqliteException).SqliteExtendedErrorCode;
                            switch (detailErr)
                            {
                                case 2067:
                                    code = 409;
                                    err.Type = ApiException.GetErrorType(409);
                                    err.Message = "Record with DNS domain already exists.";
                                    break;
                            }
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
