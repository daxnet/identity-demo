using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.Authorisation;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace IdentityDemo.APIGateway
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot();
            services.AddAuthentication()
                .AddIdentityServerAuthentication("AuthKey", options =>
                {
                    options.Authority = "http://localhost:7889";
                    options.RequireHttpsMetadata = false;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin", builder => builder.RequireRole("admin"));
                options.AddPolicy("superadmin", builder => builder.RequireRole("superadmin"));
            });

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()));
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseOcelot((b, c) =>
            {
                c.AuthorisationMiddleware = async (ctx, next) =>
                {
                    if (ctx.DownstreamReRoute.DownstreamPathTemplate.Value == "/weatherforecast")
                    {
                        var authorizationService = ctx.HttpContext.RequestServices.GetService<IAuthorizationService>();
                        var result = await authorizationService.AuthorizeAsync(ctx.HttpContext.User, "superadmin");
                        if (result.Succeeded)
                        {
                            await next.Invoke();
                        }
                        else
                        {
                            ctx.Errors.Add(new UnauthorisedError($"Fail to authorize policy: admin"));
                        }
                    }
                    else
                    {
                        await next.Invoke();
                    }
                };

                b.BuildCustomOcelotPipeline(c).Build();
                
            }).Wait();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
