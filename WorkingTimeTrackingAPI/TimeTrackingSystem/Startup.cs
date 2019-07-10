using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using TimeTrackingSystem.Data.Access.Context;
using TimeTrackingSystem.Data.Access.DAL;

namespace TimeTrackingSystem
{
    public class Startup
    {
        private readonly string _apiName = "Time tracking API";
        private readonly string _apiVersion = "V1";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var connection = "Data Source=local.db";

            services.AddDbContext<TimeTrackingSystemDbContext>(options =>
                options.UseSqlite(connection)
            );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info() { Title = _apiName, Version = _apiVersion });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddMvc();

            services.AddScoped<ITimeTrackingSystemRepository, TimeTrackingSystemRepository>();
            // BloggingContext requires
            // using EFGetStarted.AspNetCore.NewDb.Models;
            // UseSqlServer requires
            // using Microsoft.EntityFrameworkCore;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{_apiName} {_apiVersion}");
            });



            app.UseMvc();
        }
    }
}
