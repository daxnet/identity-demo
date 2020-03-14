using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            app.UseOcelot();
            

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
