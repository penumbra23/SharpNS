using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharpNS.Exceptions;
using SharpNS.Filters;
using SharpNS.Models.Database;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SharpNS
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlite().AddDbContext<DNSContext>();

            services.AddControllers(options =>
            {
                options.Filters.Add(new MediaTypeResouceFilter());
                options.Filters.Add(new GlobalExceptionFilter());
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressMapClientErrors = true;
                options.InvalidModelStateResponseFactory = ctx =>
                {
                    var actCtx = new ActionExecutedContext(ctx, new List<IFilterMetadata>(), null)
                    {
                        Exception = new ApiException(422, "ValidationFailed")
                    };
                    // Go through the global exception filter
                    new GlobalExceptionFilter().OnActionExecuted(actCtx);
                    return actCtx.Result;
                };
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DNSContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            dbContext.Database.Migrate();
            
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Welcome to SharpNS REST API!");
                });
            });
        }
    }
}
