using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SharpNS.Models.API;
using System;
using System.Text;

namespace SharpNS.Filters
{
    public class MediaTypeResouceFilter : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            if (context.HttpContext.Response.StatusCode == 415)
            {
                var err = new Error()
                {
                    Message = "Request body must be application/json format",
                    Type = "UnsupportedType"
                };
                var jsonString = JsonConvert.SerializeObject(err,
                    settings: new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new CamelCaseNamingStrategy()
                        }
                    });
                byte[] data = Encoding.UTF8.GetBytes(jsonString);
                context.HttpContext.Response.Body.WriteAsync(data, 0, data.Length);

            }
        }
    }
}
