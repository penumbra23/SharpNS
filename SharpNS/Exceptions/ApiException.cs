using System;

namespace SharpNS.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException(int statusCode, string message) : base(message) 
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; }

        public string ErrorType
        {
            get
            {
                return StatusCode switch
                {
                    401 => "Unauthorized",
                    403 => "Forbidden",
                    404 => "NotFound",
                    422 => "ValidationError",
                    _ => "ServerError",
                };
            }
        }
    }
}
